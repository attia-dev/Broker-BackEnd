using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;


namespace Broker.Advertisements
{

    [Table("AdFavorites")]
    public class AdFavorite : FullAuditedEntity<long, User>
    {
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public long AdvertisementId { get; set; }
        [ForeignKey("AdvertisementId")]
        public Advertisement Advertisement { get; set; }

        public static AdFavorite Create(long UserId, long AdvertisementId)
        {
            var AdFavorite = new AdFavorite
            {
                UserId = UserId,
                AdvertisementId = AdvertisementId
            };
            return AdFavorite;
        }

    }
}
