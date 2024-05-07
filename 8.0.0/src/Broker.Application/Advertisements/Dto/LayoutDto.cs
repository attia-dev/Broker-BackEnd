using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Helpers;
using Broker.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Advertisements.Dto
{
    [AutoMapFrom(typeof(Layout))]
    public class LayoutDto : FullAuditedEntityDto<long>
    {
        public string Avatar { get; set; }
        public long AdvertisementId { get; set; }
        //public AdvertisementDto Advertisement { get; set; }
    }
}
