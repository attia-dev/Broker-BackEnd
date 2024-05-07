using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Customers.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Customers
{
    public interface IBrokerPersonAppService : IApplicationService
    {
        Task<DataTableOutputDto<BrokerPersonDto>> IsPaged(GetBrokerPersonInput input);
        Task<BrokerPersonDto> Manage(BrokerPersonDto input);
        Task Delete(EntityDto<long> input);
        Task<BrokerPersonDto> GetById(EntityDto<long> input);
        Task<BrokerPersonDto> GetByUserId(EntityDto<long> input);
        Task<GetBrokerPersonOutput> GetAll(PagedBrokerPersonResultRequestDto input);
        Task<BrokerPersonDto> UpdateBrokerPersonFromMobile(BrokerPersonDto input);

    }
}
