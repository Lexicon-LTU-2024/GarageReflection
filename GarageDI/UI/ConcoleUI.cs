namespace GarageDI.UI;

public class ConsoleUI : IUI
{
    public void Clear()
    {
        Console.Clear();
    }

    public string GetString()
    {
        var input = Console.ReadLine() ?? string.Empty;
        return input;

    }

    public string GetKey()
    {
        return Console.ReadKey(intercept: true).KeyChar.ToString();
    }

    public void Meny(bool isFull, string options, string menyheading)
    {
        Console.WriteLine(isFull ? "No spots left" : menyheading + "\n" + options);
    }

    public void Print(string message)
    {
        Console.WriteLine(message);
    }

    public ConsoleKeyInfo GetKeyInfo()
    {
        return Console.ReadKey();
    }
}
