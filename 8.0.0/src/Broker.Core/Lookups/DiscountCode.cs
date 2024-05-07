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
    [Table("DiscountCodes")]
    public class DiscountCode : FullAuditedEntity<long, User>
    {
        public string Code { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? FixedAmount { get; set; }
        public bool IsPublish { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public static DiscountCode Create(string Code, decimal? Percentage, decimal? FixedAmount,bool IsPublish
            , DateTime? From, DateTime? To)
        {
            var DiscountCode = new DiscountCode
            {
                Code = Code,
                Percentage = Percentage,
                FixedAmount = FixedAmount,
                IsPublish = IsPublish,
                From=From,
                To=To,
            };
            return DiscountCode;
        }
    }
}
