using APBD_2.Database.Enums;
using APBD_2.Database.Abstract;

namespace APBD_2.Database.Models;

public sealed class Student : User
{
    public Student(string firstName, string lastName) : base(firstName, lastName)
    {
    }

    public override UserType UserType => UserType.Student;
    public override int RentalLimit => 2;
}
