using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApiBrokerPersonDto : EntityDto<long>
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

    public class GetAllBrokerPersonsOutput
    {
        public GetAllBrokerPersonsOutput()
        {
            brokers = new List<ApiBrokerPersonDto>();
        }
        public List<ApiBrokerPersonDto> brokers { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllBrokerPersonsInput
    {
        public string Keyword { get; set; }

    }

    public class ApiCreateBrokerPersonDto 
    {
        //public string SecondMobile { get; set; }
       // public string Avatar { get; set; }
       // public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserPassword { get; set; }
       // public DateTime? BirthDate { get; set; }
       // public string AboutAr { get; set; }
       // public string AboutEn { get; set; }

    }

    public class ApiCreateBrokerPersonOut
    {
        public ApiCreateBrokerPersonOut()
        {
        }
        public long? BrokerPersonId { get; set; }
        public object BrokerPerson { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }



    public class ApiBrokerPersonDetailsDto
    {
        public string SecondMobile { get; set; }
        public bool IsWhatsApp { get; set; }
        public long? Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Avatar { get; set; }
    }
    public class ApiBrokerPersonDetailsDtoOut
    {
        public ApiBrokerPersonDetailsDtoOut()
        {
            Details = new ApiBrokerPersonDetailsDto();
        }
        public ApiBrokerPersonDetailsDto Details { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiBrokerPersonDetailsInput
    {
        public long? UserId { get; set; }
    }


    public class ApiUpdateBrokerPersonDto 
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

    public class ApiUpdateBrokerPersonOut
    {
        public ApiUpdateBrokerPersonOut()
        {
        }
        public long? BrokerPersonId { get; set; }
        public object BrokerPerson { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

}
