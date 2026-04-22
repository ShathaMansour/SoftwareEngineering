using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace StarterApp.Database.Models;
public class Item
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal DailyRate { get; set; }
    public string Category { get; set; }
    [NotMapped] public int CategoryId { get; set; }
    [NotMapped] public int OwnerId { get; set; }
    [NotMapped] public string OwnerName { get; set; }
    [NotMapped] public double Latitude { get; set; }
    [NotMapped] public double Longitude { get; set; }
    [NotMapped] public bool IsAvailable { get; set; }
    [NotMapped] public double? AverageRating { get; set; }
    [NotMapped] public string? ImageUrl { get; set; }
    [NotMapped] public DateTime CreatedAt { get; set; }
}