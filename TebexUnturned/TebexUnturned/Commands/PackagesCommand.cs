using System.Collections.Generic;
using Rocket.API;

namespace TebexUnturned.Commands;

public class PackagesCommand : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Both;

    public string Name => "tebex:packages";

    public string Help => "View packages available to purchase";

    public string Syntax => "";

    public List<string> Aliases => new() { "tebex.packages" };

    public List<string> Permissions => new() { "tebex.admin" };

    public bool RunFromConsole => true;

    public void Execute(IRocketPlayer commandRunner, string[] command)
    {
        var _adapter = TebexUnturned.GetAdapter();
        if (!_adapter.IsReady)
        {
            _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
            return;
        }

        if (!commandRunner.HasPermission(Permissions[0]))
        {
            _adapter.ReplyPlayer(commandRunner, "You do not have permission to run that command.");
            return;
        }

        _adapter.GetPackages(packages => { TebexUnturned.PrintPackages(commandRunner, packages); });
    }
}