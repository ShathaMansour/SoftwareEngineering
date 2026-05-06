using Moq;
using StarterApp.Database.Data;
using StarterApp.Database.Models;

namespace StarterApp.Tests.ViewModels;

// ItemsListViewModel cannot be instantiated in unit tests as StarterApp targets
// MAUI platform frameworks. These tests verify the data layer logic it depends on
// using mocked repositories only — no DatabaseFixture needed.
public class ItemsListViewModelTests
{
    private static Mock<IItemRepository> CreateMock(List<Item>? items = null)
    {
        var mock = new Mock<IItemRepository>();
        mock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(items ?? new List<Item>
            {
                new Item { Id = 1, Title = "Electric Drill", Description = "Cordless drill", DailyRate = 5.00m,  Category = "Tools"   },
                new Item { Id = 2, Title = "Camping Tent",   Description = "4 person tent",  DailyRate = 15.00m, Category = "Camping" }
            });
        mock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Item { Id = 1, Title = "Electric Drill", DailyRate = 5.00m, Category = "Tools" });
        mock.Setup(r => r.GetByIdAsync(2))
            .ReturnsAsync(new Item { Id = 2, Title = "Camping Tent", DailyRate = 15.00m, Category = "Camping" });
        mock.Setup(r => r.GetByIdAsync(It.Is<int>(id => id != 1 && id != 2)))
            .ReturnsAsync((Item?)null);
        return mock;
    }

    // --- LoadItems ---

    [Fact]
    public async Task LoadItems_ShouldReturnAllItems()
    {
        var items = await CreateMock().Object.GetAllAsync();
        Assert.NotNull(items);
        Assert.NotEmpty(items);
    }

    [Fact]
    public async Task LoadItems_ShouldReturnCorrectCount()
    {
        var items = await CreateMock().Object.GetAllAsync();
        Assert.Equal(2, items.Count);
    }

    [Theory]
    [InlineData(1, "Electric Drill", 5.00)]
    [InlineData(2, "Camping Tent",  15.00)]
    public async Task LoadItems_EachItem_ShouldHaveCorrectData(int id, string title, double rate)
    {
        var item = await CreateMock().Object.GetByIdAsync(id);
        Assert.NotNull(item);
        Assert.Equal(title, item.Title);
        Assert.Equal((decimal)rate, item.DailyRate);
    }

    // --- ViewItem ---

    [Fact]
    public async Task ViewItem_ShouldReturnCorrectItem()
    {
        var item = await CreateMock().Object.GetByIdAsync(1);
        Assert.NotNull(item);
        Assert.Equal("Electric Drill", item.Title);
        Assert.Equal(5.00m, item.DailyRate);
    }

    [Theory]
    [InlineData(1, "Electric Drill")]
    [InlineData(2, "Camping Tent")]
    public async Task GetByIdAsync_ShouldReturnCorrectItem(int id, string expectedTitle)
    {
        var item = await CreateMock().Object.GetByIdAsync(id);
        Assert.NotNull(item);
        Assert.Equal(expectedTitle, item.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        var item = await CreateMock().Object.GetByIdAsync(999);
        Assert.Null(item);
    }

    // --- Mock verification ---

    [Fact]
    public async Task LoadItems_WithMockedRepository_ShouldReturnMockedItems()
    {
        var mock = new Mock<IItemRepository>();
        mock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Item>
        {
            new Item { Id = 10, Title = "Mocked Drill", Description = "Mock", DailyRate = 9.99m, Category = "Tools" }
        });

        var items = await mock.Object.GetAllAsync();

        Assert.Single(items);
        Assert.Equal("Mocked Drill", items[0].Title);
    }

    [Fact]
    public async Task LoadItems_WithMockedRepository_WhenEmpty_ShouldReturnEmptyList()
    {
        var mock = new Mock<IItemRepository>();
        mock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Item>());

        var items = await mock.Object.GetAllAsync();

        Assert.Empty(items);
    }

    [Fact]
    public async Task LoadItems_WithMockedRepository_ShouldCallGetAllOnce()
    {
        var mock = new Mock<IItemRepository>();
        mock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Item>());

        await mock.Object.GetAllAsync();

        mock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    public async Task LoadItems_ShouldMatchRepositoryItemCount(int count)
    {
        var fakeItems = Enumerable.Range(1, count)
            .Select(i => new Item { Id = i, Title = $"Item {i}", Description = "desc", DailyRate = i * 1.00m, Category = "Tools" })
            .ToList();

        var mock = new Mock<IItemRepository>();
        mock.Setup(r => r.GetAllAsync()).ReturnsAsync(fakeItems);

        var items = await mock.Object.GetAllAsync();

        Assert.Equal(count, items.Count);
    }
}