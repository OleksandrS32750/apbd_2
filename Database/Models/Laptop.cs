using APBD_2.Database.Interfaces;

namespace APBD_2.Database.Models;

public sealed class Laptop : Equipment
{
    public Laptop(int id, string name, int ramGb, int storageGb) : base(id, name)
    {
        RamGb = ramGb;
        StorageGb = storageGb;
    }

    public int RamGb { get; }
    public int StorageGb { get; }
}
