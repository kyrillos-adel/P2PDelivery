using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Application.DTOs.ApplicationDTOs;

public class ApplicationDTO
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double OfferedPrice { get; set; }
    public string ApplicationStatus { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string UserProfileUrl { get; set; }


}
