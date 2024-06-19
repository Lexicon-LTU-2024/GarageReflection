using GarageDI.DTOs;
using GarageDI.Services;
using System.Diagnostics.CodeAnalysis;

namespace GarageDI.Garage;

class GarageHandler : IGarageHandler
{
    private IGarage<IVehicle> garage;
    private readonly IUtil util;
    private readonly IGaragePersistenceService persitiance;

    public GarageHandler(IGarage<IVehicle> garage, IUtil util, IGaragePersistenceService persitiance)
    {
        this.garage = garage;
        this.util = util;
        this.persitiance = persitiance;
    }

    public bool IsGarageFull => garage.IsFull;

    public List<IVehicle> GetAll()
    {
        return garage.ToList();
    }

    public List<VehicleCountDTO> GetByType()
    {
        return garage.GroupBy(v => v.GetType().Name)
                     .Select(v => new VehicleCountDTO(v.Key, v.Count()))
                     .ToList();
    }

    public IVehicle GetVehicle(VehicleDTO dto)
    {
        foreach (var prop in dto.PropertyInfos)
        {
            switch (Type.GetTypeCode(prop.PropertyType))
            {
                case TypeCode.Int32:
                    prop.SetValue(dto.Vehicle, util.AskForInt(prop.GetDisplayText()));
                    break;
                case TypeCode.String:
                    var r = util.AskForString(prop.GetDisplayText());
                    dto.Vehicle[prop.Name] = r;
                    break;
            }
        }

        return dto.Vehicle;
    }

    public IEnumerable<IVehicle> SearchVehicle(VehicleDTO dto)
    {
        var result = dto.Vehicle is null ? 
            garage :
            garage.Where(v => v.GetType() == dto.Vehicle.GetType());

        foreach (var prop in dto.PropertyInfos)
        {
            var searchWord = util.AskForString(prop.GetDisplayText()).ToUpper();

            if(searchWord != "X") 
                result = result.Where(v => v[prop.Name].ToString() == searchWord);
        }

        return result.ToList();
    }

    public bool Park(IVehicle v)
    {
        return garage.Park(v);
    }

    public IVehicle? Get(string regNo)
    {
        return garage.FirstOrDefault(v => v.RegNo == regNo);
    }

    public bool Leave(string regNo)
    {
        var match = Get(regNo);
        return match is null ? false : garage.Leave(match);
    }
    
    public void Load()
    {
        var dto = persitiance.LoadGarage(garage.Name); //ToDo: Get GarageName as parameter
        garage = new InMemoryGarage<IVehicle>(new Settings { Name = dto.Name, Size = dto.Capacity}, persitiance);
        foreach (var vehicle in dto.Vehicles) garage.Park(vehicle);
    }

}
