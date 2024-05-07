using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;

namespace Broker.Models.TokenAuth
{
    public class PaymentRequestModel
    {
        public decimal amount { get; set; }
        public long userId { get; set; }
    }
    public class AcceptModel
    {
        public ProfileModel profile { get; set; }
        public string token { get; set; }
    }
    public class OrdersAcceptModel
    {
        public string id { get; set; }
    }
    public class ProfileModel
    {
        public string Id { get; set; }
    }
    public class TokenModel
    {
        public string token { get; set; }
    }
    public class PaymentResponseModel
    {
        public string Url { get; set; }
        public bool IsSuccess { get; set; }
    }
 
}
