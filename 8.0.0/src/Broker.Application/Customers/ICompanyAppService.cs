using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Customers.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    public interface ICompanyAppService : IApplicationService
    {
        Task<DataTableOutputDto<CompanyDto>> IsPaged(GetCompanyInput input);
        Task<CompanyDto> Manage(CompanyDto input);
        Task Delete(EntityDto<long> input);
        Task<CompanyDto> GetById(EntityDto<long> input);
        Task<CompanyDto> GetByUserId(EntityDto<long> input);
        Task<GetCompanyOutput> GetAll(PagedCompanyResultRequestDto input);
        Task<CompanyDto> UpdateCompanyFromMobile(CompanyDto input);

    }
}
