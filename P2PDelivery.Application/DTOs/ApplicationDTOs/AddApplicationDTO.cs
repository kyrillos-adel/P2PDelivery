using Microsoft.AspNetCore.Http;
using P2PDelivery.Domain.Enums;
using P2PDelivery.Domain.Validators;

namespace P2PDelivery.Application.DTOs.ApplicationDTOs;

public class AddApplicationDTO
{
    public double OfferedPrice { get; set; }
    public int DeliveryRequestId { get; set; }
}
