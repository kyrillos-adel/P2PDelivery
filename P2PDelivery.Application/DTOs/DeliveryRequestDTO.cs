using P2PDelivery.Domain.Entities;
using System.Text.Json.Serialization;

namespace P2PDelivery.Application.DTOs;

public class DeliveryRequestDTO
{
    
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double TotalWeight { get; set; }
    public string PickUpLocation { get; set; }
    public string DropOffLocation { get; set; }
    public DateTime PickUpDate { get; set; }
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
    public string Status { get; set; } // e.g. Pending, Accepted, Completed, Cancelled, Delivered
    public string UserName {  get; set; }
    public string ProfileImageUrl { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    public bool IsOwner { get; set; } = false;


    public string? DRImageUrl { get; set; }

}
