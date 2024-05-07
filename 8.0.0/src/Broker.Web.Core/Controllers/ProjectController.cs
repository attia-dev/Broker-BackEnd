using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Broker.Advertisements;
using Broker.Advertisements.Dto;
using Broker.Configuration;
using Broker.Customers;
using Broker.Customers.Dto;
using Broker.Helpers;
using Broker.Lookups;
using Broker.Models;
using Broker.Projects;
using Broker.Projects.Dto;
using Microsoft.AspNetCore.Mvc;
using static Broker.Authorization.PermissionNames;

namespace Broker.Controllers
{

    [Route("api/[controller]/[action]")]
    public class ProjectController : BrokerControllerBase
    {
        private readonly IProjectAppService _projectAppService;
        private readonly IAbpSession _abpSession;
        private readonly IDefinitionAppService _definitionAppService;
        private readonly IDurationAppService _durationAppService;
        private readonly IAdvertisementAppService _advertisementAppService;
        public ProjectController(IProjectAppService projectAppService, IAbpSession abpSession, IDefinitionAppService definitionAppService,
            IAdvertisementAppService advertisementAppService,IDurationAppService durationAppService)
        {
            _projectAppService = projectAppService;
            _abpSession = abpSession;
            _definitionAppService = definitionAppService;
            _durationAppService = durationAppService;
            _advertisementAppService = advertisementAppService;
        }

