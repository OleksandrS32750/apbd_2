using APBD_2.Database.Enums;
using APBD_2.Database.Abstract;

namespace APBD_2.Database.Models;

public sealed class Employee : User
{
    public Employee(string firstName, string lastName) : base(firstName, lastName)
    {
    }

    public override UserType UserType => UserType.Employee;
    public override int RentalLimit => 5;
}
