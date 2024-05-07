using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Authorization.Users;
using Broker.Customers;
using Broker.Datatable.Dtos;
using Broker.Localization;
using Broker.Lookups;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.SocialContact.Dto
{
    [AutoMapFrom(typeof(Broker.SocialContacts.SocialContact))]
    public class SocialContactDto : FullAuditedEntityDto<int>
    {
        public string SocialName { get; set; }
        public string SocialValue { get; set; }
        public string Avatar { get; set; }

    }
    public class GetSocialContactsInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class PagedSocialContactResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
    }
    public class GetSocialContactsOutput
    {
        public List<SocialContactDto> SocialContacts { get; set; }
        public string Error { get; set; }
    }
}