        //createProject & Update
        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiProjectOut> ManagaProject([FromBody] ApiProjectDto input)
        {
            try
            {
                var projectDto = new ProjectDto();
                projectDto.Id = input.Id;
                projectDto.Name = input.Name;
                projectDto.Description = input.Description;
                projectDto.Latitude = input.Latitude;
                projectDto.Longitude = input.Longitude;
                projectDto.DurationId = input.DurationId;
                projectDto.FeaturedAd = input.FeaturedAd;
                projectDto.CompanyId = input.CompanyId;
                projectDto.IsApprove = input.IsApprove;
                projectDto.IsPublish = input.IsPublish;
                projectDto.PhotosList = input.PhotosList;
                projectDto.LayoutsList = input.LayoutsList;

                 List<AdvertisementDto> list= new List<AdvertisementDto>();
                AdvertisementDto advertisementDto = new AdvertisementDto();

                foreach (var advertisement in input.Advertisements)
                {
                    advertisementDto.Id = advertisement.Id;
                    advertisementDto.Title = advertisement.Title;
                    advertisementDto.Type = advertisement.Type;
                    advertisementDto.CityId = advertisement.CityId;
                    advertisementDto.Compound = advertisement.Compound;
                    advertisementDto.Street = advertisement.Street;
                    advertisementDto.BuildingNumber = advertisement.BuildingNumber;
                    advertisementDto.Latitude = advertisement.Latitude;
                    advertisementDto.Longitude = advertisement.Longitude;
                    advertisementDto.FloorsNumber = advertisement.FloorsNumber;
                    advertisementDto.Area = advertisement.Area;
                    advertisementDto.BuildingArea = advertisement.BuildingArea;
                    advertisementDto.ChaletType = advertisement.ChaletType;
                    advertisementDto.AgreementStatus = advertisement.AgreementStatus;
                    advertisementDto.BuildingStatus = advertisement.BuildingStatus;
                    advertisementDto.LandingStatus = advertisement.LandingStatus;
                    advertisementDto.UsingFor = advertisement.UsingFor;
                    advertisementDto.StreetWidth = advertisement.StreetWidth;
                    advertisementDto.Width = advertisement.Width;
                    advertisementDto.Length = advertisement.Length;
                    advertisementDto.BuildingDate = advertisement.BuildingDate;
                    advertisementDto.Rooms = advertisement.Rooms;
                    advertisementDto.Reception = advertisement.Reception;
                    advertisementDto.Balcony = advertisement.Balcony;
                    advertisementDto.Kitchen = advertisement.Kitchen;
                    advertisementDto.Toilet = advertisement.Toilet;
                    advertisementDto.NumUnits = advertisement.NumUnits;
                    advertisementDto.NumPartitions = advertisement.NumPartitions;
                    advertisementDto.OfficesNum = advertisement.OfficesNum;
                    advertisementDto.OfficesFloors = advertisement.OfficesFloors;
                    advertisementDto.DurationId = advertisement.DurationId;
                    advertisementDto.SeekerId = advertisement.SeekerId;
                    advertisementDto.OwnerId = advertisement.OwnerId;
                    advertisementDto.CompanyId = advertisement.CompanyId;
                    advertisementDto.BrokerPersonId = advertisement.BrokerPersonId;
                    advertisementDto.IsPublish = advertisement.IsPublish;
                    advertisementDto.IsApprove = advertisement.IsApprove;
                    advertisementDto.Description = advertisement.Description;
                    advertisementDto.FeaturedAd = advertisement.FeaturedAd;
                    advertisementDto.Price = advertisement.Price;
                    advertisementDto.PaymentFacility = advertisement.PaymentFacility;
                    advertisementDto.MrMrs = advertisement.MrMrs;
                    advertisementDto.AdvertiseMakerName = advertisement.AdvertiseMakerName;
                    advertisementDto.AdvertiseMaker = advertisement.AdvertiseMaker;
                    advertisementDto.MobileNumber = advertisement.MobileNumber;
                    advertisementDto.IsWhatsApped = advertisement.IsWhatsApped;
                    advertisementDto.SecondMobileNumber = advertisement.SecondMobileNumber;
                    advertisementDto.ContactRegisterInTheAccount = advertisement.ContactRegisterInTheAccount;
                    advertisementDto.Furnished = advertisement.Furnished;
                    advertisementDto.Elevator = advertisement.Elevator;
                    advertisementDto.Parking = advertisement.Parking;
                    advertisementDto.ParkingSpace = advertisement.ParkingSpace;
                    advertisementDto.Garden = advertisement.Garden;
                    advertisementDto.GardenArea = advertisement.GardenArea;
                    advertisementDto.Pool = advertisement.Pool;
                    advertisementDto.Shop = advertisement.Shop;
                    advertisementDto.ShopsNumber = advertisement.ShopsNumber;
                    advertisementDto.Rent = advertisement.Rent;
                    advertisementDto.Dinning = advertisement.Dinning;
                    advertisementDto.Document = advertisement.Document;
                    advertisementDto.Decoration = advertisement.Decoration;
                    advertisementDto.DiningRoom = advertisement.DiningRoom;
                    advertisementDto.PhotosList = advertisement.PhotosList;
                    advertisementDto.LayoutsList = advertisement.LayoutsList;
                    advertisementDto.ChaletRentType = advertisement.ChaletRentType;
                    advertisementDto.ChaletRentValue = advertisement.ChaletRentValue;
                    advertisementDto.NumOfMonths = advertisement.NumOfMonths;
                    advertisementDto.MinTimeToBookForChaletId = advertisement.MinTimeToBookForChaletId;
                    advertisementDto.AdvertisementFacilitesList = advertisement.AdvertisementFacilitesList;
                    advertisementDto.AdvertisementBookings = advertisement.AdvertisementBookings;
                    advertisementDto.Officies = advertisement.Officies;
                    advertisementDto.DownPayment = advertisement.DownPayment;
                    advertisementDto.MonthlyInstallment = advertisement.MonthlyInstallment;
                    advertisementDto.YearlyInstallment = advertisement.YearlyInstallment;
                    advertisementDto.NumOfYears = advertisement.NumOfYears;
                    advertisementDto.DeliveryDate = advertisement.DeliveryDate;
                    advertisementDto.AirConditioner = advertisement.AirConditioner;
                    advertisementDto.IsEdited = advertisement.IsEdited;
                    
                    list.Add(advertisementDto);
                    advertisementDto = new AdvertisementDto();
                }

                projectDto.Advertisements = list;

                var project = await _projectAppService.Manage(projectDto);

                return new ApiProjectOut { ProjectId = project.Id, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiProjectOut { Error = ex.Message, Success = false };
            }
        }

        //[HttpPost]
        //[Language(BrokerConsts.RequestHeaders.Language)]
        //public async Task<ApiProjectOut> UpdateProject([FromBody] ApiProjectDto input)
        //{
        //    try
        //    {
        //        var projectDto = new ProjectDto();
        //        projectDto.Id = input.Id;
        //        projectDto.Name = input.Name;
        //        projectDto.Description = input.Description;
        //        projectDto.Latitude = input.Latitude;
        //        projectDto.Longitude = input.Longitude;
        //        projectDto.DurationId = input.DurationId;
        //        projectDto.FeaturedAd = input.FeaturedAd;
        //        projectDto.CompanyId = input.CompanyId;
        //        projectDto.IsPublish = input.IsPublish;
        //        projectDto.PhotosList = input.PhotosList;
        //        projectDto.LayoutsList = input.LayoutsList;

        //        projectDto.Advertisements = input.Advertisements;

        //        var project = await _projectAppService.Manage(projectDto);

        //        return new ApiProjectOut { ProjectId = project.Id, Success = true };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiProjectOut { Error = ex.Message, Success = false };
        //    }
        //}
        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]

