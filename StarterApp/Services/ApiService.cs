using StarterApp.Database.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StarterApp.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    
    private static readonly System.Text.Json.JsonSerializerOptions _jsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Items
    public async Task<List<Item>> GetItemsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ItemsResponse>("items", _jsonOptions);
        return response?.Items ?? new List<Item>();
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Item>($"items/{id}", _jsonOptions);
    }

    public async Task<List<Item>> GetNearbyItemsAsync(double lat, double lon, double radiusKm)
    {
        var response = await _httpClient.GetFromJsonAsync<NearbyItemsResponse>(
            $"items/nearby?lat={lat}&lon={lon}&radius={radiusKm}", _jsonOptions);
        return response?.Items ?? new List<Item>();
    }

    public async Task<Item> CreateItemAsync(Item item)
    {
        var response = await _httpClient.PostAsJsonAsync("items", new
        {
            title = item.Title,
            description = item.Description,
            dailyRate = item.DailyRate,
            categoryId = item.CategoryId,
            latitude = item.Latitude,
            longitude = item.Longitude
        });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<Item>(_jsonOptions))!;
    }

    public async Task UpdateItemAsync(Item item)
    {
        var response = await _httpClient.PutAsJsonAsync($"items/{item.Id}", new
        {
            title = item.Title,
            description = item.Description,
            dailyRate = item.DailyRate,
            isAvailable = item.IsAvailable
        });
        response.EnsureSuccessStatusCode();
    }
    public async Task<List<Category>> GetCategoriesAsync()
{
    var response = await _httpClient.GetFromJsonAsync<CategoriesResponse>(
        "categories", _jsonOptions);
    return response?.Categories ?? new List<Category>();
}

    private record CategoriesResponse(List<Category> Categories);

    // --- DTOs ---
    private record ItemsResponse(List<Item> Items, int TotalItems, int Page, int PageSize, int TotalPages);
    private record NearbyItemsResponse(List<Item> Items, int TotalResults);
    public async Task<Rental> CreateRentalAsync(int itemId, DateTime startDate, DateTime endDate)
{
    var response = await _httpClient.PostAsJsonAsync("rentals", new
    {
        itemId = itemId,
        startDate = startDate.ToString("yyyy-MM-dd"),
        endDate = endDate.ToString("yyyy-MM-dd")
    });

    if (!response.IsSuccessStatusCode)
    {
        var raw = await response.Content.ReadAsStringAsync();
        throw new Exception(raw);
    }

    return (await response.Content.ReadFromJsonAsync<Rental>(_jsonOptions))!;
}

public async Task<List<Rental>> GetIncomingRentalsAsync()
{
    var response = await _httpClient.GetFromJsonAsync<RentalsResponse>("rentals/incoming", _jsonOptions);
    return response?.Rentals ?? new List<Rental>();
}

public async Task<List<Rental>> GetOutgoingRentalsAsync()
{
    var response = await _httpClient.GetFromJsonAsync<RentalsResponse>("rentals/outgoing", _jsonOptions);
    return response?.Rentals ?? new List<Rental>();
}

public async Task UpdateRentalStatusAsync(int rentalId, string status)
{
    var response = await _httpClient.PatchAsJsonAsync($"rentals/{rentalId}/status", new
    {
        status = status
    });

    if (!response.IsSuccessStatusCode)
    {
        var raw = await response.Content.ReadAsStringAsync();
        throw new Exception(raw);
    }
}

private record RentalsResponse(List<Rental> Rentals, int TotalRentals);

// Reviews
public async Task<List<Review>> GetItemReviewsAsync(int itemId)
{
    var response = await _httpClient.GetFromJsonAsync<ReviewsResponse>(
        $"items/{itemId}/reviews", _jsonOptions);
    return response?.Reviews ?? new List<Review>();
}

public async Task<List<Review>> GetUserReviewsAsync(int userId)
{
    var response = await _httpClient.GetFromJsonAsync<ReviewsResponse>(
        $"users/{userId}/reviews", _jsonOptions);
    return response?.Reviews ?? new List<Review>();
}

public async Task<Review> SubmitReviewAsync(int rentalId, int rating, string? comment)
{
    var response = await _httpClient.PostAsJsonAsync("reviews", new
    {
        rentalId = rentalId,
        rating = rating,
        comment = comment
    });

    if (!response.IsSuccessStatusCode)
    {
        var raw = await response.Content.ReadAsStringAsync();
        throw new Exception(raw);
    }

    return (await response.Content.ReadFromJsonAsync<Review>(_jsonOptions))!;
}

private record ReviewsResponse(List<Review> Reviews, double AverageRating, int TotalReviews);
}