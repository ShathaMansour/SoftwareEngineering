using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Database.Data;
using StarterApp.Services;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(Item), "Item")]
public partial class ItemDetailViewModel : ObservableObject
{
    private readonly IAuthenticationService _authService;
    private readonly IApiService? _apiService;
    private readonly IItemRepository? _itemRepository;
    private readonly ICategoryRepository? _categoryRepository;
    private readonly IRentalService _rentalService;

    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private string category = string.Empty;
    [ObservableProperty] private decimal dailyRate;
    [ObservableProperty] private bool isOwner;
    [ObservableProperty] private bool isEditing;
    [ObservableProperty] private string statusMessage = string.Empty;
    [ObservableProperty] private DateTime startDate = DateTime.Today;
    [ObservableProperty] private DateTime endDate = DateTime.Today.AddDays(1);

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

    private Item? _item;
    public Item? Item
    {
        get => _item;
        set
        {
            _item = value;
            if (value != null)
            {
                Title = value.Title;
                Description = value.Description;
                Category = value.Category;
                DailyRate = value.DailyRate;
                IsOwner = _authService.CurrentUser?.Id == value.OwnerId;
                _ = LoadItemAsync();
            }
        }
    }

    public IAsyncRelayCommand SaveChangesCommand { get; }
    public IRelayCommand ToggleEditCommand { get; }
    public IAsyncRelayCommand RentItemCommand { get; }
    public IAsyncRelayCommand ViewReviewsCommand { get; }

    // API mode constructor
    public ItemDetailViewModel(IAuthenticationService authService, IApiService apiService, IRentalService rentalService)
    {
        _authService = authService;
        _apiService = apiService;
        _rentalService = rentalService;
        SaveChangesCommand = new AsyncRelayCommand(SaveChangesAsync);
        ToggleEditCommand = new RelayCommand(ToggleEdit);
        RentItemCommand = new AsyncRelayCommand(RentItemAsync);
        ViewReviewsCommand = new AsyncRelayCommand(ViewReviewsAsync);
        _ = LoadCategoriesAsync();
    }

    // Local DB mode constructor
    public ItemDetailViewModel(IAuthenticationService authService, IItemRepository itemRepository, ICategoryRepository categoryRepository, IRentalService rentalService)
    {
        _authService = authService;
        _itemRepository = itemRepository;
        _categoryRepository = categoryRepository;
        _rentalService = rentalService;
        SaveChangesCommand = new AsyncRelayCommand(SaveChangesAsync);
        ToggleEditCommand = new RelayCommand(ToggleEdit);
        RentItemCommand = new AsyncRelayCommand(RentItemAsync);
        ViewReviewsCommand = new AsyncRelayCommand(ViewReviewsAsync);
        _ = LoadCategoriesAsync();
    }

    private async Task ViewReviewsAsync()
    {
        if (_item == null) return;
        await Shell.Current.GoToAsync("ReviewsPage", new Dictionary<string, object>
        {
            { "Item", _item }
        });
    }

    private async Task LoadCategoriesAsync()
    {
        if (_apiService != null)
            Categories = await _apiService.GetCategoriesAsync();
        else
            Categories = await _categoryRepository!.GetAllAsync();

        if (_item != null)
            SelectedCategory = Categories.FirstOrDefault(c => c.Name == _item.Category);
    }

    private async Task LoadItemAsync()
    {
        if (_item == null) return;
        try
        {
            Item? freshItem;
            if (_apiService != null)
                freshItem = await _apiService.GetItemByIdAsync(_item.Id);
            else
                freshItem = await _itemRepository!.GetByIdAsync(_item.Id);

            if (freshItem != null)
                Item = freshItem;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to reload item: {ex.Message}";
        }
    }

    private void ToggleEdit() => IsEditing = !IsEditing;

    private async Task SaveChangesAsync()
    {
        if (_item == null) return;
        try
        {
            if (SelectedCategory == null)
            {
                StatusMessage = "Please select a category.";
                return;
            }
            _item.Title = Title;
            _item.Description = Description;
            _item.DailyRate = DailyRate;
            _item.Category = SelectedCategory.Name;
            _item.CategoryId = SelectedCategory.Id;

            if (_apiService != null)
                await _apiService.UpdateItemAsync(_item);
            else
                await _itemRepository!.UpdateAsync(_item);

            Title = _item.Title;
            Description = _item.Description;
            DailyRate = _item.DailyRate;
            Category = _item.Category;
            IsEditing = false;
            StatusMessage = "Item updated successfully!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Update failed: {ex.Message}";
        }
    }

    private async Task RentItemAsync()
    {
        if (_item == null) return;
        if (EndDate <= StartDate)
        {
            StatusMessage = "End date must be after start date.";
            return;
        }
        try
        {
            await _rentalService.CreateRentalAsync(_item.Id, StartDate, EndDate);
            StatusMessage = "Rental requested successfully!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to request rental: {ex.Message}";
        }
    }
}