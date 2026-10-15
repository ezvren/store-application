using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Application.DTOs
{
    public class ResponseOperationHistoryDTO
    {
        public DateTime Date { get; set; }
        public List<ResponseOperationDTO> Operations { get; set; } = [];
    }
}
