using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    public interface ICityAppService : IApplicationService
    {
        Task<DataTableOutputDto<CityDto>> IsPaged(GetCityInput input);
        Task<CityDto> Manage(CityDto input);
        Task Delete(EntityDto<long> input);
        Task<CityDto> GetById(EntityDto<long> input);
        Task<PagedResultDto<CityDto>> GetAll(PagedCityResultRequestDto input);

    }
}
