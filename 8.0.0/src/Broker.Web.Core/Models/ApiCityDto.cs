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
    public class ApiCityDto : EntityDto<long>
    {
        public string Name { get; set; }
        //public long? GovernorateId { get; set; }
        //public bool IsEnabled { get; set; }
        //public GovernorateDto Governorate { get; set; }

    }

    public class GetAllCitiesOutput
    {
        public GetAllCitiesOutput()
        {
            cities= new List<ApiCityDto>();
        }
        public List<ApiCityDto> cities { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllCitiesInput
    {
        public string Keyword { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }
        public long? GovernorateId { get; set; }

    }
}
