using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Authorization.Users;
using Broker.Datatable.Dtos;
using Broker.RateUs;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.ContactUs;

namespace Broker.ContactUs.Dto
{
    
    [AutoMapFrom(typeof(ContactUs))]
    public class ContactUsDto : FullAuditedEntityDto<long>
    {
        public string EmailAddress { get; set; }
        public string EmailSubject { get; set; }
        public string AttachmentPath { get; set; }
        public long? UserId { get; set; }
        public UserDto User { get; set; }

    }
    public class GetContactUsInput : DataTableInputDto
    {
        public string EmailAddress { get; set; }
    }
    public class PagedContactUsResultRequestDto : PagedResultRequestDto
    {
        public long? EmailAddress { get; set; }
    }
    public class GetContactUsOutput
    {
        public List<ContactUsDto> ContactUses { get; set; }
        public string Error { get; set; }
    }
}
