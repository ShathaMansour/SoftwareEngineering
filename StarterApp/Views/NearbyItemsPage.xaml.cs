namespace StarterApp.Views;

using StarterApp.ViewModels;

public partial class NearbyItemsPage : ContentPage
{
    public NearbyItemsPage(NearbyItemsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}