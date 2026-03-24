using APBD_2.Database.Models;

namespace APBD_2.Services.Interfaces;

public interface IPenaltyPolicy
{
    decimal CalculatePenalty(Rental rental, DateTime returnDate) ;
}
