using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Datatable.Dtos;
using Broker.Localization;
using Broker.Lookups;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Lookups.Dto
{
    [AutoMapFrom(typeof(Country))]
    public class CountryDto : FullAuditedEntityDto<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsEnabled { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto LastModifierUser { get; set; }
        public UserDto DeleterUser { get; set; }
    }
    public class GetCountriesInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class PagedCountryResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
    }
    public class GetCountriesOutput
    {
        public List<CountryDto> Countries { get; set; }
        public string Error { get; set; }
    }
}
