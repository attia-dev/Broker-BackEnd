using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Advertisements;
using Broker.Lookups;
using Broker.Lookups.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Customers.Dto
{
    [AutoMapFrom(typeof(CompanyPackageTransaction))]
    public class CompanyPackageTransactionDto : FullAuditedEntityDto<long>
    {
        public DateTime SubscribeDate { get; set; }
        public long PackageId { get; set; }
        public PackageDto Package { get; set; }
        public long CompanyId { get; set; }
        public CompanyDto Company { get; set; }
    }

    public class PagedCompanyPackageTransactionResultRequestDto : PagedResultRequestDto
    {
        public long? CompanyId { get; set; }
    }
}
