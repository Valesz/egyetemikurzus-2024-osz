using System.Text;

using BWE8LT.Services;

namespace BWE8LT.Controller;

public class ConsoleController
{
	public Window CurrentWindow { get; set; }

	public List<Window> Windows { get; }

    public List<string> Clipboard { get; }

	public ConsoleController(string workingDirectory)
	{
		Windows = new List<Window>();
        Clipboard = new List<string>();
		
		Window initalWindow = new Window(workingDirectory);
		
		CurrentWindow = initalWindow;
		Windows.Add(initalWindow);
	}

    public int CreateNewWindow(string workingDirectory)
    {
        Window newWindow = new Window(workingDirectory);
        Windows.Add(newWindow);
        
        return Windows.Count;
    }

    public void DeleteWindow(int index)
    {
        Windows.RemoveAt(index);
    }

    public void SwitchCurrentWindow(int index)
    {
        int indexOfWindow = Math.Clamp(index, 0, Windows.Count - 1);
        
        CurrentWindow = Windows[indexOfWindow];
    }

    public int GetIndexOfCurrentWindow()
    {
        return Windows.IndexOf(CurrentWindow);
    }

    public string GetWindowIndicators()
    {
        StringBuilder footerString = new StringBuilder();
        
        foreach (Window window in Windows)
        {
            if (window == CurrentWindow)
            {
                footerString.Append($"*{window.FileService.GetLastDirectoryInPath()}*, ");
                continue;
            }
            
            footerString.Append($"{window.FileService.GetLastDirectoryInPath()}, ");
        }

        footerString.Remove(footerString.Length - 2, 2);
        
        return footerString.ToString();
    }

    public void Copy(string[] path)
    {
        Clipboard.Clear();
        Clipboard.AddRange(path);
    }
}