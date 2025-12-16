using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerLibrary
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
        public string? ErrorCode { get; set; }

        public static ApiResponse<T> Ok(T data) =>
            new() {Success = true, Data = data };

        public static ApiResponse<T> Fail(string error, string errorCode) =>
            new() {Success = false, Error = error, ErrorCode = errorCode };
    }
}
