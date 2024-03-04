using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Player;

namespace TebexUnturned.Commands
{
    public class PackagesCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Console;

        public bool RunFromConsole => true;

        public string Name => "tebex:packages";

        public string Help => "View packages available to purchase";
        
        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "tebex.admin" };

        public void Execute(IRocketPlayer commandRunner, string[] command)
        {
            var _adapter = Tebex.Plugins.TebexUnturned.GetAdapter();
            if (!commandRunner.HasPermission(Permissions[0]))
            {
                _adapter.ReplyPlayer(commandRunner, "You do not have permission to run that command.");
                return;
            }

            _adapter.GetPackages(packages => { Tebex.Plugins.TebexUnturned.PrintPackages(commandRunner as UnturnedPlayer, packages); });
        }
    }
}