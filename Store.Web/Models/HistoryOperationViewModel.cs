namespace Store.Web.Models
{
    public class HistoryOperationViewModel
    {
        public DateTime Date { get; set; }
        public List<OperationViewModel> Operations { get; set; } = [];
    }
}