        public async Task<ApiProjectDetailsDtoOut> GetProjectDetails([FromBody] ApiProjectDetailsInput input)
        {
            try
            {
                ApiProjectDto projectDetails = new ApiProjectDto();
                if (input.ProjectId.HasValue && input.ProjectId > 0)
                {
                    var data = await _projectAppService.GetById(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.ProjectId.Value });

                    projectDetails.Id = data.Id;
                    projectDetails.Name = data.Name;
                    projectDetails.Description = data.Description;
                    projectDetails.Latitude = data.Latitude;
                    projectDetails.Longitude = data.Longitude;
                    projectDetails.DurationId = data.DurationId;
                    projectDetails.FeaturedAd = data.FeaturedAd;
                    projectDetails.CompanyId = data.CompanyId;
                    projectDetails.IsPublish = data.IsPublish;
                   // projectDetails.PhotosList = data.PhotosList;
                   // projectDetails.LayoutsList = data.LayoutsList;

                    projectDetails.PhotosList = (data.Photos!=null && data.Photos.Count>0)  ?
                        data.Photos.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "AddPhoto.png"}").ToList()
                    : new List<string>();

                    projectDetails.LayoutsList =(data.Layouts!=null && data.Layouts.Count > 0 ) ?
                   data.Layouts.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "AddPhoto.png"}").ToList()
                   : new List<string>();

                    projectDetails.PhotosList = CheckPhoto(projectDetails.PhotosList);
                    projectDetails.LayoutsList = CheckPhoto(projectDetails.LayoutsList);

                    List<ApiCreateAdvertisementDto> list = new List<ApiCreateAdvertisementDto>();
                    ApiCreateAdvertisementDto advertiseDetails = new ApiCreateAdvertisementDto();
                    foreach (var advertisement in data.Advertisements)
                    {

                        advertiseDetails.Id = advertisement.Id;
                        advertiseDetails.Title = advertisement.Title;
                        advertiseDetails.Type = advertisement.Type;
                        advertiseDetails.CityId = advertisement.CityId;
                        advertiseDetails.Compound = advertisement.Compound;
                        advertiseDetails.Street = advertisement.Street;
                        advertiseDetails.BuildingNumber = advertisement.BuildingNumber;
                        advertiseDetails.Latitude = advertisement.Latitude;
                        advertiseDetails.Longitude = advertisement.Longitude;
                        advertiseDetails.FloorsNumber = advertisement.FloorsNumber;
                        advertiseDetails.Area = advertisement.Area;
                        advertiseDetails.BuildingArea = advertisement.BuildingArea;
                        advertiseDetails.ChaletType = advertisement.ChaletType;
                        advertiseDetails.UsingFor = advertisement.UsingFor;
                        advertiseDetails.StreetWidth = advertisement.StreetWidth;
                        advertiseDetails.Width = advertisement.Width;
                        advertiseDetails.Length = advertisement.Length;
                        advertiseDetails.BuildingDate = advertisement.BuildingDate;
                        advertiseDetails.Rooms = advertisement.Rooms;
                        advertiseDetails.Reception = advertisement.Reception;
                        advertiseDetails.Balcony = advertisement.Balcony;
                        advertiseDetails.Kitchen = advertisement.Kitchen;
                        advertiseDetails.Toilet = advertisement.Toilet;
                        advertiseDetails.NumUnits = advertisement.NumUnits;
                        advertiseDetails.NumPartitions = advertisement.NumPartitions;
                        advertiseDetails.OfficesNum = advertisement.OfficesNum;
                        advertiseDetails.OfficesFloors = advertisement.OfficesFloors;
                        advertiseDetails.DurationId = advertisement.DurationId;
                        advertiseDetails.SeekerId = advertisement.SeekerId;
                        advertiseDetails.OwnerId = advertisement.OwnerId;
                        advertiseDetails.CompanyId = advertisement.CompanyId;
                        advertiseDetails.BrokerPersonId = advertisement.BrokerPersonId;
                        advertiseDetails.IsPublish = advertisement.IsPublish;
                        advertiseDetails.IsApprove = advertisement.IsApprove;
                        advertiseDetails.Description = advertisement.Description;
                        advertiseDetails.FeaturedAd = advertisement.FeaturedAd;
                        advertiseDetails.Price = advertisement.Price;
                        advertiseDetails.PaymentFacility = advertisement.PaymentFacility;
                        advertiseDetails.MrMrs = advertisement.MrMrs;
                        advertiseDetails.AdvertiseMakerName = advertisement.AdvertiseMakerName;

                        // advertiseDetails.AdvertiseMakerName = (advertisement.MrMrs == MrMrsType.Mrs ? L("Common.Mrs") : L("Common.Mr")) + advertisement.AdvertiseMakerName;
                        advertiseDetails.AdvertiseMaker = advertisement.AdvertiseMaker;
                        advertiseDetails.MobileNumber = advertisement.MobileNumber;
                        advertiseDetails.IsWhatsApped = advertisement.IsWhatsApped;
                        advertiseDetails.SecondMobileNumber = advertisement.SecondMobileNumber;
                        advertiseDetails.ContactRegisterInTheAccount = advertisement.ContactRegisterInTheAccount;
                        advertiseDetails.ParkingSpace = advertisement.ParkingSpace;
                        advertiseDetails.Garden = advertisement.Garden;
                        advertiseDetails.GardenArea = advertisement.GardenArea;
                        advertiseDetails.Pool = advertisement.Pool;
                        advertiseDetails.Shop = advertisement.Shop;
                        advertiseDetails.ShopsNumber = advertisement.ShopsNumber;

                        advertiseDetails.Dinning = advertisement.Dinning;
                        advertiseDetails.DiningRoom = advertisement.DiningRoom;
                        advertiseDetails.Rent = advertisement.Rent;

                        advertiseDetails.PhotosList = advertisement.Photos.Count > 0 ?
                        advertisement.Photos.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "AddPhoto.png"}").ToList()
                        : new List<string>();

