using APBD_2.Database.Models;

namespace APBD_2.Services;

public sealed class SimplePenaltyPolicy : IPenaltyPolicy
{
    private readonly decimal _dailyPenalty;

    public SimplePenaltyPolicy(decimal dailyPenalty = 15m)
    {
        _dailyPenalty = dailyPenalty;
    }

    public decimal CalculatePenalty(Rental rental, DateTime returnDate)
    {
        if (returnDate.Date <= rental.DueDate.Date)
        {
            return 0m;
        }

        var daysLate = (returnDate.Date - rental.DueDate.Date).Days;

        // +1 , because any delay (1min,1hour,etc...) is counted as 1 day
        return daysLate * (_dailyPenalty + 1);
    }
}
