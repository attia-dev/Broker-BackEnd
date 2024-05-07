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
    [Table("Packages")]
    public class Package : FullAuditedEntity<long, User>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int Price { get; set; }
        public int Points { get; set; }
        public static Package Create(string NameAr, string NameEn, int Price, int Points)
        {
            var Package = new Package
            {
                NameAr = NameAr,
                NameEn = NameEn,
                Price = Price,
                Points = Points,
            };
            return Package;
        }
    }
}
