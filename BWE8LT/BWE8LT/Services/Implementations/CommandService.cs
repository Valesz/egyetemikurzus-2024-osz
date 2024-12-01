using System.Text.Json;

using BWE8LT.Commands.CommandTypes;
using BWE8LT.Controller;
using BWE8LT.Model;
using BWE8LT.Model.JSON_Parse_Objects;

namespace BWE8LT.Services.Implementations;

public class CommandService : ICommandService
{
    public CommandConfigJson CommandConfigJson { get; set; }
    
    public void LoadCommands(string configPath)
    {
        string configText = File.ReadAllText(configPath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        
        CommandConfigJson = JsonSerializer.Deserialize<CommandConfigJson>(configText, options) 
                            ?? throw new ArgumentException("Couldn't deserialize config");

        foreach (CommandJson command in CommandConfigJson.CommandsList)
        {
            switch (command)
            {
                case KeyCommandJson keyCommandJson:
                    KeyCommand keyCommandModel = keyCommandJson.ToCommand();
                    IKeyCommand.Commands.Add(keyCommandModel.Key, keyCommandModel.Action);
                    break;
                case LineCommandJson lineCommandJson:
                    LineCommand lineCommandModel = lineCommandJson.ToCommand();
                    ILineCommand.Commands.Add(lineCommandModel.Key, lineCommandModel.Action);
                    break;
                default:
                    throw new ArgumentException("Command type not recognized");
            }   
        }
    }

    public ConsoleKeyInfo ReadKeyCommand()
    {
        ConsoleKeyInfo readKeyInfo = Console.ReadKey(true);
        
        return new ConsoleKeyInfo(
            '.',
            readKeyInfo.Key,
            readKeyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift),
            readKeyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt),
            readKeyInfo.Modifiers.HasFlag(ConsoleModifiers.Control)
        );
    }

    public string ReadLineCommand()
    {
        return Console.ReadLine() ?? throw new ArgumentException("Nothing provided to execute");
    }

    public void ExecuteKeyCommand(ConsoleKeyInfo commandKey, IConsoleController consoleController)
    {
        try
        {
            IKeyCommand.Commands[commandKey].Invoke(commandKey, consoleController);
        }
        catch (KeyNotFoundException _)
        {
            consoleController.CurrentWindow.UpdateFooter([
                $"Key '{commandKey.Key}' was not found in dictionary",
                consoleController.GetWindowIndicators()
            ]);
            consoleController.CurrentWindow.RefreshDisplay();
        }
        catch (Exception ex)
        {
            consoleController.CurrentWindow.UpdateFooter([
                ex.Message,
                consoleController.GetWindowIndicators()
            ]);
            consoleController.CurrentWindow.RefreshDisplay();
        }
    }

    public void ExecuteLineCommand(string line, IConsoleController consoleController)
    {
        try
        {
            string command = line.Split(" ")[0];
            ILineCommand.Commands[command].Invoke(line, consoleController);
        }
        catch (Exception ex)
        {
            consoleController.CurrentWindow.UpdateFooter([
                ex.Message,
                consoleController.GetWindowIndicators()
            ]);
            consoleController.CurrentWindow.RefreshDisplay();
        }
    }
}