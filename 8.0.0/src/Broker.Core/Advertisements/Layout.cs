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
    [Table("Layouts")]
    public class Layout : FullAuditedEntity<long, User>
    {
        public string Avatar { get; set; }
        public long AdvertisementId { get; set; }
        [ForeignKey("AdvertisementId")]
        public Advertisement Advertisement { get; set; }

        public static Layout Create( string Avatar,  long AdvertisementId)
        {
            var Layout = new Layout
            {
                Avatar = Avatar,
                AdvertisementId = AdvertisementId
            };
            return Layout;
        }

    }
}
