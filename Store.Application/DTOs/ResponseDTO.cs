using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Application.DTOs
{
    public class ResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
