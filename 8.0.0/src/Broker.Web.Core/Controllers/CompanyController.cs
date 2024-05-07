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
using Microsoft.AspNetCore.Http;
using Abp.Runtime.Session;
using Broker.Advertisements;
using static Broker.Authorization.PermissionNames;

namespace Broker.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CompanyController : BrokerControllerBase
    {
        private readonly ICompanyAppService _companyAppService;
        private readonly IAbpSession _abpSession;
        private readonly IAdvertisementAppService _AdvertisementAppService;

        public CompanyController (ICompanyAppService companyAppService, IAbpSession abpSession, IAdvertisementAppService AdvertisementAppService)
        {
            _companyAppService = companyAppService;
            _abpSession = abpSession;
            _AdvertisementAppService = AdvertisementAppService;
        }

        //createCompany
        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiCreateCompanyOut> CreateCompany([FromBody] ApiCreateCompanyDto input)
        {
            try
            {
                var companyDto = new CompanyDto();
                companyDto.UserName = input.UserName;
                companyDto.UserSurname = input.UserSurname;
                companyDto.UserEmailAddress = input.UserEmailAddress;
                companyDto.UserPhoneNumber = input.UserPhoneNumber;
                companyDto.UserPassword = input.UserPassword;

                var company = await _companyAppService.Manage(companyDto);

                return new ApiCreateCompanyOut { CompanyId = company.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCreateCompanyOut { Error = ex.Message, Success = false };
            }
        }



        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]

        public async Task<ApiCompanyDetailsDtoOut> GetCompanyDetails(ApiCompanyDetailsInput input)
        {
            try
            {
                ApiCompanyDetailsDto companyDetails = new ApiCompanyDetailsDto();
                if (!input.UserId.HasValue || input.UserId == 0)
                    input.UserId = _abpSession.UserId.Value;
                var data = await _companyAppService.GetByUserId(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.UserId.Value });
                companyDetails.Id = data.Id;
                companyDetails.Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? data.UserName : data.UserSurname;
                
                companyDetails.PhoneNumber = data.UserPhoneNumber;
                companyDetails.EmailAddress = data.UserEmailAddress;
                companyDetails.SecondMobile = data.SecondMobile;
                companyDetails.IsWhatsApped = data.User.IsWhatsApped;
                companyDetails.About = data.About;
                companyDetails.Avatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{data.Logo ?? "usericon.png"}";
                companyDetails.BWLogo= $"{AppSettingsConstants.Url}/Resources/UploadFiles/{data.BWLogo ?? "notFound.png"}";
                companyDetails.CommericalAvatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{data.CommericalAvatar ?? "notFound.png"}";
                companyDetails.IsSponser = data.IsSponser;
                companyDetails.IsActive = data.IsActive;
                companyDetails.Facebook = data.Facebook;
                companyDetails.Instagram = data.Instagram;
                companyDetails.Snapchat = data.Snapchat;
                companyDetails.Tiktok = data.Tiktok;
                companyDetails.Website = data.Website;
                companyDetails.AboutAr = data.AboutAr;
                companyDetails.AboutEn = data.AboutEn;
                companyDetails.Latitude = data.Latitude;
                companyDetails.Longitude = data.Longitude;

                return new ApiCompanyDetailsDtoOut { Details = companyDetails, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCompanyDetailsDtoOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiCompanyiesOut> GetAllCompanies([FromBody] ApiCompanyInput input)
        {
            try
            {
                var data = await _companyAppService.GetAll(new PagedCompanyResultRequestDto
                {
                    Name = input.Name,
                    IsSponsor = input.IsSponsor
                });
                var companies = data.Companys.Select(x => new ApiCompaniesDto
                {
                    Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.User.Name : x.User.Surname,
                    Logo = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{x.Logo ?? "notFound.png"}" 

                }).ToList();

                return new ApiCompanyiesOut { Companies = companies, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCompanyiesOut { Error = ex.Message, Success = false };
            }

        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiUpdateCompanyOut> UpdateCompany([FromBody] ApiUpdateCompanyDto input)
        {
            try
            {
                var compamnyDto = new CompanyDto();
                compamnyDto.Id = input.Id;
                compamnyDto.UserName = input.UserName;
                compamnyDto.UserSurname = input.UserSurname;
                compamnyDto.UserEmailAddress = input.UserEmailAddress;
                compamnyDto.UserPhoneNumber = input.UserPhoneNumber;
                compamnyDto.UserIsWhatsApped = input.IsWhatsApped;
                
                compamnyDto.SecondMobile = input.SecondMobile;
                compamnyDto.Logo = input.Logo;
                compamnyDto.BWLogo = input.BWLogo;
                compamnyDto.CommericalAvatar = input.CommericalAvatar;
                compamnyDto.About = input.About;
                compamnyDto.Longitude = input.Longitude;
                compamnyDto.Latitude = input.Latitude;
                compamnyDto.Facebook = input.Facebook;
                compamnyDto.Instagram = input.Instagram;
                compamnyDto.Snapchat = input.Snapchat;
                compamnyDto.Tiktok = input.Tiktok;
                compamnyDto.Website = input.Website;

                compamnyDto.PackageId = input.PackageId;
                compamnyDto.Balance = input.Balance;
                var compamny = await _companyAppService.UpdateCompanyFromMobile(compamnyDto);

                return new ApiUpdateCompanyOut { CompanyId = compamny.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiUpdateCompanyOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiCompanyPackageDtoOut> GetCompanyPackageDetailsApi(ApiCompanyDetailsInput input)
        {
            try
            {
                ApiCompanyPackageDto PackageDetails = new ApiCompanyPackageDto();
                if (!input.UserId.HasValue || input.UserId == 0)
                    input.UserId = _abpSession.UserId.Value;
                var data = await _companyAppService.GetByUserId(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.UserId.Value });
                
                    PackageDetails.Name =data.Package!=null? Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? data.Package.NameAr : data.Package.NameEn:"";
                    PackageDetails.Price = data.Package != null ? data.Package.Price:0;
                    PackageDetails.Points = data.Package != null ? data.Package.Points:0;
                
                PackageDetails.CompanyBalance = data.Balance??0;

                var allAds = await _AdvertisementAppService.GetAllAdsByUserId(new Advertisements.Dto.PagedAdvertisementResultRequestForUserDto
                {
                    CompanyID = data.Id,
                });
                var totalPoints = allAds!=null? allAds.Advertisements.Sum(x => x.Points):0;
                PackageDetails.BrolerPoints = totalPoints;
                return new ApiCompanyPackageDtoOut { Details = PackageDetails, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCompanyPackageDtoOut { Error = ex.Message, Success = false };
            }
        }


    }
}
