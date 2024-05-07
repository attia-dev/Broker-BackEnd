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

namespace Broker.Payments
{
    [Table("Wallets")]
    public class Wallet : FullAuditedEntity<long, User>
    {
        public TransactionType Type { get; set; }
        public decimal? Amount { get; set; }
        public int? Points { get; set; }
        public DateTime TransactionTime { get; set; }
        public long CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public static Wallet Create(TransactionType Type, decimal Amount, int Points, DateTime TransactionTime, long CompanyId)
        {
            var Wallet = new Wallet
            {
                Type = Type,
                Amount = Amount,
                Points = Points,
                TransactionTime = TransactionTime,
                CompanyId = CompanyId,
            };
            return Wallet;
        }
    }
}
