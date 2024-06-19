namespace GarageDI.Contracts;

public interface IUtil
{
    int AskForInt(string prompt);
    int AskForKey(string prompt);
    string AskForString(string prompt);
}