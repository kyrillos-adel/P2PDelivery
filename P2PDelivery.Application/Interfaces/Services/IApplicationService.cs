using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Application.Response;


namespace P2PDelivery.Application.Interfaces.Services;
public interface IApplicationService
{
    Task<RequestResponse<ICollection<ApplicationDTO>>> GetApplicationByRequestAsync(int deliveryRequestID,int userID);
    Task<RequestResponse<ICollection<DRApplicationDTO>>> GetMyApplicationsAsync(int userID);
    Task<RequestResponse<string>> UpdateApplication(int id, UpdateApplicatioDTO updateApplicatioDTO);
    Task<RequestResponse<bool>> AddApplicationAsync(AddApplicationDTO addApplicationDTO, int userID);
}
