using System.Threading.Tasks;
using Abp.Application.Services;
using Broker.Sessions.Dto;

namespace Broker.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
