using Abp.Runtime.Session;
using Broker.Helpers;
using Broker.Models;
using Broker.RateUs.Dto;
using Broker.RateUs;
using Broker.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.ContactUs;
using Broker.ContactUs.Dto;

namespace Broker.Controllers
{

    [Route("api/[controller]/[action]")]
    public class ContactUsController : BrokerControllerBase
    {
        private readonly IContactUsAppService _contactUsAppService;
        private readonly IUserAppService _userAppService;
        private readonly IAbpSession _abpSession;
        public ContactUsController(IContactUsAppService contactUsAppService, IUserAppService userAppService, IAbpSession abpSession)
        {
            _contactUsAppService = contactUsAppService; 
            _userAppService = userAppService;
            _abpSession = abpSession;
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiCreateContactUsOut> CreateContactUs([FromBody] ApiCreateContactUsDto input)
        {
            try
            {
                var contactUsDto = new ContactUsDto();
                contactUsDto.EmailAddress = input.EmailAddress;
                contactUsDto.EmailSubject = input.EmailSubject;
                contactUsDto.AttachmentPath = input.AttachmentPath;
                contactUsDto.UserId = input.UserId;

                var contactUs = await _contactUsAppService.Manage(contactUsDto);

                return new ApiCreateContactUsOut { ContactUsId = contactUs.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCreateContactUsOut { Error = ex.Message, Success = false };
            }
        }

    }
}
