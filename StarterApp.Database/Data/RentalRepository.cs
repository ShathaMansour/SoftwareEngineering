using StarterApp.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace StarterApp.Database.Data;

public class RentalRepository : IRentalRepository
{
    private readonly AppDbContext _context;

    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rental>> GetIncomingAsync(int ownerId)
    {
        return await _context.Rentals
            .Where(r => r.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<List<Rental>> GetOutgoingAsync(int borrowerId)
    {
        return await _context.Rentals
            .Where(r => r.BorrowerId == borrowerId)
            .ToListAsync();
    }

    public async Task<Rental?> GetByIdAsync(int id)
    {
        return await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Rental> CreateAsync(Rental rental)
    {
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
        return rental;
    }

    public async Task UpdateStatusAsync(int rentalId, string status)
    {
        var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == rentalId);
        if (rental != null)
        {
            rental.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}