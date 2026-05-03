using Moq;
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
        // Arrange & Act
        var rentals = await _rentalRepository.GetIncomingAsync(1);

        // Assert
        Assert.NotNull(rentals);
        Assert.All(rentals, r => Assert.Equal(1, r.OwnerId));
    }

    [Fact]
    public async Task GetOutgoingAsync_ShouldReturnRentalsForBorrower()
    {
        // Arrange & Act
        var rentals = await _rentalRepository.GetOutgoingAsync(2);

        // Assert
        Assert.NotNull(rentals);
        Assert.All(rentals, r => Assert.Equal(2, r.BorrowerId));
    }

    [Fact]
    public async Task UpdateStatusAsync_ShouldChangeRentalStatus()
    {
        // Arrange & Act
        await _rentalRepository.UpdateStatusAsync(1, "Approved");
        var rental = await _rentalRepository.GetByIdAsync(1);

        // Assert
        Assert.Equal("Approved", rental?.Status);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetByIdAsync_ShouldReturnExistingRental(int rentalId)
    {
        // Arrange & Act
        var rental = await _rentalRepository.GetByIdAsync(rentalId);

        // Assert
        Assert.NotNull(rental);
        Assert.True(rental.Id > 0);
    }
}