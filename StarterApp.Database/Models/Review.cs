public class Review
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public int ReviewerId { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    // For item reviews response
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
}