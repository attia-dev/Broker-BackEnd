using System.Threading.Tasks;
using Broker.Configuration.Dto;

namespace Broker.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
