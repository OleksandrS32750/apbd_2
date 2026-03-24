using APBD_2.Database.Enums;

namespace APBD_2.Database.Interfaces;

public abstract class Equipment
{
    protected Equipment(int id, string name)
    {
        Id = id;
        Name = name;
        Status = EquipmentStatus.Available;
    }

    public int Id { get; }
    public string Name { get; }
    public EquipmentStatus Status { get; private set; }

    public bool IsAvailableForRental => Status == EquipmentStatus.Available;

    public void MarkRented()
    {
        Status = EquipmentStatus.Rented;
    }

    public void MarkAvailable()
    {
        Status = EquipmentStatus.Available;
    }

    public void MarkUnavailable()
    {
        Status = EquipmentStatus.Unavailable;
    }
}
