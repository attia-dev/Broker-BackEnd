using Abp.Application.Services.Dto;
using Broker.Helpers;
using Broker.Lookups.Dto;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{

    public class ApiCommonyDto
    {
        public string PhoneOrEmail { get; set; }


    }

    public class ApiSocialContactDto 
    {
        public string SocialName { get; set; }
        public string SocialValue { get; set; }
        public string Avatar { get; set; }

    }

    public class GetAllSocialContactsOutput
    {
        public GetAllSocialContactsOutput()
        {
            SocialContacts = new List<ApiSocialContactDto>();
        }
        public List<ApiSocialContactDto> SocialContacts { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllSocialContactsInput
    {
        public string Keyword { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }

    }

    public class ApiPackageDto : EntityDto<int>
    {
        public string Name { get; set; }
        public int? Price { get; set; }
        public int? Points { get; set; }

    }

    public class GetAllPackagesOutput
    {
        public GetAllPackagesOutput()
        {
            Packages = new List<ApiPackageDto>();
        }
        public List<ApiPackageDto> Packages { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllPackagesInput
    {
        public string Keyword { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }

    }
    public class GetDiscontCodeInput
    {
        public string Keyword { get; set; }

    }
    public class GetDiscontCodeOutput
    {
        public ApiDiscountDto Discount { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }
    public class ApiDiscountDto
    {
        public decimal? Percentage { get; set; }
       // public decimal? FixedAmount { get; set; }

    }

    public class ApiNotificationsForUserInput
    {
        public long? UserId { get; set; }
        //public int? Start { get; set; }
        //public int? Length { get; set; }
        
    }

    public class ApiUserNotificationDto : EntityDto<long>
    {
        public string Description { get; set; }
        public string Date { get; set; }

    }

    public class ApiUserNotificationsOut
    {
        public ApiUserNotificationsOut()
        {
            Notifications = new List<ApiUserNotificationDto>();
        }
        public List<ApiUserNotificationDto> Notifications { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

}
