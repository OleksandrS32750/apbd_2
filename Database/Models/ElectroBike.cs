using APBD_2.Database.Interfaces;

namespace APBD_2.Database.Models;

public sealed class ElectroBike : Equipment
{
    public ElectroBike(int id, string name, decimal weight, int modelYear) : base(id, name)
    {
        Weight = weight;
        ModelYear = modelYear;
    }

    public int Weight { get; }
    public int ModelYear { get; }
}
