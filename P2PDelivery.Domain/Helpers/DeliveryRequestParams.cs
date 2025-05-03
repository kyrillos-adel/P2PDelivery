using P2PDelivery.Domain.Enums;

namespace P2PDelivery.Domain.Helpers
{
    public class DeliveryRequestParams
    {
        private const int MaxPageSize = 30;
        private int _pageSize = 5;
        private int _pageNumber = 1;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value < 1) ? 1 : value;
        }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? Title { get; set; }
        public List<DeliveryRequestStatus>? Status { get; set; } = new List<DeliveryRequestStatus> { }; // e.g. Pending, Accepted, Completed, Cancelled, Delivered
        public string? PickUpLocation { get; set; }
        public string? DropOffLocation { get; set; }
        public DateTime? StartPickUpDate { get; set; }
        public double StartPrice { get; set; }

    }
}
