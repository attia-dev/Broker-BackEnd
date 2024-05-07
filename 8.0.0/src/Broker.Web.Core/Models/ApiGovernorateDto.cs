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
    public class ApiGovernorateDto : EntityDto<long>
    {
        public string Name { get; set; }
        //public long CountryId { get; set; }
        //public CountryDto Country { get; set; }

    }

    public class GetAllGovernoratesOutput
    {
        public GetAllGovernoratesOutput()
        {
            governorates = new List<ApiGovernorateDto>();
        }
        public List<ApiGovernorateDto> governorates { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllGovernoratesInput
    {
        public string Keyword { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }

    }
}
