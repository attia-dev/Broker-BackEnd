using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    public interface IDiscountCodeAppService : IApplicationService
    {
        Task<DataTableOutputDto<DiscountCodeDto>> IsPaged(GetDiscountCodesInput input);
        Task<DiscountCodeDto> Manage(DiscountCodeDto input);
        Task Delete(EntityDto<long> input);
        Task<DiscountCodeDto> GetById(EntityDto<long> input);
        Task<PagedResultDto<DiscountCodeDto>> GetAll(PagedDiscountCodeResultRequestDto input);
        Task<DiscountCodeDto> GetByCode(GetDiscountCodeByCodeInput input);
    }
}
