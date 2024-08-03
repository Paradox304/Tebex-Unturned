using System.Collections.Generic;
using Rocket.API;

namespace TebexUnturned.Commands;

public class ForceCheckCommand : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Both;

    public string Name => "tebex:forcecheck";

    public string Help => "Force check packages currently waiting to be executed";

    public string Syntax => "";

    public List<string> Aliases => new() { "tebex.forcecheck" };

    public List<string> Permissions => new() { "tebex.admin" };

    public bool RunFromConsole => true;

    public void Execute(IRocketPlayer player, string[] command)
    {
        var _adapter = TebexUnturned.GetAdapter();
        if (!_adapter.IsReady)
        {
            _adapter.ReplyPlayer(player, "Tebex is not setup.");
            return;
        }

        if (!player.HasPermission(Permissions[0]))
        {
            _adapter.ReplyPlayer(player, "You do not have permission to run that command.");
            return;
        }

        _adapter.RefreshStoreInformation(true);
        _adapter.ProcessCommandQueue(true);
        _adapter.ProcessJoinQueue(true);
        _adapter.DeleteExecutedCommands(true);
    }
}