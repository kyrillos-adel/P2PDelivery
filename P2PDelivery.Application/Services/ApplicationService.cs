using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly IRepository<DRApplication> _applicationRepository;
    public ApplicationService(IRepository<DRApplication> applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }
    public async Task<RequestResponse<ICollection<ApplicationDTO>>> GetApplicationByRequestAsync(int deliveryRequestID)
    {
        
        var applications = _applicationRepository.GetAll(x => x.DeliveryRequestId == deliveryRequestID)
            .Select(x => new ApplicationDTO{
                ApplicationStatus = x.ApplicationStatus.ToString(),
                Date = x.Date,
                OfferedPrice = x.OfferedPrice,
                UserId = x.UserId,
                UserName=x.User.FullName
            }).ToList();

        return RequestResponse<ICollection<ApplicationDTO>>.Success(applications);
    }
}
