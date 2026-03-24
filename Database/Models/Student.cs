using APBD_2.Database.Enums;
using APBD_2.Database.Interfaces;

namespace APBD_2.Database.Models;

public sealed class Student : User
{
    public Student(int id, string firstName, string lastName) : base(id, firstName, lastName)
    {
    }

    public override UserType UserType => UserType.Student;
    public override int RentalLimit => 2;
}
