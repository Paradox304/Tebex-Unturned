using System.Collections.Generic;
using Rocket.API;
using TebexUnturned.Shared;

namespace TebexUnturned.Commands;

public class DebugCommand : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Console;

    public string Name => "tebex:debug";

    public string Help => "Toggles more in-depth logging for the Tebex plugin";

    public string Syntax => "<on/true/off/false>";

    public List<string> Aliases => new() { "tebex.debug" };

    public List<string> Permissions => new() { "tebex.admin" };

    public bool RunFromConsole => true;

    public void Execute(IRocketPlayer player, string[] args)
    {
        var _adapter = TebexUnturned.GetAdapter();

        if (!player.HasPermission(Permissions[0]))
        {
            _adapter.ReplyPlayer(player, "You do not have permission to run that command.");
            return;
        }

        if (args.Length != 1)
        {
            _adapter.ReplyPlayer(player, "Usage: tebex.debug <on/off>");
            return;
        }

        if (args[0].Equals("on"))
        {
            BaseTebexAdapter.PluginConfig.DebugMode = true;
            //Config.WriteObject(BaseTebexAdapter.PluginConfig); FIXME
            _adapter.ReplyPlayer(player, "Debug mode is enabled.");
        }
        else if (args[0].Equals("off"))
        {
            BaseTebexAdapter.PluginConfig.DebugMode = false;
            //Config.WriteObject(BaseTebexAdapter.PluginConfig); FIXME
            _adapter.ReplyPlayer(player, "Debug mode is disabled.");
        }
        else
            _adapter.ReplyPlayer(player, "Usage: tebex.debug <on/off>");
    }
}