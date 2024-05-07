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
    [AutoMapFrom(typeof(Definition))]
    public class DefinitionDto : FullAuditedEntityDto<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Avatar { get; set; }
        public int? Value { get; set; }
        public DefinitionTypes Type { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto LastModifierUser { get; set; }
        public UserDto DeleterUser { get; set; }
    }
    public class GetDefinitionsInput : DataTableInputDto
    {
        public string Name { get; set; }
        public DefinitionTypes? EnumCategory { get; set; }
    }
    public class PagedDefinitionResultRequestDto : PagedResultRequestDto
    {
        public string keyword { get; set; }
        public DefinitionTypes EnumCategory { get; set; }
    }
    public class GetDefinitionsOutput
    {
        public List<DefinitionDto> Definitions { get; set; }
        public string Error { get; set; }
    }
}
