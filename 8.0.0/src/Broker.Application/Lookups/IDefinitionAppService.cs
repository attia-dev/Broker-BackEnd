using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    public interface IDefinitionAppService : IApplicationService
    {
        Task<DataTableOutputDto<DefinitionDto>> IsPaged(GetDefinitionsInput input);
        Task<GetDefinitionsOutput> GetAll(PagedDefinitionResultRequestDto input);
        Task<DefinitionDto> GetById(EntityDto<int> input);
        Task<DefinitionDto> Manage(DefinitionDto input);
        Task Delete(EntityDto<int> input);

    }
}
