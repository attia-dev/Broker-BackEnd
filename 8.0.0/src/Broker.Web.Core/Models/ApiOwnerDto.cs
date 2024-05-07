using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApiOwnerDto : EntityDto<long>
    {
        public string SecondMobile { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserPassword { get; set; }
        public DateTime? BirthDate { get; set; }
        public string AboutAr { get; set; }
        public string AboutEn { get; set; }

    }

    public class GetAllOwnersOutput
    {
        public GetAllOwnersOutput()
        {
            owners = new List<ApiOwnerDto>();
        }
        public List<ApiOwnerDto> owners { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllOwnersInput
    {
        public string Keyword { get; set; }

    }

    public class ApiCreateOwnerDto 
    {
      //  public string SecondMobile { get; set; }
     //   public string Avatar { get; set; }
       // public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserPassword { get; set; }
       // public DateTime? BirthDate { get; set; }
       // public string AboutAr { get; set; }
      //  public string AboutEn { get; set; }

    }

    public class ApiCreateOwnerOut
    {
        public ApiCreateOwnerOut()
        {
        }
        public long? OwnerId { get; set; }
        public object Owner { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiUpdateOwnerDto 
    {
        public long Id { get; set; }
        public string SecondMobile { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPhoneNumber { get; set; }
        public bool IsWhatsApped { get; set; }

        public long? PackageId { get; set; }
        public int? Balance { get; set; }
    }

    public class ApiUpdateOwnerOut
    {
        public ApiUpdateOwnerOut()
        {
        }
        public long? OwnerId { get; set; }
        public object Owner { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiOwnerDetailsDto
    {
        public string SecondMobile { get; set; }
        public bool IsWhatsApped { get; set; }
        public long? Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Avatar { get; set; }
    }
    public class ApiOwnerDetailsDtoOut
    {
        public ApiOwnerDetailsDtoOut()
        {
            Details = new ApiOwnerDetailsDto();
        }
        public ApiOwnerDetailsDto Details { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiOwnerDetailsInput
    {
        public long? UserId { get; set; }
    }
}
