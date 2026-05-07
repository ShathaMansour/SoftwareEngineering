namespace StarterApp.Database.States;
using StarterApp.Database.Models;

public class OutForRentState : IRentalState
{
    public string StateName => "Out for Rent";

    public Task<IRentalState> Approve(Rental rental) =>
        throw new InvalidOperationException("Rental is already in progress.");

    public Task<IRentalState> Reject(Rental rental) =>
        throw new InvalidOperationException("Cannot reject a rental in progress.");

    public Task<IRentalState> StartRental(Rental rental) =>
        throw new InvalidOperationException("Rental has already started.");

    public async Task<IRentalState> Return(Rental rental)
    {
        rental.Status = "Returned";
        return new ReturnedState();
    }

    public Task<IRentalState> Complete(Rental rental) =>
        throw new InvalidOperationException("Cannot complete until item is returned.");

    public async Task<IRentalState> MarkOverdue(Rental rental)
    {
        rental.Status = "Overdue";
        return new OverdueState();
    }
}