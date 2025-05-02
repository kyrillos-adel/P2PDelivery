using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace P2PDelivery.Application.DTOs;

public class CreateDeliveryRequestDTO
{
    [StringLength(50)] 
    public string Title { get; set; }

    [StringLength(500)]
    public string Description { get; set; }
    public double TotalWeight { get; set; } // derived from the items

    public string PickUpLocation { get; set; }

    public string DropOffLocation { get; set; }
    

    public DateTime PickUpDate { get; set; }

    [DataType(DataType.Currency)]
    public double MinPrice { get; set; }

    [DataType(DataType.Currency)]
    public double MaxPrice { get; set; }

    public IFormFile? DRImage { get; set; } // Image for the delivery request
    [JsonIgnore]
    public int UserId { get; set; } // Get this from token in production

}
