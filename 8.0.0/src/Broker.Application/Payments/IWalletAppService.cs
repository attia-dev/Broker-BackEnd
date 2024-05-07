using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using Broker.Payments.Dto;
using System.Threading.Tasks;

namespace Broker.Payments
{
    public interface IWalletAppService : IApplicationService
    {
        Task<DataTableOutputDto<WalletDto>> IsPaged(GetWalletsInput input);
        Task<GetWalletsOutput> GetAll(PagedWalletResultRequestDto input);
        Task<WalletDto> GetById(EntityDto<long> input);
        Task<WalletDto> Manage(WalletDto input);
        Task Delete(EntityDto<long> input);

    }
}
