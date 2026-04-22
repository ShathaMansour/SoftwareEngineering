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

    // API mode
    public CreateItemViewModel(IApiService apiService)
    {
        _apiService = apiService;
        CreateItemCommand = new AsyncRelayCommand(CreateItemAsync);
        _ = LoadCategoriesAsync();
    }

    // Local DB mode
    public CreateItemViewModel(IItemRepository itemRepository, ICategoryRepository categoryRepository)
    {
        _itemRepository = itemRepository;
        _categoryRepository = categoryRepository;
        CreateItemCommand = new AsyncRelayCommand(CreateItemAsync);
        _ = LoadCategoriesAsync();
    }

    private async Task LoadCategoriesAsync()
    {
        if (_apiService != null)
            Categories = await _apiService.GetCategoriesAsync();
        else
            Categories = await _categoryRepository!.GetAllAsync();
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

            var item = new Item
            {
                Title = Title,
                Description = Description,
                DailyRate = DailyRate,
                CategoryId = SelectedCategory.Id,
                Category = SelectedCategory.Name
            };

            if (_apiService != null)
                await _apiService.CreateItemAsync(item);
            else
                await _itemRepository!.CreateAsync(item);

            // Clear fields
            Title = string.Empty;
            Description = string.Empty;
            DailyRate = 0;
            SelectedCategory = null;
            StatusMessage = "Item created successfully!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed: {ex.Message}";
        }
    }
}