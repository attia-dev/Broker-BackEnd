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
    [AutoMapFrom(typeof(Company))]
    public class CompanyDto : FullAuditedEntityDto<long>
    {
        public long UserId { get; set; }
        public UserDto User { get; set; }
        public bool IsSponser { get; set; }
        public bool IsActive { get; set; }

        public string SecondMobile { get; set; }
        public string Logo { get; set; }
        public string BWLogo { get; set; }
        public string CommericalAvatar { get; set; }
        //CompanyDetails
        public string About { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Snapchat { get; set; }
        public string Tiktok { get; set; }
        public string Website { get; set; }


        //User Details
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
        // public long WalletId { get; set; }
        //public WalletDto Wallet { get; set; }

    }
    public class GetCompanyInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class PagedCompanyResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
       public bool? IsSponsor { get; set; }
    }
    public class GetCompanyOutput
    {
        public List<CompanyDto> Companys { get; set; }
        public string Error { get; set; }
    }
}
