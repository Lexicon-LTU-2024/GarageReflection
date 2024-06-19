namespace GarageDI.Contracts;

public interface IGarageHandler
{
    bool IsGarageFull { get; }

    bool Park(IVehicle v);
    IVehicle GetVehicle(VehicleDTO dto);
    IVehicle? Get(string regNo);
    List<VehicleCountDTO> GetByType();
    List<IVehicle> GetAll();
    bool Leave(string regNo);
    IEnumerable<IVehicle> SearchVehicle(VehicleDTO dto);
    void Load();

}
