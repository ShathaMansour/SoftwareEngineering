using StarterApp.Database.Data;
using StarterApp.Database.Models;
using StarterApp.Tests.Fixtures;

namespace StarterApp.Tests.Repositories;

public class ItemRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly ItemRepository _repository;

    public ItemRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _repository = new ItemRepository(_fixture.Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllItems()
    {
        // Arrange & Act
        var items = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(items);
        Assert.NotEmpty(items);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnItem()
    {
        // Arrange & Act
        var item = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(item);
        Assert.Equal("Electric Drill", item.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange & Act
        var item = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(item);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddItem()
    {
        // Arrange
        var newItem = new Item
        {
            Title = "New Tool",
            Description = "A new tool",
            DailyRate = 10.00m,
            Category = "Tools"
        };

        // Act
        var created = await _repository.CreateAsync(newItem);

        // Assert
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal("New Tool", created.Title);
    }
}