using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using StarterApp.Database.Models;
using StarterApp.Services;
using StarterApp.Views;

namespace StarterApp.ViewModels;

public class RentalsViewModel : ObservableObject
{
    private readonly IRentalService _rentalService;
    public Color IncomingButtonColor => IsIncomingSelected ? Colors.Blue : Colors.Grey;
    public Color OutgoingButtonColor => IsIncomingSelected ? Colors.Grey : Colors.Blue;

    private ObservableCollection<Rental> _incomingRentals = new();
    public ObservableCollection<Rental> IncomingRentals
    {
        get => _incomingRentals;
        set => SetProperty(ref _incomingRentals, value);
    }

    private ObservableCollection<Rental> _outgoingRentals = new();
    public ObservableCollection<Rental> OutgoingRentals
    {
        get => _outgoingRentals;
        set => SetProperty(ref _outgoingRentals, value);
    }

    private string _statusMessage = string.Empty;
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    private bool _isIncomingSelected = true;
    public bool IsIncomingSelected
    {
        get => _isIncomingSelected;
        set
        {
            SetProperty(ref _isIncomingSelected, value);
            OnPropertyChanged(nameof(IncomingButtonColor));
            OnPropertyChanged(nameof(OutgoingButtonColor));
        }
    }

    public IAsyncRelayCommand LoadRentalsCommand { get; }
    public IAsyncRelayCommand<Rental> ApproveRentalCommand { get; }
    public IAsyncRelayCommand<Rental> RejectRentalCommand { get; }
    public IAsyncRelayCommand<Rental> MarkReturnedCommand { get; }
    public IAsyncRelayCommand<Rental> MarkCompletedCommand { get; }
    public IAsyncRelayCommand<Rental> LeaveReviewCommand { get; }
    public IAsyncRelayCommand<Rental> MarkOutForRentCommand { get; }
    public IRelayCommand ShowIncomingCommand { get; }
    public IRelayCommand ShowOutgoingCommand { get; }

    public RentalsViewModel(IRentalService rentalService)
    {
        _rentalService = rentalService;
        LoadRentalsCommand = new AsyncRelayCommand(LoadRentalsAsync);
        ApproveRentalCommand = new AsyncRelayCommand<Rental>(ApproveRentalAsync);
        RejectRentalCommand = new AsyncRelayCommand<Rental>(RejectRentalAsync);
        MarkOutForRentCommand = new AsyncRelayCommand<Rental>(MarkOutForRentAsync);
        MarkReturnedCommand = new AsyncRelayCommand<Rental>(MarkReturnedAsync);
        MarkCompletedCommand = new AsyncRelayCommand<Rental>(MarkCompletedAsync);
        LeaveReviewCommand = new AsyncRelayCommand<Rental>(LeaveReviewAsync);
        ShowIncomingCommand = new RelayCommand(() => IsIncomingSelected = true);
        ShowOutgoingCommand = new RelayCommand(() => IsIncomingSelected = false);
        _ = LoadRentalsAsync();
    }

    private async Task LoadRentalsAsync()
    {
        try
        {
            var incoming = await _rentalService.GetIncomingRentalsAsync();
            var outgoing = await _rentalService.GetOutgoingRentalsAsync();
            IncomingRentals = new ObservableCollection<Rental>(incoming);
            OutgoingRentals = new ObservableCollection<Rental>(outgoing);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to load rentals: {ex.Message}";
        }
    }

    private async Task MarkOutForRentAsync(Rental? rental)
    {
        if (rental == null) return;
        try
        {
            await _rentalService.UpdateRentalStatusAsync(rental.Id, "Out for Rent");
            StatusMessage = "Marked as out for rent.";
            await LoadRentalsAsync();
        }
        catch (Exception ex) { StatusMessage = $"Failed: {ex.Message}"; }
    }

    private async Task MarkReturnedAsync(Rental? rental)
    {
        if (rental == null) return;
        try
        {
            await _rentalService.UpdateRentalStatusAsync(rental.Id, "Returned");
            StatusMessage = "Marked as returned.";
            await LoadRentalsAsync();
        }
        catch (Exception ex) { StatusMessage = $"Failed: {ex.Message}"; }
    }

    private async Task MarkCompletedAsync(Rental? rental)
    {
        if (rental == null) return;
        try
        {
            await _rentalService.UpdateRentalStatusAsync(rental.Id, "Completed");
            StatusMessage = "Rental completed!";
            await LoadRentalsAsync();
        }
        catch (Exception ex) { StatusMessage = $"Failed: {ex.Message}"; }
    }

    private async Task ApproveRentalAsync(Rental? rental)
    {
        if (rental == null) return;
        try
        {
            await _rentalService.UpdateRentalStatusAsync(rental.Id, "Approved");
            StatusMessage = "Rental approved!";
            await LoadRentalsAsync();
        }
        catch (Exception ex) { StatusMessage = $"Failed: {ex.Message}"; }
    }

    private async Task RejectRentalAsync(Rental? rental)
    {
        if (rental == null) return;
        try
        {
            await _rentalService.UpdateRentalStatusAsync(rental.Id, "Rejected");
            StatusMessage = "Rental rejected.";
            await LoadRentalsAsync();
        }
        catch (Exception ex) { StatusMessage = $"Failed: {ex.Message}"; }
    }

    private async Task LeaveReviewAsync(Rental? rental)
    {
        if (rental == null) return;
        await Shell.Current.GoToAsync(nameof(ReviewsPage), new Dictionary<string, object>
        {
            { "Rental", rental }
        });
    }
}