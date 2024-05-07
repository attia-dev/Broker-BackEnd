using System.Threading.Tasks;
using Abp.Application.Services;
using Broker.Authorization.Accounts.Dto;

namespace Broker.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
