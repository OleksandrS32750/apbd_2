using APBD_2.Database.Abstract;

namespace APBD_2.Database.Models;

public sealed class Laptop : Equipment
{
    public Laptop(string name, int ramGb, int storageGb) : base(name)
    {
        RamGb = ramGb;
        StorageGb = storageGb;
    }

    public int RamGb { get; }
    public int StorageGb { get; }
}
