using System.ComponentModel;

namespace Store.Web.Models
{
    public class PaymentViewModel
    {
        [DisplayName("Total Amount")]
        public decimal? TotalAmount { get; set; }

        [DisplayName("Receive Amount")]
        public decimal? ReceivedAmount { get; set; }
    }
}