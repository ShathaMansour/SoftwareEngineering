namespace StarterApp.Database.Data;

public interface IReviewRepository
{
    Task<List<Review>> GetByItemIdAsync(int itemId);
    Task<List<Review>> GetByUserIdAsync(int userId);
    Task<Review> CreateAsync(Review review);
    Task<bool> ExistsForRentalAsync(int rentalId, int reviewerId);
}