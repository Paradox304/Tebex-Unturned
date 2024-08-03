using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace TebexUnturned.Shared.Components;

public class WebRequests
{
    private static BaseTebexAdapter _adapter;
    private static Queue<TebexRequest> _requestQueue;

    public WebRequests(BaseTebexAdapter adapter)
    {
        _adapter = adapter;
        _requestQueue = new();
    }

    public void Enqueue(
        string url, string body, Action<int, string> callback, TebexApi.HttpVerb method = TebexApi.HttpVerb.GET, Dictionary<string, string> headers = null, float timeout = 0.0f)
    {
        _requestQueue.Enqueue(new(url, body, callback, method, headers, timeout));
    }

    public void Enqueue(TebexRequest request)
    {
        _requestQueue.Enqueue(request);
    }

    public int GetNumQueuedRequests() => _requestQueue.Count;

    public async Task ProcessNextRequestAsync()
    {
        if (_requestQueue.Count == 0)
            return;

        var request = _requestQueue.Dequeue();

        try
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            webRequest.Method = request.Method.ToString();
            webRequest.Timeout = (int)(request.Timeout * 1000);

            foreach (var header in request.Headers)
                webRequest.Headers[header.Key] = header.Value;

            if (!string.IsNullOrEmpty(request.Body) && (request.Method == TebexApi.HttpVerb.POST || request.Method == TebexApi.HttpVerb.PUT))
            {
                webRequest.ContentType = "application/json";
                using (var stream = await Task.Factory.FromAsync(webRequest.BeginGetRequestStream, webRequest.EndGetRequestStream, null))
                using (var writer = new StreamWriter(stream))
                {
                    var logOutStr = $"-> {request.Method.ToString()} {request.Url} | {request.Body}";

                    _adapter.LogDebug(logOutStr); // Write the full output entry to a debug log
                    if (logOutStr.Length > 256) // Limit any sent size of an output string to 256 characters, to prevent sending too much data
                        logOutStr = logOutStr.Substring(0, 251) + "[...]";

                    await writer.WriteAsync(request.Body);
                }
            }

            using (var response = await Task.Factory.FromAsync(webRequest.BeginGetResponse, webRequest.EndGetResponse, null))
            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                var responseBody = await reader.ReadToEndAsync();
                var truncatedResponse = responseBody.Length > 256 ? responseBody.Substring(0, 251) + "[...]" : responseBody;

                var logInStr = $"{((HttpWebResponse)response).StatusCode} | '{truncatedResponse}' <- {request.Method.ToString()} {request.Url}";
                _adapter.LogDebug(logInStr);

                request.Callback?.Invoke((int)((HttpWebResponse)response).StatusCode, responseBody);
            }
        }
        catch (Exception ex)
        {
            _adapter.LogDebug("Error sending request: " + ex.Message);
            request.Callback?.Invoke(0, $"Request failed: {ex.Message}");
        }
    }
}

public class TebexRequest
{
    public string Url { get; }
    public string Body { get; }
    public Action<int, string> Callback { get; }
    public TebexApi.HttpVerb Method { get; }
    public Dictionary<string, string> Headers { get; }
    public float Timeout { get; }

    public TebexRequest(string url, string body, Action<int, string> callback, TebexApi.HttpVerb method, Dictionary<string, string> headers, float timeout)
    {
        Url = url;
        Body = body;
        Callback = callback;
        Method = method;
        Headers = headers ?? new Dictionary<string, string>();
        Timeout = timeout;
    }
}