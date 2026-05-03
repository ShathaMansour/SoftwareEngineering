using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using StarterApp.Database.Models;
using StarterApp.Services;
using StarterApp.Views;

namespace StarterApp.ViewModels;

public class NearbyItemsViewModel : ObservableObject
{
    private readonly IApiService _apiService;
    private readonly ILocationService _locationService;

    private ObservableCollection<Item> _items = new();
    public ObservableCollection<Item> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    private string _statusMessage = string.Empty;
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    private double _radius = 5;
    public double Radius
    {
        get => _radius;
        set => SetProperty(ref _radius, value);
    }

    public IAsyncRelayCommand FindNearMeCommand { get; }
    public IAsyncRelayCommand<Item> ViewItemCommand { get; }

    public NearbyItemsViewModel(IApiService apiService, ILocationService locationService)
    {
        _apiService = apiService;
        _locationService = locationService;
        FindNearMeCommand = new AsyncRelayCommand(FindNearMeAsync);
        ViewItemCommand = new AsyncRelayCommand<Item>(ViewItemAsync);
    }

    private async Task FindNearMeAsync()
    {
        StatusMessage = "Getting your location...";
        var location = await _locationService.GetCurrentLocationAsync();
        if (location == null)
        {
            StatusMessage = "Could not get location. Please allow location access.";
            return;
        }

        try
        {
            StatusMessage = "Searching nearby items...";
            var items = await _apiService.GetNearbyItemsAsync(
                location.Value.Latitude,
                location.Value.Longitude,
                Radius);
            Items = new ObservableCollection<Item>(items);
            StatusMessage = Items.Count == 0 ? "No items found nearby." : string.Empty;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed: {ex.Message}";
        }
    }

    private async Task ViewItemAsync(Item? item)
    {
        if (item == null) return;
        await Shell.Current.GoToAsync(nameof(ItemDetailPage), new Dictionary<string, object>
        {
            { "Item", item }
        });
    }
}