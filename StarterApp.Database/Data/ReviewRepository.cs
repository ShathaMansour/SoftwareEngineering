using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetByItemIdAsync(int itemId)
    {
        return await _context.Reviews
            .Where(r => r.RentalId == itemId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Review>> GetByUserIdAsync(int userId)
    {
        return await _context.Reviews
            .Where(r => r.ReviewerId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review> CreateAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<bool> ExistsForRentalAsync(int rentalId, int reviewerId)
    {
        return await _context.Reviews
            .AnyAsync(r => r.RentalId == rentalId && r.ReviewerId == reviewerId);
    }
}