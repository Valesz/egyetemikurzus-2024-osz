using BWE8LT.Services;
using BWE8LT.Utils;

namespace BWE8LT.Commands.DirectoryCommands;

public class LeaveDirectoryCommand : ICommand
{
    public void Execute(ConsoleKey pressedKey)
    {
        FileService.Instance.LeaveDirectory();
        Cursor.Instance.MoveCursor(0);
    }
}