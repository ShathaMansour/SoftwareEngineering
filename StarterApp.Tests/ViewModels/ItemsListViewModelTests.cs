using StarterApp.Database.Data;
using StarterApp.Database.Models;
using StarterApp.Tests.Fixtures;

namespace StarterApp.Tests.ViewModels;

// Note: MAUI ViewModels cannot be instantiated in unit tests due to framework dependencies.
// These tests verify the data layer logic that ItemsListViewModel depends on.
public class ItemsListViewModelTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly ItemRepository _itemRepository;

    public ItemsListViewModelTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _itemRepository = new ItemRepository(_fixture.Context);
    }

    [Fact]
    public async Task LoadItems_ShouldReturnAllItems()
    {
        // Arrange & Act
        var items = await _itemRepository.GetAllAsync();

        // Assert
        Assert.NotNull(items);
        Assert.NotEmpty(items);
    }

    [Fact]
    public async Task LoadItems_ShouldReturnCorrectCount()
    {
        // Arrange & Act
        var items = await _itemRepository.GetAllAsync();

        // Assert
        Assert.Equal(2, items.Count);
    }

    [Fact]
    public async Task ViewItem_ShouldReturnCorrectItem()
    {
        // Arrange & Act
        var item = await _itemRepository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(item);
        Assert.Equal("Electric Drill", item.Title);
        Assert.Equal(5.00m, item.DailyRate);
    }

    [Theory]
    [InlineData(1, "Electric Drill")]
    [InlineData(2, "Camping Tent")]
    public async Task GetByIdAsync_ShouldReturnCorrectItem(int id, string expectedTitle)
    {
        // Arrange & Act
        var item = await _itemRepository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(item);
        Assert.Equal(expectedTitle, item.Title);
    }
}