using APBD_2.Database.Interfaces;

namespace APBD_2.Database.Models;

public sealed class ElectroBike : Equipment
{
    public ElectroBike(string name, decimal weight, int modelYear) : base(name)
    {
        Weight = weight;
        ModelYear = modelYear;
    }

    public decimal Weight { get; }
    public int ModelYear { get; }
}
