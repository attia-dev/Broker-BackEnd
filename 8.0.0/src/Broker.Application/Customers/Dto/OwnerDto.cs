using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Authorization.Users;
using Broker.Customers;
using Broker.Datatable.Dtos;
using Broker.Localization;
using Broker.Lookups;
using Broker.Lookups.Dto;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Customers.Dto
{
    [AutoMapFrom(typeof(Owner))]
    public class OwnerDto : FullAuditedEntityDto<long>
    {
        public long UserId { get; set; }
        public UserDto User { get; set; }
        public string SecondMobile { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }

        //CompanyDetails

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserPassword { get; set; }
        public bool UserIsWhatsApped { get; set; }
        public DateTime? BirthDate { get; set; }
        public string AboutAr { get; set; }
        public string AboutEn { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto LastModifierUser { get; set; }
        public UserDto DeleterUser { get; set; }
        public long? PackageId { get; set; }
        public PackageDto Package { get; set; }
        public int? Balance { get; set; }

    }
    public class GetOwnerInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class PagedOwnerResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
    }
    public class GetOwnerOutput
    {
        public List<OwnerDto> Owners { get; set; }
        public string Error { get; set; }
    }
}
