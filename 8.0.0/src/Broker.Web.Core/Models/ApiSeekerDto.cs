using Abp.Application.Services.Dto;
using Broker.Lookups.Dto;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApiSeekerDto : EntityDto<long>
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

    public class GetAllSeekersOutput
    {
        public GetAllSeekersOutput()
        {
            seekers = new List<ApiSeekerDto>();
        }
        public List<ApiSeekerDto> seekers { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllSeekersInput
    {
        public string Keyword { get; set; }

    }

    public class ApiCreateSeekerDto 
    {
       // public string SecondMobile { get; set; }
       // public string Avatar { get; set; }
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

    public class ApiCreateSeekerOut
    {
        public ApiCreateSeekerOut()
        {
        }
        public long? SeekerId { get; set; }
        public object Seeker { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }


    public class ApiSeekerDetailsDto
    {
        public string SecondMobile { get; set; }
        public bool IsWhatsApped { get; set; }
        public long? Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Avatar { get; set; }
    }
    public class ApiSeekerDetailsDtoOut
    {
        public ApiSeekerDetailsDtoOut()
        {
            Details = new ApiSeekerDetailsDto();
        }
        public ApiSeekerDetailsDto Details { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiSeekerDetailsInput
    {
        public long? UserId { get; set; }
    }

    public class ApiUpdateSeekerDto 
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

    public class ApiUpdateSeekerOut
    {
        public ApiUpdateSeekerOut()
        {
        }
        public long? SeekerId { get; set; }
        public object Seeker { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
