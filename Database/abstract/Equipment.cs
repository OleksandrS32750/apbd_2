using APBD_2.Database.Enums;

namespace APBD_2.Database.Abstract;

public abstract class Equipment
{
    private static int _nextId = 1;

    protected Equipment(string name)
    {
        Id = _nextId++;
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
