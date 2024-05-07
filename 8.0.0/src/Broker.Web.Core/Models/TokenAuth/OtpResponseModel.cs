using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;

namespace Broker.Models.TokenAuth
{
    public class OtpResponseModel
    {
        public string Message { get; set; }
        public string Otp { get; set; }
        public bool IsSuccess { get; set; }
    }
    public class OtpWessageResponseModel
    {
        public string Message { get; set; }
        public string Link { get; set; }
        public string Otp { get; set; }
        public bool IsSuccess { get; set; }
    }
    public class OtpRequestModel
    {
        public string PhoneNumber { get; set; }
    }
}
