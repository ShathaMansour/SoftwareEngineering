namespace StarterApp.Views;

using StarterApp.ViewModels;

public partial class ReviewsPage : ContentPage
{
    public ReviewsPage(ReviewsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}