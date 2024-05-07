using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApiDefinitionDto : EntityDto<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public int? Value { get; set; }
    }
    public class GetAllDefinitionsOutput
    {
        public GetAllDefinitionsOutput()
        {
            Definitions = new List<ApiDefinitionDto>();
        }
        public List<ApiDefinitionDto> Definitions { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }
    public class GetAllDefinitionsInput
    {
        public string Keyword { get; set; }
        public int Type { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }

    }

   
}
