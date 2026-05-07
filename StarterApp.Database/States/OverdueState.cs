namespace StarterApp.Database.States;
using StarterApp.Database.Models;

public class OverdueState : IRentalState
{
    public string StateName => "Overdue";

    public Task<IRentalState> Approve(Rental rental) =>
        throw new InvalidOperationException("Cannot approve an overdue rental.");

    public Task<IRentalState> Reject(Rental rental) =>
        throw new InvalidOperationException("Cannot reject an overdue rental.");

    public Task<IRentalState> StartRental(Rental rental) =>
        throw new InvalidOperationException("Rental has already started.");

    public async Task<IRentalState> Return(Rental rental)
    {
        rental.Status = "Returned";
        return new ReturnedState();
    }

    public Task<IRentalState> Complete(Rental rental) =>
        throw new InvalidOperationException("Cannot complete until item is returned.");

    public Task<IRentalState> MarkOverdue(Rental rental) =>
        throw new InvalidOperationException("Rental is already overdue.");
}