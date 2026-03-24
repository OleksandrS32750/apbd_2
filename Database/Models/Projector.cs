using APBD_2.Database.Abstract;

namespace APBD_2.Database.Models;

public sealed class Projector : Equipment
{
    public Projector(string name, int lumens, bool supports4K) : base(name)
    {
        Lumens = lumens;
        Supports4K = supports4K;
    }

    public int Lumens { get; }
    public bool Supports4K { get; }
}
