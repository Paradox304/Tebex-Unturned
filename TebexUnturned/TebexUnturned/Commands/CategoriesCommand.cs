using System.Collections.Generic;
using Rocket.API;

namespace TebexUnturned.Commands;

public class CategoriesCommand : IRocketCommand
{
    public AllowedCaller AllowedCaller => AllowedCaller.Both;

    public string Name => "tebex:categories";

    public string Help => "Print available package categories.";

    public string Syntax => "";

    public List<string> Aliases => new() { "tebex.categories" };

    public List<string> Permissions => new();

    public bool RunFromConsole => true;

    public void Execute(IRocketPlayer commandRunner, string[] command)
    {
        var _adapter = TebexUnturned.GetAdapter();
        if (!_adapter.IsReady)
        {
            _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
            return;
        }

        _adapter.GetCategories(categories => { TebexUnturned.PrintCategories(commandRunner, categories); });
    }
}