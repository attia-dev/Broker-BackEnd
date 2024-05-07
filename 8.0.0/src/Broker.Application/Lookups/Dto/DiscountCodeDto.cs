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
    [AutoMapFrom(typeof(DiscountCode))]
    public class DiscountCodeDto : FullAuditedEntityDto<int>
    {
        public string Code { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? FixedAmount { get; set; }
        public bool IsPublish { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto LastModifierUser { get; set; }
        public UserDto DeleterUser { get; set; }
    }
    public class GetDiscountCodesInput : DataTableInputDto
    {
    }
    public class PagedDiscountCodeResultRequestDto : PagedResultRequestDto
    {

    }
    public class GetDiscountCodeByCodeInput
    {
        public string? Code { get; set; }
    }
}
