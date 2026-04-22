using StarterApp.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace StarterApp.Database.Data;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Item>> GetAllAsync()
    {
        return await _context.Items.ToListAsync();
    }

    public async Task<Item> GetByIdAsync(int id)
    {
        return await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Item>> GetNearbyAsync(double lat, double lon, double radiusKm)
    {
        // simple version (no PostGIS logic needed yet)
        return await _context.Items.ToListAsync();
    }

    public async Task<Item> CreateAsync(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task UpdateAsync(Item item)
    {
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
    }
}