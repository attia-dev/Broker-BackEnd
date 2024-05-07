using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Advertisements.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    public interface IAdvertisementBookingAppService : IApplicationService
    {
        Task<PagedResultDto<AdvertisementBookingDto>> GetAll(PagedAdvertisementBookingResultRequestDto input);
        Task<AdvertisementBookingDto> GetById(EntityDto<long> input);
        Task<AdvertisementBookingDto> Manage(AdvertisementBookingDto input);
        Task Delete(EntityDto<long> input);

    }
}
