using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Application.DTOs
{
    public class ResponseOperationDTO
    {
        public decimal TotalAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public List<DenominationBreakdownDTO> ChangeBreakdown { get; set; } = new List<DenominationBreakdownDTO>();
    }
}
