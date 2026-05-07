using StarterApp.Database.Models;
using StarterApp.Database.States;

namespace StarterApp.Tests.States;

public class RentalStateMachineTests
{
    private static Rental CreateRental(string status) => new Rental
    {
        Id = 1, ItemId = 1, ItemTitle = "Electric Drill",
        BorrowerId = 2, OwnerId = 1,
        StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(3),
        Status = status, TotalPrice = 15.00m, RequestedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task RequestedState_Approve_ShouldTransitionToApproved()
    {
        var rental = CreateRental("Requested");
        var machine = new RentalStateMachine(rental);

        await machine.Approve(rental);

        Assert.Equal("Approved", rental.Status);
        Assert.Equal("Approved", machine.CurrentState);
    }

    [Fact]
    public async Task RequestedState_Reject_ShouldTransitionToRejected()
    {
        var rental = CreateRental("Requested");
        var machine = new RentalStateMachine(rental);

        await machine.Reject(rental);

        Assert.Equal("Rejected", rental.Status);
    }

    [Fact]
    public async Task ApprovedState_StartRental_ShouldTransitionToOutForRent()
    {
        var rental = CreateRental("Approved");
        var machine = new RentalStateMachine(rental);

        await machine.StartRental(rental);

        Assert.Equal("Out for Rent", rental.Status);
        Assert.Equal("Out for Rent", machine.CurrentState);
    }

    [Fact]
    public async Task OutForRentState_Return_ShouldTransitionToReturned()
    {
        var rental = CreateRental("Out for Rent");
        var machine = new RentalStateMachine(rental);

        await machine.Return(rental);

        Assert.Equal("Returned", rental.Status);
    }

    [Fact]
    public async Task ReturnedState_Complete_ShouldTransitionToCompleted()
    {
        var rental = CreateRental("Returned");
        var machine = new RentalStateMachine(rental);

        await machine.Complete(rental);

        Assert.Equal("Completed", rental.Status);
    }

    [Fact]
    public async Task OverdueState_Return_ShouldTransitionToReturned()
    {
        var rental = CreateRental("Overdue");
        var machine = new RentalStateMachine(rental);

        await machine.Return(rental);

        Assert.Equal("Returned", rental.Status);
    }

    // --- Invalid transition tests ---

    [Fact]
    public async Task RequestedState_StartRental_ShouldThrow()
    {
        var rental = CreateRental("Requested");
        var machine = new RentalStateMachine(rental);

        await Assert.ThrowsAsync<InvalidOperationException>(() => machine.StartRental(rental));
    }

    [Fact]
    public async Task CompletedState_AnyTransition_ShouldThrow()
    {
        var rental = CreateRental("Completed");
        var machine = new RentalStateMachine(rental);

        await Assert.ThrowsAsync<InvalidOperationException>(() => machine.Approve(rental));
    }

    [Fact]
    public async Task RejectedState_AnyTransition_ShouldThrow()
    {
        var rental = CreateRental("Rejected");
        var machine = new RentalStateMachine(rental);

        await Assert.ThrowsAsync<InvalidOperationException>(() => machine.Approve(rental));
    }

    [Theory]
    [InlineData("Requested", "Approved")]
    [InlineData("Approved", "Out for Rent")]
    [InlineData("Out for Rent", "Returned")]
    [InlineData("Returned", "Completed")]
    [InlineData("Overdue", "Returned")]
    public async Task StateMachine_ShouldFollowValidTransitions(string fromStatus, string expectedStatus)
    {
        var rental = CreateRental(fromStatus);
        var machine = new RentalStateMachine(rental);

        await (fromStatus switch
        {
            "Requested"    => machine.Approve(rental),
            "Approved"     => machine.StartRental(rental),
            "Out for Rent" => machine.Return(rental),
            "Returned"     => machine.Complete(rental),
            "Overdue"      => machine.Return(rental),
            _ => throw new InvalidOperationException()
        });

        Assert.Equal(expectedStatus, rental.Status);
    }
        // --- CompletedState invalid transitions ---
    [Fact]
    public async Task CompletedState_Return_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Completed")).Return(CreateRental("Completed")));

    [Fact]
    public async Task CompletedState_StartRental_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Completed")).StartRental(CreateRental("Completed")));

    // --- RejectedState invalid transitions ---
    [Fact]
    public async Task RejectedState_Reject_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Rejected")).Reject(CreateRental("Rejected")));

    [Fact]
    public async Task RejectedState_Return_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Rejected")).Return(CreateRental("Rejected")));

    // --- ReturnedState invalid transitions ---
    [Fact]
    public async Task ReturnedState_Approve_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Returned")).Approve(CreateRental("Returned")));

    [Fact]
    public async Task ReturnedState_StartRental_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Returned")).StartRental(CreateRental("Returned")));

    // --- OutForRentState invalid transitions ---
    [Fact]
    public async Task OutForRentState_Approve_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Out for Rent")).Approve(CreateRental("Out for Rent")));

    [Fact]
    public async Task OutForRentState_Complete_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Out for Rent")).Complete(CreateRental("Out for Rent")));

    // --- OverdueState invalid transitions ---
    [Fact]
    public async Task OverdueState_Approve_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Overdue")).Approve(CreateRental("Overdue")));

    [Fact]
    public async Task OverdueState_Complete_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Overdue")).Complete(CreateRental("Overdue")));

    // --- ApprovedState invalid transitions ---
    [Fact]
    public async Task ApprovedState_Approve_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Approved")).Approve(CreateRental("Approved")));

    [Fact]
    public async Task ApprovedState_Return_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Approved")).Return(CreateRental("Approved")));

    // --- RequestedState invalid transitions ---
    [Fact]
    public async Task RequestedState_Return_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Requested")).Return(CreateRental("Requested")));

    [Fact]
    public async Task RequestedState_Complete_ShouldThrow() =>
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RentalStateMachine(CreateRental("Requested")).Complete(CreateRental("Requested")));
}