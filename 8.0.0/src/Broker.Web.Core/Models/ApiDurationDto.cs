using Abp.Application.Services.Dto;
using Broker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Broker.Models
{
    public class ApiDurationDto : EntityDto<int>
    {
        public int Period { get; set; }
        public decimal Amount { get; set; }
        //public BuildingType? Type { get; set; }
         public List<string> Types { get; set; }
        public bool IsPublish { get; set; }
    }

    public class GetAllDurationsOutput
    {
        public GetAllDurationsOutput()
        {
            Durations = new List<ApiDurationDto>();
        }
        public List<ApiDurationDto> Durations { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllDurationsInput
    {
        public BuildingType Type { get; set; }
        public bool? IsPublish { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }

    }
}
