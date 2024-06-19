namespace GarageDI.Entities;

class Buss : Vehicle
{
    [Include]
    public int Seats { get; set; }
}
