using Domain.Registries.Requests.Enums;
using System;

namespace Domain.Registries.Requests.Models
{
    public class RequestListFilters
    {
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public long? ContainerTypeId { get; set; }
        public long? CarId { get; set; }
        public RequestType? RequestType { get; set; }
        public RequestStatus? RequestStatus { get; set; }
        public IsPaid? IsPaids { get; set; }
        public long? DriverId { get; set; }
        public string CustomerFilter { get; set; }
        public string AddressFilter { get; set; }
        public string ContactPersonPhoneFilter { get; set; }
        public int? RequestId { get; set; }
    }
}
