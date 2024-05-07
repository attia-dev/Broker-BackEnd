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
    [Table("AdvertisementBookings")]
    public class AdvertisementBooking : FullAuditedEntity<long, User>
    {
        public DateTime BookingDate { get; set; }
        public long AdvertisementId { get; set; }
        [ForeignKey("AdvertisementId")]
        public Advertisement Advertisement { get; set; }

        public static AdvertisementBooking Create(
        DateTime BookingDate, long AdvertisementId
        )
        {
            var AdvertisementBooking = new AdvertisementBooking
            {
                BookingDate = BookingDate,
                AdvertisementId= AdvertisementId
            };
            return AdvertisementBooking;
        }

    }
}
