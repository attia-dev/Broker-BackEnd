using Abp.Application.Services.Dto;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApiRateDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public int UserRate { get; set; }

    }
    public class ApiCreateRateOut
    {
        public ApiCreateRateOut()
        {
        }
        public long? RateId { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiCreateRateDto
    {
        public long UserId { get; set; }
        public int UserRate { get; set; }

    }

    public class GetAllRatesOutput
    {
        public GetAllRatesOutput()
        {
            Rates = new List<ApiRateDto>();
        }
        public List<ApiRateDto> Rates { get; set; }
        public long? Count { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllRatesInput
    {
        public string Keyword { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }

    }
}
