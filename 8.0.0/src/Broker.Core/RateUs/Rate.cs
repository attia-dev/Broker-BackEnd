using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using Broker.Customers;
using Broker.Helpers;
using Broker.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.RateUs
{
    [Table("Rates")]
    public class Rate : FullAuditedEntity<long, User>
    {
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserRate { get; set; }
        public static Rate Create(long UserId, int UserRate)
        {
            var Rate = new Rate
            {
                UserId= UserId,
                UserRate=UserRate
            };
            return Rate;
        }
    }
}
