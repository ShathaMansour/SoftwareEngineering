using StarterApp.Database.Data;
using StarterApp.Database.Models;
using StarterApp.Tests.Fixtures;

namespace StarterApp.Tests.Services;

public class RentalServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly RentalRepository _rentalRepository;

    public RentalServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _rentalRepository = new RentalRepository(_fixture.Context);
    }

    [Fact]
    public async Task GetIncomingAsync_ShouldReturnRentalsForOwner()
    {
        var rentals = await _rentalRepository.GetIncomingAsync(1);
        Assert.NotNull(rentals);
        Assert.All(rentals, r => Assert.Equal(1, r.OwnerId));
    }

    [Fact]
    public async Task GetOutgoingAsync_ShouldReturnRentalsForBorrower()
    {
        var rentals = await _rentalRepository.GetOutgoingAsync(2);
        Assert.NotNull(rentals);
        Assert.All(rentals, r => Assert.Equal(2, r.BorrowerId));
    }

    [Fact]
    public async Task UpdateStatusAsync_ShouldChangeRentalStatus()
    {
        // Arrange — create a fresh rental so we don't depend on seeded data order
        var rental = new Rental
        {
            ItemId = 1, ItemTitle = "Electric Drill",
            BorrowerId = 2, OwnerId = 1,
            StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1),
            Status = "Requested", TotalPrice = 5.00m, RequestedAt = DateTime.UtcNow
        };
        _fixture.Context.Rentals.Add(rental);
        await _fixture.Context.SaveChangesAsync();

        // Act
        await _rentalRepository.UpdateStatusAsync(rental.Id, "Approved");
        var updated = await _rentalRepository.GetByIdAsync(rental.Id);

        // Assert
        Assert.Equal("Approved", updated?.Status);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnExistingRental()
    {
        // Arrange — create a known rental
        var rental = new Rental
        {
            ItemId = 1, ItemTitle = "Electric Drill",
            BorrowerId = 2, OwnerId = 1,
            StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(2),
            Status = "Requested", TotalPrice = 10.00m, RequestedAt = DateTime.UtcNow
        };
        _fixture.Context.Rentals.Add(rental);
        await _fixture.Context.SaveChangesAsync();

        // Act
        var fetched = await _rentalRepository.GetByIdAsync(rental.Id);

        // Assert
        Assert.NotNull(fetched);
        Assert.True(fetched.Id > 0);
    }
}