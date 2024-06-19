namespace GarageDI.Attributes;

class Beautify : Attribute
{
    public string Text { get; }

    public Beautify(string text)
    {
        Text = text;
    }

}
