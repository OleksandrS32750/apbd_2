using APBD_2.Database.Abstract;

namespace APBD_2.Database.Models;

public sealed class Camera : Equipment
{
    public Camera(string name, bool mirrorless, int megapixels) : base(name)
    {
        Mirrorless = mirrorless;
        Megapixels = megapixels;
    }

    public bool Mirrorless { get; }
    public int Megapixels { get; }
}
