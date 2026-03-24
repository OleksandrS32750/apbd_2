using APBD_2.Database.Abstract;

namespace APBD_2.Database.Models;

public sealed class Rental
{
    public Rental(int id, User user, Equipment equipment, DateTime rentalDate, DateTime dueDate)
    {
        Id = id;
        User = user;
        Equipment = equipment;
        RentalDate = rentalDate;
        DueDate = dueDate;
    }

    public int Id { get; }
    public User User { get; }
    public Equipment Equipment { get; }
    public DateTime RentalDate { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnDate { get; private set; }
    public decimal Penalty { get; private set; }
    public bool IsActive => ReturnDate is null;

    // return of rental item is delayed
    public bool IsOverdue(DateTime now) => IsActive && now.Date > DueDate.Date;

    public void CompleteReturn(DateTime returnDate, decimal penalty)
    {
        ReturnDate = returnDate;
        Penalty = penalty;
    }
}
