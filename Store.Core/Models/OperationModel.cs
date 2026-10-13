using System;
using System.Collections.Generic;

namespace Store.Core.Models
{
    public class OperationModel
    {
        public Guid Id { get; set; }

        public Guid CashierId { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal ReceivedAmount { get; set; }

        public decimal ChangeAmount { get; set; }
    }
}
