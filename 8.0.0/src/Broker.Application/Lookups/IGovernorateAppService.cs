using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    public interface IGovernorateAppService : IApplicationService
    {
        Task<DataTableOutputDto<GovernorateDto>> IsPaged(GetGovernoratesInput input);
        Task<GetGovernoratesOutput> GetAll(PagedGovernorateResultRequestDto input);
        Task<GovernorateDto> GetById(EntityDto<long> input);
        Task<GovernorateDto> Manage(GovernorateDto input);
        Task Delete(EntityDto<long> input);

    }
}
