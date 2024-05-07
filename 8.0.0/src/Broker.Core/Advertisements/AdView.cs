using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Advertisements
{
    [Table("AdViews")]
    public class AdView : FullAuditedEntity<long, User>
    {
        public string DeviceToken { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public long AdvertisementId { get; set; }
        //[ForeignKey("AdvertisementId")]
        //public Advertisement Advertisement { get; set; }

        public static AdView Create(/*string DeviceToken,*/long UserId, long AdvertisementId)
        {
            var AdView = new AdView
            {
               // DeviceToken= DeviceToken,
                UserId = UserId,
                AdvertisementId = AdvertisementId
            };
            return AdView;
        }

    }
}
