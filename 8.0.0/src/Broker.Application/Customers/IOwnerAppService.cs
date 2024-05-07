using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Customers.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Customers
{
    public interface IOwnerAppService : IApplicationService
    {
        Task<DataTableOutputDto<OwnerDto>> IsPaged(GetOwnerInput input);
        Task<OwnerDto> Manage(OwnerDto input);
        Task Delete(EntityDto<long> input);
        Task<OwnerDto> GetById(EntityDto<long> input);
        Task<OwnerDto> GetByUserId(EntityDto<long> input);
        Task<GetOwnerOutput> GetAll(PagedOwnerResultRequestDto input);
        Task<OwnerDto> UpdateOwnerFromMobile(OwnerDto input);

    }
}
