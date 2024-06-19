using GarageDI.Services;

namespace GarageDI.Garage;

public class InMemoryGarage<T> : IGarage<T> where T : IVehicle
{

    private T[] vehicles;
    private readonly IGaragePersistenceService percistance;

    public string Name { get; }

    public int Capacity { get; init; }

    public int Count { get; private set; } = 0;

    public bool IsFull => Count >= Capacity;


    public InMemoryGarage(ISettings sett, IGaragePersistenceService percistance)
    {
        if (sett is null) throw new ArgumentNullException(nameof(ISettings));

        Name = sett.Name;
        Capacity = Math.Max(2, sett.Size);
        vehicles = new T[Capacity];
        this.percistance = percistance;
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var v in vehicles)
        {
            if (v != null) yield return v;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    public bool Park(T vehicle)
    {
        bool result = false;
        if (IsFull) return result;

        var index = Array.IndexOf(vehicles, null);

        if (index != -1)
        {
            vehicles[index] = vehicle;
            result = true;
            Count++;
            Save();
            result = true;
        }

        return result;
    }

    private void Save()
    {
        var onlyParkedVehicles = vehicles.Cast<IVehicle>().Where(v => v != null).ToList();
        percistance.SaveGarage(Name, Capacity, onlyParkedVehicles);
    }

    public bool Leave(T vehicle)
    {
        bool result = false;
        if (vehicle is null) return result;

        var index = Array.IndexOf(vehicles, vehicle);

        if (index != -1)
        {
            vehicles[index] = default!;
            Count--;
            Save();
            result = true;
        }

        return result;
    }

}
