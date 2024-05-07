using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Broker.Configuration.Dto;

namespace Broker.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : BrokerAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
