using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    public interface IDurationAppService : IApplicationService
    {
        Task<DataTableOutputDto<DurationDto>> IsPaged(GetDurationInput input);
        Task<PagedResultDto<DurationDto>> GetAll(PagedDurationResultRequestDto input);
        Task<DurationDto> GetById(EntityDto<long> input);
        Task<DurationDto> Manage(DurationDto input);
        Task Delete(EntityDto<long> input);

    }
}
