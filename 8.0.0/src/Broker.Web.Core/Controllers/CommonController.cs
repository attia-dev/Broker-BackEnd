using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Broker.Authorization;
using Broker.Authorization.Users;
using Broker.Configuration;
using Broker.Customers.Dto;
using Broker.Helpers;
using Broker.Lookups;
using Broker.Lookups.Dto;
using Broker.Models;
using Broker.Models.TokenAuth;
using Broker.Notifications;
using Broker.Notifications.Dto;
using Broker.SocialContact.Dto;
using Broker.SocialContacts;
using Broker.Users;
using Broker.Users.Dto;
using Castle.Core.Resource;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Broker.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CommonController : BrokerControllerBase
    {
        private readonly CountryAppService _countryAppService;
        private readonly GovernorateAppService _governorateAppService;
        private readonly CityAppService _cityAppService;
        private readonly DefinitionAppService _definitionAppService;
        private readonly CompanyAppService _companyAppService;
        private readonly DurationAppService _durationAppService;
        private readonly IUserAppService _userAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly SocialContactAppService _socialContactAppService;
        private readonly DiscountCodeAppService _discountCodeAppService;
        private readonly PackageAppService _PackageAppService;
        private readonly INotificationAppService _NotificationAppService;
        public CommonController(CountryAppService countryAppService,
            GovernorateAppService governorateAppService,
           CityAppService cityAppService, DefinitionAppService definitionAppService
            , IUserAppService userAppService, CompanyAppService companyAppService,
           DurationAppService durationAppService, IRepository<User, long> userRepository,
           SocialContactAppService socialContactAppService, DiscountCodeAppService discountCodeAppService,
           PackageAppService PackageAppService, INotificationAppService NotificationAppService)
        {
            this._countryAppService = countryAppService;
            this._governorateAppService = governorateAppService;
            this._cityAppService = cityAppService;
            this._definitionAppService = definitionAppService;
            this._userAppService = userAppService;
            this._companyAppService = companyAppService;
            this._durationAppService = durationAppService;
            _userRepository = userRepository;
            _socialContactAppService = socialContactAppService;
            _discountCodeAppService = discountCodeAppService;
            _discountCodeAppService = discountCodeAppService;
            _PackageAppService = PackageAppService;
            _NotificationAppService = NotificationAppService;
            LocalizationSourceName = BrokerConsts.LocalizationSourceName;

        }

        [HttpPost]
        public async Task<GetIsEmailExist> IsEmailOrPhoneExist([FromBody] ApiCommonyDto input)
        {
           

            try
            {
                var user = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == input.PhoneOrEmail || x.PhoneNumber == input.PhoneOrEmail);

                if (user == null)
                    return new GetIsEmailExist {Exist=false,Success = true };
                return new GetIsEmailExist { Exist = true, Success = true };
            }
            catch (Exception e)
            {

                return new GetIsEmailExist { Error=e.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<GetAllCountriesOutput> GetAllCountries([FromBody] GetAllCountriesInput input)
        {
            try
            {
                

                var countries = await _countryAppService.GetAll(
                    new PagedCountryResultRequestDto { Name = input.Keyword , SkipCount = input.Start ?? 0, MaxResultCount = input.Length ?? 10 });
                var data = countries.Countries.Select(x => new ApiCountryDto
                {
                    Id = x.Id,
                    Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.NameAr : x.NameEn,
                   
                }).ToList();


                return new GetAllCountriesOutput { Countries = data, Success = true };
            }
            catch (Exception ex)
            {
                return new GetAllCountriesOutput { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<GetAllGovernoratesOutput> GetAllGovernorates([FromBody] GetAllGovernoratesInput input)
        {
            try
            {


                var governorates = await _governorateAppService.GetAll(new PagedGovernorateResultRequestDto { Name = input.Keyword, SkipCount = input.Start ?? 0, MaxResultCount = input.Length ?? 10 });
                var data = governorates.Governorates.Select(x => new ApiGovernorateDto
                {
                    Id = x.Id,
                    Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.NameAr : x.NameEn,
                   
                }).ToList();


                return new GetAllGovernoratesOutput { governorates = data, Success = true };
            }
            catch (Exception ex)
            {
                return new GetAllGovernoratesOutput { Error = ex.Message, Success = false };
            }
        }


        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<GetAllCitiesOutput> GetAllCities([FromBody] GetAllCitiesInput input)
        {
            try
            {


                var cities = await _cityAppService.GetAll(new PagedCityResultRequestDto { Name = input.Keyword,GovernorateId=input.GovernorateId , SkipCount = input.Start ?? 0, MaxResultCount = input.Length ?? 10 });
                var data = cities.Items.Select(x => new ApiCityDto
                {
                    Id = x.Id,
                    Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.NameAr : x.NameEn
                    //GovernorateId=x.GovernorateId
                   
                }).ToList();


                return new GetAllCitiesOutput { cities = data, Success = true };
            }
            catch (Exception ex)
            {
                return new GetAllCitiesOutput { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<GetAllDefinitionsOutput> GetAllDefinitions([FromBody] GetAllDefinitionsInput input)
        {
            try
            {
                var data = await _definitionAppService.GetAll(new Lookups.Dto.PagedDefinitionResultRequestDto { keyword = input.Keyword, EnumCategory = (DefinitionTypes)input.Type, SkipCount = input.Start ?? 0, MaxResultCount = input.Length ?? 10 });
                var definations = data.Definitions.Select(x => new ApiDefinitionDto
                {
                    Id = x.Id,
                    Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.NameAr : x.NameEn,
                    Avatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{x.Avatar ?? "notFound.png"}",
                    Description = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.DescriptionAr : x.DescriptionEn,
                    Value=x.Value,
                }).ToList();

                return new GetAllDefinitionsOutput { Definitions = definations, Success = true };
            }
            catch (Exception ex)
            {
                return new GetAllDefinitionsOutput { Error = ex.Message, Success = false };
            }

        }


        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<GetAllSponsorsOutput> GetAllSponsors([FromBody] GetAllSponsorsInput input)
        {
            try
            {
                var data = await _companyAppService.GetAll(new PagedCompanyResultRequestDto { Name = input.Keyword, SkipCount = input.Start ?? 0, MaxResultCount = input.Length ?? 10 });
                List<ApiSponsorDto> sponsors = new List<ApiSponsorDto>();
                if (data.Companys != null)
                {
                     sponsors = data.Companys.Where(x => x.IsSponser==true).Select(x => new ApiSponsorDto
                    {
                        Id = x.Id,
                        SecondMobile = x.SecondMobile,
                        UserId = x.UserId,
                        Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.UserName : x.UserSurname,
                        PhoneNumber = x.User.PhoneNumber,
                        EmailAddress = x.User.EmailAddress,
                         Photo = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{x.Logo ?? "notFound.png"}"

                     }).ToList();
                }
               
                return new GetAllSponsorsOutput { Sponsors = sponsors, Success = true };
            }
            catch (Exception ex)
            {
                return new GetAllSponsorsOutput { Error = ex.Message, Success = false };
            }

        }


        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<GetAllDurationsOutput> GetAllDurations([FromBody] GetAllDurationsInput input)
        {
            try
            {
                var durations = await _durationAppService.GetAll(
                    new PagedDurationResultRequestDto {Type=input.Type, IsPublish=input.IsPublish ??true, SkipCount = input.Start ?? 0, MaxResultCount = input.Length ?? 10 });
                var data = durations.Items.Select(x => new ApiDurationDto
                {
                    Id = x.Id,
                    Period = x.Period,
                    Amount = x.Amount,
                    //Type = x.Type,
                    Types=x.DurationBuildingTypes.Count>0?x.DurationBuildingTypes.Select(y=>y.Type.ToString()).ToList():new List<string>(),
                    IsPublish = x.IsPublish,

                }).ToList();
                return new GetAllDurationsOutput { Durations = data, Success = true };
            }
            catch (Exception ex)
            {
                return new GetAllDurationsOutput { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        public async Task<ApiDeleteAccountOut> DeleteAccountApi([FromBody] ApiDeleteAccountDto input)
        {
            try
            {
                await _userAppService.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long>(input.UserId));
                return new ApiDeleteAccountOut { Success = true, msg = L("SuccessfullyDeleted") };
            }
            catch (Exception ex)
            {
                return new ApiDeleteAccountOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiChangePasswordOut> UpdateUserPassword([FromBody] ApiChangePasswordDto input)
        {
            try
            {
                var inq = await _userAppService.ChangePassword(new ChangePasswordDto
                {
                    CurrentPassword = input.CurrentPassword,
                    NewPassword = input.NewPassword
                });

                return new ApiChangePasswordOut { Success = true, msg = L("Api.SucssesChangPass") };
            }
            catch (Exception ex)
            {
                return new ApiChangePasswordOut { Error = ex.Message, msg = L("Api.ErroChangPass") , Success = false };
            }
        }

        [HttpPost]
        public async Task<GetAllSocialContactsOutput> GetAllSocialContacts([FromBody] GetAllSocialContactsInput input)
        {
            try
            {


                var socialContacts = await _socialContactAppService.GetAll(
                    new PagedSocialContactResultRequestDto { Name = input.Keyword, SkipCount = input.Start ?? 0, MaxResultCount = input.Length ?? 10 });
                var data = socialContacts.SocialContacts.Select(x => new ApiSocialContactDto
                {
                    
                    SocialName = x.SocialName,
                    SocialValue=x.SocialValue,
                    Avatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{x.Avatar ?? "notFound.png"}",

                }).ToList();


                return new GetAllSocialContactsOutput { SocialContacts = data, Success = true };
            }
            catch (Exception ex)
            {
                return new GetAllSocialContactsOutput { Error = ex.Message, Success = false };
            }
        }
        [HttpPost]
        public async Task<GetDiscontCodeOutput> GetDiscountPrecentageAndFixedAmount([FromBody] GetDiscontCodeInput input)
        {
            try
            {
                var discount = await _discountCodeAppService.GetByCode(
                    new GetDiscountCodeByCodeInput { Code = input.Keyword });
                var data = new ApiDiscountDto();
                data.Percentage = discount.Percentage;
                // data.FixedAmount = discount.FixedAmount;
                
                if (discount.Percentage!=null) { 
                    if(discount.From <= DateTime.Now && discount.To >= DateTime.Now)
                    {
                        return new GetDiscontCodeOutput { Discount = data, Success = true };
                        
                    }
                    else
                    {
                        return new GetDiscontCodeOutput { Error = L("Common.DiscountCodeExpired"), Success = false };

                    }
                }
                else
                {
                    return new GetDiscontCodeOutput { Error = L("Common.InvalidDiscountCode"), Success = false };
                }
                //return new GetDiscontCodeOutput { Discount = data, Success = true };
            }
            catch (Exception ex)
            {
                return new GetDiscontCodeOutput { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<GetAllPackagesOutput> GetAllPackages([FromBody] GetAllPackagesInput input)
        {
            try
            {


                var packages = await _PackageAppService.GetAll(new PagedPackageResultRequestDto { Name = input.Keyword,SkipCount = input.Start ?? 0, MaxResultCount = input.Length ?? 10 });
                var data = packages.Packages.Select(x => new ApiPackageDto
                {
                    Id = x.Id,
                    Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.NameAr : x.NameEn,
                    Price=x.Price,
                    Points=x.Points

                }).ToList();


                return new GetAllPackagesOutput { Packages = data, Success = true };
            }
            catch (Exception ex)
            {
                return new GetAllPackagesOutput { Error = ex.Message, Success = false };
            }
        }
        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiUserNotificationsOut> GetAllNotificationsForUserApi([FromBody] ApiNotificationsForUserInput input)
        {
            try
            {
                var data = await _NotificationAppService.GetAllNotificationsByUserId(new PagedNotificationResultRequestForUserDto
                {
                    UserId=input.UserId,
                    //SkipCount = input.Start ?? 0,
                    //MaxResultCount = input.Length ?? 10

                });
                var notifications = data.Notifications.Select(x =>
                {
                    return new ApiUserNotificationDto
                    {
                        Id = x.Id,
                        Description = x.Description,
                        Date = x.CreationTime.ToString("dd/MM//yyyy h:mm tt")
                    };
                }).ToList();

                return new ApiUserNotificationsOut { Notifications = notifications, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiUserNotificationsOut { Error = ex.Message, Success = false };
            }

        }

    }
}
