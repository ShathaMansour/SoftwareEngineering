namespace StarterApp.Database.States;
using StarterApp.Database.Models;

public class RejectedState : IRentalState
{
    public string StateName => "Rejected";

    public Task<IRentalState> Approve(Rental rental) =>
        throw new InvalidOperationException("Cannot approve a rejected rental.");

    public Task<IRentalState> Reject(Rental rental) =>
        throw new InvalidOperationException("Rental is already rejected.");

    public Task<IRentalState> StartRental(Rental rental) =>
        throw new InvalidOperationException("Cannot start a rejected rental.");

    public Task<IRentalState> Return(Rental rental) =>
        throw new InvalidOperationException("Cannot return a rejected rental.");

    public Task<IRentalState> Complete(Rental rental) =>
        throw new InvalidOperationException("Cannot complete a rejected rental.");

    public Task<IRentalState> MarkOverdue(Rental rental) =>
        throw new InvalidOperationException("Cannot mark a rejected rental as overdue.");
}