using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using Broker.Helpers;
using Broker.Lookups;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Customers
{
    [Table("Seekers")]
    public class Seeker : FullAuditedEntity<long, User>
    {
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string SecondMobile { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        public long? PackageId { get; set; }
        [ForeignKey("PackageId")]
        public Package Package { get; set; }
        public int? Balance { get; set; }
        public static Seeker Create(long UserId, string SecondMobile, string Avatar, bool IsActive )
        {
            var Seeker = new Seeker
            {
                UserId = UserId,
                SecondMobile = SecondMobile,
                Avatar = Avatar,
                IsActive = IsActive
            };
            return Seeker;
        }
    }
}
