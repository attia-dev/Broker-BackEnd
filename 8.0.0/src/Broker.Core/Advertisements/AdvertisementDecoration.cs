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
    [Table("AdvertisementDecorations")]
    public class AdvertisementDecoration : FullAuditedEntity<long, User>
    {
       public int DecorationId { get; set; }
        [ForeignKey("DecorationId")]
        public Definition Decoration { get; set; }
        public long AdvertisementId { get; set; }
        [ForeignKey("AdvertisementId")]
        public Advertisement Advertisement { get; set; }

        public static AdvertisementDecoration Create(
        int DecorationId, long AdvertisementId
        )
        {
            var AdvertisementDecoration = new AdvertisementDecoration
            {
                DecorationId= DecorationId,
                AdvertisementId= AdvertisementId
            };
            return AdvertisementDecoration;
        }

    }
}
