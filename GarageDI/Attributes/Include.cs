namespace GarageDI.Attributes;

class Include : Attribute
{
    public int Order { get; }
    public Include(int order = 100) => Order = order;
}
