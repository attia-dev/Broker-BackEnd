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

namespace Broker.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BrokerPersonController : BrokerControllerBase
    {
        private readonly IBrokerPersonAppService _brokerPersonAppService;
        private readonly IAbpSession _abpSession;
        public BrokerPersonController(IBrokerPersonAppService brokerPersonAppService, IAbpSession abpSession)
        {
            _brokerPersonAppService = brokerPersonAppService;
            _abpSession = abpSession;

        }

        //createBrokerPerson
        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiCreateBrokerPersonOut> CreateBrokerPerson([FromBody] ApiCreateBrokerPersonDto input)
        {
            try
            {
                var brokerPersonDto = new BrokerPersonDto();
                brokerPersonDto.UserName = input.UserName;
                brokerPersonDto.UserSurname = input.UserSurname;
                brokerPersonDto.UserEmailAddress = input.UserEmailAddress;
                brokerPersonDto.UserPhoneNumber = input.UserPhoneNumber;
                brokerPersonDto.UserPassword = input.UserPassword;

             //   brokerPersonDto.BirthDate = input.BirthDate;
             //   brokerPersonDto.AboutAr = input.AboutAr;
             //   brokerPersonDto.AboutEn = input.AboutEn;
              //  brokerPersonDto.Avatar = input.Avatar;
              //  brokerPersonDto.SecondMobile = input.SecondMobile;
             //   brokerPersonDto.IsActive = input.IsActive;

                var brokerPerson = await _brokerPersonAppService.Manage(brokerPersonDto);

                return new ApiCreateBrokerPersonOut { BrokerPersonId = brokerPerson.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCreateBrokerPersonOut { Error = ex.Message, Success = false };
            }
        }


        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]

        public async Task<ApiBrokerPersonDetailsDtoOut> GetBrokerPersonDetails(ApiBrokerPersonDetailsInput input)
        {
            try
            {
                ApiBrokerPersonDetailsDto brokerPersonDetails = new ApiBrokerPersonDetailsDto();
                if (!input.UserId.HasValue || input.UserId == 0)
                    input.UserId = _abpSession.UserId.Value;
                var data = await _brokerPersonAppService.GetByUserId(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.UserId.Value });
                brokerPersonDetails.Id = data.Id;
                brokerPersonDetails.Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? data.UserName : data.UserSurname;
                
                brokerPersonDetails.PhoneNumber = data.UserPhoneNumber;
                brokerPersonDetails.EmailAddress = data.UserEmailAddress;
                brokerPersonDetails.SecondMobile = data.SecondMobile;
                brokerPersonDetails.IsWhatsApp = data.User.IsWhatsApped;
                brokerPersonDetails.Avatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{data.Avatar ?? "usericon.png"}";


                return new ApiBrokerPersonDetailsDtoOut { Details = brokerPersonDetails, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiBrokerPersonDetailsDtoOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiUpdateBrokerPersonOut> UpdateBrokerPerson([FromBody] ApiUpdateBrokerPersonDto input)
        {
            try
            {
                var brokerPersonDto = new BrokerPersonDto();
                brokerPersonDto.Id = input.Id;
                brokerPersonDto.UserName = input.UserName;
                brokerPersonDto.UserSurname = input.UserSurname;
                brokerPersonDto.UserEmailAddress = input.UserEmailAddress;
                brokerPersonDto.UserPhoneNumber = input.UserPhoneNumber;
                brokerPersonDto.Avatar = input.Avatar;
                brokerPersonDto.SecondMobile = input.SecondMobile;
                brokerPersonDto.UserIsWhatsApped = input.IsWhatsApped;

                brokerPersonDto.PackageId = input.PackageId;
                brokerPersonDto.Balance = input.Balance;
                var brokerPerson = await _brokerPersonAppService.UpdateBrokerPersonFromMobile(brokerPersonDto);

                return new ApiUpdateBrokerPersonOut { BrokerPersonId = brokerPerson.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiUpdateBrokerPersonOut { Error = ex.Message, Success = false };
            }
        }

    }
}
