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
    [AutoMapFrom(typeof(AdvertisementFacility))]
    public class AdvertisementFacilityDto : FullAuditedEntityDto<long>
    {
        public int FacilityId { get; set; }
        public DefinitionDto Facility { get; set; }
        public long? AdvertisementId { get; set; }
      // public AdvertisementDto Advertisement { get; set; }
    }
}
