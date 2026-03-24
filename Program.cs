using APBD_2.Services;
using APBD_2.Database.Models;

var service = new RentalService(new SimplePenaltyPolicy(dailyPenalty: 20m));
var today = DateTime.Today;

var laptop = new Laptop("Dell Laptop", ramGb: 16, storageGb: 256);
var projector = new Projector("Projector1", lumens: 999, supports4K: false);
var camera = new Camera("Canon Camera", mirrorless: true, megapixels: 5);

service.AddEquipment(laptop);
service.AddEquipment(projector);
service.AddEquipment(camera);


service.MarkEquipmentUnavailable(camera.Id);

var student = new Student("Student", "One");
var employee = new Employee("Emp", "One");
var emp2 = new Employee("Emp", "Two");

service.AddUser(student);
service.AddUser(employee);
service.AddUser(emp2);

var rental1 = service.RentEquipment(student.Id, laptop.Id, days: 3, rentalDate: today);
Console.WriteLine($"Rental created: #{rental1.Id} -> {rental1.User.FullName} rented {rental1.Equipment.Name}");

// Try to rent unavailable equipment => fail
TryAction("Rent unavailable equipment", () =>
{
    service.RentEquipment(employee.Id, camera.Id, days: 2, rentalDate: today);
});

var onTimePenalty = service.ReturnEquipment(rental1.Id, today.AddDays(3));
Console.WriteLine($"On-time return penalty: {onTimePenalty:C}");

var rental2 = service.RentEquipment(employee.Id, projector.Id, days: 2, rentalDate: today);
var latePenalty = service.ReturnEquipment(rental2.Id, today.AddDays(5));
Console.WriteLine($"Late return penalty: {latePenalty:C}");

Console.WriteLine("\nAll equipment:");
foreach (var item in service.EquipmentItems)
{
    Console.WriteLine($"#{item.Id} {item.Name} [{item.GetType().Name}] - {item.Status}");
}

Console.WriteLine("\nAvailable equipment:");
foreach (var item in service.GetAvailableEquipment())
{
    Console.WriteLine($"#{item.Id} {item.Name}");
}

Console.WriteLine($"\nActive rentals for {employee.FullName}:");
foreach (var active in service.GetActiveRentalsForUser(employee.Id))
{
    Console.WriteLine($"Rental #{active.Id}: {active.Equipment.Name}, due {active.DueDate:yyyy-MM-dd}");
}

Console.WriteLine("\nOverdue rentals:");
foreach (var overdue in service.GetOverdueRentals(today.AddDays(10)))
{
    Console.WriteLine($"Rental #{overdue.Id}: {overdue.User.FullName} -> {overdue.Equipment.Name}");
}

Console.WriteLine("\nFinal summary report:");
Console.WriteLine(service.BuildSummaryReport(today.AddDays(10)));

static void TryAction(string label, Action action)
{
    try
    {
        action();
        Console.WriteLine($"{label}: OK");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{label}: BLOCKED ({ex.Message})");
    }
}
