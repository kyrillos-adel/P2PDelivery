using AutoMapper;
using Microsoft.AspNetCore.Identity;
using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Application.DTOs.MatchDTO;
using P2PDelivery.Application.DTOs.Notifications;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly IRepository<DRApplication> _applicationRepository;
    private readonly IRepository<Match> _matchrepository;
    private readonly INotificationService _notificationService;
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IDeliveryRequestService _deliveryRequestService;
    private readonly ImatchService _matchService;

    public ApplicationService(IRepository<DRApplication> applicationRepository,IAuthService authService,
                            UserManager<User> userManager, IMapper mapper, IDeliveryRequestService deliveryRequestService,
                            ImatchService matchservice, IRepository<Match> matchrepository, INotificationService notificationService)
    {
        _applicationRepository = applicationRepository;
        _authService = authService;
        _userManager = userManager;
        _mapper = mapper;
        _deliveryRequestService = deliveryRequestService;
        _matchService = matchservice;
        _matchrepository = matchrepository;
        _notificationService = notificationService;
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
            application.UpdatedBy = application.UserId;
            await _applicationRepository.SaveChangesAsync();
            return RequestResponse<string>.Success("Updated done");
        }
    }
    public async Task<RequestResponse<bool>> AddApplicationAsync(AddApplicationDTO addApplicationDTO, int userID)
    {

        var requester = await _deliveryRequestService.GetDeliveryRequestByIdAsync(addApplicationDTO.DeliveryRequestId);
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
        await _notificationService.CreateAsync(new NotificationDto
        {
            UserId = requester.Data.UserId,
            Message = $" {requester.Data.Title} has a new application",
        });
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
    public async Task<RequestResponse<bool>> UpdateApplicationStatuseAsync(int deId,int id ,int status,int userid)
    {
        var ismatch = _matchService.GetByDelivery(deId);
        var application = await _applicationRepository.GetByIDAsync(id);
        if (application == null)
            return RequestResponse<bool>.Failure(ErrorCode.ApplicationNotExist, "Application not exist");

        else if (status == 1 && ismatch.Result == null)
        {
            MatchDTO matchDTO = new MatchDTO
            {
                Price = application.OfferedPrice,
                ApplicationId = id,
                DeliveryRequestId = deId
            };
            var match = await _matchService.Addmatch(matchDTO);
            application.UpdatedAt = DateTime.Now;
            application.UpdatedBy = userid;
            application.ApplicationStatus = (ApplicationStatus)status;
            await _applicationRepository.SaveChangesAsync();

            await _deliveryRequestService.updatestatuse(deId, 1);
            await _notificationService.CreateAsync(new NotificationDto
            {
                UserId = application.UserId,
                Message = $"Your application for delivary has been accepted.",
            });
            return RequestResponse<bool>.Success(true);
        }
        else if (status == 2 && ismatch.Result != null) 
        {
            await _matchService.deletematch(ismatch.Result.Id);
            application.UpdatedAt = DateTime.Now;
            application.UpdatedBy = userid;
            application.ApplicationStatus = (ApplicationStatus)status;
            await _applicationRepository.SaveChangesAsync();
            
            await _deliveryRequestService.updatestatuse(deId, 0);
            await _notificationService.CreateAsync(new NotificationDto
            {
                UserId = application.UserId,
                Message = $"Your application for delivary #{application.DeliveryRequest.Title} has been rejected.",
            });
            return RequestResponse<bool>.Success(true);
        }
        return RequestResponse<bool>.Success(true);
    }
}
