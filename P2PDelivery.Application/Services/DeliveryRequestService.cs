using Microsoft.EntityFrameworkCore;
using P2PDelivery.Application.DTOs.DeliveryRequestDTOs;
using AutoMapper;
using P2PDelivery.Application.DTOs;
using P2PDelivery.Application.Interfaces;
using P2PDelivery.Application.Interfaces.Services;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
using P2PDelivery.Application.DTOs.ApplicationDTOs;
using P2PDelivery.Domain.Enums;
using P2PDelivery.Domain.Helpers;
using P2PDelivery.Domain;


namespace P2PDelivery.Application.Services
{
    public class DeliveryRequestService : IDeliveryRequestService
    {
        private readonly IRepository<DeliveryRequest> _requestRepository;
        private readonly IMapper _mapper;

        public DeliveryRequestService(IRepository<DeliveryRequest> requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
        }

        public async Task<RequestResponse<DeliveryRequestDTO>> CreateDeliveryRequestAsync(CreateDeliveryRequestDTO dto)
        {
            var entity=_mapper.Map<DeliveryRequest>(dto);
            entity.Status = DeliveryRequestStatus.Pending;

            // Handle image saving
            if (dto.DRImage != null && dto.DRImage.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "deliveryRequest");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var uniqueFileName = $"{Guid.NewGuid()}_{dto.DRImage.FileName}";
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.DRImage.CopyToAsync(stream);
                }

                // Save relative path to entity
                entity.DRImageUrl = $"/images/deliveryRequest/{uniqueFileName}";
            }
            await _requestRepository.AddAsync(entity);

            await _requestRepository.SaveChangesAsync();


