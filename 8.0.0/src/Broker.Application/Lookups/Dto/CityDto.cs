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
    [AutoMapFrom(typeof(City))]
    public class CityDto : FullAuditedEntityDto<int>
    {
        public long GovernorateId { get; set; }
        public GovernorateDto Governorate { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsEnabled { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto LastModifierUser { get; set; }
        public UserDto DeleterUser { get; set; }

    }
    public class GetCityInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class PagedCityResultRequestDto : PagedResultRequestDto
    {
        public long? GovernorateId { get; set; }
        public string Name { get; set; }
    }
}
