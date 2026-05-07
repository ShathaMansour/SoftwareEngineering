namespace StarterApp.Database.States;
using StarterApp.Database.Models;

public class ReturnedState : IRentalState
{
    public string StateName => "Returned";

    public Task<IRentalState> Approve(Rental rental) =>
        throw new InvalidOperationException("Cannot approve a returned rental.");

    public Task<IRentalState> Reject(Rental rental) =>
        throw new InvalidOperationException("Cannot reject a returned rental.");

    public Task<IRentalState> StartRental(Rental rental) =>
        throw new InvalidOperationException("Rental has already ended.");

    public Task<IRentalState> Return(Rental rental) =>
        throw new InvalidOperationException("Rental has already been returned.");

    public async Task<IRentalState> Complete(Rental rental)
    {
        rental.Status = "Completed";
        return new CompletedState();
    }

    public Task<IRentalState> MarkOverdue(Rental rental) =>
        throw new InvalidOperationException("Cannot mark a returned rental as overdue.");
}