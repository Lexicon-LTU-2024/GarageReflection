namespace GarageDI.Extensions;

internal static class Generic
{
    //ToDo: Save result per Type and reuse create and save if not found
    public static PropertyInfo[] GetPropertiesWithIncludedAttribute<T>(this T type) where T : IVehicle
    {
        return type.GetType()
                   .GetProperties()
                   .Where(p => p.IsDefined(typeof(Include), true))
                   .OrderBy(p => ((Include)p.GetCustomAttribute(typeof(Include))!).Order)
                   .ToArray();
    }
}
