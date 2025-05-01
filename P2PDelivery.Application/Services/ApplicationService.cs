using AutoMapper;
using Microsoft.AspNetCore.Identity;
using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly IRepository<DRApplication> _applicationRepository;
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IDeliveryRequestService _deliveryRequestService;
    public ApplicationService(IRepository<DRApplication> applicationRepository,IAuthService authService,
                            UserManager<User> userManager, IMapper mapper, IDeliveryRequestService deliveryRequestService)
    {
        _applicationRepository = applicationRepository;
        _authService = authService;
        _userManager = userManager;
        _mapper = mapper;
        _deliveryRequestService = deliveryRequestService;
    }

    public UserManager<User> UserManager { get; }

    public async Task<RequestResponse<ICollection<ApplicationDTO>>> GetApplicationByRequestAsync(int deliveryRequestID, int userID)
    {
        var isRequestExist = await _deliveryRequestService.IsDeliveryRequestExist(deliveryRequestID);
        if (!isRequestExist)
        {
            return RequestResponse<ICollection<ApplicationDTO>>.Failure(ErrorCode.DeliveryRequestNotExist, "This Delivery Request is not Exist");
        }
        var requestUserID = _applicationRepository.GetAll(x => x.DeliveryRequestId == deliveryRequestID)
                .Select(x => x.DeliveryRequest.UserId).FirstOrDefault();
        if(requestUserID != userID)
        {
            return RequestResponse<ICollection<ApplicationDTO>>.Failure(ErrorCode.Unauthorized, "You don't have permission to access applications for delivery requests that you don't own.");
        }

        var applications = _mapper.ProjectTo<ApplicationDTO>(_applicationRepository.GetAll(x => x.DeliveryRequestId == deliveryRequestID)).ToList();
            
            //.Select(x => new ApplicationDTO{
            //    ApplicationStatus = x.ApplicationStatus.ToString(),
            //    Date = x.Date,
            //    OfferedPrice = x.OfferedPrice,
            //    UserId = x.UserId,
            //    UserName=x.User.FullName
            //}).ToList();

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

    public async Task<RequestResponse<bool>> AddApplicationAsync(AddApplicationDTO addApplicationDTO, int userID)
    {
        var isRequestExist = await _deliveryRequestService.IsDeliveryRequestExist(addApplicationDTO.DeliveryRequestId);
        if (!isRequestExist)
        {
            return RequestResponse<bool>.Failure(ErrorCode.DeliveryRequestNotExist, "This Delivery Request is not Exist");
        }
        var application = _mapper.Map<DRApplication>(addApplicationDTO);
        application.UserId= userID;
        application.Date = DateTime.Now;
        await _applicationRepository.AddAsync(application);
        await _applicationRepository.SaveChangesAsync();

        return RequestResponse<bool>.Success(true);

    }

    public async Task<RequestResponse<bool>> DeleteApplicationAsync(int id,int userid)
    {
        var application = await _applicationRepository.GetByIDAsync(id);
        if (application == null)
            return RequestResponse<bool>.Failure(ErrorCode.ApplicationNotExist, "Application not exist");
        else
        {
            application.DeletedAt = DateTime.Now;
            application.IsDeleted = true;
            application.DeletedBy = userid;
            await _applicationRepository.SaveChangesAsync();
            return RequestResponse<bool>.Success(true);
        }

    }
}
