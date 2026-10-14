using Store.Core.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Core.Entities
{
    [Table("Tbl_OperationBreakdown")]
    public class OperationBreakdown : BaseEntity
    {

        [ForeignKey(nameof(Operation))]
        public Guid OperationId { get; set; }

        public virtual Operation Operation { get; set; }

        public DenominationKind DenominationKind { get; set; }

        public decimal DenominationValue { get; set; }

        public int Quantity { get; set; }
    }
}