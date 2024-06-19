namespace GarageDI.Extensions;

internal static class PropertyInfoExtensions
{
    public static string GetDisplayText(this PropertyInfo prop)
    {
        var attr = prop.GetCustomAttribute<Beautify>();
        return attr is null ? prop.Name : attr.Text;
    }
}
