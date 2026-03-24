using APBD_2.Database.Enums;
using APBD_2.Database.Interfaces;
using APBD_2.Database.Models;

namespace APBD_2.Services;

public sealed class RentalService
{
    private readonly List<User> _users = [];
    private readonly List<Equipment> _equipment = [];
    private readonly List<Rental> _rentals = [];
    private readonly IPenaltyPolicy _penaltyPolicy;
    private int _nextUserId = 1;
    private int _nextEquipmentId = 1;
    private int _nextRentalId = 1;

    public RentalService(IPenaltyPolicy penaltyPolicy)
    {
        _penaltyPolicy = penaltyPolicy;
    }

    public IReadOnlyList<User> Users => _users;
    public IReadOnlyList<Equipment> EquipmentItems => _equipment;
    public IReadOnlyList<Rental> Rentals => _rentals;

    public Student AddStudent(string firstName, string lastName)
    {
        var student = new Student(_nextUserId++, firstName, lastName);
        _users.Add(student);
        return student;
    }

    public Employee AddEmployee(string firstName, string lastName)
    {
        var employee = new Employee(_nextUserId++, firstName, lastName);
        _users.Add(employee);
        return employee;
    }

    public Laptop AddLaptop(string name, int ramGb, int storageGb)
    {
        var laptop = new Laptop(_nextEquipmentId++, name, ramGb, storageGb);
        _equipment.Add(laptop);
        return laptop;
    }

    public Projector AddProjector(string name, int lumens, bool supports4K)
    {
        var projector = new Projector(_nextEquipmentId++, name, lumens, supports4K);
        _equipment.Add(projector);
        return projector;
    }

    public Camera AddCamera(string name, bool mirrorless, int megapixels)
    {
        var camera = new Camera(_nextEquipmentId++, name, mirrorless, megapixels);
        _equipment.Add(camera);
        return camera;
    }

    public IReadOnlyList<Equipment> GetAvailableEquipment() =>
        _equipment.Where(e => e.IsAvailableForRental).ToList();

    public Rental RentEquipment(int userId, int equipmentId, int days, DateTime rentalDate)
    {
        var user = GetUser(userId);
        var item = GetEquipment(equipmentId);

        if (!item.IsAvailableForRental)
        {
            throw new InvalidOperationException("Equipment is not available for rental.");
        }

        var activeRentalsForUser = _rentals.Count(r => r.IsActive && r.User.Id == userId);
        if (activeRentalsForUser >= user.RentalLimit)
        {
            throw new InvalidOperationException(
                $"User has exceeded rental limit ({user.RentalLimit}) for type {user.UserType}.");
        }

        var rental = new Rental(
            _nextRentalId++,
            user,
            item,
            rentalDate.Date,
            rentalDate.Date.AddDays(days));

        _rentals.Add(rental);
        item.MarkRented();
        return rental;
    }

    public decimal ReturnEquipment(int rentalId, DateTime returnDate)
    {
        var rental = _rentals.SingleOrDefault(r => r.Id == rentalId)
            ?? throw new InvalidOperationException("Rental not found.");

        if (!rental.IsActive)
        {
            throw new InvalidOperationException("Rental is already closed.");
        }

        var penalty = _penaltyPolicy.CalculatePenalty(rental, returnDate.Date);
        rental.CompleteReturn(returnDate.Date, penalty);
        rental.Equipment.MarkAvailable();
        return penalty;
    }

    public void MarkEquipmentUnavailable(int equipmentId)
    {
        var item = GetEquipment(equipmentId);
        if (item.Status == EquipmentStatus.Rented)
        {
            throw new InvalidOperationException("Cannot mark rented equipment as unavailable.");
        }

        item.MarkUnavailable();
    }

    public IReadOnlyList<Rental> GetActiveRentalsForUser(int userId) =>
        _rentals.Where(r => r.IsActive && r.User.Id == userId).ToList();

    public IReadOnlyList<Rental> GetOverdueRentals(DateTime now) =>
        _rentals.Where(r => r.IsOverdue(now.Date)).ToList();

    public string BuildSummaryReport(DateTime now)
    {
        var active = _rentals.Count(r => r.IsActive);
        var overdue = _rentals.Count(r => r.IsOverdue(now));
        var unavailable = _equipment.Count(e => e.Status == EquipmentStatus.Unavailable);
        var available = _equipment.Count(e => e.Status == EquipmentStatus.Available);
        var totalPenalties = _rentals.Sum(r => r.Penalty);

        return
            $"Users: {_users.Count}\n" +
            $"Equipment total: {_equipment.Count}\n" +
            $"Equipment available: {available}\n" +
            $"Equipment unavailable: {unavailable}\n" +
            $"Active rentals: {active}\n" +
            $"Overdue rentals: {overdue}\n" +
            $"Collected penalties: {totalPenalties:C}";
    }

    private User GetUser(int userId) =>
        _users.SingleOrDefault(u => u.Id == userId)
        ?? throw new InvalidOperationException("User not found.");

    private Equipment GetEquipment(int equipmentId) =>
        _equipment.SingleOrDefault(e => e.Id == equipmentId)
        ?? throw new InvalidOperationException("Equipment not found.");
}