                        advertiseDetails.LayoutsList = advertisement.Layouts.Count > 0 ?
                       advertisement.Layouts.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "AddPhoto.png"}").ToList()
                       : new List<string>();

                        advertiseDetails.PhotosList = CheckPhoto(advertiseDetails.PhotosList);
                        advertiseDetails.LayoutsList = CheckPhoto(advertiseDetails.LayoutsList);
                        advertiseDetails.Document = advertisement.Document;
                        advertiseDetails.Decoration = advertisement.Decoration;
                        advertiseDetails.AgreementStatus = advertisement.AgreementStatus;
                        advertiseDetails.Furnished = advertisement.Furnished;
                        advertiseDetails.Elevator = advertisement.Elevator;
                        advertiseDetails.Parking = advertisement.Parking;
                        advertiseDetails.LandingStatus = advertisement.LandingStatus;
                        advertiseDetails.BuildingStatus = advertisement.BuildingStatus;
                        advertiseDetails.ChaletRentType = advertisement.ChaletRentType;
                        advertiseDetails.ChaletRentValue = advertisement.ChaletRentValue;
                        advertiseDetails.NumOfMonths = advertisement.NumOfMonths;
                        advertiseDetails.MinTimeToBookForChaletId = advertisement.MinTimeToBookForChaletId;
                        advertiseDetails.MinTimeToBookName = advertisement.MinTimeToBookForChaletId > 0 ? Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? advertisement.MinTimeToBookForChalet.NameAr : advertisement.MinTimeToBookForChalet.NameEn : "";
                        advertiseDetails.AdvertisementBookings = advertisement.AdvertisementBookings;
                        advertiseDetails.ShowChalet = advertisement.Type == BuildingType.ChaletForSummer && advertisement.AgreementStatus == AgreementStatus.Rent ? true : false;
                        advertiseDetails.Officies = advertisement.Officies;
                        var allFacilities = _definitionAppService.GetAll(new Lookups.Dto.PagedDefinitionResultRequestDto
                        { EnumCategory = DefinitionTypes.Facilities }).Result.Definitions;
                        advertiseDetails.FacilitesApi = allFacilities.Select(
                        x => new FacilityDtoForEdit
                        {
                            Id = x.Id,
                            Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.NameAr : x.NameEn,
                            Description = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? x.DescriptionAr : x.DescriptionEn,
                            Avatar = $"{AppSettingsConstants.Url}/Resources/UploadFiles/{x.Avatar ?? "notFound.png"}",
                            IsSelected = advertisement.AdvertisementFacilites.Select(f => f.Facility).ToList().Any(c => c.Id == x.Id) ?
                        true : false
                        }).ToList();
                        advertiseDetails.DownPayment = advertisement.DownPayment;
                        advertiseDetails.MonthlyInstallment = advertisement.MonthlyInstallment;
                        advertiseDetails.YearlyInstallment = advertisement.YearlyInstallment;
                        advertiseDetails.NumOfYears = advertisement.NumOfYears;
                        advertiseDetails.DeliveryDate = advertisement.DeliveryDate;
                        advertiseDetails.AirConditioner = advertisement.AirConditioner;
                        advertiseDetails.IsEdited = advertisement.IsEdited;

