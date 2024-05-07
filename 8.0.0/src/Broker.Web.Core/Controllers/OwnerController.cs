using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Broker.Configuration;
using Broker.Customers;
using Broker.Customers.Dto;
using Broker.Helpers;
using Broker.Models;
using Microsoft.AspNetCore.Mvc;
namespace Broker.Controllers
{

    [Route("api/[controller]/[action]")]
    public class OwnerController : BrokerControllerBase
    {
        private readonly IOwnerAppService _ownerAppService;
        private readonly IAbpSession _abpSession;
        public OwnerController(IOwnerAppService ownerAppService,IAbpSession abpSession)
        {
            _ownerAppService = ownerAppService;
            _abpSession = abpSession;
        }

        //createOwner
        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiCreateOwnerOut> CreateOwner([FromBody] ApiCreateOwnerDto input)
        {
            try
            {
                var ownerDto = new OwnerDto();
                ownerDto.UserName = input.UserName;
                ownerDto.UserSurname = input.UserSurname;
                ownerDto.UserEmailAddress = input.UserEmailAddress;
                ownerDto.UserPhoneNumber = input.UserPhoneNumber;
                ownerDto.UserPassword = input.UserPassword;

              //  ownerDto.BirthDate = input.BirthDate;
              //  ownerDto.AboutAr = input.AboutAr;
              //  ownerDto.AboutEn = input.AboutEn;
              //  ownerDto.Avatar = input.Avatar;
              //  ownerDto.SecondMobile = input.SecondMobile;
              //  ownerDto.IsActive = input.IsActive;

                var owner = await _ownerAppService.Manage(ownerDto);

                return new ApiCreateOwnerOut { OwnerId = owner.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCreateOwnerOut { Error = ex.Message, Success = false };
            }
        }


        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]

        public async Task<ApiOwnerDetailsDtoOut> GetOwnerDetails(ApiOwnerDetailsInput input)
        {
            try
            {
                ApiOwnerDetailsDto ownerDetails = new ApiOwnerDetailsDto();
                if (!input.UserId.HasValue || input.UserId == 0)
                    input.UserId = _abpSession.UserId.Value;
                var data = await _ownerAppService.GetByUserId(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.UserId.Value });
                ownerDetails.Id = data.Id;
                ownerDetails.Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? data.UserName : data.UserSurname;
              
                ownerDetails.PhoneNumber = data.UserPhoneNumber;
                ownerDetails.EmailAddress = data.UserEmailAddress;
                ownerDetails.SecondMobile = data.SecondMobile;
                ownerDetails.IsWhatsApped = data.User.IsWhatsApped;
                ownerDetails.Avatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{data.Avatar ?? "usericon.png"}";

                return new ApiOwnerDetailsDtoOut { Details = ownerDetails, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiOwnerDetailsDtoOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiUpdateOwnerOut> UpdateOwner([FromBody] ApiUpdateOwnerDto input)
        {
            try
            {
                var ownerDto = new OwnerDto();
                ownerDto.Id = input.Id;
                ownerDto.UserName = input.UserName;
                ownerDto.UserSurname = input.UserSurname;
                ownerDto.UserEmailAddress = input.UserEmailAddress;
                ownerDto.UserPhoneNumber = input.UserPhoneNumber;
                ownerDto.Avatar = input.Avatar;
                ownerDto.SecondMobile = input.SecondMobile;
                ownerDto.UserIsWhatsApped = input.IsWhatsApped;

                ownerDto.PackageId = input.PackageId;
                ownerDto.Balance = input.Balance;

                var owner = await _ownerAppService.UpdateOwnerFromMobile(ownerDto);

                return new ApiUpdateOwnerOut { OwnerId = owner.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiUpdateOwnerOut { Error = ex.Message, Success = false };
            }
        }

    }
}
