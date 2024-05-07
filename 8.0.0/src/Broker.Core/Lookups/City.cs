using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    [Table("Cities")]
    public class City : FullAuditedEntity<long, User>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public long GovernorateId { get; set; }
        [ForeignKey("GovernorateId")]
        public Governorate Governorate { get; set; }
        public bool IsEnabled { get; set; }
        public static City Create(string NameAr, string NameEn, long GovernorateId, bool IsEnabled)
        {
            var City = new City
            {
                NameAr = NameAr,
                NameEn = NameEn,
                GovernorateId = GovernorateId,
                IsEnabled = IsEnabled
            };
            return City;
        }
    }
}
