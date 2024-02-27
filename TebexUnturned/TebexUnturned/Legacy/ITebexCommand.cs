using System;
using Newtonsoft.Json.Linq;
using Rocket.API;

namespace TebexUnturned.Legacy
{
    public interface ITebexCommand : IRocketCommand
    {
        void HandleResponse(JObject response);

        void HandleError(Exception e);
        
    }  
}