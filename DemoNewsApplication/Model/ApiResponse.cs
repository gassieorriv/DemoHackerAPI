using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoNewsApplication.Model
{
    public class ApiResponse<T>
    {
        public T data { get; set; }
        public string errorMessage { get; set; }
        public bool isSuccessful { get; set; }
        public string friendlyMessage { get; set; }
    }
}
