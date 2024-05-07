using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Net.Mail;
using Abp.Runtime.Session;
using Broker.Configuration.Dto;
using Broker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Configuration
{
    public class SettingAppService : ISettingAppService
    {
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<Setting, long> _settingRepository;
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public IAbpSession Session { get; set; }

        public SettingAppService(ISettingDefinitionManager settingDefinitionManager, ISettingManager settingManager, IRepository<Setting, long> settingRepository)
        {
            _settingDefinitionManager = settingDefinitionManager;
            _settingManager = settingManager;
            _settingRepository = settingRepository;
        }
        [AbpAuthorize]
        public async Task<GetSettingDefinitionsOutput> GetSettingDefinitions(GetSettingDefinitionsInput input)
        {
            //get definitions
            List<SettingDefinition> definitions = _settingDefinitionManager.GetAllSettingDefinitions()
                .Where(x => x.Scopes.HasFlag((SettingScopes)input.Scope))
                .ToList();
            IReadOnlyList<ISettingValue> values = null;
            List<ISettingValue> explicitValues = null;
            if ((SettingScopes)input.Scope == SettingScopes.User)
            {
                values = await _settingManager.GetAllSettingValuesAsync(SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User);
            }
            else if ((SettingScopes)input.Scope == SettingScopes.Tenant)
            {
                if (Session.TenantId.HasValue) // it's tenant
                {
                    values = await _settingManager.GetAllSettingValuesAsync(SettingScopes.Application | SettingScopes.Tenant);
                    //Get Explicit values for tenant email settings
                    explicitValues = (await _settingManager.GetAllSettingValuesAsync(SettingScopes.Tenant)).Where(sv => sv.Name.Contains("Abp.Net.Mail")).ToList();
                }
                else // it's Tenancy Owner
                {
                    values = await _settingManager.GetAllSettingValuesAsync(SettingScopes.Application);
                }
            }
            return new GetSettingDefinitionsOutput() { Items = definitions, Values = values.ToList(), ExplicitValues = explicitValues };
        }

        [AbpAuthorize]
        // save tenant or application settings 
        public async Task SaveSettings(SaveSettingsInput input)
        {
            foreach (SettingDto setting in input.Settings)
            {
                if (Session.TenantId.HasValue) // it's tenant
                    await _settingManager.ChangeSettingForTenantAsync(Session.TenantId.Value, setting.Name, setting.Value);
                else // it's Tenancy Owner
                    await _settingManager.ChangeSettingForApplicationAsync(setting.Name, setting.Value);
            }
        }


        public async Task<MailData> GetMailSetting()
        {
            var mailSettingsData = new MailData();
            var host = await _settingRepository.FirstOrDefaultAsync(x => x.Name == EmailSettingNames.Smtp.Host);
            if (host == null)
                mailSettingsData.Host = await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host);
            else
                mailSettingsData.Host = host.Value;
            var password = await _settingRepository.FirstOrDefaultAsync(x => x.Name == EmailSettingNames.Smtp.Password);
            if (password == null)
                mailSettingsData.Password = await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);
            else
                mailSettingsData.Password = password.Value;
            var port = await _settingRepository.FirstOrDefaultAsync(x => x.Name == EmailSettingNames.Smtp.Port);
            if (port == null)
                mailSettingsData.Port = Convert.ToInt32(await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Port));
            else
                mailSettingsData.Port = Convert.ToInt32(port.Value);
            var userName = await _settingRepository.FirstOrDefaultAsync(x => x.Name == EmailSettingNames.Smtp.UserName);
            if (userName == null)
                mailSettingsData.Sender = await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName);
            else
                mailSettingsData.Sender = userName.Value;
            var enableSsl = await _settingRepository.FirstOrDefaultAsync(x => x.Name == EmailSettingNames.Smtp.EnableSsl);
            if (enableSsl == null)
                mailSettingsData.EnableSsl = Convert.ToBoolean(await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.EnableSsl));
            else
                mailSettingsData.EnableSsl = Convert.ToBoolean(enableSsl.Value);
            var useDefaultCredentials = await _settingRepository.FirstOrDefaultAsync(x => x.Name == EmailSettingNames.Smtp.UseDefaultCredentials);
            if (useDefaultCredentials == null)
                mailSettingsData.UseDefaultCredentials = Convert.ToBoolean(await _settingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UseDefaultCredentials));
            else
                mailSettingsData.UseDefaultCredentials = Convert.ToBoolean(useDefaultCredentials.Value);

            return mailSettingsData;
        }


        public async Task<string> GetSettingValueByKey(GetSystemDefinitionInput input)
        {
            try
            {
                var setting = await _settingRepository.FirstOrDefaultAsync(x => x.Name == input.Key);
                if (setting != null)
                    return setting.Value;
                return await _settingManager.GetSettingValueForTenantAsync(input.Key, 1);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        
    }
}
