using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IReviewService
{
    Task<List<Review>> GetItemReviewsAsync(int itemId);
    Task<List<Review>> GetUserReviewsAsync(int userId);
    Task<Review> SubmitReviewAsync(int rentalId, int rating, string? comment);
}