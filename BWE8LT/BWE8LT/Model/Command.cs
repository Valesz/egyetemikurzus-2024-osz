namespace BWE8LT.Model;

public class Command
{
    public required ConsoleKey Key { get; set; }

    public required Action<ConsoleKey> Action { get; set; }
}