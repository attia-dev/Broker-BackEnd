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
    [AutoMapFrom(typeof(Governorate))]
    public class GovernorateDto : FullAuditedEntityDto<int>
    {
        public long CountryId { get; set; }
        public CountryDto Country { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto LastModifierUser { get; set; }
        public UserDto DeleterUser { get; set; }
    }
    public class GetGovernoratesInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class PagedGovernorateResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
        public long? CountryId { get; set; }
    }
    public class GetGovernoratesOutput
    {
        public List<GovernorateDto> Governorates { get; set; }
        public string Error { get; set; }
    }
}
