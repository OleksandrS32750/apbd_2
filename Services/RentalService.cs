using APBD_2.Database.Enums;
using APBD_2.Database.Abstract;
using APBD_2.Database.Models;
using APBD_2.Services.Interfaces;

namespace APBD_2.Services;

public sealed class RentalService
{
    private readonly List<User> _users = [];
    private readonly List<Equipment> _equipment = [];
    private readonly List<Rental> _rentals = [];
    private readonly IPenaltyPolicy _penaltyPolicy;
    private int _nextRentalId = 1;

    public RentalService(IPenaltyPolicy penaltyPolicy)
    {
        _penaltyPolicy = penaltyPolicy;
    }

    public IReadOnlyList<User> Users => _users;
    public IReadOnlyList<Equipment> EquipmentItems => _equipment;
    public IReadOnlyList<Rental> Rentals => _rentals;

    public void AddUser(User user)
    {   
        ArgumentNullException.ThrowIfNull(user);

        if (_users.Any(u => u.Id == user.Id))
        {
            throw new InvalidOperationException($"user with id {user.Id} already exists");
        }

        _users.Add(user);
    }

    public void AddEquipment(Equipment equipment)
    {
        ArgumentNullException.ThrowIfNull(equipment);

        if (_equipment.Any(e => e.Id == equipment.Id))
        {
            throw new InvalidOperationException($"equipment with id {equipment.Id} already exists");
        }

        _equipment.Add(equipment);
    }

    public IReadOnlyList<Equipment> GetAvailableEquipment() =>
        _equipment.Where(e => e.IsAvailableForRental).ToList();

    public Rental RentEquipment(int userId, int equipmentId, int days, DateTime rentalDate)
    {
        var user = GetUser(userId);
        var item = GetEquipment(equipmentId);

        if (!item.IsAvailableForRental)
        {
            throw new InvalidOperationException($"{item.Name} is not available");
        }

        var activeRentalsForUser = _rentals.Count(r => r.IsActive && r.User.Id == userId);
        if (activeRentalsForUser >= user.RentalLimit)
        {
            throw new InvalidOperationException(
                $"user has exceeded his rental limit: ({user.RentalLimit})");
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
        var rental = GetRental(rentalId);

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
        var available = _equipment.Count(e => e.Status == EquipmentStatus.Available);
        var unavailable = _equipment.Count(e => e.Status == EquipmentStatus.Unavailable);
        var totalPenalties = _rentals.Sum(r => r.Penalty);

        return
            $"users using the service: {_users.Count}\n" +
            $"total equipment (any status): {_equipment.Count}\n" +
            $"equipment avaialble: {available}\n" +
            $"equipment unavaialble: {unavailable}\n" +
            $"active rentals: {active}\n" +
            $"recieved penalties: {totalPenalties}$";
    }

    private User GetUser(int userId) =>
        FindUserById(userId) ?? throw new InvalidOperationException("user not found");

    private Equipment GetEquipment(int equipmentId) =>
        FindEquipmentById(equipmentId) ?? throw new InvalidOperationException("eqipment not found");

    private Rental GetRental(int rentalId) =>
        FindRentalById(rentalId) ?? throw new InvalidOperationException("rental not found");

    private User? FindUserById(int userId)
    {
        for (var i = 0; i < _users.Count; i++)
        {
            if (_users[i].Id == userId)
            {
                return _users[i];
            }
        }

        return null;
    }

    private Equipment? FindEquipmentById(int equipmentId)
    {
        for (var i = 0; i < _equipment.Count; i++)
        {
            if (_equipment[i].Id == equipmentId)
            {
                return _equipment[i];
            }
        }

        return null;
    }

    private Rental? FindRentalById(int rentalId)
    {
        for (var i = 0; i < _rentals.Count; i++)
        {
            if (_rentals[i].Id == rentalId)
            {
                return _rentals[i];
            }
        }

        return null;
    }
}
