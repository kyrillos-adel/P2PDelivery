using Microsoft.AspNetCore.Identity;
using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
using System.Security.Claims;

namespace P2PDelivery.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly IRepository<DRApplication> _applicationRepository;
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;
    public ApplicationService(IRepository<DRApplication> applicationRepository,IAuthService authService,UserManager<User> userManager)
    {
        _applicationRepository = applicationRepository;
        _authService = authService;
        _userManager = userManager;
    }

    public UserManager<User> UserManager { get; }

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


    public async Task<RequestResponse<ICollection<DRApplicationDTO>>> GetMyApplicationsAsync(int userID)
    {
        var applications = _applicationRepository.GetAll(x => x.UserId == userID)
            .Select(x => new DRApplicationDTO
            {
                Id = x.Id,
                ApplicationStatus = x.ApplicationStatus.ToString(),
                Date = x.Date,
                OfferedPrice = x.OfferedPrice,
                DeliveryRequestId = x.DeliveryRequestId,
                DeliveryTitle = x.DeliveryRequest.Title
            }).ToList();
        return RequestResponse<ICollection<DRApplicationDTO>>.Success(applications);
    }

    public async Task<RequestResponse<string>> UpdateApplication(int id ,UpdateApplicatioDTO updateApplicatioDTO)
    {
       var application = await _applicationRepository.GetByIDAsync(id);
        if (application == null)
            return RequestResponse<string>.Failure(ErrorCode.ApplicationNotExist, "Application not exist");
        else
        {
            application.OfferedPrice = updateApplicatioDTO.OfferedPrice;
            application.UpdatedAt = DateTime.Now;
            var username = _authService.respond.UserName;
            var user = await _userManager.FindByNameAsync(username);
            var userid = user.Id;
            application.UpdatedBy = userid;
            await _applicationRepository.SaveChangesAsync();
            return RequestResponse<string>.Success("Updated done");
        }

    }
}
