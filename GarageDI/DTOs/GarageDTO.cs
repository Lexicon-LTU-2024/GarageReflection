#nullable disable

namespace GarageDI.DTOs;

public class GarageDTO
{
    public string Name { get; set; }

    public int Capacity { get; set; }

    public IEnumerable<IVehicle> Vehicles { get; set; }
}


