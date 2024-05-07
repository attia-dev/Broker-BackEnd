
using Broker.Configuration;
using Broker.Helpers;
using Broker.Lookups.Dto;
using Broker.Lookups;
using Broker.Models;
using Broker.Users.Dto;
using Broker.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Broker.Customers;
using Broker.Customers.Dto;
using Abp.Runtime.Session;
using Broker.RateUs;
using Broker.RateUs.Dto;

namespace Broker.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RateUsController : BrokerControllerBase
    {
        private readonly IRateAppService _rateAppService;
        private readonly IUserAppService _userAppService;
        private readonly IAbpSession _abpSession;
        public RateUsController(IRateAppService rateAppService, IUserAppService userAppService, IAbpSession abpSession)
        {
            _rateAppService = rateAppService; ;
            this._userAppService = userAppService;
            _abpSession = abpSession;
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiCreateRateOut> CreateRate([FromBody] ApiCreateRateDto input)
        {
            try
            {
                var rateDto = new RateDto();
                rateDto.UserId = input.UserId;
                rateDto.UserRate = input.UserRate;

                var rate = await _rateAppService.Manage(rateDto);

                return new ApiCreateRateOut { RateId = rate.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCreateRateOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<GetAllRatesOutput> GetAllRates([FromBody] GetAllRatesInput input)
        {
            try
            {

                var rates = await _rateAppService.GetAll(
                    new PagedRateResultRequestDto { Name = input.Keyword, SkipCount = input.Start ?? 0, MaxResultCount = input.Length ?? 10 });
                List<ApiRateDto> rateDtos = new List<ApiRateDto>();
                if (rates != null)
                {
                    rateDtos = rates.Rates.Select(x => new ApiRateDto
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        UserRate = x.UserRate

                    }).ToList();
                }
                

                return new GetAllRatesOutput { Rates = rateDtos, Count= rateDtos.Count, Success = true };
            }
            catch (Exception ex)
            {
                return new GetAllRatesOutput { Error = ex.Message, Success = false };
            }
        }

    }
}
