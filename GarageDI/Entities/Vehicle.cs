using GarageDI.Attributes;
namespace GarageDI.Entities;

public class Vehicle : IVehicle
{
    private string regNo = string.Empty;
    private string color = string.Empty;

    public static Predicate<string> Check { get; internal set; } = default!;
    public static Action Callback { get; internal set; } = default!;

    public string Name { get; }


    public Vehicle()
    {
        Name = GetType().Name;
    }


    [Beautify("Registration number")]
    [Include(1)]
    public string RegNo
    {
        get => regNo;
        set
        {
            if (Check(value.ToUpper()))
                regNo = value.ToUpper();
            else
                Callback();
            return;
        }
    }


    [Include]
    public string Color
    {
        get { return color; }
        set { color = value.ToUpper(); }
    }

    public virtual object this[string name]
    {
        get
        {
            PropertyInfo? prop = GetType().GetProperty(name);

            if (prop != null)
                return prop.GetValue(this)!;
            else
                throw new ArgumentNullException("Invalid property name");

        }
        set
        {
            PropertyInfo? prop = GetType().GetProperty(name);
            if (prop != null)
                prop.SetValue(this, value);
            else
                throw new ArgumentNullException("Invalid property name");

        }
    }

    public virtual string GetInfo()
    {
        var builder = new StringBuilder().Append($"[{this.GetType().Name}]\t");

        Array.ForEach(this.GetPropertiesWithIncludedAttribute(),
                       p => builder.Append($" {p.GetDisplayText()}:{p.GetValue(this)}"));

        return builder.ToString();
    }
}
