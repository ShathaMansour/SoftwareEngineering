using StarterApp.ViewModels;
using StarterApp.Views;
namespace StarterApp;

public partial class AppShell : Shell
{
	public AppShell(AppShellViewModel viewModel)
	{	
		BindingContext = viewModel;
		InitializeComponent();
		Routing.RegisterRoute(nameof(CreateItemPage), typeof(CreateItemPage));
		Routing.RegisterRoute(nameof(ItemsListPage), typeof(ItemsListPage));
		Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        Routing.RegisterRoute(nameof(RentalsPage), typeof(RentalsPage));
		Routing.RegisterRoute(nameof(ReviewsPage), typeof(ReviewsPage));
	}
}
