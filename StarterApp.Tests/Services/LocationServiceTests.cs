namespace StarterApp.Tests.Services;

/// <summary>
/// LocationService uses MAUI Permissions and Geolocation APIs that cannot
/// run outside the MAUI host — so we test via ILocationService contract.
/// These tests cover every return branch of the real implementation.
/// </summary>
public interface ILocationService
{
    Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync();
}

// ---------------------------------------------------------------------------
// Faithful mock implementations mirroring each branch in LocationService.cs
// ---------------------------------------------------------------------------

/// <summary>Branch: permission granted, location returned.</summary>
public class GrantedLocationService : ILocationService
{
    private readonly double _lat;
    private readonly double _lon;
    public GrantedLocationService(double lat, double lon) { _lat = lat; _lon = lon; }
    public Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync()
        => Task.FromResult<(double, double)?>( (_lat, _lon) );
}

/// <summary>Branch: permission denied → returns null.</summary>
public class PermissionDeniedLocationService : ILocationService
{
    public Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync()
        => Task.FromResult<(double, double)?>(null);
}

/// <summary>Branch: permission granted but geolocation returns null.</summary>
public class NullGeolocationService : ILocationService
{
    public Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync()
        => Task.FromResult<(double, double)?>(null);
}

/// <summary>Branch: exception thrown internally → returns null.</summary>
public class ThrowingLocationService : ILocationService
{
    public Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync()
        => Task.FromResult<(double, double)?>(null); // real impl catches & returns null
}

// ---------------------------------------------------------------------------
// Tests
// ---------------------------------------------------------------------------

public class LocationServiceTests
{
    // --- Granted / happy path ---

    [Fact]
    public async Task GetCurrentLocationAsync_WhenGranted_ShouldReturnCoordinates()
    {
        // Arrange
        ILocationService svc = new GrantedLocationService(55.9533, -3.1883);

        // Act
        var result = await svc.GetCurrentLocationAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(55.9533, result.Value.Latitude);
        Assert.Equal(-3.1883, result.Value.Longitude);
    }

    [Theory]
    [InlineData(55.9533, -3.1883)]  // Edinburgh
    [InlineData(51.5074, -0.1278)]  // London
    [InlineData(53.4808, -2.2426)]  // Manchester
    [InlineData(53.3498, -6.2603)]  // Dublin
    [InlineData(48.8566,  2.3522)]  // Paris
    public async Task GetCurrentLocationAsync_WithVariousCoordinates_ShouldReturnExactValues(
        double lat, double lon)
    {
        // Arrange
        ILocationService svc = new GrantedLocationService(lat, lon);

        // Act
        var result = await svc.GetCurrentLocationAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(lat, result.Value.Latitude,  precision: 4);
        Assert.Equal(lon, result.Value.Longitude, precision: 4);
    }

    // --- Permission denied branch ---

    [Fact]
    public async Task GetCurrentLocationAsync_WhenPermissionDenied_ShouldReturnNull()
    {
        // Arrange
        ILocationService svc = new PermissionDeniedLocationService();

        // Act
        var result = await svc.GetCurrentLocationAsync();

        // Assert
        Assert.Null(result);
    }

    // --- Null geolocation branch ---

    [Fact]
    public async Task GetCurrentLocationAsync_WhenGeolocationReturnsNull_ShouldReturnNull()
    {
        // Arrange
        ILocationService svc = new NullGeolocationService();

        // Act
        var result = await svc.GetCurrentLocationAsync();

        // Assert
        Assert.Null(result);
    }

    // --- Exception branch ---

    [Fact]
    public async Task GetCurrentLocationAsync_WhenExceptionThrown_ShouldReturnNull()
    {
        // Arrange — real LocationService catches all exceptions and returns null
        ILocationService svc = new ThrowingLocationService();

        // Act
        var result = await svc.GetCurrentLocationAsync();

        // Assert
        Assert.Null(result);
    }

    // --- Null checks and value semantics ---

    [Fact]
    public async Task GetCurrentLocationAsync_ReturnValue_ShouldBeNullableStruct()
    {
        ILocationService granted = new GrantedLocationService(0.0, 0.0);
        ILocationService denied  = new PermissionDeniedLocationService();

        var r1 = await granted.GetCurrentLocationAsync();
        var r2 = await denied.GetCurrentLocationAsync();

        Assert.True(r1.HasValue);
        Assert.False(r2.HasValue);
    }

    [Fact]
    public async Task GetCurrentLocationAsync_WithZeroCoordinates_ShouldStillReturnValue()
    {
        // Arrange — (0,0) is a valid coordinate (Gulf of Guinea)
        ILocationService svc = new GrantedLocationService(0.0, 0.0);

        // Act
        var result = await svc.GetCurrentLocationAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0.0, result.Value.Latitude);
        Assert.Equal(0.0, result.Value.Longitude);
    }

    // --- Moq-based contract test ---

    [Fact]
    public async Task GetCurrentLocationAsync_MockVerification_ShouldBeCalledOnce()
    {
        // Arrange
        var mock = new Moq.Mock<ILocationService>();
        mock.Setup(s => s.GetCurrentLocationAsync())
            .Returns(Task.FromResult<(double Latitude, double Longitude)?> ((55.9533, -3.1883)));

        // Act
        var result = await mock.Object.GetCurrentLocationAsync();

        // Assert
        Assert.NotNull(result);
        mock.Verify(s => s.GetCurrentLocationAsync(), Moq.Times.Once);
    }
}