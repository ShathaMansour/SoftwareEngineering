namespace StarterApp.Database.States;


using StarterApp.Database.Models;
public class RentalStateMachine
{
    private IRentalState _currentState;

    public RentalStateMachine(Rental rental)
    {
        _currentState = rental.Status switch
        {
            "Requested"    => new RequestedState(),
            "Approved"     => new ApprovedState(),
            "Out for Rent" => new OutForRentState(),
            "Overdue"      => new OverdueState(),
            "Returned"     => new ReturnedState(),
            "Completed"    => new CompletedState(),
            "Rejected"     => new RejectedState(),
            _ => throw new InvalidOperationException($"Unknown rental status: {rental.Status}")
        };
    }

    public string CurrentState => _currentState.StateName;

    public async Task Approve(Rental rental)    => _currentState = await _currentState.Approve(rental);
    public async Task Reject(Rental rental)     => _currentState = await _currentState.Reject(rental);
    public async Task StartRental(Rental rental)=> _currentState = await _currentState.StartRental(rental);
    public async Task Return(Rental rental)     => _currentState = await _currentState.Return(rental);
    public async Task Complete(Rental rental)   => _currentState = await _currentState.Complete(rental);
    public async Task MarkOverdue(Rental rental)=> _currentState = await _currentState.MarkOverdue(rental);
}