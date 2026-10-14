using Store.Core.Enum;

namespace Store.Application.DTOs
{
    public class DenominationBreakdownDTO
    {
        public decimal DenominationValue { get; set; }
        public int Quantity { get; set; }
        public DenominationKind DenominationKind { get; set; }
    }
}
