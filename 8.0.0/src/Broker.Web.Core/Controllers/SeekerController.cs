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
    public class SeekerController : BrokerControllerBase
    {
        private readonly ISeekerAppService _seekerAppService;
        private readonly IUserAppService _userAppService;
        private readonly IAbpSession _abpSession;
        public SeekerController(ISeekerAppService seekerAppService, IUserAppService userAppService, IAbpSession abpSession)
        {
            _seekerAppService = seekerAppService; ;
            this._userAppService = userAppService;
            _abpSession = abpSession;
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiCreateSeekerOut> CreateSeeker([FromBody] ApiCreateSeekerDto input)
        {
            try
            {
                var seekerDto = new SeekerDto();
                seekerDto.UserName = input.UserName;
                seekerDto.UserSurname = input.UserSurname;
                seekerDto.UserEmailAddress = input.UserEmailAddress;
                seekerDto.UserPhoneNumber = input.UserPhoneNumber;
                seekerDto.UserPassword = input.UserPassword;

              //  seekerDto.BirthDate = input.BirthDate;
              //  seekerDto.AboutAr = input.AboutAr;
              //  seekerDto.AboutEn = input.AboutEn;
              //  seekerDto.Avatar = input.Avatar;
             //   seekerDto.SecondMobile = input.SecondMobile;
             //   seekerDto.IsActive = input.IsActive;

                var seeker = await _seekerAppService.Manage(seekerDto);

                return new ApiCreateSeekerOut { SeekerId = seeker.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCreateSeekerOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]

        public async Task<ApiSeekerDetailsDtoOut> GetSeekerDetails(ApiSeekerDetailsInput input)
        {
            try
            {
                ApiSeekerDetailsDto seekerDetails = new ApiSeekerDetailsDto();
                if (!input.UserId.HasValue || input.UserId == 0)
                    input.UserId = _abpSession.UserId.Value;
                var data = await _seekerAppService.GetByUserId(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.UserId.Value });
                seekerDetails.Id = data.Id;
                seekerDetails.Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? data.UserName : data.UserSurname;
           
                seekerDetails.PhoneNumber = data.UserPhoneNumber;
                seekerDetails.EmailAddress = data.UserEmailAddress;
                seekerDetails.SecondMobile = data.SecondMobile;
                seekerDetails.IsWhatsApped = data.User.IsWhatsApped;
                seekerDetails.Avatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{data.Avatar ?? "usericon.png"}";

                return new ApiSeekerDetailsDtoOut { Details = seekerDetails, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiSeekerDetailsDtoOut { Error = ex.Message, Success = false };
            }
        }


        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiUpdateSeekerOut> UpdateSeeker([FromBody] ApiUpdateSeekerDto input)
        {
            try
            {
                var seekerDto = new SeekerDto();
                seekerDto.Id = input.Id;
                seekerDto.UserName = input.UserName;
                seekerDto.UserSurname = input.UserSurname;
                seekerDto.UserEmailAddress = input.UserEmailAddress;
                seekerDto.UserPhoneNumber = input.UserPhoneNumber;
                seekerDto.Avatar = input.Avatar;
                seekerDto.SecondMobile = input.SecondMobile;
                seekerDto.PackageId = input.PackageId;
                seekerDto.Balance = input.Balance;
                seekerDto.UserIsWhatsApped = input.IsWhatsApped;

                var seeker = await _seekerAppService.UpdateSeekerFromMobile(seekerDto);

                return new ApiUpdateSeekerOut { SeekerId = seeker.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiUpdateSeekerOut { Error = ex.Message, Success = false };
            }
        }

    }
}
