using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Core.Entities
{
    [Table("Tbl_Operations")]
    public class Operation : BaseEntity
    {

        [ForeignKey(nameof(Cashier))]
        public Guid CashierId { get; set; }

        public virtual Cashier Cashier { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public decimal ReceivedAmount { get; set; }

        public decimal ChangeAmount { get; set; }

        public ICollection<OperationBreakdown> OperationItems { get; set; } = [];

    }
}