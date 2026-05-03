using StarterApp.Database.Models;
using StarterApp.Database.Data;

namespace StarterApp.Services;

public class ReviewService : IReviewService
{
    private readonly IApiService? _apiService;
    private readonly IReviewRepository? _reviewRepository;
    private readonly IAuthenticationService _authService;

    // API mode
    public ReviewService(IApiService apiService, IAuthenticationService authService)
    {
        _apiService = apiService;
        _authService = authService;
    }

    // Local DB mode
    public ReviewService(IReviewRepository reviewRepository, IAuthenticationService authService)
    {
        _reviewRepository = reviewRepository;
        _authService = authService;
    }

    public async Task<List<Review>> GetItemReviewsAsync(int itemId)
    {
        if (_apiService != null)
            return await _apiService.GetItemReviewsAsync(itemId);
        return await _reviewRepository!.GetByItemIdAsync(itemId);
    }

    public async Task<List<Review>> GetUserReviewsAsync(int userId)
    {
        if (_apiService != null)
            return await _apiService.GetUserReviewsAsync(userId);
        return await _reviewRepository!.GetByUserIdAsync(userId);
    }

    public async Task<Review> SubmitReviewAsync(int rentalId, int rating, string? comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.");

        if (_apiService != null)
            return await _apiService.SubmitReviewAsync(rentalId, rating, comment);

        var userId = _authService.CurrentUser?.Id ?? 0;
        var alreadyReviewed = await _reviewRepository!.ExistsForRentalAsync(rentalId, userId);
        if (alreadyReviewed)
            throw new InvalidOperationException("You have already reviewed this rental.");

        var review = new Review
        {
            RentalId = rentalId,
            ReviewerId = userId,
            ReviewerName = _authService.CurrentUser?.FullName ?? string.Empty,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.UtcNow
        };
        return await _reviewRepository!.CreateAsync(review);
    }
}
