using Store.Application.DTOs;

namespace Store.Web.Models
{
    public class OperationViewModel
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public List<DenominationBreakdownDTO> ChangeBreakdown { get; set; } = [];
    }
}
