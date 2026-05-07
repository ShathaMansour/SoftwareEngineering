namespace StarterApp.Database.States;
using StarterApp.Database.Models;

public class ApprovedState : IRentalState
{
    public string StateName => "Approved";

    public Task<IRentalState> Approve(Rental rental) =>
        throw new InvalidOperationException("Rental is already approved.");

    public Task<IRentalState> Reject(Rental rental) =>
        throw new InvalidOperationException("Cannot reject an already approved rental.");

    public async Task<IRentalState> StartRental(Rental rental)
    {
        rental.Status = "Out for Rent";
        return new OutForRentState();
    }

    public Task<IRentalState> Return(Rental rental) =>
        throw new InvalidOperationException("Cannot return a rental that has not started.");

    public Task<IRentalState> Complete(Rental rental) =>
        throw new InvalidOperationException("Cannot complete a rental that has not started.");

    public async Task<IRentalState> MarkOverdue(Rental rental)
    {
        rental.Status = "Overdue";
        return new OverdueState();
    }
}