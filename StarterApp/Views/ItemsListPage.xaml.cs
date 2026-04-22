using StarterApp.ViewModels;

namespace StarterApp.Views;
public partial class ItemsListPage : ContentPage
{
    private readonly ItemsListViewModel _vm;

    public ItemsListPage(ItemsListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadItemsCommand.ExecuteAsync(null);
    }
}