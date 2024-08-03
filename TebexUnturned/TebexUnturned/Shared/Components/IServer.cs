namespace TebexUnturned.Shared.Components;

public interface IServer
{
    string Address { get; }
    string Version { get; }
    string Protocol { get; }
    string Command(string value);
}