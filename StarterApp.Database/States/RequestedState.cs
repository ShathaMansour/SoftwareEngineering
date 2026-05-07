namespace StarterApp.Database.States;
using StarterApp.Database.Models;

public class RequestedState : IRentalState
{
    public string StateName => "Requested";

    public async Task<IRentalState> Approve(Rental rental)
    {
        rental.Status = "Approved";
        rental.ApprovedAt = DateTime.UtcNow;
        return new ApprovedState();
    }

    public async Task<IRentalState> Reject(Rental rental)
    {
        rental.Status = "Rejected";
        return new RejectedState();
    }

    public Task<IRentalState> StartRental(Rental rental) =>
        throw new InvalidOperationException("Cannot start rental until approved.");

    public Task<IRentalState> Return(Rental rental) =>
        throw new InvalidOperationException("Cannot return a rental that has not started.");

    public Task<IRentalState> Complete(Rental rental) =>
        throw new InvalidOperationException("Cannot complete a rental that has not started.");

    public Task<IRentalState> MarkOverdue(Rental rental) =>
        throw new InvalidOperationException("Cannot mark as overdue until rental has started.");
}