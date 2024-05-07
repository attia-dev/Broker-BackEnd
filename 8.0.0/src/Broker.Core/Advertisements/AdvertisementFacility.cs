using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using Broker.Helpers;
using Broker.Lookups;
using Castle.MicroKernel.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Advertisements
{
    [Table("AdvertisementFacilities")]
    public class AdvertisementFacility : FullAuditedEntity<long, User>
    {
       public int FacilityId { get; set; }
        [ForeignKey("FacilityId")]
        public Definition Facility { get; set; }
        public long AdvertisementId { get; set; }
        [ForeignKey("AdvertisementId")]
        public Advertisement Advertisement { get; set; }

        public static AdvertisementFacility Create(
      int FacilityId, long AdvertisementId
      )
        {
            var AdvertisementFacility = new AdvertisementFacility
            {
                FacilityId = FacilityId,
                AdvertisementId = AdvertisementId
            };
            return AdvertisementFacility;
        }

    }
}
