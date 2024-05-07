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
using Broker.Advertisements.Dto;

namespace Broker.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AdvertisementBookingController : BrokerControllerBase
    {
        private readonly IAdvertisementBookingAppService _advertisementBookingAppService;
        private readonly IUserAppService _userAppService;
        private readonly IAbpSession _abpSession;
        public AdvertisementBookingController(IAdvertisementBookingAppService advertisementBookingAppService, IUserAppService userAppService, IAbpSession abpSession)
        {
            _advertisementBookingAppService = advertisementBookingAppService; ;
            this._userAppService = userAppService;
            _abpSession = abpSession;
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiCreateAdvertisementBookingOut> CreateAdvertisementBooking([FromBody] ApiCreateAdvertisementBookingDto input)
        {
            try
            {
                var advertisementBookingDto = new AdvertisementBookingDto();
                advertisementBookingDto.BookingDate = input.BookingDate;
                advertisementBookingDto.AdvertisementId = input.AdvertisementId;

                var advertisementBooking = await _advertisementBookingAppService.Manage(advertisementBookingDto);

                return new ApiCreateAdvertisementBookingOut { AdvertisementBookingId = advertisementBooking.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCreateAdvertisementBookingOut { Error = ex.Message, Success = false };
            }
        }

        //[HttpPost]
        //[Language(BrokerConsts.RequestHeaders.Language)]

        //public async Task<ApiAdvertisementBookingDetailsDtoOut> GetAdvertisementBookingDetails(ApiAdvertisementBookingDetailsInput input)
        //{
        //    try
        //    {
        //        ApiAdvertisementBookingDetailsDto advertisementBookingDetails = new ApiAdvertisementBookingDetailsDto();
        //        if (!input.UserId.HasValue || input.UserId == 0)
        //            input.UserId = _abpSession.UserId.Value;
        //        var data = await _advertisementBookingAppService.GetByUserId(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.UserId.Value });
        //        advertisementBookingDetails.Id = data.Id;
        //        advertisementBookingDetails.Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? data.UserName : data.UserSurname;
           
        //        advertisementBookingDetails.PhoneNumber = data.UserPhoneNumber;
        //        advertisementBookingDetails.EmailAddress = data.UserEmailAddress;
        //        advertisementBookingDetails.SecondMobile = data.SecondMobile;
        //        advertisementBookingDetails.IsWhatsApped = true;
        //        advertisementBookingDetails.Avatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{data.Avatar ?? "notFound.png"}";

        //        return new ApiAdvertisementBookingDetailsDtoOut { Details = advertisementBookingDetails, Success = true };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiAdvertisementBookingDetailsDtoOut { Error = ex.Message, Success = false };
        //    }
        //}


        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiUpdateAdvertisementBookingOut> UpdateAdvertisementBooking([FromBody] ApiUpdateAdvertisementBookingDto input)
        {
            try
            {
                var advertisementBookingDto = new AdvertisementBookingDto();
                advertisementBookingDto.Id = input.Id;
                advertisementBookingDto.BookingDate = input.BookingDate;
                advertisementBookingDto.AdvertisementId = input.AdvertisementId;

                var advertisementBooking = await _advertisementBookingAppService.Manage(advertisementBookingDto);

                return new ApiUpdateAdvertisementBookingOut { AdvertisementBookingId = advertisementBooking.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiUpdateAdvertisementBookingOut { Error = ex.Message, Success = false };
            }
        }
        [HttpPost]
        public async Task<ApiDeleteAdvertisementBookingOut> DeleteAdvertisementBooking([FromBody] ApiDeleteAdvertisementBookingDto input)
        {
            try
            {
                await _advertisementBookingAppService.Delete(new Abp.Application.Services.Dto.EntityDto<long>(input.AdvertisementBookingId));
                return new ApiDeleteAdvertisementBookingOut { Success = true, msg = L("SuccessfullyDeleted") };
            }
            catch (Exception ex)
            {
                return new ApiDeleteAdvertisementBookingOut { Error = ex.Message, Success = false };
            }
        }


    }
}
