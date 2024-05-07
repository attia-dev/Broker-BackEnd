using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;


namespace Broker.SocialContacts
{

    [Table("SocialContacts")]
    public class SocialContact : FullAuditedEntity<int, User>
    {
        public string SocialName { get; set; }
        public string SocialValue { get; set; }
        public string Avatar { get; set; }
        public static SocialContact Create(string SocialName, string SocialValue, string Avatar)
        {
            var SocialContact = new SocialContact
            {
                SocialName = SocialName,
                SocialValue = SocialValue ,
                Avatar=Avatar
            };
            return SocialContact;
        }
    }
}
