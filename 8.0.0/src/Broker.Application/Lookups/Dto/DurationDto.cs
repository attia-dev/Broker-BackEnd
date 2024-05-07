using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Datatable.Dtos;
using Broker.Helpers;
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
    [AutoMapFrom(typeof(Duration))]
    public class DurationDto : FullAuditedEntityDto<int>
    {
        public int Period { get; set; }
        public decimal Amount { get; set; }
       // public BuildingType? Type { get; set; }
        public bool IsPublish { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto LastModifierUser { get; set; }
        public UserDto DeleterUser { get; set; }
        public List<BuildingType> BuildingTypes { get; set; }
        public List<DurationBuildingTypeShowDto> DurationBuildingTypes { get; set; }

    }
    public class GetDurationInput : DataTableInputDto
    {
    }
    public class PagedDurationResultRequestDto : PagedResultRequestDto
    {
        public BuildingType? Type { get; set; }
        public bool? IsPublish { get; set; }
    }
    [AutoMapFrom(typeof(DurationBuildingType))]
    public class DurationBuildingTypeShowDto : FullAuditedEntityDto<long>
    {
        public long DurationId { get; set; }
        public BuildingType Type { get; set; }

    }
}
