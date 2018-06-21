using Domain.Registries.Requests.Enums;

namespace Domain.MobileApi.Models
{
    public class RequestListModel
    {
        public long Id { get; set; }
        public int Priority { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }
}
