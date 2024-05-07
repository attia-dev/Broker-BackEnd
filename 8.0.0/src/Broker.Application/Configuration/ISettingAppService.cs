using Abp.Application.Services;
using Broker.Configuration.Dto;
using Broker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Configuration
{
    public interface ISettingAppService : IApplicationService
    {
        Task<GetSettingDefinitionsOutput> GetSettingDefinitions(GetSettingDefinitionsInput input);
        Task<MailData> GetMailSetting();
        Task SaveSettings(SaveSettingsInput input);
        Task<string> GetSettingValueByKey(GetSystemDefinitionInput input);
    }
}
