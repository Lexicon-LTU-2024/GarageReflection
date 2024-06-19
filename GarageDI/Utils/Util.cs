namespace GarageDI.Utils;

public class Util : IUtil
{
    private readonly IUI ui;

    public Util(IUI ui)
    {
        this.ui = ui;
    }
    public string AskForString(string prompt)
    {
        bool correct = true;
        string answer;

        do
        {
            ui.Print(prompt);
            answer = ui.GetString();

            if (!string.IsNullOrEmpty(answer))
            {
                correct = false;
            }

        } while (correct);

        return answer;
    }

    public int AskForInt(string prompt)
    {
        bool success;
        uint answer;
        do
        {
            string input = AskForString(prompt);

            success = uint.TryParse(input, out answer);

            if (!success)
            {
                ui.Print("Wrong format only numbers. Negative numbers not allowed");
            }

        } while (!success);

        return (int)answer;
    }

    public int AskForKey(string prompt)
    {
        bool success;
        int keyPressed;

        do
        {
            Console.Write(prompt);
            var input = ui.GetKey();
            success = int.TryParse(input, out keyPressed);
            if (!string.IsNullOrEmpty(input) && success)
                success = true;

        } while (!success);

        return keyPressed;
    }
}
