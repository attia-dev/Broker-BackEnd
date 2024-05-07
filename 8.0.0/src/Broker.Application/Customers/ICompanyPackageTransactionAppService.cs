using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Advertisements.Dto;
using Broker.Customers.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Customers
{
    public interface ICompanyPackageTransactionAppService : IApplicationService
    {
        Task<PagedResultDto<CompanyPackageTransactionDto>> GetAll(PagedCompanyPackageTransactionResultRequestDto input);
        Task<CompanyPackageTransactionDto> GetById(EntityDto<long> input);
        Task<CompanyPackageTransactionDto> Manage(CompanyPackageTransactionDto input);
        Task Delete(EntityDto<long> input);

    }
}
