namespace StarterApp.Database.States;
using StarterApp.Database.Models;

using Rental = StarterApp.Database.Models.Rental;
public interface IRentalState
{
    string StateName { get; }
    Task<IRentalState> Approve(Rental rental);
    Task<IRentalState> Reject(Rental rental);
    Task<IRentalState> StartRental(Rental rental);
    Task<IRentalState> Return(Rental rental);
    Task<IRentalState> Complete(Rental rental);
    Task<IRentalState> MarkOverdue(Rental rental);
}