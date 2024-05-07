using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Broker.Helpers;
using Broker.Authorization.Users;
using Broker.Lookups;
using static Broker.Authorization.PermissionNames;
using Broker.Advertisements;

namespace Broker.Notifications
{
    [Table("Notifications")]
    public class Notification : CreationAuditedEntity<long, User>
    {
        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public long? AdId { get; set; }
        [ForeignKey("AdvertisementId")]
        public Advertisement Advertisement { get; set; }
 
        public NotificationTypes Type { get; set; }
        public string  Description { get; set; }
        public bool? IsRead { get; set; }

        public long? BrokerId { get; set; }
        public long? SeekerId { get; set; }
        public long? OwnerId { get; set; }
        public long? CompanyId { get; set; }

        public static Notification Create(long UserId, long? AdId,
            NotificationTypes Type, string Description, bool? IsRead, long? BrokerId,
            long? SeekerId, long? OwnerId, long? CompanyId)
        {
            var Notification = new Notification
            {
                UserId = UserId,
                AdId = AdId,
                Type = Type,
                Description = Description,
                IsRead = IsRead,
                BrokerId= BrokerId,
                SeekerId= SeekerId,
                OwnerId= OwnerId,
                CompanyId= CompanyId

            };
            return Notification;
        }

    }
}