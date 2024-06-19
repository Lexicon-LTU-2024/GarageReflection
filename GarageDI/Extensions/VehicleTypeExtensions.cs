using GarageDI.DTOs;

namespace GarageDI.Extensions;

public static class VehicleTypeExtensions
{
    public static VehicleDTO GetInstanceAndPropsForType(this VehicleType vehicleType)
    {
        Type type = Type.GetType($"{ConfigHelpers.Assembly}.{vehicleType}", throwOnError: true)!;

        if (Activator.CreateInstance(type) is IVehicle vehicle)
        {
            PropertyInfo[] properties = vehicle.GetPropertiesWithIncludedAttribute();
            return new VehicleDTO(properties, vehicle);
        }

        throw new ArgumentException($"Type {type} does not implement IVehicle.", nameof(vehicleType));

    }

   

}

