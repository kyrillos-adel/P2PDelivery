using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace P2PDelivery.Application.DTOs;


public class DeliveryRequestUpdateDto
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; }
    
    public double TotalWeight { get; set; } // derived from the items
    
    [Required]
    public string PickUpLocation { get; set; }
    
    [Required]
    public string DropOffLocation { get; set; }
    
    [Required]
    public DateTime PickUpDate { get; set; }
    
    [Required]
    public double MinPrice { get; set; }
    
    [Required]
    public double MaxPrice { get; set; }
    public string? DRImageUrl { get; set; } // Image for the delivery request
    public IFormFile? DRImage { get; set; } // Image for the delivery request
}