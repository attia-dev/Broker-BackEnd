using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Customers.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Customers
{
    public interface ISeekerAppService : IApplicationService
    {
        Task<DataTableOutputDto<SeekerDto>> IsPaged(GetSeekerInput input);
        Task<SeekerDto> Manage(SeekerDto input);
        Task Delete(EntityDto<long> input);
        Task<SeekerDto> GetById(EntityDto<long> input);
        Task<GetSeekerOutput> GetAll(PagedSeekerResultRequestDto input);
        Task<SeekerDto> UpdateSeekerFromMobile(SeekerDto input);
        Task<SeekerDto> GetByUserId(EntityDto<long> input);

    }
}
