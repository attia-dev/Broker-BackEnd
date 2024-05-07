using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Lookups;
using Broker.Lookups.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Advertisements.Dto
{
    [AutoMapFrom(typeof(AdvertisementDecoration))]
    public class AdvertisementDecorationDto : FullAuditedEntityDto<long>
    {
        public int DecorationId { get; set; }
        public DefinitionDto Decoration { get; set; }
        public long? AdvertisementId { get; set; }
        public AdvertisementDto Advertisement { get; set; }
    }

}
