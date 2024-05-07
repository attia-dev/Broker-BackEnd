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
    [Table("Countries")]
    public class Country : FullAuditedEntity<long, User>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsEnabled { get; set; }
        public static Country Create(string NameAr, string NameEn, bool IsEnabled)
        {
            var Country = new Country
            {
                NameAr = NameAr,
                NameEn = NameEn,
                IsEnabled = IsEnabled
            };
            return Country;
        }
    }
}
