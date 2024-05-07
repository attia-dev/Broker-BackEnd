using Abp.Runtime.Session;
using Broker.Advertisements;
using Broker.Advertisements.Dto;
using Broker.Configuration;
using Broker.Customers;
using Broker.Helpers;
using Broker.Models;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using Broker.Lookups;
using System.Runtime.CompilerServices;
using System.Diagnostics.Metrics;
using Broker.Projects;
using Abp.Domain.Repositories;
using Broker.Security;

namespace Broker.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AdvertisementController : BrokerControllerBase
    {
        private readonly IAdvertisementAppService _advertisementAppService;
        private readonly IDefinitionAppService _definitionAppService;
        private readonly IAbpSession _abpSession;
        private readonly IDurationAppService _durationAppService;
        private readonly IProjectAppService _ProjectAppService;
        private readonly IRepository<AdFavorite, long> _adFavoriteRepository;
        public AdvertisementController(IAdvertisementAppService advertisementAppService,
            IDefinitionAppService definitionAppService, IAbpSession abpSession,
            IDurationAppService durationAppService, IProjectAppService ProjectAppService,
            IRepository<AdFavorite, long> adFavoriteRepository)
        {
            _advertisementAppService = advertisementAppService;
            _definitionAppService = definitionAppService;
            _abpSession = abpSession;
            _durationAppService = durationAppService;
            _ProjectAppService = ProjectAppService;
            LocalizationSourceName = BrokerConsts.LocalizationSourceName;
            _adFavoriteRepository = adFavoriteRepository;
        }

        [HttpPost]
        public async Task<ApiCreatAdvertisementOut> CreateAdvertisement([FromBody] ApiCreateAdvertisementDto input)
        {
            try
            {
                
                var advertisementDto = new AdvertisementDto();
                advertisementDto.Id = input.Id;
                advertisementDto.Title = input.Title;
                advertisementDto.Type = input.Type;
                advertisementDto.CityId = input.CityId;
                advertisementDto.GovernorateId = input.GovernorateId;
                advertisementDto.Compound = input.Compound;
                advertisementDto.Street = input.Street;
                advertisementDto.BuildingNumber = input.BuildingNumber;
                advertisementDto.Latitude = input.Latitude;
                advertisementDto.Longitude = input.Longitude;
                advertisementDto.FloorsNumber = input.FloorsNumber;
                advertisementDto.Area = input.Area;
                advertisementDto.BuildingArea = input.BuildingArea;
                advertisementDto.ChaletType = input.ChaletType;
                advertisementDto.AgreementStatus = input.AgreementStatus;
                advertisementDto.BuildingStatus = input.BuildingStatus;
                advertisementDto.LandingStatus = input.LandingStatus;
                advertisementDto.UsingFor = input.UsingFor;
                advertisementDto.StreetWidth = input.StreetWidth;
                advertisementDto.Width = input.Width;
                advertisementDto.Length = input.Length;
                advertisementDto.BuildingDate = input.BuildingDate;
                advertisementDto.Rooms = input.Rooms;
                advertisementDto.Reception = input.Reception;
                advertisementDto.Balcony = input.Balcony;
                advertisementDto.Kitchen = input.Kitchen;
                advertisementDto.Toilet = input.Toilet;
                advertisementDto.NumUnits = input.NumUnits;
                advertisementDto.NumPartitions = input.NumPartitions;
                advertisementDto.OfficesNum = input.OfficesNum;
                advertisementDto.OfficesFloors = input.OfficesFloors;
                advertisementDto.DurationId = input.DurationId;
                advertisementDto.SeekerId = input.SeekerId;
                advertisementDto.OwnerId = input.OwnerId;
                advertisementDto.CompanyId = input.CompanyId;
                advertisementDto.BrokerPersonId = input.BrokerPersonId;
                advertisementDto.IsPublish = input.IsPublish;
                advertisementDto.IsApprove = input.IsApprove;
                advertisementDto.Description = input.Description;
                advertisementDto.FeaturedAd = input.FeaturedAd;
                advertisementDto.Price = input.Price;
                advertisementDto.PaymentFacility = input.PaymentFacility;
                advertisementDto.MrMrs = input.MrMrs;
                advertisementDto.AdvertiseMakerName = input.AdvertiseMakerName;
                advertisementDto.AdvertiseMaker = input.AdvertiseMaker;
                advertisementDto.MobileNumber = input.MobileNumber;
                advertisementDto.IsWhatsApped = input.IsWhatsApped;
                advertisementDto.SecondMobileNumber = input.SecondMobileNumber;
                advertisementDto.ContactRegisterInTheAccount = input.ContactRegisterInTheAccount;
                advertisementDto.Furnished = input.Furnished;
                advertisementDto.Elevator = input.Elevator;
                advertisementDto.Parking = input.Parking;
                advertisementDto.ParkingSpace = input.ParkingSpace;
                advertisementDto.Garden = input.Garden;
                advertisementDto.GardenArea = input.GardenArea;
                advertisementDto.Pool = input.Pool;
                advertisementDto.Shop = input.Shop;
                advertisementDto.ShopsNumber = input.ShopsNumber;
                advertisementDto.Rent = input.Rent;
                advertisementDto.Dinning = input.Dinning;
                advertisementDto.Document = input.Document;
                advertisementDto.Decoration = input.Decoration;
                advertisementDto.DiningRoom = input.DiningRoom;
                advertisementDto.PhotosList = input.PhotosList;
                advertisementDto.LayoutsList = input.LayoutsList;
                advertisementDto.ChaletRentType = input.ChaletRentType;
                advertisementDto.ChaletRentValue = input.ChaletRentValue;
                advertisementDto.NumOfMonths = input.NumOfMonths;
                advertisementDto.MinTimeToBookForChaletId = input.MinTimeToBookForChaletId;
                advertisementDto.AdvertisementFacilitesList = input.AdvertisementFacilitesList;
                advertisementDto.AdvertisementBookings = input.AdvertisementBookings;
                advertisementDto.Officies = input.Officies;
                advertisementDto.DownPayment = input.DownPayment;
                advertisementDto.MonthlyInstallment = input.MonthlyInstallment;
                advertisementDto.YearlyInstallment = input.YearlyInstallment;
                advertisementDto.NumOfYears = input.NumOfYears;
                advertisementDto.DeliveryDate = input.DeliveryDate;
                advertisementDto.AirConditioner = input.AirConditioner;
                advertisementDto.IsEdited = input.IsEdited;
                var advertisement = await _advertisementAppService.Manage(advertisementDto);

                return new ApiCreatAdvertisementOut { AdvertisementId = advertisement.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCreatAdvertisementOut { Error = ex.Message, Success = false };
            }
        }
       
        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiCreatAdvertisementOut> GetAdvertiseDetailsForEdit([FromBody] ApiAdvertisesInput input)
        {
            try
            {
                ApiCreateAdvertisementDto advertiseDetails = new ApiCreateAdvertisementDto();

                var data = await _advertisementAppService.GetById(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.AdvertiseId.Value });
                if (data.Id <= 0)
                    return new ApiCreatAdvertisementOut { Advertisement = null, Error = L("Common.Message.ObjectNotFound"), Success = false };
                else
                {
                    advertiseDetails.Id = data.Id;
                    advertiseDetails.Title = data.Title;
                    advertiseDetails.Type = data.Type;
                    advertiseDetails.CityId = data.CityId;
                    advertiseDetails.GovernorateId = data.GovernorateId;
                    advertiseDetails.Compound = data.Compound;
                    advertiseDetails.Street = data.Street;
                    advertiseDetails.BuildingNumber = data.BuildingNumber;
                    advertiseDetails.Latitude = data.Latitude;
                    advertiseDetails.Longitude = data.Longitude;
                    advertiseDetails.FloorsNumber = data.FloorsNumber;
                    advertiseDetails.Area = data.Area;
                    advertiseDetails.BuildingArea = data.BuildingArea;
                    advertiseDetails.ChaletType = data.ChaletType;
                    advertiseDetails.UsingFor = data.UsingFor;
                    advertiseDetails.StreetWidth = data.StreetWidth;
                    advertiseDetails.Width = data.Width;
                    advertiseDetails.Length = data.Length;
                    advertiseDetails.BuildingDate = data.BuildingDate;
                    advertiseDetails.Rooms = data.Rooms;
                    advertiseDetails.Reception = data.Reception;
                    advertiseDetails.Balcony = data.Balcony;
                    advertiseDetails.Kitchen = data.Kitchen;
                    advertiseDetails.Toilet = data.Toilet;
                    advertiseDetails.NumUnits = data.NumUnits;
                    advertiseDetails.NumPartitions = data.NumPartitions;
                    advertiseDetails.OfficesNum = data.OfficesNum;
                    advertiseDetails.OfficesFloors = data.OfficesFloors;
                    advertiseDetails.DurationId = data.DurationId;
                    advertiseDetails.SeekerId = data.SeekerId;
                    advertiseDetails.OwnerId = data.OwnerId;
                    advertiseDetails.CompanyId = data.CompanyId;
                    advertiseDetails.BrokerPersonId = data.BrokerPersonId;
                    advertiseDetails.IsPublish = data.IsPublish;
                    advertiseDetails.IsApprove = data.IsApprove;
                    advertiseDetails.Description = data.Description;
                    advertiseDetails.FeaturedAd = data.FeaturedAd;
                    advertiseDetails.Price = data.Price;
                    advertiseDetails.PaymentFacility = data.PaymentFacility;
                    advertiseDetails.MrMrs = data.MrMrs;
                    advertiseDetails.AdvertiseMakerName = data.AdvertiseMakerName;

                    // advertiseDetails.AdvertiseMakerName = (data.MrMrs == MrMrsType.Mrs ? L("Common.Mrs") : L("Common.Mr")) + data.AdvertiseMakerName;
                    advertiseDetails.AdvertiseMaker = data.AdvertiseMaker;
                    advertiseDetails.MobileNumber = data.MobileNumber;
                    advertiseDetails.IsWhatsApped = data.IsWhatsApped;
                    advertiseDetails.SecondMobileNumber = data.SecondMobileNumber;
                    advertiseDetails.ContactRegisterInTheAccount = data.ContactRegisterInTheAccount;
                    advertiseDetails.ParkingSpace = data.ParkingSpace;
                    advertiseDetails.Garden = data.Garden;
                    advertiseDetails.GardenArea = data.GardenArea;
                    advertiseDetails.Pool = data.Pool;
                    advertiseDetails.Shop = data.Shop;
                    advertiseDetails.ShopsNumber = data.ShopsNumber;

                    advertiseDetails.Dinning = data.Dinning;
                    advertiseDetails.DiningRoom = data.DiningRoom;
                    advertiseDetails.Rent = data.Rent;

                   advertiseDetails.PhotosList = data.Photos.Count > 0 ?
                   data.Photos.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "AddPhoto.png"}").ToList()
                   : new List<string> ();
                 
                   advertiseDetails.LayoutsList = data.Layouts.Count > 0 ?
                  data.Layouts.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "AddPhoto.png"}").ToList()
                  : new List<string>();

                    advertiseDetails.PhotosList = CheckPhoto( advertiseDetails.PhotosList);
                    advertiseDetails.LayoutsList = CheckPhoto( advertiseDetails.LayoutsList);
                    advertiseDetails.Document = data.Document;
                    advertiseDetails.Decoration = data.Decoration;
                    advertiseDetails.AgreementStatus = data.AgreementStatus;
                    advertiseDetails.Furnished = data.Furnished;
                    advertiseDetails.Elevator = data.Elevator;
                    advertiseDetails.Parking = data.Parking;
                    advertiseDetails.LandingStatus = data.LandingStatus;
                    advertiseDetails.BuildingStatus = data.BuildingStatus;
                    advertiseDetails.ChaletRentType = data.ChaletRentType;
                    advertiseDetails.ChaletRentValue = data.ChaletRentValue;
                    advertiseDetails.NumOfMonths = data.NumOfMonths;
                    advertiseDetails.MinTimeToBookForChaletId = data.MinTimeToBookForChaletId;
                    advertiseDetails.MinTimeToBookName = data.MinTimeToBookForChaletId >0 ? Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? data.MinTimeToBookForChalet.NameAr : data.MinTimeToBookForChalet.NameEn:"";
                    advertiseDetails.AdvertisementBookings = data.AdvertisementBookings;
                    advertiseDetails.ShowChalet = data.Type == BuildingType.ChaletForSummer && data.AgreementStatus == AgreementStatus.Rent ? true : false;
                    advertiseDetails.Officies = data.Officies;
                    var allFacilities = _definitionAppService.GetAll(new Lookups.Dto.PagedDefinitionResultRequestDto
                    { EnumCategory = DefinitionTypes.Facilities }).Result.Definitions;
                    advertiseDetails.FacilitesApi = allFacilities.Select(
                    x => new FacilityDtoForEdit
                    {
                        Id=x.Id,
                        Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.NameAr : x.NameEn,
                        Description = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.DescriptionAr : x.DescriptionEn,
                        Avatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{x.Avatar ?? "notFound.png"}",
                        IsSelected = data.AdvertisementFacilites.Select(f => f.Facility).ToList().Any(c => c.Id == x.Id) ?
                    true : false
                    }).ToList();
                    advertiseDetails.DownPayment = data.DownPayment;
                    advertiseDetails.MonthlyInstallment = data.MonthlyInstallment;
                    advertiseDetails.YearlyInstallment = data.YearlyInstallment;
                    advertiseDetails.NumOfYears = data.NumOfYears;
                    advertiseDetails.DeliveryDate = data.DeliveryDate;
                    advertiseDetails.AirConditioner = data.AirConditioner;
                    advertiseDetails.IsEdited = data.IsEdited;
                    advertiseDetails.ProjectId = data.ProjectId;
                    return new ApiCreatAdvertisementOut { Advertisement = advertiseDetails, Success = true };
                }

            }
            catch (Exception ex)
            {
                return new ApiCreatAdvertisementOut { Error = ex.Message, Success = false };
            }
        }

       private List<string> CheckPhoto( List<string> PhotosList)
        {
            switch (PhotosList.Count)
            {
                case 1:
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");

                    break;
                case 2:
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    break;
                case 3:
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    break;
                case 4:
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    break;
                case 5:
                    PhotosList.Add($"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png");
                    break;
            }
            return PhotosList;
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]

        public async Task<ApiAdvertiseDetailsDtoOut> GetAdvertiseDetails([FromBody] ApiAdvertisesInput input)
        {
            try
            {
                ApiGetAdvertiseDto advertiseDetails = new ApiGetAdvertiseDto();

                var data = await _advertisementAppService.GetById(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.AdvertiseId.Value });
                if (data.Id <= 0)
                {

                    return new ApiAdvertiseDetailsDtoOut { Details = null, Error = L("Common.Message.ObjectNotFound"), Success = false };
                }
                else
                {
                    advertiseDetails.Id = data.Id;
                    advertiseDetails.Title = data.Title;
                    advertiseDetails.Type = data.Type;
                    advertiseDetails.CityId = data.CityId;
                    advertiseDetails.GovernorateId = data.GovernorateId;
                    advertiseDetails.GovernorateName = data.Governorate != null ? Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? data.Governorate.NameAr : data.Governorate.NameEn : "";

                    advertiseDetails.Compound = data.Compound;
                    advertiseDetails.Street = data.Street;
                    advertiseDetails.BuildingNumber = data.BuildingNumber;
                    advertiseDetails.Latitude = data.Latitude;
                    advertiseDetails.Longitude = data.Longitude;
                    advertiseDetails.FloorsNumber = data.FloorsNumber;
                    advertiseDetails.Area = data.Area.Length>0 ? data.Area+ L("Common.M"): data.Area;
                    advertiseDetails.BuildingArea = data.BuildingArea;
                    advertiseDetails.ChaletType = data.ChaletType;
                    advertiseDetails.UsingFor = data.UsingFor;
                    advertiseDetails.UsingForString = data.UsingFor != null ? getUsingFor((UsingFor)data.UsingFor) : "";

                    advertiseDetails.StreetWidth = data.StreetWidth;
                    advertiseDetails.Width = data.Width;
                    advertiseDetails.Length = data.Length;
                    advertiseDetails.BuildingDate = data.BuildingDate;
                    advertiseDetails.Rooms = data.Rooms;
                    advertiseDetails.Reception = data.Reception;
                    advertiseDetails.Balcony = data.Balcony;
                    advertiseDetails.Kitchen = data.Kitchen;
                    advertiseDetails.Toilet = data.Toilet;
                    advertiseDetails.NumUnits = data.NumUnits;
                    advertiseDetails.NumPartitions = data.NumPartitions;
                    advertiseDetails.OfficesNum = data.OfficesNum;
                    advertiseDetails.OfficesFloors = data.OfficesFloors;
                    advertiseDetails.DurationId = data.DurationId;
                    advertiseDetails.SeekerId = data.SeekerId;
                    advertiseDetails.OwnerId = data.OwnerId;
                    advertiseDetails.CompanyId = data.CompanyId;
                    advertiseDetails.BrokerPersonId = data.BrokerPersonId;
                    advertiseDetails.IsPublish = data.IsPublish;
                    advertiseDetails.IsApprove = data.IsApprove;
                    advertiseDetails.Description = data.Description;
                    advertiseDetails.FeaturedAd = data.FeaturedAd;
                    advertiseDetails.Price = (data.AgreementStatus == AgreementStatus.Rent && data.Type == BuildingType.ChaletForSummer) ? FormatNumber(data.ChaletRentValue ?? 0) : FormatNumber(data.Price);
                    advertiseDetails.PaymentFacility = data.PaymentFacility;
                    advertiseDetails.AdvertiseMakerName = (data.MrMrs == MrMrsType.Mrs ? L("Common.Mrs") : L("Common.Mr")) + data.AdvertiseMakerName;
                    advertiseDetails.AdvertiseMaker = data.AdvertiseMaker == AdvertiseMakerType.Broker ?
                     (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "السمسار" : "Thr Broker") :
                     (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "المالك" : "The Owner");
                    advertiseDetails.MobileNumber = data.MobileNumber;
                    advertiseDetails.IsWhatsApped = data.IsWhatsApped == true ? (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نعم" : "Yes") :
                        (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "لا" : "No");
                    advertiseDetails.WhatsApped = data.IsWhatsApped;
                    advertiseDetails.SecondMobileNumber = data.SecondMobileNumber;
                    advertiseDetails.ContactRegisterInTheAccount = data.ContactRegisterInTheAccount;
                    advertiseDetails.ParkingSpace = data.ParkingSpace;
                    advertiseDetails.Garden = data.Garden == true ? (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نعم" : "Yes") :
                        (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "لا" : "No") + data.GardenArea;
                    advertiseDetails.GardenArea = data.GardenArea;
                    advertiseDetails.Pool = data.Pool == true ? (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نعم" : "Yes") :
                        (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "لا" : "No");
                    advertiseDetails.Shop = data.Shop == true ? (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نعم" : "Yes") :
                        (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "لا" : "No");
                    advertiseDetails.ShopsNumber = data.ShopsNumber;

                    advertiseDetails.Dinning = data.Dinning;
                    advertiseDetails.DiningRoom = data.DiningRoom == true ? (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نعم" : "Yes") :
                        (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "لا" : "No");
                    advertiseDetails.Rent = data.Rent;
                    advertiseDetails.Officies = data.Officies;

                    advertiseDetails.Photos = data.Photos.Count > 0 ?
                    data.Photos.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "notFound.png"}").ToList()
                    : new List<string> { $"{AppSettingsConstants.Url}/Resources/UploadFiles/notFound.png" };

                    advertiseDetails.Layouts = data.Layouts.Count > 0 ?
                    data.Layouts.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "notFound.png"}").ToList()
                    : new List<string> { $"{AppSettingsConstants.Url}/Resources/UploadFiles/notFound.png" };

                    var allFacilities = _definitionAppService.GetAll(new Lookups.Dto.PagedDefinitionResultRequestDto
                    { EnumCategory = DefinitionTypes.Facilities }).Result.Definitions;

                    advertiseDetails.Facilites = allFacilities.Select(
                    x => new FacilityDtoApi
                    {
                        //Id=x.Id,
                        Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.NameAr : x.NameEn,
                        Description = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.DescriptionAr : x.DescriptionEn,
                        Avatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{x.Avatar ?? "notFound.png"}",
                        IsChecked = data.AdvertisementFacilites.Select(f => f.Facility).ToList().Any(c => c.Id == x.Id) ?
                     (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نعم" : "Yes") :
                        (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "لا" : "No")
                    }).ToList();
                    advertiseDetails.Document = data.Document != null ? getDocumentStatus((DocumentStatus)data.Document) : "";
                    advertiseDetails.Decoration = data.Decoration != null ? getDecorationStatus((DecorationStatus)data.Decoration) : "";
                    advertiseDetails.ViewsCount = data.views.Count();
                    //////////////////////////////////////////
                    advertiseDetails.AgreementStatus = data.AgreementStatus == AgreementStatus.Sell ? L("Common.Sell") : L("Common.Rent");
                    advertiseDetails.AgreementStatusId = data.AgreementStatus;
                    advertiseDetails.RentValue = data.Rent != null ? getRentType((RentType)data.Rent) : "";

                    advertiseDetails.Furnished = data.Furnished == true ? (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نعم" : "Yes") :
                        (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "لا" : "No");
                    advertiseDetails.Elevator = data.Elevator == true ? (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نعم" : "Yes") :
                        (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "لا" : "No");
                    advertiseDetails.Parking = data.Parking == true ? (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نعم" : "Yes") :
                        (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "لا" : "No");
                    advertiseDetails.LandingStatus = data.LandingStatus != null ? getLandingStatus((LandingStatus)data.LandingStatus) : "";
                    advertiseDetails.BuildingStatus = data.BuildingStatus != null ? getBuildingStatus((BuildingStatus)data.BuildingStatus) : "";
                    advertiseDetails.PropertyFor = $"{getBuildingType(data.Type)} {(Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "من اجل" : "for")} {advertiseDetails.AgreementStatus}";
                    advertiseDetails.AdvertisementBookings = data.AdvertisementBookings;
                    //.Select(
                    //    x=> new ApiReturnAdvertisementBookingDto{
                    //        //AdvertisementId=x.AdvertisementId,
                    //        BookingDate=x,//.BookingDate,
                    //
                    //}).ToList();

                    //  advertiseDetails. = data.Dinning;
                    advertiseDetails.ChaletRentType = data.ChaletRentType;

                    advertiseDetails.ChaletRentValue = data.ChaletRentValue;
                    advertiseDetails.NumOfMonths = data.NumOfMonths;
                    advertiseDetails.MinTimeToBookForChaletId = data.MinTimeToBookForChaletId;
                    advertiseDetails.MinTimeToBookName = data.MinTimeToBookForChaletId > 0 ? Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? data.MinTimeToBookForChalet.NameAr : data.MinTimeToBookForChalet.NameEn : "";
                    advertiseDetails.ShowChalet = data.Type == BuildingType.ChaletForSummer && data.AgreementStatus == AgreementStatus.Rent ? true : false;
                    advertiseDetails.DownPayment = data.DownPayment;
                    advertiseDetails.MonthlyInstallment = data.MonthlyInstallment;
                    advertiseDetails.YearlyInstallment = data.YearlyInstallment;
                    advertiseDetails.NumOfYears = data.NumOfYears;
                    advertiseDetails.DeliveryDate = data.DeliveryDate;
                    advertiseDetails.IsEdited = data.IsEdited;
                    advertiseDetails.IsCompany = data.Company != null;
                    if (advertiseDetails.IsCompany)
                    {
                        advertiseDetails.CompanyLogo = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{data.Company.Logo ?? "notFound.png"}";
                        advertiseDetails.CompanyAbout = data.Company.About ?? "";
                        advertiseDetails.CompanyFacebook = data.Company.Facebook ?? "";
                        advertiseDetails.CompanyInstagram = data.Company.Instagram ?? "";
                        advertiseDetails.CompanySnapchat = data.Company.Snapchat ?? "";
                        advertiseDetails.CompanyTiktok = data.Company.Tiktok ?? "";
                        advertiseDetails.CompanyWebsite = data.Company.Website ?? "";
                        advertiseDetails.CompanyName = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? data.Company.UserName : data.Company.UserSurname;
                        advertiseDetails.CompanyLatitude = data.Company.Latitude;
                        advertiseDetails.CompanyLongitude = data.Company.Longitude;
                    }
                    advertiseDetails.ChaletRentTypeString = data.ChaletRentType != null ? getChaletRentType((ChaletRentType)data.ChaletRentType) : "";
                    if (data.ProjectId!=null&&data.ProjectId > 0)
                    {
                        var project = await _ProjectAppService.GetById(new Abp.Application.Services.Dto.EntityDto<long>((long)data.ProjectId));
                        advertiseDetails.ProjectName = project.Id > 0 ? project.Name : "";
                    }
                    if (input.UserId != null && input.UserId.HasValue)
                    {
                        var fav = _adFavoriteRepository.FirstOrDefault(x => x.AdvertisementId == data.Id && x.UserId == input.UserId);
                        advertiseDetails.IsFavourite = fav == null ? false : true;
                    }
                    
                    return new ApiAdvertiseDetailsDtoOut { Details = advertiseDetails, Success = true };
                }

            }
            catch (Exception ex)
            {
                return new ApiAdvertiseDetailsDtoOut { Error = ex.Message, Success = false };
            }
        }


        //[HttpPost]
        //[Language(BrokerConsts.RequestHeaders.Language)]
        //public async Task<ApiAllAdstOut> GetAllAdvertisements([FromBody] ApiAdvertisementInput input)
        //{
        //    try
        //    {
        //        var data = await _advertisementAppService.GetAll(new Advertisements.Dto.PagedAdvertisementResultRequestDto
        //        {
        //            Name = input.Name,
        //            AgreementStatus=(input.AgreementStatus == null || input.AgreementStatus==0)?
        //            0:input.AgreementStatus

        //        });
        //        var advertisements = data.Advertisements.Select(x => new ApiAllAdsDto
        //        {
        //            Id = x.Id,
        //            Title = x.Title,
        //            Latitude = x.Latitude,
        //            Longitude = x.Longitude,
        //            IsPublish = x.IsPublish,
        //            ViewCount = x.views.Count(),
        //            From = x.Duration != null ? x.CreationTime : DateTime.Now,
        //            To = x.Duration != null ? x.CreationTime.AddMonths(x.Duration.Period) : DateTime.Now,
        //            Photos = x.Photos.Count > 0 ? x.Photos.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "notFound.png"}").ToList() : new List<string> { $"{AppSettingsConstants.Url}/Resources/UploadFiles/notFound.png" }
        //            ,Price=x.Price

        //        }).ToList();

        //        return new ApiAllAdstOut { advertisements = advertisements, Success = true };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiAllAdstOut { Error = ex.Message, Success = false };
        //    }

        //}

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiAllAdstOut> GetAllAdvertisements([FromBody] ApiAdvertisementSearchInput input)
        {
            try
            {
                var data = await _advertisementAppService.SearchForApi(new Advertisements.Dto.GetAdvertisementSearchInput
                {
                    Type = input.Type,
                    CityId = input.CityId,
                    GovernorateId=input.GovernorateId,
                    StreetOrCompund = input.StreetOrCompund,
                    Rooms = input.Rooms,
                    Area = input.Area,
                    AreaFrom=input.AreaFrom,
                    AreaTo=input.AreaTo,
                    Decoration = input.Decoration,
                    Furnished = input.Furnished,
                    Parking = input.Parking,
                    AgreementStatus = input.AgreementStatus,
                    PriceFrom = input.PriceFrom,
                    PriceTo = input.PriceTo,
                    IsEdited = input.IsEdited,
                    CompanyId=input.CompanyId,
                    Latitude=input.Latitude,
                    Longitude=input.Longitude
                    //AgreementStatus = (input.AgreementStatus == null || input.AgreementStatus == 0) ?
                    //0 : input.AgreementStatus

                });
                var advertisements = data.Advertisements.Where(x => x.IsApprove!=null&&x.IsApprove==true&&x.IsPublish).Select(x => new ApiAllAdsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    IsPublish = x.IsPublish,
                    ViewCount = x.views.Count(),
                    From = x.Duration != null ? x.CreationTime : DateTime.Now,
                    To = x.Duration != null ? x.CreationTime.AddMonths(x.Duration.Period) : DateTime.Now,
                    Photos = x.Photos.Count > 0 ? x.Photos.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "notFound.png"}").ToList() : new List<string> { $"{AppSettingsConstants.Url}/Resources/UploadFiles/notFound.png" },
                    Price =(x.AgreementStatus==AgreementStatus.Rent&& x.Type==BuildingType.ChaletForSummer)? FormatNumber (x.ChaletRentValue??0): FormatNumber (x.Price),
                    FeaturedAd = x.FeaturedAd,
                    IsCompany = x.Company != null,
                    CompanyLogo =x.Company!=null? $"{AppSettingsConstants.Url}/Resources/UploadFiles/{x.Company.Logo ?? "notFound.png"}":""

                }).ToList();

                return new ApiAllAdstOut { advertisements = advertisements, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiAllAdstOut { Error = ex.Message, Success = false };
            }

        }

        [HttpPost]
        public async Task<ApiCreateFavoritetOut> CreateFavoriteApi([FromBody] ApiCreateFavoritetDto input)
        {
            try
            {
                var favoriteDto = new AdFavoriteDto();
                favoriteDto.UserId = input.UserId;
                favoriteDto.AdvertisementId = input.AdvertisementId;

                var favorite = await _advertisementAppService.CreateFavorite(favoriteDto);

                return new ApiCreateFavoritetOut { FavoriteId = favorite.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCreateFavoritetOut { Error = ex.Message, Success = false };
            }
        }


        [HttpPost]
        public async Task<DeleteFavoriteOutput> DeleteFavorite([FromBody] long favoriteId)
        {

            try
            {
                await _advertisementAppService.DeleteFavorite(favoriteId);

                return new DeleteFavoriteOutput { message = "", Success = true };
            }
            catch (Exception ex)
            {
                return new DeleteFavoriteOutput { Error = ex.Message, Success = false };
            }

        }

        [HttpPost]
        public async Task<DeleteFavoriteOutput> DeleteFavoriteByAdId([FromBody] long addId)
        {

            try
            {
                await _advertisementAppService.DeleteFavoriteByAddId(addId);

                return new DeleteFavoriteOutput { message = "", Success = true };
            }
            catch (Exception ex)
            {
                return new DeleteFavoriteOutput { Error = ex.Message, Success = false };
            }

        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiFavoriteOut> GetAllFavoritesForUser([FromBody] ApiFavoriteInput input)
        {
            try
            {
                var data = await _advertisementAppService.GetFavoritesForUser(input.UserId);
                var favorites = data.AdFavorites.Select(x => new ApiFavoriteDto
                {
                    Id = x.Id,
                    AdvertisementId = x.Advertisement.Id,
                    Title = x.Advertisement.Title,
                    BuildingType = x.Advertisement.Type,
                    Latitude = x.Advertisement.Latitude,
                    Longitude = x.Advertisement.Longitude,
                    MobileNumber = x.Advertisement.MobileNumber !=null ? x.Advertisement.MobileNumber : x.Advertisement.SecondMobileNumber,
                    CityName = x.Advertisement.City != null ? Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.Advertisement.City.NameAr : x.Advertisement.City.NameEn:"",
                    GovernorateName = x.Advertisement.Governorate != null ? Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.Advertisement.Governorate.NameAr : x.Advertisement.Governorate.NameEn:"",

                    IsPublish = x.Advertisement.IsPublish,
                    Price =FormatNumber(x.Advertisement.Price),
                    IsWhatsApped=x.Advertisement.IsWhatsApped,
                    Photos = x.Advertisement.Photos.Count > 0 ? x.Advertisement.Photos.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "notFound.png"}").ToList() : new List<string> { $"{AppSettingsConstants.Url}/Resources/UploadFiles/notFound.png" }

                }).ToList();

                return new ApiFavoriteOut { favorites = favorites, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiFavoriteOut { Error = ex.Message, Success = false };
            }

        }

        [HttpPost]
        public async Task<ApiCreateViewOut> CreateViewApi([FromBody] ApiCreateViewDto input)
        {
            try
            {
                var viewDto = new AdViewDto();
                //viewDto.DeviceToken = input.DeviceToken;
                viewDto.UserId = input.UserId;
                viewDto.AdvertisementId = input.AdvertisementId;

                var view = await _advertisementAppService.CreateView(viewDto);

                return new ApiCreateViewOut { ViewId = view.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiCreateViewOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        public async Task<ApiViewOut> GetViewsForAdvertisementApi([FromBody] ApiViewInput input)
        {
            try
            {
                var data = await _advertisementAppService.GetViewsForAdvertisement(input.AdvertisementId);
                var count = data.AdViews.Count;
                return new ApiViewOut { Count = count, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiViewOut { Error = ex.Message, Success = false };
            }

        }
        [HttpPost]
        public async Task<ApiViewForChartOut> GetViewsForChartApi([FromBody] ApiViewForChartInput input)
        {
            try
            {
                var adv = await _advertisementAppService
                    .GetById(new Abp.Application.Services.Dto.EntityDto<long>( input.AdvertisementId));
                ApiViewForChartOut returnedData = new ApiViewForChartOut();
                var data = await _advertisementAppService.GetViewsForAdvertisement(input.AdvertisementId);

                if (data.AdViews.Count > 0)
                {
                    List<List<AdViewDto>> groupedList = new List<List<AdViewDto>>();
                    var firstCreationDate = adv.CreationTime;
                    for (DateTime d = firstCreationDate; d < firstCreationDate.AddDays(input.DurationByDays); d = d.AddDays(input.Interval))
                    {
                        List<AdViewDto> list = data.AdViews.Where(x => x.CreationTime >= d && x.CreationTime < d.AddDays(input.Interval)).ToList();
                        groupedList.Add(list);
                        returnedData.Counts.Add(list.Count);
                    }
                }

                returnedData.Success = true;
                return returnedData;
            }
            catch (Exception ex)
            {
                return new ApiViewForChartOut { Error = ex.Message, Success = false };
            }

        }
        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiUserAdsOut> GetAllAdsByUserIdApi([FromBody] ApiAdvertisementForUserInput input)
        {
            try
            {
                var data = await _advertisementAppService.GetAllAdsByUserId(new Advertisements.Dto.PagedAdvertisementResultRequestForUserDto
                {
                    BrokerID = input.BrokerId,
                    SeekerID = input.SeekerId,
                    OwnerID = input.OwnerId,
                    CompanyID = input.CompanyId,
                    SkipCount = input.Start ?? 0,
                    MaxResultCount = input.Length ?? 10

                });
                var advertisements = data.Advertisements.Select(x =>
                {

                    return new ApiUserAdsDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Latitude = x.Latitude,
                        Longitude = x.Longitude,
                        IsPublish = x.IsPublish,
                        IsApproved = x.IsApprove,
                        ViewCount = x.views.Count(),
                        From = x.Duration != null ? x.CreationTime : DateTime.Now,
                        
                        To = x.Duration != null ? ( x.FeaturedAd == true? x.CreationTime.AddMonths(x.Duration.Period) : x.CreationTime.AddMonths(1) ) : DateTime.Now,
                      
                        Type = getBuildingType((BuildingType)x.Type),
                        TypeId = x.Type,
                        Photos = x.Photos.Count > 0 ? x.Photos.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "notFound.png"}").ToList() : new List<string> { $"{AppSettingsConstants.Url}/Resources/UploadFiles/notFound.png" },
                        ExpireStatus = x.Duration!=null? DateTime.Now > (x.CreationTime.AddMonths(x.Duration.Period)) ? ExpiredStatus.Expired : ExpiredStatus.NotExpired : ExpiredStatus.Pending

                    };
                }).ToList();

                return new ApiUserAdsOut { advertisements = advertisements, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiUserAdsOut { Error = ex.Message, Success = false };
            }

        }

        //points
        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiAdsPointsOut> GetAdsPointsByUserIdApi([FromBody] ApiAdvertisementForUserInput input)
        {
            try
            {
                var data = await _advertisementAppService.GetAllAdsByUserId(new Advertisements.Dto.PagedAdvertisementResultRequestForUserDto
                {
                    BrokerID = input.BrokerId,
                    SeekerID = input.SeekerId,
                    OwnerID = input.OwnerId,
                    CompanyID = input.CompanyId,
                    SkipCount = input.Start ?? 0,
                    MaxResultCount = input.Length ?? 10

                });
                var advertisements = data.Advertisements.Select(x =>
                {

                    return new ApiAdPointDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Points=x.Points??0
                    };
                }).ToList();

                var totalPoints = advertisements.Sum(x=>x.Points);

                return new ApiAdsPointsOut {
                    advertisements = advertisements,TotalPoints=totalPoints, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiAdsPointsOut { Error = ex.Message, Success = false };
            }

        }

        //points

        [HttpPost]
        public async Task<DeleteAdOutput> DeleteAdById([FromBody] long advertiseId)
        {

            try
            {
                await _advertisementAppService.Delete(new Abp.Application.Services.Dto.EntityDto<long>(advertiseId));

                return new DeleteAdOutput { message = "", Success = true };
            }
            catch (Exception ex)
            {
                return new DeleteAdOutput { Error = ex.Message, Success = false };
            }

        }

        [HttpPost]
        public async Task<CheckStatusOutput> CheckStatus([FromBody] long advertiseId)
        {

            try
            {
                var advertisement = await _advertisementAppService.CheckAdvertisementStatus(advertiseId);


                    return new CheckStatusOutput
                    {
                        Success = true,
                        Status = advertisement
                    };

                    
            }
            catch (Exception ex)
            {
                return new CheckStatusOutput { Error = ex.Message, Success = false };
            }

        }

        [HttpPost]
        public async Task<ChangeAdStatusOutput> ChangeActiveStatus([FromBody] long advertiseId)
        {

            try
            {
                var advertisement = await _advertisementAppService.ChangeStatus(advertiseId);

                return new ChangeAdStatusOutput
                {
                    message = "",
                    Success = true,
                    AdvertiseID = advertisement.Id,
                    ActiveStatus = advertisement.IsPublish
                };
            }
            catch (Exception ex)
            {
                return new ChangeAdStatusOutput { Error = ex.Message, Success = false };
            }

        }

        [HttpPost]
        public async Task<DeleteAdOutput> ClearAllByUserId([FromBody] ApiAdvertisementForUserInput input)
        {

            try
            {
                await _advertisementAppService.DeleteAllByUserId(new Advertisements.Dto.PagedAdvertisementResultRequestForUserDto
                {
                    BrokerID = input.BrokerId,
                    SeekerID = input.SeekerId,
                    OwnerID = input.OwnerId,
                    CompanyID = input.CompanyId,

                });

                return new DeleteAdOutput { message = "", Success = true };
            }
            catch (Exception ex)
            {
                return new DeleteAdOutput { Error = ex.Message, Success = false };
            }

        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiAllAdstOut> SearchAdvertisements([FromBody] ApiAdvertisementSearchInput input)
        {
            try
            {
                var data = await _advertisementAppService.SearchForApi(new Advertisements.Dto.GetAdvertisementSearchInput
                {
                    Type = input.Type,
                    CityId = input.CityId,
                    GovernorateId=input.GovernorateId,
                    StreetOrCompund = input.StreetOrCompund,
                    Rooms = input.Rooms,
                    Area = input.Area,
                    Decoration = input.Decoration,
                    Furnished = input.Furnished,
                    Parking = input.Parking,
                    AgreementStatus = input.AgreementStatus,
                    PriceFrom = input.PriceFrom,
                    PriceTo = input.PriceTo,
                    IsEdited = input.IsEdited

                    //AgreementStatus = (input.AgreementStatus == null || input.AgreementStatus == 0) ?
                    //0 : input.AgreementStatus

                });
                var advertisements = data.Advertisements.Select(x => new ApiAllAdsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    IsPublish = x.IsPublish,
                    ViewCount = x.views.Count(),
                    From = x.Duration != null ? x.CreationTime : DateTime.Now,
                    To = x.Duration != null ? x.CreationTime.AddMonths(x.Duration.Period) : DateTime.Now,
                    Photos = x.Photos.Count > 0 ? x.Photos.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "notFound.png"}").ToList() : new List<string> { $"{AppSettingsConstants.Url}/Resources/UploadFiles/notFound.png" },
                    Price =FormatNumber( x.Price)

                }).ToList();

                return new ApiAllAdstOut { advertisements = advertisements, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiAllAdstOut { Error = ex.Message, Success = false };
            }

        }


        //IsAdInFavourites

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]

        public async Task<ApiIsFavouriteExistDtoOut> IsAdInFavourites([FromBody] ApiFavoriteExistInput input)
        {
            try
            {

                var fav = _adFavoriteRepository.FirstOrDefault(x=>x.AdvertisementId==input.AdvertisementId&&x.UserId==input.UserId);
                if (fav==null)
                    return new ApiIsFavouriteExistDtoOut { IsExist = false, Success = true };
                else
                    return new ApiIsFavouriteExistDtoOut { IsExist = true, Success = true };

            }
            catch (Exception ex)
            {
                return new ApiIsFavouriteExistDtoOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]

        public async Task<ApiAdvertiseDetailsMapDtoOut> GetAdvertiseDetailsForMap([FromBody] ApiAdvertisesInput input)
        {
            try
            {
                ApiAdvertiseMapDto advertiseDetails = new ApiAdvertiseMapDto();

                var data = await _advertisementAppService.GetById(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.AdvertiseId.Value });
                if (data.Id <= 0)
                    return new ApiAdvertiseDetailsMapDtoOut { Details = null, Error = L("Common.Message.ObjectNotFound"), Success = false };
                else if (data.IsPublish == false||data.IsApprove==false)
                    return new ApiAdvertiseDetailsMapDtoOut { Details = null, Error = L("Common.Message.ObjectNotPuplished"), Success = false };

                else
                {
                    var type = getBuildingType((BuildingType)data.Type);
                    var agreementStatus = data.AgreementStatus == AgreementStatus.Sell ? (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "بيع" : "Sell") :
                        (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "ايجار" : "Rent");
                    var area = data.Area;
                    advertiseDetails.Title = $"{type} {(Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "لل" : "For")} {agreementStatus} {(Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "فى" : "in")} {area}";
                    advertiseDetails.Price = $"{FormatNumber(data.Price)} / {(Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "الشهر" : "Month")}";
                    if (input.UserId != null && input.UserId.HasValue)
                    {
                        var fav = _adFavoriteRepository.FirstOrDefault(x => x.AdvertisementId == data.Id && x.UserId == input.UserId);
                        advertiseDetails.IsFavourite = fav == null ? false : true;
                    }


                    return new ApiAdvertiseDetailsMapDtoOut { Details = advertiseDetails, Success = true };
                }

            }
            catch (Exception ex)
            {
                return new ApiAdvertiseDetailsMapDtoOut { Error = ex.Message, Success = false };
            }
        }
        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiPlaceDurationOut> PlaceDurationToAds([FromBody] PlaceAdsDurationInput input)
        {
            try
            {
               
                    var duration = await _durationAppService.GetById(new Abp.Application.Services.Dto.EntityDto<long> ((long)input.DurationId));
                    
                    if (duration.Id<=0)
                    {
                        return new ApiPlaceDurationOut { Error = "there is no duration with this id", Success = false };
                    }
                    else
                    {
                    
                        foreach (var id in input.Ads)
                        {
                        var item =await _advertisementAppService.GetById(new Abp.Application.Services.Dto.EntityDto<long>(id));
                        if (item.Id>0)
                        {
                            item.DurationId = input.DurationId;
                            item.IsPublish = (bool)input.IsPublish;
                            await _advertisementAppService.Manage(item);
                        }

                        }
                    }


                return new ApiPlaceDurationOut { Success = true };
            }
            catch (Exception ex)
            {
                return new ApiPlaceDurationOut { Error = ex.Message, Success = false };
            }

        }
        [Language(BrokerConsts.RequestHeaders.Language)]
        private string getDecorationStatus(DecorationStatus val)
        {
            switch (val)
            {
                case Helpers.DecorationStatus.Without:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "بدون" : "Without");
                case Helpers.DecorationStatus.SemiFinished:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نصف تشتطيب" : "Semi Finished");
                case Helpers.DecorationStatus.Full:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "كامل" : "Full");
                case Helpers.DecorationStatus.HighQuality:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "عالى الجودة" : "High Quality");
                default:
                    return "";
            };
        }
        [Language(BrokerConsts.RequestHeaders.Language)]
        private string getLandingStatus(LandingStatus val)
        {
            switch (val)
            {
                case Helpers.LandingStatus.Empety:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "فارغ" : "Empty");
                case Helpers.LandingStatus.Used:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "مستخدم" : "Used");
                default:
                    return "";
            };
        }
        [Language(BrokerConsts.RequestHeaders.Language)]
        private string getBuildingStatus(BuildingStatus val)
        {
            switch (val)
            {
                case Helpers.BuildingStatus.New:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "جديد" : "New");
                case Helpers.BuildingStatus.Used:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "مستخدم" : "Used");
                case Helpers.BuildingStatus.Renewed:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "مجدد" : "Renewed");
                default:
                    return "";
            };
        }
        [Language(BrokerConsts.RequestHeaders.Language)]
        private string getDocumentStatus(DocumentStatus val)
        {
            switch (val)
            {
                case Helpers.DocumentStatus.Registered:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "مسجل" : "Registered");
                case Helpers.DocumentStatus.Unregistered:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "غير مسجل" : "Unregistered");
                case Helpers.DocumentStatus.Registerable:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "قابل للتسجيل" : "Registerable");
                case Helpers.DocumentStatus.Unregisterable:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "غير قابل للتسجيل" : "Unregisterable");
                default:
                    return "";
            };
        }
        [Language(BrokerConsts.RequestHeaders.Language)]
        private string getBuildingType(BuildingType val)
        {
            switch (val)
            {
                case BuildingType.Apartment:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "شقة" : "Apartment");
                case BuildingType.Villa:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "فيلا" : "Villa");
                case BuildingType.ChaletForSummer:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "شاليه للصيف" : "ChaletForSummer");
                case BuildingType.Building:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "مبنى" : "Building");
                case BuildingType.AdminOffice:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "مكتب ادرة" : "Adminstration Office");
                case BuildingType.Shop:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "محل" : "Shop");
                case BuildingType.Land:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "ارض" : "Land");
                default:
                    return "";
            };
        }
        [Language(BrokerConsts.RequestHeaders.Language)]
        private string getRentType(RentType val)
        {
            switch (val)
            {
                case RentType.Monthly:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "شهرى" : "Monthly");
                case RentType.MidTerm:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "نصف سنوى" : "Mid Term");
                case RentType.Annual:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "سنوى" : "Annual");
                default:
                    return "";
            };
        }
        [Language(BrokerConsts.RequestHeaders.Language)]
        private string getChaletRentType(ChaletRentType val)
        {
            switch (val)
            {
                case ChaletRentType.Day:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "يوم" : "Day");
                case ChaletRentType.Week:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "اسبوع" : "Week");
                
                default:
                    return "";
            };
        }
        [Language(BrokerConsts.RequestHeaders.Language)]
        private string getUsingFor(UsingFor val)
        {
            switch (val)
            {
                case Helpers.UsingFor.Buildings:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "مبانى" : "Buildings");
                case Helpers.UsingFor.Industrial:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "صناعى" : "Industrial");
                case Helpers.UsingFor.Agriculture:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "زراعى" : "Agriculture");
                case Helpers.UsingFor.Investment:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "استثمارى" : "Investment");
                case Helpers.UsingFor.Residential:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "سكنى" : "Residential");
                case Helpers.UsingFor.Commercial:
                    return (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? "تجارى" : "Commercial");
                default:
                    return "";
            };
        }
       
        [Language(BrokerConsts.RequestHeaders.Language)]
        static string FormatNumber(decimal num)
        {
            
            if (num >= 1000000)
                return Decimal.Round(num / 1000000, 1, MidpointRounding.AwayFromZero) +
                    (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? " M" : " M");

            if (num >= 1000)
                return Decimal.Round(num/1000, 1, MidpointRounding.AwayFromZero) +
                     (Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? " K" : " K");

            return num.ToString("#,0");
        }

        [HttpGet]
        [Language(BrokerConsts.RequestHeaders.Language)]

        public async Task<LastAdvertiseInsertedOut> GetLastAdvertiseId()
        {
            try
            {


                var data = await _advertisementAppService.GetLastAdvertiseInsertedToDB();
                if (data.Id <= 0)
                {

                    return new LastAdvertiseInsertedOut { Id = null, Error = L("Common.Message.ObjectNotFound"), Success = false };
                }
                else
                {
                    return new LastAdvertiseInsertedOut { Id = data.Id, Success = true };
                }

            }
            catch (Exception ex)
            {
                return new LastAdvertiseInsertedOut { Error = ex.Message, Success = false };
            }
        }


    }
}
