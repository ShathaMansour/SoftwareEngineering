using Microsoft.Extensions.Logging;
using StarterApp.ViewModels;
using StarterApp.Database.Data;
using StarterApp.Views;
using StarterApp.Services;

namespace StarterApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        const bool useSharedApi = true; 

        if (useSharedApi)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://set09102-api.b-davison.workers.dev/")
            };
            builder.Services.AddSingleton(httpClient);
            builder.Services.AddSingleton<IAuthenticationService, ApiAuthenticationService>();
            builder.Services.AddSingleton<IApiService, ApiService>();
            builder.Services.AddTransient<IRentalService, RentalService>();
            builder.Services.AddTransient<IReviewService, ReviewService>();
            builder.Services.AddTransient<ReviewsViewModel>();
            builder.Services.AddTransient<ReviewsPage>();
            builder.Services.AddSingleton<ILocationService, LocationService>();
            builder.Services.AddTransient<NearbyItemsViewModel>();
            builder.Services.AddTransient<NearbyItemsPage>();
        }
        else
        {
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddTransient<IItemRepository, ItemRepository>();
            builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
            builder.Services.AddTransient<IRentalRepository, RentalRepository>();
            builder.Services.AddTransient<IRentalService, RentalService>();
            builder.Services.AddSingleton<ILocationService, LocationService>();
        }

        // --- Services (both modes) ---
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        // --- Shell & App ---
        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<App>();

        // --- Pages & ViewModels (both modes) ---
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<UserListViewModel>();
        builder.Services.AddTransient<UserListPage>();
        builder.Services.AddTransient<UserDetailPage>();
        builder.Services.AddTransient<UserDetailViewModel>();
        builder.Services.AddSingleton<TempViewModel>();
        builder.Services.AddTransient<TempPage>();
        builder.Services.AddTransient<ItemsListViewModel>();
        builder.Services.AddTransient<ItemsListPage>();
        builder.Services.AddTransient<CreateItemViewModel>();
        builder.Services.AddTransient<CreateItemPage>();
        builder.Services.AddTransient<ItemDetailViewModel>();
        builder.Services.AddTransient<ItemDetailPage>();
        builder.Services.AddTransient<RentalsViewModel>();
        builder.Services.AddTransient<RentalsPage>();
        builder.Services.AddTransient<IReviewService, ReviewService>();
        builder.Services.AddTransient<ReviewsViewModel>();
        builder.Services.AddTransient<ReviewsPage>();
        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<ProfilePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}