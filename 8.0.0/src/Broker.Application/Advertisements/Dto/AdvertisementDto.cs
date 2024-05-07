using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Customers;
using Broker.Customers.Dto;
using Broker.Datatable.Dtos;
using Broker.Helpers;
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


namespace Broker.Advertisements.Dto
{
    [AutoMapFrom(typeof(Advertisement))]
    public class AdvertisementDto : FullAuditedEntityDto<long>
   
    {
        public string Title { get; set; }
        public BuildingType Type { get; set; }
        // public int PropertyId { get; set; }
        // public Definition Property { get; set; }
        public long? CityId { get; set; }
        public CityDto City { get; set; }

        public long? GovernorateId { get; set; }
        public GovernorateDto Governorate { get; set; }
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
        public DurationDto Duration { get; set; }
        public long? SeekerId { get; set; }
        public SeekerDto Seeker { get; set; }
        public long? OwnerId { get; set; }
        public OwnerDto Owner { get; set; }
        public long? CompanyId { get; set; }
        public CompanyDto Company { get; set; }
        public long? BrokerPersonId { get; set; }
        public BrokerPersonDto BrokerPerson { get; set; }
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
        //public UserDto? CreatorUser { get; set; }
        //public UserDto? LastModifierUser { get; set; }
        //public UserDto? DeleterUser { get; set; }

        public DecorationStatus? Decoration { get; set; }
       // public int? DecorationId { get; set; }
       // public DefinitionDto Decoration { get; set; }
        public DocumentStatus? Document { get; set; }
        // public int? DocumentId { get; set; }
        // public DefinitionDto Document { get; set; }
        /// ///////////////////
        //  public List<AdvertisementDecorationDto>? AdvertisementDecorations { get; set; }
        //  public List<AdvertisementDocumentDto>? AdvertisementDocuments { get; set; }
        // public List<AdvertisementFacilityDto> AdvertisementFacilites { get; set; }
        // public List<AdvertisementImageDto> AdvertisementImages { get; set; }
        // public List<PhotoDto> Photos { get; set; }
        //  public List<LayoutDto> Layouts { get; set; }
        //edits
        public ProximityToTheSeaType? ProximityToTheSea { get; set; }
        public OfficiesType? Officies { get; set; }
        public bool? AirConditioner { get; set; }
        public bool? DiningRoom { get; set; }


        public int? Dinning { get; set; }
        public RentType? Rent { get; set; }
        public ChaletRentType? ChaletRentType { get; set; }
        public decimal? ChaletRentValue { get; set; }
        public int? NumOfMonths { get; set; }

        public int? MinTimeToBookForChaletId { get; set; }
        public DefinitionDto MinTimeToBookForChalet { get; set; }
        // public MinTimeToBookType? MinTimeToBook { get; set; }

        public string RejectReason { get; set; }
        public int? Points { get; set; }

        public List<AdViewDto> views { get; set; }
        public List<AdvertisementFacilityDto> AdvertisementFacilites { get; set; }
        public List<PhotoDto> Photos { get; set; }
         public List<LayoutDto> Layouts { get; set; }
        public List<int> AdvertisementFacilitesList { get; set; }
        public List<string> PhotosList { get; set; }
        public List<string> LayoutsList { get; set; }
        //public List<string> LayoutsList { get; set; }
        public List<DateTime> AdvertisementBookings { get; set; }
        public decimal? DownPayment { get; set; }
        public decimal? MonthlyInstallment { get; set; }
        public decimal? YearlyInstallment { get; set; }
        public decimal? NumOfYears { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long? ProjectId { get; set; }
        public bool? IsEdited { get; set; }

    }

    public class GetAdvertisementInput : DataTableInputDto
    {
        public string Name { get; set; }
        public ApprovalStatus? IsApprove { get; set; }
        //public bool? Newer { get; set; }
        public bool? IsEdited { get; set; }
    }

    public class GetAdvertisementSearchInput : DataTableInputDto
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
    public class PagedAdvertisementResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
        public AgreementStatus? AgreementStatus { get; set; }
        
    }

    public class PagedAdvertisementResultRequestForUserDto : PagedResultRequestDto
    {
        //public UserType UserType { get; set; }
       // public long UserId { get; set; }
        public long? BrokerID { get; set; }
        public long? SeekerID { get; set; }
        public long? OwnerID { get; set; }
        public long? CompanyID { get; set; }
    }
    public class GetAdvertisementOutput
    {
        public List<AdvertisementDto> Advertisements { get; set; }
        public string Error { get; set; }
    }
}
