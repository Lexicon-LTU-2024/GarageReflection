namespace GarageDI.Contracts;

public interface IUI
{
    string GetString();
    string GetKey();
    void ShowMeny();
   // IEnumerable<string> GetMainMenyOptions();
    void Print(string message);
    void Clear();
    void Meny(bool isFull, string options, string menyheading);
   
}