            var resultDto=_mapper.Map<DeliveryRequestDTO>(entity);
            return RequestResponse<DeliveryRequestDTO>.Success(resultDto,"Created Successfully");

        }

        public  async Task<RequestResponse<DeliveryRequestDTO>> GetDeliveryRequestByIdAsync(int id)
        {
            var entity = await _requestRepository.GetByIDAsync(id);
            if (entity==null)
            {
                return RequestResponse<DeliveryRequestDTO>.Failure(ErrorCode.None, "Delivery Request not found");
            }
            var dto = _mapper.Map<DeliveryRequestDTO>(entity);
            return RequestResponse<DeliveryRequestDTO>.Success(dto);
        }



        public async Task<RequestResponse<List<DeliveryRequestDTO>>> GetDeliveryRequestsByUserIdAsync(int userId)
        {
            var query = _requestRepository.GetAll(x => x.UserId == userId).Include(x=>x.User);
            var entities = await query.ToListAsync();

            if (entities == null || !entities.Any())
            {
                return RequestResponse<List<DeliveryRequestDTO>>.Failure(ErrorCode.None, "No delivery requests found for this user");
            }

            var dtos = _mapper.Map<List<DeliveryRequestDTO>>(entities);
            return RequestResponse<List<DeliveryRequestDTO>>.Success(dtos);
        }

        public async Task<RequestResponse<DeliveryRequestDTO>> DeleteDeliveryRequestAsync(int id)
        {
            var entity = await _requestRepository.GetByIDAsync(id);
            if (entity == null)
            {
                return RequestResponse<DeliveryRequestDTO>.Failure(ErrorCode.None, "Delivery Request not found");
            }
            _requestRepository.Delete(entity);
            await _requestRepository.SaveChangesAsync();
            var dto = _mapper.Map<DeliveryRequestDTO>(entity);
            return RequestResponse<DeliveryRequestDTO>.Success(dto, "Deleted Successfully");
        }
        public async Task<RequestResponse<PageList<DeliveryRequestDTO>>> GetAllDeliveryRequestsAsync(DeliveryRequestParams deliveryRequestParams, int UserID)
        {
            var requests = _requestRepository.GetAll();
            if(deliveryRequestParams.Title != null)
            {
                requests =  requests.Where(x => x.Title.Contains(deliveryRequestParams.Title));
            }
            
            if(deliveryRequestParams.Status.Count > 0 )
            {
                requests = requests.Where(x => deliveryRequestParams.Status.Contains(x.Status));
            }
            if(deliveryRequestParams.PickUpLocation  != null)
            {
                requests = requests.Where(x => x.PickUpLocation.Contains(deliveryRequestParams.PickUpLocation));
            }
            if (deliveryRequestParams.DropOffLocation != null)
            {
                requests = requests.Where(x => x.DropOffLocation.Contains(deliveryRequestParams.DropOffLocation));
            }
            if (deliveryRequestParams.StartPickUpDate != null)
            {
                requests = requests.Where(x => x.PickUpDate > deliveryRequestParams.StartPickUpDate);
            }
            if (deliveryRequestParams.StartPrice > 0)
            {
                requests = requests.Where(x => x.MinPrice > deliveryRequestParams.StartPrice);
            }
            requests = requests.OrderByDescending(x => x.CreatedAt);
            

            var result = _mapper.ProjectTo<DeliveryRequestDTO>(requests);
            var paginatedResult = await PageList<DeliveryRequestDTO>.CreateAsync(result, deliveryRequestParams.PageNumber, deliveryRequestParams.PageSize);
            paginatedResult.Data.ForEach(r => r.IsOwner = r.UserId == UserID);

            return RequestResponse<PageList<DeliveryRequestDTO>>.Success(paginatedResult);
        }





        public async Task<RequestResponse<DeliveryRequestDetailsDTO>> GetDeliveryRequestDetailsAsync(int deliveryId, int userID)
        {
            // validation
            if (!await IsDeliveryRequestExist(deliveryId))
            {
                return RequestResponse<DeliveryRequestDetailsDTO>.Failure(ErrorCode.DeliveryRequestNotExist, "This Delivery Request is not found.");
            }

            var deliveryRequestDTO = _requestRepository.GetAll(x => x.Id == deliveryId)
                .Select(x => new DeliveryRequestDetailsDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    DropOffLocation = x.DropOffLocation,
                    PickUpLocation = x.PickUpLocation,
                    PickUpDate = x.PickUpDate,
                    MaxPrice = x.MaxPrice,
                    MinPrice = x.MinPrice,
                    Status = x.Status.ToString(),
                    TotalWeight = x.TotalWeight,
                    UserName=x.User.FullName,
                    UserId = x.UserId,
                    DRImageUrl = x.DRImageUrl,
                    ProfileImageUrl = x.User.ProfileImageUrl,
                }).FirstOrDefault();

            if (deliveryRequestDTO.UserId == userID)
            {
                var response = _mapper.ProjectTo<ApplicationDTO>(_requestRepository.GetAll(x => x.Id == deliveryId)
                            .SelectMany(x => x.Applications.Where(a => !a.IsDeleted)))
                    .ToList();

                deliveryRequestDTO.IsOwner = true;    
                deliveryRequestDTO.ApplicationDTOs = response;
            }

            return RequestResponse<DeliveryRequestDetailsDTO>.Success(deliveryRequestDTO);
        }

        public async Task<bool> IsDeliveryRequestExist(int deliveryId)
        {
            var isExist = await _requestRepository.IsExistAsync(deliveryId);
            if (!isExist)
            {
                return false;
            }
            return true;
        }

        
        public async Task<RequestResponse<DeliveryRequest>> UpdateAsync(int id, DeliveryRequestUpdateDto deliveryRequestUpdateDto)
        {
            var deliveryRequest = await _requestRepository.GetByIDAsync(id);
            if (deliveryRequest == null)
                return RequestResponse<DeliveryRequest>.Failure(ErrorCode.DeliveryRequestNotExist,
                    "Delivery request not found");
            
            _mapper.Map(deliveryRequestUpdateDto, deliveryRequest);

            if (deliveryRequestUpdateDto.DRImage != null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(deliveryRequest.DRImageUrl))
                {
                    var oldImagePath = Path.Combine("wwwroot", deliveryRequest.DRImageUrl.TrimStart('/'));

                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }



                // Generate unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(deliveryRequestUpdateDto.DRImage.FileName);
                var folderPath = Path.Combine("wwwroot", "images", "deliveryRequest");

                // Ensure the directory exists
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await deliveryRequestUpdateDto.DRImage.CopyToAsync(stream);
                }

                // Set image URL (relative path or full URL based on your needs)
                deliveryRequest.DRImageUrl = $"/images/deliveryRequest/{fileName}";
            }



            await _requestRepository.SaveChangesAsync();
            
            return RequestResponse<DeliveryRequest>.Success(deliveryRequest, "Successfully updated delivery request");
        }

        public async Task<RequestResponse<bool>> DeleteAsync(int id)
        {
            var deliveryRequest = await _requestRepository.GetByIDAsync(id);
            if (deliveryRequest == null)
                return RequestResponse<bool>.Failure(ErrorCode.DeliveryRequestNotExist,
                    "Delivery request not found");
            
            _requestRepository.Delete(deliveryRequest);

            await _requestRepository.SaveChangesAsync();

            return RequestResponse<bool>.Success(true, "Successfully deleted delivery request");
        }

        public async Task<bool> updatestatuse(int id ,int statuse)
        {
          var respond=  await _requestRepository.GetByIDAsync(id);
            if (respond == null)
                return false;
            else
            {
                respond.Status = (DeliveryRequestStatus)statuse;
                await _requestRepository.SaveChangesAsync();
                return true;
            }
               

        }
    }
}
