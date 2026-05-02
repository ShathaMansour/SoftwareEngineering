using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IRentalService
{
    Task<List<Rental>> GetIncomingRentalsAsync();
    Task<List<Rental>> GetOutgoingRentalsAsync();
    Task<Rental> CreateRentalAsync(int itemId, DateTime startDate, DateTime endDate);
    Task UpdateRentalStatusAsync(int rentalId, string status);
    Task<bool> CanTransitionAsync(Rental rental, string newStatus);
}