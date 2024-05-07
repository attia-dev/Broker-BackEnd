using Abp.Domain.Entities.Auditing;
using Broker.Advertisements;
using Broker.Authorization.Users;
using Broker.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Customers
{

    [Table("CompanyPackageTransactions")]
    public class CompanyPackageTransaction : FullAuditedEntity<long, User>
    {
        public DateTime SubscribeDate { get; set; }
        public long PackageId { get; set; }
        [ForeignKey("PackageId")]
        public Package Package { get; set; }

        public long CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public static CompanyPackageTransaction Create ( long PackageId, long CompanyId )
        {
            var CompanyPackageTransaction = new CompanyPackageTransaction
            {
                SubscribeDate = DateTime.Now,
                PackageId = PackageId,
                CompanyId= CompanyId
            };
            return CompanyPackageTransaction;
        }

    }
}
