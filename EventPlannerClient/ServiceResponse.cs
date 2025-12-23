using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient
{
    public class ServiceResponse<T>
    {
        public bool IsSuccessed { get; set; }
        public T? Result { get; set; }
        public string? ErrorMessage { get; set; }
        public int? ErrorCode { get; set; }
    }
}
