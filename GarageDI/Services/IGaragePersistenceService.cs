namespace GarageDI.Services
{
    public interface IGaragePersistenceService
    {
        GarageDTO LoadGarage(string garageName);
        void SaveGarage(string garageName, int capacity, IEnumerable<IVehicle> vehicles);
    }
}

