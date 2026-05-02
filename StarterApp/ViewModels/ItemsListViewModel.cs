using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using StarterApp.Database.Models;
using StarterApp.Database.Data;
using StarterApp.Services;
using StarterApp.Views;

namespace StarterApp.ViewModels;

public class ItemsListViewModel : ObservableObject
{
    private readonly IApiService? _apiService;
    private readonly IItemRepository? _itemRepository;
    private ObservableCollection<Item> _items = new();

    public ObservableCollection<Item> Items

    
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public IAsyncRelayCommand LoadItemsCommand { get; }
    public IAsyncRelayCommand GoToCreateItemCommand { get; }
    public IAsyncRelayCommand<Item> ViewItemCommand { get; }

    public ItemsListViewModel(IApiService apiService)
    {
        _apiService = apiService;
        LoadItemsCommand = new AsyncRelayCommand(LoadItemsAsync);
        GoToCreateItemCommand = new AsyncRelayCommand(GoToCreateItemAsync);
        ViewItemCommand = new AsyncRelayCommand<Item>(ViewItemAsync);
        _ = LoadItemsCommand.ExecuteAsync(null);
        GoToRentalsCommand = new AsyncRelayCommand(GoToRentalsAsync);
    }

    public ItemsListViewModel(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
        LoadItemsCommand = new AsyncRelayCommand(LoadItemsAsync);
        GoToCreateItemCommand = new AsyncRelayCommand(GoToCreateItemAsync);
        ViewItemCommand = new AsyncRelayCommand<Item>(ViewItemAsync);
        _ = LoadItemsCommand.ExecuteAsync(null);
        GoToRentalsCommand = new AsyncRelayCommand(GoToRentalsAsync);
    }

    private async Task LoadItemsAsync()
{
    try
    {
        List<Item> items;

        if (_apiService != null)
            items = await _apiService.GetItemsAsync();
        else
            items = await _itemRepository!.GetAllAsync();

        Items = new ObservableCollection<Item>(items);
    }
    catch (Exception ex)
    {
        await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
    }
}
    private async Task ViewItemAsync(Item item)
{
    try
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "Item", item }
        };
        await Shell.Current.GoToAsync(nameof(ItemDetailPage), navigationParameter);
    }
    catch (Exception ex)
    {
        await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
    }
}

    private async Task GoToCreateItemAsync()
    {
        await Shell.Current.GoToAsync(nameof(CreateItemPage));
    }
    public IAsyncRelayCommand GoToRentalsCommand { get; }




private async Task GoToRentalsAsync()
{
    try
    {
        await Shell.Current.GoToAsync(nameof(RentalsPage));
    }
    catch (Exception ex)
    {
        await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
    }}
}