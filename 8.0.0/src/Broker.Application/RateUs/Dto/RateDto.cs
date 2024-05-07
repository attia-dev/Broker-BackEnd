using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Authorization.Users;
using Broker.Customers;
using Broker.Datatable.Dtos;
using Broker.Localization;
using Broker.Lookups;
using Broker.RateUs;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.RateUs.Dto
{
    [AutoMapFrom(typeof(Rate))]
    public class RateDto : FullAuditedEntityDto<long>
    {
        public long UserId { get; set; }
        public UserDto User { get; set; }
        public int UserRate { get; set; }


    }
    public class GetRatesInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class PagedRateResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
    }
    public class GetRatesOutput
    {
        public List<RateDto> Rates { get; set; }
        public string Error { get; set; }
    }
}
