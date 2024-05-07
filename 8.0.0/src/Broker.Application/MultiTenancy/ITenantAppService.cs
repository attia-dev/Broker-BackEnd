using Abp.Application.Services;
using Broker.MultiTenancy.Dto;

namespace Broker.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

