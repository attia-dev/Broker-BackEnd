using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    public interface ICountryAppService : IApplicationService
    {
        Task<DataTableOutputDto<CountryDto>> IsPaged(GetCountriesInput input);
        Task<GetCountriesOutput> GetAll(PagedCountryResultRequestDto input);
        Task<CountryDto> GetById(EntityDto<long> input);
        Task<CountryDto> Manage(CountryDto input);
        Task Delete(EntityDto<long> input);

    }
}
