using BWE8LT.Utils;

namespace BWE8LT.Commands;

public class ExitApplicationCommand : ICommand
{
    public void Execute(ConsoleKey pressedKey)
    {
        Console.Clear();
        Console.CursorVisible = true;
        Environment.Exit(0);
    }
}