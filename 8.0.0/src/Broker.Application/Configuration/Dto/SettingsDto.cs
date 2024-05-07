using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Configuration.Dto
{
    [AutoMapFrom(typeof(Setting))]
    public class SettingDto : AuditedEntityDto
    {
        public string Name { get; set; }
        public int? TenantId { get; set; }
        public long? UserId { get; set; }
        public string Value { get; set; }
    }
    public class GetSettingDefinitionsInput
    {
        public int Scope { get; set; }
    }

    public class GetSettingDefinitionsOutput
    {
        public List<SettingDefinition> Items { get; set; }
        public List<ISettingValue> Values { get; set; }
        public List<ISettingValue> ExplicitValues { get; set; }
    }

    public class SaveSettingsInput
    {
        [Required]
        public List<SettingDto> Settings { get; set; }
    }

    public class GetSystemDefinitionInput
    {
        //public Language? Language { get; set; }
        [Required]
        public string Key { get; set; }
    }

}
