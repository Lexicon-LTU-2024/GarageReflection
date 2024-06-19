namespace GarageDI.Contracts;

public interface IGarage<T> : IEnumerable<T> where T : IVehicle
{
    string Name { get; }
    int Count { get; }
    int Capacity { get; }
    bool Park(T vehicle);
    bool Leave(T vehicle);
    bool IsFull { get; }

}
