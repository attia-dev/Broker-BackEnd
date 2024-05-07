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
    [Table("AdvertisementDocuments")]
    public class AdvertisementDocument : FullAuditedEntity<long, User>
    {
       public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Definition Document { get; set; }
        public long AdvertisementId { get; set; }
        [ForeignKey("AdvertisementId")]
        public Advertisement Advertisement { get; set; }

        public static AdvertisementDocument Create(
       int DocumentId, long AdvertisementId
       )
        {
            var AdvertisementDocument = new AdvertisementDocument
            {
                DocumentId = DocumentId,
                AdvertisementId = AdvertisementId
            };
            return AdvertisementDocument;
        }

    }
}
