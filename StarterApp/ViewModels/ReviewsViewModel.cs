using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(Item), "Item")]
[QueryProperty(nameof(Rental), "Rental")]
public partial class ReviewsViewModel : ObservableObject
{
    private readonly IReviewService _reviewService;

    [ObservableProperty] private int selectedRating = 5;
    [ObservableProperty] private string reviewComment = string.Empty;
    [ObservableProperty] private string statusMessage = string.Empty;
    [ObservableProperty] private double averageRating;
    [ObservableProperty] private int totalReviews;
    [ObservableProperty] private bool canReview;

    private ObservableCollection<Review> _reviews = new();
    public ObservableCollection<Review> Reviews
    {
        get => _reviews;
        private set => SetProperty(ref _reviews, value);
    }

    private Item? _item;
    public Item? Item
    {
        get => _item;
        set
        {
            _item = value;
            if (value != null)
                _ = LoadReviewsAsync();
        }
    }

    private Rental? _rental;
    public Rental? Rental
    {
        get => _rental;
        set
        {
            _rental = value;
            // Can only review if rental is Completed
            CanReview = value?.Status == "Completed";
        }
    }

    public IAsyncRelayCommand SubmitReviewCommand { get; }

    public ReviewsViewModel(IReviewService reviewService)
    {
        _reviewService = reviewService;
        SubmitReviewCommand = new AsyncRelayCommand(SubmitReviewAsync);
    }

    private async Task LoadReviewsAsync()
    {
        if (_item == null) return;
        try
        {
            var reviews = await _reviewService.GetItemReviewsAsync(_item.Id);
            Reviews = new ObservableCollection<Review>(reviews);
            AverageRating = reviews.Count > 0 ? reviews.Average(r => r.Rating) : 0;
            TotalReviews = reviews.Count;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to load reviews: {ex.Message}";
        }
    }

    private async Task SubmitReviewAsync()
    {
        if (_rental == null)
        {
            StatusMessage = "No completed rental to review.";
            return;
        }
        try
        {
            await _reviewService.SubmitReviewAsync(_rental.Id, SelectedRating, ReviewComment);
            StatusMessage = "Review submitted!";
            ReviewComment = string.Empty;
            SelectedRating = 5;
            CanReview = false;
            await LoadReviewsAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to submit review: {ex.Message}";
        }
    }
}