                        list.Add(advertiseDetails);
                        advertiseDetails = new ApiCreateAdvertisementDto();
                    }
                  projectDetails.Advertisements = list;
                }
                return new ApiProjectDetailsDtoOut { Details = projectDetails, Success = true };
            }
            catch (Exception ex)
            {
                return new ApiProjectDetailsDtoOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        public async Task<ApiDeleteProjectOut> DeleteProject([FromBody] ApiDeleteProjectDto input)
        {
            try
            {
                await _projectAppService.Delete(new Abp.Application.Services.Dto.EntityDto<long>(input.ProjectId));
                return new ApiDeleteProjectOut { Success = true, msg = L("SuccessfullyDeleted") };
            }
            catch (Exception ex)
            {
                return new ApiDeleteProjectOut { Error = ex.Message, Success = false };
            }
        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<GetAllProjectsOutput> GetAllProjects([FromBody] ApiProjectInput input)
        {
            try
            {
                var data = await _projectAppService.GetAll(new Projects.Dto.PagedProjectResultRequestDto { Name = input.Keyword,UserId = input.UserId , CompanyId=input.CompanyId });
                var projects = data.Projects.Where(x => x.IsApprove!=null&&x.IsApprove==true && x.IsPublish).Select(x => new ApiProjectDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    DurationId = x.DurationId,
                    FeaturedAd = x.FeaturedAd,
                    CompanyId = x.CompanyId,
                    IsPublish = x.IsPublish,
                    PhotosList = x.Photos.Count > 0 ? x.Photos.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "AddPhoto.png"}").ToList() : new List<string> { $"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png" },
                    LayoutsList = x.Layouts.Count > 0 ? x.Layouts.Select(p => $"{AppSettingsConstants.Url}/Resources/UploadFiles/{p.Avatar ?? "AddPhoto.png"}").ToList() : new List<string> { $"{AppSettingsConstants.Url}/Resources/UploadFiles/AddPhoto.png" },
                    //x.Advertisements.ForEach(
                    //{
                    //    
                    //}),
                    //Advertisements = x.Advertisements,  //Call GetProjectDetails For Each Project Click To Be More Performance 
                }).ToList();

                return new GetAllProjectsOutput { projects = projects, Success = true };
            }
            catch (Exception ex)
            {
                return new GetAllProjectsOutput { Error = ex.Message, Success = false };
            }

        }

        [HttpPost]
        [Language(BrokerConsts.RequestHeaders.Language)]
        public async Task<ApiPlaceDurationOut> PlaceDurationToProjectAds([FromBody] PlaceDurationInput input)
        {
            try
            {
                if (input.ProjectId.HasValue && input.ProjectId > 0 && input.DurationId.HasValue && input.DurationId > 0)
                {
                    var data = await _projectAppService.GetById(new Abp.Application.Services.Dto.EntityDto<long> { Id = input.ProjectId.Value });
                    var duration = await _durationAppService.GetById(new Abp.Application.Services.Dto.EntityDto<long> {Id = input.DurationId.Value });
                    if (data.Id <= 0)
                    {
                        return new ApiPlaceDurationOut { Error = "there is no Project with this id", Success = false };
                    }
                    else
                    if (duration == null)
                    {
                        return new ApiPlaceDurationOut { Error = "there is no duration with this id", Success = false };
                    }
                    if (data.Advertisements != null && data.Advertisements.Count > 0)
                    {
                        foreach (var item in data.Advertisements)
                        {

                            item.DurationId = input.DurationId;
                            item.IsPublish = (bool)input.IsPublish;
                            await _advertisementAppService.Manage(item);

                        }
                    } 

                }

                else
                {
                    return new ApiPlaceDurationOut { Error = "no project id or duration id sent", Success = false };
                }

                return new ApiPlaceDurationOut {  Success = true };
            }
            catch (Exception ex)
            {
                return new ApiPlaceDurationOut { Error = ex.Message, Success = false };
            }

        }
        private List<string> CheckPhoto(List<string> PhotosList)
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


    }
}
