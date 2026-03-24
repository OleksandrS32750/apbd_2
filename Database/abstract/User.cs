using APBD_2.Database.Enums;

namespace APBD_2.Database.Abstract;

public abstract class User
{
    private static int _nextId = 1;

    protected User(string firstName, string lastName)
    {
        Id = _nextId++;
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
