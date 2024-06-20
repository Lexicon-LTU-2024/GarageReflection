namespace GarageDI.Contracts;

public interface IUI
{
    string GetString();
    string GetKey();
    void Print(string message);
    void Clear();
    void Meny(bool isFull, string options, string menyheading);
    ConsoleKeyInfo GetKeyInfo();
}
