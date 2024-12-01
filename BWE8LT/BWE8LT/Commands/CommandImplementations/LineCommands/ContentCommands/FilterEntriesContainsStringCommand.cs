using BWE8LT.Commands.CommandTypes;
using BWE8LT.Controller;
using BWE8LT.Model;

namespace BWE8LT.Commands.CommandImplementations.LineCommands.ContentCommands;

public class FilterEntriesContainsStringCommand : ILineCommand
{
    public void Execute(string line, ConsoleController consoleController)
    {
        string[] args = line.Split(" ")[1..];

        FileItem[] newFiles = consoleController.CurrentWindow.FileService.Files.Where(
            file => file.Name.Contains(args[0])
        ).ToArray();

        consoleController.CurrentWindow.FileService.Files = newFiles;
        
        consoleController.CurrentWindow.Clear();
        consoleController.CurrentWindow.WriteLoadedFilesToConsole();
        consoleController.CurrentWindow.RefreshDisplay();
    }
}