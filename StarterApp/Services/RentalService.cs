using StarterApp.Database.Models;
using StarterApp.Database.Data;

namespace StarterApp.Services;

public class RentalService : IRentalService
{
    private readonly IApiService? _apiService;
    private readonly IRentalRepository? _rentalRepository;
    private readonly IAuthenticationService _authService;

    // Valid status transitions and who can perform them
    private readonly Dictionary<(string From, string To), Func<Rental, int, bool>> _validTransitions = new()
    {
        { ("Requested", "Approved"),    (rental, userId) => rental.OwnerId == userId },
        { ("Requested", "Rejected"),    (rental, userId) => rental.OwnerId == userId },
        { ("Approved", "Out for Rent"), (rental, userId) => rental.OwnerId == userId },
        { ("Out for Rent", "Returned"), (rental, userId) => rental.BorrowerId == userId },
        { ("Overdue", "Returned"),      (rental, userId) => rental.BorrowerId == userId },
        { ("Returned", "Completed"),    (rental, userId) => rental.OwnerId == userId }
    };

    // API mode
    public RentalService(IApiService apiService, IAuthenticationService authService)
    {
        _apiService = apiService;
        _authService = authService;
    }

    // Local DB mode
    public RentalService(IRentalRepository rentalRepository, IAuthenticationService authService)
    {
        _rentalRepository = rentalRepository;
        _authService = authService;
    }

    public async Task<List<Rental>> GetIncomingRentalsAsync()
    {
        if (_apiService != null)
            return await _apiService.GetIncomingRentalsAsync();

        var userId = _authService.CurrentUser?.Id ?? 0;
        return await _rentalRepository!.GetIncomingAsync(userId);
    }

    public async Task<List<Rental>> GetOutgoingRentalsAsync()
    {
        if (_apiService != null)
            return await _apiService.GetOutgoingRentalsAsync();

        var userId = _authService.CurrentUser?.Id ?? 0;
        return await _rentalRepository!.GetOutgoingAsync(userId);
    }

    public async Task<Rental> CreateRentalAsync(int itemId, DateTime startDate, DateTime endDate)
    {
        if (_apiService != null)
            return await _apiService.CreateRentalAsync(itemId, startDate, endDate);

        var rental = new Rental
        {
            ItemId = itemId,
            BorrowerId = _authService.CurrentUser?.Id ?? 0,
            StartDate = startDate,
            EndDate = endDate,
            Status = "Requested",
            RequestedAt = DateTime.UtcNow
        };
        return await _rentalRepository!.CreateAsync(rental);
    }

    public async Task UpdateRentalStatusAsync(int rentalId, string status)
    {
        if (_apiService != null)
        {
            await _apiService.UpdateRentalStatusAsync(rentalId, status);
            return;
        }
        await _rentalRepository!.UpdateStatusAsync(rentalId, status);
    }

    public async Task<bool> CanTransitionAsync(Rental rental, string newStatus)
    {
        var userId = _authService.CurrentUser?.Id ?? 0;
        var key = (rental.Status, newStatus);
        return await Task.FromResult(
            _validTransitions.TryGetValue(key, out var check) && check(rental, userId));
    }
}