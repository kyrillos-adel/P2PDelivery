using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace P2PDelivery.Application.DTOs.DeliveryRequestDTOs;

public class DeliveryRequestDetailsDTO
{
    public int Id { get; set; }
    [StringLength(50)]
    public string Title { get; set; }

    [StringLength(500)]
    public string Description { get; set; }
    public double TotalWeight { get; set; } // derived from the items
    public string PickUpLocation { get; set; }
    public string DropOffLocation { get; set; }
    public DateTime PickUpDate { get; set; }
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
    public int UserId { get; set; }
    public bool IsOwner { get; set; } = false; 
    public string UserName { get; set; }
    public string Status { get; set; }
    public string? DRImageUrl { get; set; }
    public string? ProfileImageUrl { get; set; }
    public ICollection<ApplicationDTO>? ApplicationDTOs { get; set; } = new List<ApplicationDTO>();


}
