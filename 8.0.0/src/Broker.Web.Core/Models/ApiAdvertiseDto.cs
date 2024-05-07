using Abp.Application.Services.Dto;
using Broker.Advertisements.Dto;
using Broker.Customers.Dto;
using Broker.Helpers;
using Broker.Lookups.Dto;
using Broker.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.Users.Dto;
using Broker.Advertisements;

namespace Broker.Models
{
    public class ApiAdvertiseDto : EntityDto<long>
    {
        public string Title { get; set; }
        public BuildingType Type { get; set; }
        //public int PropertyId { get; set; }
        //public Definition Property { get; set; }
        public long? CityId { get; set; }
        public long? GovernorateId { get; set; }

        //public CityDto City { get; set; }
        public string Compound { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? FloorsNumber { get; set; }
        public string Area { get; set; }
        public decimal? BuildingArea { get; set; }
        public ChaletType? ChaletType { get; set; }
        
        public string BuildingStatus { get; set; }
        public string LandingStatus { get; set; }
        public UsingFor? UsingFor { get; set; }
        public decimal? StreetWidth { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public DateTime? BuildingDate { get; set; }
        public int? Rooms { get; set; }
        public int? Reception { get; set; }
        public int? Balcony { get; set; }
        public int? Kitchen { get; set; }
        public int? Toilet { get; set; }
        public int? NumUnits { get; set; }
        public int? NumPartitions { get; set; }
        public int? OfficesNum { get; set; }
        public int? OfficesFloors { get; set; }
        public long? DurationId { get; set; }
        // public DurationDto Duration { get; set; }
        public long? SeekerId { get; set; }
        // public SeekerDto Seeker { get; set; }
        public long? OwnerId { get; set; }
        //public OwnerDto Owner { get; set; }
        public long? CompanyId { get; set; }
        //public CompanyDto Company { get; set; }
        public long? BrokerPersonId { get; set; }
        //public BrokerPersonDto BrokerPerson { get; set; }
        public bool IsPublish { get; set; }
        public bool? IsApprove { get; set; }
        public string Description { get; set; }
        public bool? FeaturedAd { get; set; }
        public decimal Price { get; set; }
        public PaymentFacilitiesType? PaymentFacility { get; set; }
       // public MrMrsType? MrMrs { get; set; }
        public string AdvertiseMakerName { get; set; }
        public string AdvertiseMaker { get; set; }
        public string MobileNumber { get; set; }
        public string IsWhatsApped { get; set; }
        public string SecondMobileNumber { get; set; }
        public bool ContactRegisterInTheAccount { get; set; }
        
        public decimal? ParkingSpace { get; set; }
        public string Garden { get; set; }
        public decimal? GardenArea { get; set; }
        public string Pool { get; set; }
        public string Shop { get; set; }
        public int? ShopsNumber { get; set; }
        public int? Dinning { get; set; }
        public RentType? Rent { get; set; }
        public string DiningRoom { get; set; }
        public List<FacilityDtoApi> Facilites { get; set; }
        public List<string> Photos { get; set; }
        public string Decoration { get; set; }
        public string Document { get; set; }
        public double? ViewsCount { get; set; }
        public string AgreementStatus { get; set; }
        public string Furnished { get; set; }
        public string Elevator { get; set; }
        public string Parking { get; set; }
        public string PropertyFor { get; set; }

    }
    public class ApiGetAdvertiseDto : EntityDto<long>
    {
        public string Title { get; set; }
        public BuildingType Type { get; set; }
        public long? CityId { get; set; }
        public long? GovernorateId { get; set; }
        public string GovernorateName { get; set; }

        public string Compound { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? FloorsNumber { get; set; }
        public string Area { get; set; }
        public decimal? BuildingArea { get; set; }
        public ChaletType? ChaletType { get; set; }
        
        public string BuildingStatus { get; set; }
        public string LandingStatus { get; set; }
        public UsingFor? UsingFor { get; set; }
        public string UsingForString { get; set; }
        public decimal? StreetWidth { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public DateTime? BuildingDate { get; set; }
        public int? Rooms { get; set; }
        public int? Reception { get; set; }
        public int? Balcony { get; set; }
        public int? Kitchen { get; set; }
        public int? Toilet { get; set; }
        public int? NumUnits { get; set; }
        public int? NumPartitions { get; set; }
        public int? OfficesNum { get; set; }
        public int? OfficesFloors { get; set; }
        public long? DurationId { get; set; }
        public long? SeekerId { get; set; }
        public long? OwnerId { get; set; }
        public long? CompanyId { get; set; }
        public long? BrokerPersonId { get; set; }
        public bool IsPublish { get; set; }
        public bool? IsApprove { get; set; }
        public string Description { get; set; }
        public bool? FeaturedAd { get; set; }
        public string Price { get; set; }
        public PaymentFacilitiesType? PaymentFacility { get; set; }
        public string AdvertiseMakerName { get; set; }
        public string AdvertiseMaker { get; set; }
        public string MobileNumber { get; set; }
        public string IsWhatsApped { get; set; }
        public bool? WhatsApped { get; set; }
        public string SecondMobileNumber { get; set; }
        public bool ContactRegisterInTheAccount { get; set; }
        public decimal? ParkingSpace { get; set; }
        public string Garden { get; set; }
        public decimal? GardenArea { get; set; }
        public string Pool { get; set; }
        public string Shop { get; set; }
        public int? ShopsNumber { get; set; }
        public int? Dinning { get; set; }
        public RentType? Rent { get; set; }
        public string RentValue { get; set; }
        public string DiningRoom { get; set; }
        public List<FacilityDtoApi> Facilites { get; set; }
        public List<string> Photos { get; set; }
        public List<string> Layouts { get; set; }
        public string Decoration { get; set; }
        public string Document { get; set; }
        public double? ViewsCount { get; set; }
        public string AgreementStatus { get; set; }
        public AgreementStatus? AgreementStatusId { get; set; }
        public string Furnished { get; set; }
        public string Elevator { get; set; }
        public string Parking { get; set; }
        public string PropertyFor { get; set; }
        public ChaletRentType? ChaletRentType { get; set; }
        public string ChaletRentTypeString { get; set; }
        public int? NumOfMonths { get; set; }
        public decimal? ChaletRentValue { get; set; }
        public int? MinTimeToBookForChaletId { get; set; }
        public string MinTimeToBookName { get; set; }
        public bool? ShowChalet { get; set; }
        public OfficiesType? Officies { get; set; }
        public List<DateTime> AdvertisementBookings { get; set; }
        public decimal? DownPayment { get; set; }
        public decimal? MonthlyInstallment { get; set; }
        public decimal? YearlyInstallment { get; set; }
        public decimal? NumOfYears { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool? IsEdited { get; set; }

        //Company
        public bool IsCompany { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyAbout { get; set; }
        public string CompanyFacebook { get; set; }
        public string CompanyInstagram { get; set; }
        public string CompanySnapchat { get; set; }
        public string CompanyTiktok { get; set; }
        public string CompanyWebsite { get; set; }
        public string CompanyName { get; set; }
        public double? CompanyLatitude { get; set; }
        public double? CompanyLongitude { get; set; }
        public string ProjectName { get; set; }
        public bool IsFavourite { get; set; }

    }
    public class ApiReturnAdvertisementBookingDto
    {
        public DateTime BookingDate { get; set; }
       // public long AdvertisementId { get; set; }

    }
    public class FacilityDtoApi
        {
       // public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string IsChecked { get; set; }
        
    }

    public class FacilityDtoForEdit
    {
         public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public bool IsSelected { get; set; }

    }



    public class GetAllAdvertisementsOutput
    {
        public GetAllAdvertisementsOutput()
        {
            advertisements = new List<ApiAdvertiseDto>();
        }
        public List<ApiAdvertiseDto> advertisements { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class ApiCreateAdvertisementDto : EntityDto<long>
    {
        public string Title { get; set; }
        public BuildingType Type { get; set; }
        public long? CityId { get; set; }
        public long? GovernorateId { get; set; }

        public string Compound { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? FloorsNumber { get; set; }
        public string Area { get; set; }
        public decimal? BuildingArea { get; set; }
        public ChaletType? ChaletType { get; set; }
        public AgreementStatus? AgreementStatus { get; set; }
        public BuildingStatus? BuildingStatus { get; set; }
        public LandingStatus? LandingStatus { get; set; }
        public UsingFor? UsingFor { get; set; }
        public decimal? StreetWidth { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public DateTime? BuildingDate { get; set; }
        public int? Rooms { get; set; }
        public int? Reception { get; set; }
        public int? Balcony { get; set; }
        public int? Kitchen { get; set; }
        public int? Toilet { get; set; }
        public int? NumUnits { get; set; }
        public int? NumPartitions { get; set; }
        public int? OfficesNum { get; set; }
        public int? OfficesFloors { get; set; }
        public long? DurationId { get; set; }
        public long? SeekerId { get; set; }
        public long? OwnerId { get; set; }
        public long? CompanyId { get; set; }
        public long? BrokerPersonId { get; set; }
        public bool IsPublish { get; set; }
        public bool? IsApprove { get; set; }
        public string Description { get; set; }
        public bool? FeaturedAd { get; set; }
        public decimal Price { get; set; }
        public PaymentFacilitiesType? PaymentFacility { get; set; }
        public MrMrsType? MrMrs { get; set; }
        public string AdvertiseMakerName { get; set; }
        public AdvertiseMakerType? AdvertiseMaker { get; set; }
        public string MobileNumber { get; set; }
        public bool IsWhatsApped { get; set; }
        public string SecondMobileNumber { get; set; }
        public bool ContactRegisterInTheAccount { get; set; }
        public bool? Furnished { get; set; }
        public bool? Elevator { get; set; }
        public bool? Parking { get; set; }
        public decimal? ParkingSpace { get; set; }
        public bool? Garden { get; set; }
        public decimal? GardenArea { get; set; }
        public bool? Pool { get; set; }
        public bool? Shop { get; set; }
        public int? ShopsNumber { get; set; }  
        public DecorationStatus? Decoration { get; set; }
        public DocumentStatus? Document { get; set; }
        public ChaletRentType? ChaletRentType { get; set; }
        public decimal? ChaletRentValue { get; set; }
        public int? NumOfMonths { get; set; }
        public int? MinTimeToBookForChaletId { get; set; }
        public string MinTimeToBookName { get; set; }
        public bool? ShowChalet { get; set; }
        public int? Dinning { get; set; }
        public RentType? Rent { get; set; }
        public bool? DiningRoom { get; set; }
        public OfficiesType? Officies { get; set; }
        public List<string> LayoutsList { get; set; }
        public List<string> PhotosList { get; set; }
        public List<int> AdvertisementFacilitesList { get; set; }
        public List<DateTime> AdvertisementBookings { get; set; }
        public List<FacilityDtoForEdit> FacilitesApi { get; set; }
        public decimal? DownPayment { get; set; }
        public decimal? MonthlyInstallment { get; set; }
        public decimal? YearlyInstallment { get; set; }
        public decimal? NumOfYears { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool? AirConditioner { get; set; }
        public bool? IsEdited { get; set; }
        public long? ProjectId { get; set; }

    }

    public class ApiCreatAdvertisementOut
    {
        public ApiCreatAdvertisementOut()
        {
        }
        public long? AdvertisementId { get; set; }
        public object Advertisement { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class GetAllAdvertisementsInput
    {
        public string Keyword { get; set; }

    }
    public class ApiAdvertisesInput
    {
        public long? AdvertiseId { get; set; }
        public long? UserId { get; set; }
    }
    public class ApiAdvertiseDetailsDtoOut
    {
        public ApiAdvertiseDetailsDtoOut()
        {
            Details = new ApiGetAdvertiseDto();
        }
        public ApiGetAdvertiseDto Details { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiAdvertisementOut
    {
        public ApiAdvertisementOut()
        {
            advertisements = new List<ApiAdvertiseDto>();
        }
        public List<ApiAdvertiseDto> advertisements { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiAdvertisementInput
    {
        public string Name { get; set; }
        public AgreementStatus? AgreementStatus { get; set; }
    }

    public class ApiAdvertisementSearchInput
    {
        public BuildingType? Type { get; set; }
        public long? CityId { get; set; }
        public long? GovernorateId { get; set; }

        //public string Compound { get; set; }
        public string StreetOrCompund { get; set; }
        public int? Rooms { get; set; }
        public string Area { get; set; }
        public decimal? AreaFrom { get; set; }
        public decimal? AreaTo { get; set; }
        public DecorationStatus? Decoration { get; set; }
        public bool? Furnished { get; set; }
        public bool? Parking { get; set; }
        //TypeOfContract
        public AgreementStatus? AgreementStatus { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public bool? IsEdited { get; set; }
        public long? CompanyId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class ApiCompanyInput
    {
        public string Name { get; set; }
        public bool? IsSponsor { get; set; }
    }

    public class ApiAdvertisementForUserInput
    {
        public int? Start { get; set; }
        public int? Length { get; set; }

        public long? BrokerId { get; set; }
        public long? SeekerId { get; set; }
        public long? OwnerId { get; set; }
        public long? CompanyId { get; set; }

    }


    public class ApiCreateFavoritetDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public long AdvertisementId { get; set; }

    }
    public class ApiCreateFavoritetOut
    {
        public ApiCreateFavoritetOut()
        {
        }
        public long? FavoriteId { get; set; }
        public object AdFavorite { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiFavoriteOut
    {
        public ApiFavoriteOut()
        {
            favorites = new List<ApiFavoriteDto>();
        }
        public List<ApiFavoriteDto> favorites { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class GetFavoriteOutput
    {
        public List<AdFavoriteDto> AdFavorites { get; set; }
        public string Error { get; set; }
    }

    public class DeleteFavoriteOutput
    {
        public string message { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class DeleteAdOutput
    {
        public string message { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class ChangeAdStatusOutput
    {
        public long? AdvertiseID { get; set; }
        public bool? ActiveStatus { get; set; }
        public string message { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }
    public class CheckStatusOutput
    {
        public bool Status { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }
    public class ApiFavoriteDto : EntityDto<long>
    {
        public long AdvertisementId { get; set; }
        public string Title { get; set; }
        public BuildingType BuildingType { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string CityName { get; set; }
        public string GovernorateName { get; set; }
        public bool IsPublish { get; set; }
        public string Price { get; set; }
        public string MobileNumber { get; set; }
        public bool IsWhatsApped { get; set; }
        public List<string> Photos { get; set; }
    }
    public class ApiFavoriteInput
    {
        public long UserId { get; set; }

    }

    public class ApiFavoriteExistInput
    {
        public long UserId { get; set; }
        public long AdvertisementId { get; set; }


    }

    public class ApiIsFavouriteExistDtoOut
    {
        public bool IsExist { get; set; }

        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiCreateViewOut
    {
        public ApiCreateViewOut()
        {
        }
        public long? ViewId { get; set; }
        public object AdView { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiCreateViewDto 
    {
        //public string DeviceToken { get; set; }
        public long UserId { get; set; }
        public long AdvertisementId { get; set; }

    }

    public class ApiViewInput
    {
        public long AdvertisementId { get; set; }

    }
    public class ApiViewForChartInput
    {
        public long AdvertisementId { get; set; }
        public int DurationByDays { get; set; }
        public int Interval { get; set; }

    }

    public class ApiViewForChartOut
    {
        public ApiViewForChartOut()
        {
            Counts =new List<double>();
        }
        public List<double> Counts { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiViewOut
    {
        public ApiViewOut()
        {
            Count = 0;
        }
        public double Count { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiUserAdsOut
    {
        public ApiUserAdsOut()
        {
            advertisements = new List<ApiUserAdsDto>();
        }
        public List<ApiUserAdsDto> advertisements { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiUserAdsDto : EntityDto<long>
    {
        public string Title { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool IsPublish { get; set; }
        public bool? IsApproved { get; set; }
        public double? ViewCount { get; set; }
        public List<string> Photos { get; set; }
        public string Type { get; set; }
        public BuildingType TypeId { get; set; }
        public ExpiredStatus? ExpireStatus { get; set; }

    }
    /// <points>
    public class ApiAdsPointsOut
    {
        public ApiAdsPointsOut()
        {
            advertisements = new List<ApiAdPointDto>();
        }
        public List<ApiAdPointDto> advertisements { get; set; }
        public int? TotalPoints { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiAdPointDto 
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Points { get; set; }


    }
    /// </points>

    public class ApiAdstOut
    {
        public ApiAdstOut()
        {
            advertisements = new List<ApiAdsDto>();
        }
        public List<ApiAdsDto> advertisements { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }


    public class ApiAdsDto : EntityDto<long>
    {
        public string Title { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool IsPublish { get; set; }
        public double? ViewCount { get; set; }
        public List<string> Photos { get; set; }

    }
    public class ApiAllAdstOut
    {
        public ApiAllAdstOut()
        {
            advertisements = new List<ApiAllAdsDto>();
        }
        public List<ApiAllAdsDto> advertisements { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiAllAdsDto : EntityDto<long>
    {
        public string Title { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool IsPublish { get; set; }
        public double? ViewCount { get; set; }
        public List<string> Photos { get; set; }
        //public decimal? Price { get; set; }
        public string Price { get; set; }
        public bool? FeaturedAd { get; set; }

        //Company
        public bool IsCompany { get; set; }
        public string CompanyLogo { get; set; }

    }

    public class ApiAdvertiseDetailsMapDtoOut
    {
        public ApiAdvertiseDetailsMapDtoOut()
        {
            Details = new ApiAdvertiseMapDto();
        }
        public ApiAdvertiseMapDto Details { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiAdvertiseMapDto 
    {
        public string Title { get; set; }
        //public string Type { get; set; }
        //public string AgreementStatus { get; set; }
        //public string Area { get; set; }
        public string Price { get; set; }
        public bool IsFavourite { get; set; }
    }
    public class LastAdvertiseInsertedOut
    {

        public long? Id { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class PlaceAdsDurationInput
    {
        public List<long> Ads { get; set; }
        public long? DurationId { get; set; }

        public bool? IsPublish { get; set; }
    }


}
