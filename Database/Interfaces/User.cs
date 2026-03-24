using APBD_2.Database.Enums;

namespace APBD_2.Database.Interfaces;

public abstract class User
{
    protected User(int id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string FullName => $"{FirstName} {LastName}";
    public abstract UserType UserType { get; }
    public abstract int RentalLimit { get; }
}
