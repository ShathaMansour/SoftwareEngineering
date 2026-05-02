using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IApiService
{    
    // Items
    Task<List<Item>> GetItemsAsync();
    Task<Item?> GetItemByIdAsync(int id);
    Task<Item> CreateItemAsync(Item item);
    Task UpdateItemAsync(Item item);
    Task<List<Item>> GetNearbyItemsAsync(double lat, double lon, double radiusKm);
    Task<List<Category>> GetCategoriesAsync();
    // Rentals
Task<Rental> CreateRentalAsync(int itemId, DateTime startDate, DateTime endDate);
Task<List<Rental>> GetIncomingRentalsAsync();
Task<List<Rental>> GetOutgoingRentalsAsync();
Task UpdateRentalStatusAsync(int rentalId, string status);
}