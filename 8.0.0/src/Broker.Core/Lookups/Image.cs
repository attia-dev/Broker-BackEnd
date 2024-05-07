using Abp.Domain.Entities.Auditing;
using Broker.Advertisements;
using Broker.Authorization.Users;
using Broker.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    [Table("Images")]
    public class Image : FullAuditedEntity<long, User>
    {
        public string Name { get; set; }
        public AvatarType Avatar { get; set; }
        // public long AdvertisementId { get; set; }
        // [ForeignKey("AdvertisementId")]
        // public Advertisement Advertisement { get; set; }
        public static Image Create(string Name , AvatarType Avatar/*, long AdvertisementId*/)
        {
            var Image = new Image
            {
                Name = Name,
                Avatar = Avatar,
               // AdvertisementId = AdvertisementId,
            };
            return Image;
        }
    }
}
