namespace StarterApp.Database.States;
using StarterApp.Database.Models;

public class CompletedState : IRentalState
{
    public string StateName => "Completed";

    public Task<IRentalState> Approve(Rental rental) =>
        throw new InvalidOperationException("Rental is already completed.");

    public Task<IRentalState> Reject(Rental rental) =>
        throw new InvalidOperationException("Rental is already completed.");

    public Task<IRentalState> StartRental(Rental rental) =>
        throw new InvalidOperationException("Rental is already completed.");

    public Task<IRentalState> Return(Rental rental) =>
        throw new InvalidOperationException("Rental is already completed.");

    public Task<IRentalState> Complete(Rental rental) =>
        throw new InvalidOperationException("Rental is already completed.");

    public Task<IRentalState> MarkOverdue(Rental rental) =>
        throw new InvalidOperationException("Rental is already completed.");
}