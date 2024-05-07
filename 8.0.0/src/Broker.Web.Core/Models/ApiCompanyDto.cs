using Abp.Application.Services.Dto;
using Broker.Lookups.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApiCompanyDto : EntityDto<long>
    {
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

        

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserPassword { get; set; }
        public DateTime? BirthDate { get; set; }

        public long PackageId { get; set; }
        //public PackageDto Package { get; set; }
    }
    public class GetAllCompaniesOutput
    {
        public GetAllCompaniesOutput()
        {
            companies = new List<ApiCompanyDto>();
        }
        public List<ApiCompanyDto> companies { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllCompaniesInput
    {
        public string Keyword { get; set; }

    }

    public class ApiCreateCompanyDto 
    {
       // public bool IsSponser { get; set; }
       // public bool IsActive { get; set; }

      //  public string SecondMobile { get; set; }
       // public string Logo { get; set; }
      //  public string BWLogo { get; set; }
      //  public string CommericalAvatar { get; set; }
        //CompanyDetails
      //  public string About { get; set; }
      //  public double? Latitude { get; set; }
      //  public double? Longitude { get; set; }
     //   public string Facebook { get; set; }
      //  public string Instagram { get; set; }
      //  public string Snapchat { get; set; }
     //   public string Tiktok { get; set; }
     //   public string Website { get; set; }

 

        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserPassword { get; set; }
       // public DateTime? BirthDate { get; set; }
     //   public long PackageId { get; set; }
    }

    public class ApiCreateCompanyOut
    {
        public ApiCreateCompanyOut()
        {
        }
        public long? CompanyId { get; set; }
        public object Company { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiCompanyPackageDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Points { get; set; }
        public int? CompanyBalance { get; set; }
        public int? BrolerPoints { get; set; }
    }

    public class ApiCompanyPackageDtoOut
    {
        public ApiCompanyPackageDtoOut()
        {
            Details = new ApiCompanyPackageDto();
        }
        public ApiCompanyPackageDto Details { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiCompanyDetailsDto
    {
        public string SecondMobile { get; set; }

        public bool IsWhatsApped { get; set; }
        public long? Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Avatar { get; set; }

        
        public string BWLogo { get; set; }
        public string CommericalAvatar { get; set; }
        public bool IsSponser { get; set; }
        public bool IsActive { get; set; }
        
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Snapchat { get; set; }
        public string Tiktok { get; set; }
        public string Website { get; set; }
        public string AboutAr { get; set; }
        public string AboutEn { get; set; }
        
        public string About { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
    public class ApiCompanyDetailsDtoOut
    {
        public ApiCompanyDetailsDtoOut()
        {
            Details = new ApiCompanyDetailsDto();
        }
        public ApiCompanyDetailsDto Details { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiUpdateCompanyDto 
    {
        // public bool IsSponser { get; set; }
        public long Id { get; set; }
        public string SecondMobile { get; set; }
        public string Logo { get; set; }
        public string BWLogo { get; set; }
        public string CommericalAvatar { get; set; }
        public string About { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Snapchat { get; set; }
        public string Tiktok { get; set; }
        public string Website { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public string UserPhoneNumber { get; set; }
        public bool IsWhatsApped { get; set; }
        public long? PackageId { get; set; }
        public int? Balance { get; set; }
    }

    public class ApiUpdateCompanyOut
    {
        public ApiUpdateCompanyOut()
        {
        }
        public long? CompanyId { get; set; }
        public object Company { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiCompanyDetailsInput
    {
        public long? UserId { get; set; }
    }

    public class ApiSponsorDto : EntityDto<long>
    {
        public string SecondMobile { get; set; }
        public long? UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Photo { get; set; }
    }
    public class GetAllSponsorsOutput
    {
        public GetAllSponsorsOutput()
        {
            Sponsors = new List<ApiSponsorDto>();
        }
        public List<ApiSponsorDto> Sponsors { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllSponsorsInput
    {
        public string Keyword { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }

    }

    public class ApiCompanyiesOut
    {
        public ApiCompanyiesOut()
        {
            Companies = new List<ApiCompaniesDto>();
        }
        public List<ApiCompaniesDto> Companies { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiCompaniesDto /*: EntityDto<long>*/
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        
    }
   
}
