using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using Broker.Helpers;
using Broker.Lookups;
using Broker.Payments;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Customers
{
    [Table("Companies")]
    public class Company : FullAuditedEntity<long, User>
    {
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public bool IsSponser { get; set; }
        public bool IsActive { get; set; }

        public string SecondMobile { get; set; }
        public string Logo { get; set; }
        public string BWLogo { get; set; }
        public string CommericalAvatar { get; set; }
        //CompanyDetails
        public string About { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Snapchat { get; set; }
        public string Tiktok { get; set; }
        public string Website { get; set; }
       // public long WalletId { get; set; }
       // [ForeignKey("WalletId")]
       // public Wallet Wallet { get; set; }
        public long? PackageId { get; set; }
        [ForeignKey("PackageId")]
        public Package Package { get; set; }
        public int? Balance { get; set; }
        public static Company Create(long UserId, bool IsSponser, bool IsActive, string SecondMobile, string Logo, string BWLogo, string CommericalAvatar,
                                        string About, double? Latitude,double? Longitude , string Facebook, string Instagram,
                                             string Snapchat, string Tiktok, string Website, /*long WalletId,*/ long? PackageId)
        {
            var Company = new Company
            {
                UserId = UserId,
                SecondMobile = SecondMobile,
                Logo = Logo,
                BWLogo = BWLogo,
                CommericalAvatar = CommericalAvatar,
                About = About,
                Latitude = Latitude,
                Longitude = Longitude,
                Facebook = Facebook,
                Instagram = Instagram,
                Snapchat = Snapchat,
                Tiktok = Tiktok,
                Website = Website,
                IsSponser = IsSponser,
                IsActive = IsActive,
               // WalletId = WalletId,
                PackageId = PackageId,
            };
            return Company;
        }
    }
}
