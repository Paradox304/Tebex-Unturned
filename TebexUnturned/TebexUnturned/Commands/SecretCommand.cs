using System.Collections.Generic;
using Rocket.API;
using TebexUnturned.Shared;

namespace TebexUnturned.Commands;

public class SecretCommand : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Console;

    public string Name => "tebex:secret";

    public string Help => "Connects to your webstore using the secret key.";

    public string Syntax => "<secretKey>";

    public List<string> Aliases => new() { "tebex.secret" };

    public List<string> Permissions => new() { "tebex.admin" };

    public bool RunFromConsole => true;

    public void Execute(IRocketPlayer player, string[] args)
    {
        var _adapter = TebexUnturned.GetAdapter() as TebexUnturnedAdapter;

        // Secret can only be ran as the admin
        if (!player.HasPermission(Permissions[0]))
        {
            _adapter.ReplyPlayer(player, "You do not have permission to run this command.");
            if (player.IsAdmin)
                _adapter.ReplyPlayer(player, "- You must grant the `tebex.admin` permission to your player character");

            return;
        }

        if (args.Length != 1)
        {
            _adapter.ReplyPlayer(player, "Invalid syntax. Usage: \"tebex.secret <secret>\"");
            return;
        }

        _adapter.ReplyPlayer(player, "Setting your secret key...");
        BaseTebexAdapter.PluginConfig.SecretKey = args[0];
        _adapter.SaveConfiguration();

        // Reset store info so that we don't fetch from the cache
        BaseTebexAdapter.Cache.Instance.Remove("information");

        // Any failure to set secret key is logged to console automatically
        _adapter.FetchStoreInfo(info =>
        {
            _adapter.ReplyPlayer(player, "Successfully set your secret key.");
            _adapter.ReplyPlayer(player, $"Store set as: {info.ServerInfo.Name} for the web store {info.AccountInfo.Name}");
        });
    }
}