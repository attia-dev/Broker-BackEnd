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
    [AutoMapFrom(typeof(Package))]
    public class PackageDto : FullAuditedEntityDto<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int Price { get; set; }
        public int Points { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto LastModifierUser { get; set; }
        public UserDto DeleterUser { get; set; }
    }
    public class GetPackagesInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class PagedPackageResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
    }
    public class GetPackagesOutput
    {
        public List<PackageDto> Packages { get; set; }
        public string Error { get; set; }
    }
}
