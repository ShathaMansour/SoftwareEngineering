using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public class CreateItemViewModel : ObservableObject
{
    private readonly IApiService? _apiService;
    private readonly IItemRepository? _itemRepository;
    private readonly ICategoryRepository? _categoryRepository;
    private readonly ILocationService _locationService;

    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private decimal _dailyRate;
    public decimal DailyRate
    {
        get => _dailyRate;
        set => SetProperty(ref _dailyRate, value);
    }

    private string _statusMessage = string.Empty;
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    private double _latitude;
    public double Latitude
    {
        get => _latitude;
        set => SetProperty(ref _latitude, value);
    }

    private double _longitude;
    public double Longitude
    {
        get => _longitude;
        set => SetProperty(ref _longitude, value);
    }

    private bool _hasLocation;
    public bool HasLocation
    {
        get => _hasLocation;
        set => SetProperty(ref _hasLocation, value);
    }

    private List<Category> _categories = new();
    public List<Category> Categories
    {
        get => _categories;
        private set => SetProperty(ref _categories, value);
    }

    private Category? _selectedCategory;
    public Category? SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    public IAsyncRelayCommand CreateItemCommand { get; }
    public IAsyncRelayCommand UseMyLocationCommand { get; }

    // API mode
    public CreateItemViewModel(IApiService apiService, ILocationService locationService)
    {
        _apiService = apiService;
        _locationService = locationService;
        CreateItemCommand = new AsyncRelayCommand(CreateItemAsync);
        UseMyLocationCommand = new AsyncRelayCommand(UseMyLocationAsync);
        _ = LoadCategoriesAsync();
    }

    // Local DB mode
    public CreateItemViewModel(IItemRepository itemRepository, ICategoryRepository categoryRepository, ILocationService locationService)
    {
        _itemRepository = itemRepository;
        _categoryRepository = categoryRepository;
        _locationService = locationService;
        CreateItemCommand = new AsyncRelayCommand(CreateItemAsync);
        UseMyLocationCommand = new AsyncRelayCommand(UseMyLocationAsync);
        _ = LoadCategoriesAsync();
    }

    private async Task LoadCategoriesAsync()
    {
        if (_apiService != null)
            Categories = await _apiService.GetCategoriesAsync();
        else
            Categories = await _categoryRepository!.GetAllAsync();
    }

    private async Task UseMyLocationAsync()
    {
        StatusMessage = "Getting location...";
        var location = await _locationService.GetCurrentLocationAsync();
        if (location == null)
        {
            StatusMessage = "Could not get location. Please allow location access.";
            return;
        }
        Latitude = location.Value.Latitude;
        Longitude = location.Value.Longitude;
        HasLocation = true;
        StatusMessage = "Location set!";
    }

    private async Task CreateItemAsync()
    {
        try
        {
            if (SelectedCategory == null)
            {
                StatusMessage = "Please select a category.";
                return;
            }

            if (!HasLocation)
            {
                StatusMessage = "Please set your location.";
                return;
            }

            var item = new Item
            {
                Title = Title,
                Description = Description,
                DailyRate = DailyRate,
                CategoryId = SelectedCategory.Id,
                Category = SelectedCategory.Name,
                Latitude = Latitude,
                Longitude = Longitude
            };

            if (_apiService != null)
                await _apiService.CreateItemAsync(item);
            else
                await _itemRepository!.CreateAsync(item);

            Title = string.Empty;
            Description = string.Empty;
            DailyRate = 0;
            SelectedCategory = null;
            HasLocation = false;
            Latitude = 0;
            Longitude = 0;
            StatusMessage = "Item created successfully!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed: {ex.Message}";
        }
    }
}