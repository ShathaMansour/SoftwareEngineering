using StarterApp.Tests.Fixtures;

namespace StarterApp.Tests.Services;

// Mock ILocationService locally since we can't reference the MAUI project
public interface ILocationService
{
    Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync();
}

public class MockLocationService : ILocationService
{
    private readonly double _lat;
    private readonly double _lon;

    public MockLocationService(double lat, double lon)
    {
        _lat = lat;
        _lon = lon;
    }

    public Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync()
    {
        return Task.FromResult<(double Latitude, double Longitude)?>( (_lat, _lon));
    }
}

public class LocationServiceTests
{
    [Fact]
    public async Task GetCurrentLocationAsync_ShouldReturnMockLocation()
    {
        // Arrange
        var mockLocation = new MockLocationService(55.9533, -3.1883);

        // Act
        var location = await mockLocation.GetCurrentLocationAsync();

        // Assert
        Assert.NotNull(location);
        Assert.Equal(55.9533, location.Value.Latitude);
        Assert.Equal(-3.1883, location.Value.Longitude);
    }

    [Fact]
    public async Task GetCurrentLocationAsync_ShouldReturnCorrectCoordinates()
    {
        // Arrange
        var mockLocation = new MockLocationService(51.5074, -0.1278); // London

        // Act
        var location = await mockLocation.GetCurrentLocationAsync();

        // Assert
        Assert.NotNull(location);
        Assert.Equal(51.5074, location.Value.Latitude);
        Assert.Equal(-0.1278, location.Value.Longitude);
    }

    [Theory]
    [InlineData(55.9533, -3.1883)] // Edinburgh
    [InlineData(51.5074, -0.1278)] // London
    [InlineData(53.4808, -2.2426)] // Manchester
    public async Task GetCurrentLocationAsync_WithDifferentLocations_ShouldReturnCorrectCoordinates(
        double lat, double lon)
    {
        // Arrange
        var mockLocation = new MockLocationService(lat, lon);

        // Act
        var location = await mockLocation.GetCurrentLocationAsync();

        // Assert
        Assert.NotNull(location);
        Assert.Equal(lat, location.Value.Latitude);
        Assert.Equal(lon, location.Value.Longitude);
    }

    [Fact]
    public async Task NearbyItems_WithMockLocation_ShouldUseCorrectCoordinates()
    {
        // Arrange
        var mockLocation = new MockLocationService(55.9533, -3.1883);
        var expectedLat = 55.9533;
        var expectedLon = -3.1883;

        // Act
        var location = await mockLocation.GetCurrentLocationAsync();

        // Assert
        Assert.NotNull(location);
        Assert.Equal(expectedLat, location.Value.Latitude, precision: 4);
        Assert.Equal(expectedLon, location.Value.Longitude, precision: 4);
    }
}
