using Broker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApiChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ApiChangePasswordOut
    {
        public ApiChangePasswordOut()
        {
        }
        public string msg { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }
    public class ApiDeleteAccountOut
    {
        public ApiDeleteAccountOut()
        {
        }
        public string msg { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }
    public class ApiDeleteAccountDto
    {
        public long UserId { get; set; }
       
    }
}
