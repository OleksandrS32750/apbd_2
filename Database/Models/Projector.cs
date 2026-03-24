using APBD_2.Database.Interfaces;

namespace APBD_2.Database.Models;

public sealed class Projector : Equipment
{
    public Projector(int id, string name, int lumens, bool supports4K) : base(id, name)
    {
        Lumens = lumens;
        Supports4K = supports4K;
    }

    public int Lumens { get; }
    public bool Supports4K { get; }
}
