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
}