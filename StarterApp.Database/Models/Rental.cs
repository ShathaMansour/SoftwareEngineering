namespace StarterApp.Database.Models;

public class Rental
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string ItemTitle { get; set; } = string.Empty;
    public int BorrowerId { get; set; }
    public string BorrowerName { get; set; } = string.Empty;
    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsRequested => Status == "Requested";
    public decimal TotalPrice { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public bool IsApproved => Status == "Approved";
    public bool IsCompleted => Status == "Completed";  
    public bool IsRejected => Status == "Rejected"; 
    public bool IsOutForRent => Status == "Out for Rent";
    public bool IsReturned => Status == "Returned";
    public bool IsOverdue => Status == "Overdue";
}