using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApiCountryDto : EntityDto<int>
    {
        public string Name { get; set; }
       

    }

    public class GetAllCountriesOutput
    {
        public GetAllCountriesOutput()
        {
            Countries = new List<ApiCountryDto>();
        }
        public List<ApiCountryDto> Countries { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllCountriesInput
    {
        public string Keyword { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }

    }
}
