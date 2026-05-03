using StarterApp.Database.Data;
using StarterApp.Database.Models;
using StarterApp.Tests.Fixtures;

namespace StarterApp.Tests.Repositories;

public class CategoryRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly CategoryRepository _repository;

    public CategoryRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _repository = new CategoryRepository(_fixture.Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        // Arrange & Act
        var categories = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(categories);
        Assert.NotEmpty(categories);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCorrectCategories()
    {
        // Arrange & Act
        var categories = await _repository.GetAllAsync();

        // Assert
        Assert.Contains(categories, c => c.Name == "Tools");
        Assert.Contains(categories, c => c.Name == "Camping");
    }

    [Theory]
    [InlineData("Tools")]
    [InlineData("Camping")]
    public async Task GetAllAsync_ShouldContainCategory(string categoryName)
    {
        // Arrange & Act
        var categories = await _repository.GetAllAsync();

        // Assert
        Assert.Contains(categories, c => c.Name == categoryName);
    }
}
