using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using Broker.Customers;
using Broker.Helpers;
using Broker.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Broker.Authorization.PermissionNames;

namespace Broker.Advertisements
{
    [Table("Advertisements")]
    public class Advertisement : FullAuditedEntity<long, User>
    {
        public string Title { get; set; }
        public BuildingType Type { get; set; }
        //public int PropertyId { get; set; }
        //[ForeignKey("PropertyId")]
        //public Definition Property { get; set; }
        public long? CityId { get; set; }

        [ForeignKey("CityId")]
        public City City { get; set; }

        public long? GovernorateId { get; set; }

        [ForeignKey("GovernorateId")]
        public Governorate Governorate { get; set; }

        public string Compound { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        //building , chalet, Villa ,Appartmrnt
        public int? FloorsNumber { get; set; }

        //All ==> Office ,Land , Shop ,building , chalet, Villa ,Appartmrnt
        public string Area { get; set; }

        //Building Only
        public decimal? BuildingArea { get; set; }

        //Chalet only
        public ChaletType? ChaletType { get; set; }

        public AgreementStatus? AgreementStatus { get; set; }

        //All Except Land
        public BuildingStatus? BuildingStatus { get; set; }

        //Land
        public LandingStatus? LandingStatus { get; set; }

        //Land
        public UsingFor? UsingFor { get; set; }

        //Office ,Land , Shop
        public decimal? StreetWidth { get; set; }

        //Land & Building
        public decimal? Width { get; set; }

        public decimal? Length { get; set; }

        //Building
        public DateTime? BuildingDate { get; set; }

        // chalet, Villa ,Appartment
        public int? Rooms { get; set; }
        public int? Dinning { get; set; }
        public int? Reception { get; set; }
        public int? Balcony { get; set; }
        public int? Kitchen { get; set; }
        public int? Toilet { get; set; }

        //Building
        public int? NumUnits { get; set; }

        public int? NumPartitions { get; set; }

        //Office
        public int? OfficesNum { get; set; }

        public int? OfficesFloors { get; set; }

        //All Except Land
     
        public DecorationStatus? Decoration { get; set; }
        // public int? DecorationId { get; set; }
        // [ForeignKey("DecorationId")]
        // public Definition Decoration { get; set; }
        /// ///////////////////////
        // public ICollection<AdvertisementDecoration> AdvertisementDecorations { get; set; }

        //public Finishing? Finishing { get; set; }

        //All
        public DocumentStatus? Document { get; set; }
      //  public int? DocumentId { get; set; }
      //  [ForeignKey("DocumentId")]
      //  public Definition Document { get; set; }
        /// /////////////////////////
       // public ICollection<AdvertisementDocument> AdvertisementDocuments { get; set; }

        // public Documents Documents { get; set; }

        //Facilites
        

        // public  bool Gas { get; set; }
        //  public bool Water { get; set; }
        //  public bool Phone { get; set; }
        //  public bool Internet { get; set; }
        //  public bool Electricity { get; set; }

        public long? DurationId { get; set; }
        [ForeignKey("DurationId")]
        public Duration Duration { get; set; }

        public long? SeekerId { get; set; }
        [ForeignKey("SeekerId")]
        public Seeker Seeker { get; set; }

        public long? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public Owner Owner { get; set; }

        public long? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public long? BrokerPersonId { get; set; }
        [ForeignKey("BrokerPersonId")]
        public BrokerPerson BrokerPerson { get; set; }

        public bool IsPublish { get; set; }
        public bool? IsApprove { get; set; }

        //Edits
        //Table Avatar
        //Enum type(photo-layout) one to many property
        //property description
        //Facilites

       // public ICollection<AdvertisementImage> AdvertisementImages { get; set; }

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
        ////////////////
        public bool? Furnished { get; set; }
        public bool? Elevator { get; set; }
        public bool? Parking { get; set; }
        public decimal? ParkingSpace { get; set; }
        public bool? Garden { get; set; }
        public decimal? GardenArea { get; set; }
        public bool? Pool { get; set; }
        public bool? Shop { get; set; }
        public int? ShopsNumber { get; set; }


        //edits
        public ProximityToTheSeaType? ProximityToTheSea { get; set; }
        public OfficiesType? Officies { get; set; }
        public bool? AirConditioner { get; set; }
        public bool? DiningRoom { get; set; }
        public RentType? Rent { get; set; }
        public ChaletRentType? ChaletRentType { get; set; }
        public decimal? ChaletRentValue { get; set; }
        public int? NumOfMonths { get; set; }

        public int? MinTimeToBookForChaletId { get; set; }
        [ForeignKey("MinTimeToBookForChaletId")]
        public Definition MinTimeToBookForChalet { get; set; }

       // public MinTimeToBookType? MinTimeToBook { get; set; }

        //Company Installment
        public decimal? DownPayment { get; set; }
        public decimal? MonthlyInstallment { get; set; }
        public decimal? YearlyInstallment { get; set; }
        public decimal? NumOfYears { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? Points { get; set; }
        public string RejectReason { get; set; }
        public ICollection<AdView> views { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Layout> Layouts { get; set; }
        public ICollection<AdvertisementFacility> AdvertisementFacilites { get; set; }
        public ICollection<AdvertisementBooking> AdvertisementBookingsList { get; set; }

        public ICollection<AdFavorite> AdvertisementFavorites { get; set; }

        //To Add Some Advs to a one Project
        public long? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public bool? IsEdited { get; set; }
        public static Advertisement Create(
        string Title, BuildingType type, long? CityId,long? GovernorateId, string Compound, string Street, string BuildingNumber
        , double? Latitude, double? Longitude, int? FloorsNumber, string Area, decimal? BuildingArea,
        ChaletType? ChaletType, AgreementStatus? AgreementStatus, BuildingStatus? BuildingStatus,
         LandingStatus? LandingStatus, UsingFor? UsingFor, decimal? StreetWidth, decimal? Width,
         decimal? Length, DateTime? BuildingDate, int? Rooms, int? Reception, int? Balcony, int? Kitchen,
         int? Toilet, int? NumUnits, int? NumPartitions, int? OfficesNum, int? OfficesFloors, long? DurationId,
         long? SeekerId, long? OwnerId, long? CompanyId, long? BrokerPersonId, bool IsPublish, bool? IsApprove,
         string Description, bool? FeaturedAd, decimal Price, PaymentFacilitiesType? PaymentFacility,
         MrMrsType? MrMrs, string AdvertiseMakerName, AdvertiseMakerType? AdvertiseMaker, string MobileNumber,
         bool IsWhatsApped, string SecondMobileNumber, bool ContactRegisterInTheAccount, bool? Furnished,
         bool? Elevator, bool? Parking, decimal? ParkingSpace, bool? Garden, decimal? GardenArea, bool? Pool,
         bool? Shop, int? ShopsNumber
        , ProximityToTheSeaType? ProximityToTheSea, OfficiesType? Officies, bool? AirConditioner, bool? DiningRoom,
          DecorationStatus? Decoration, DocumentStatus? Document ,int? Dinning, RentType? Rent, ChaletRentType? ChaletRentType, decimal? ChaletRentValue 
            ,int? NumOfMonths, int? MinTimeToBookForChaletId, int? Points,
             decimal? DownPayment, decimal? MonthlyInstallment, decimal? YearlyInstallment, decimal? NumOfYears,DateTime? DeliveryDate, long? ProjectId, bool? IsEdited)
        { 
            var Advertisement = new Advertisement
            {
                Title= Title,
                Type = type,
                CityId= CityId,
                GovernorateId= GovernorateId,
                Compound = Compound,
                Street= Street,
                BuildingNumber= BuildingNumber,
                Latitude= Latitude,
                Longitude= Longitude,
                FloorsNumber = FloorsNumber,
                Area = Area,
                BuildingArea = BuildingArea,
                ChaletType = ChaletType,
                AgreementStatus = AgreementStatus,
                BuildingStatus = BuildingStatus,
                LandingStatus = LandingStatus,
                UsingFor = UsingFor,
                StreetWidth = StreetWidth,
                Width= Width,
                Length= Length,
                BuildingDate= BuildingDate,
                Rooms= Rooms,
                Reception= Reception,
                Balcony= Balcony,
                Kitchen= Kitchen,
                Toilet= Toilet,
                NumUnits= NumUnits,
                NumPartitions= NumPartitions,
                OfficesNum= OfficesNum,
                OfficesFloors= OfficesFloors,
                DurationId= DurationId,
                SeekerId= SeekerId,
                OwnerId= OwnerId,
                CompanyId= CompanyId,
                BrokerPersonId= BrokerPersonId,
                IsPublish= IsPublish,
                IsApprove= IsApprove,
                Description= Description,
                FeaturedAd= FeaturedAd,
                Price= Price,
                PaymentFacility= PaymentFacility,
                MrMrs= MrMrs,
                AdvertiseMakerName= AdvertiseMakerName,
                AdvertiseMaker= AdvertiseMaker,
                MobileNumber= MobileNumber,
                IsWhatsApped = IsWhatsApped,
                SecondMobileNumber= SecondMobileNumber,
                ContactRegisterInTheAccount = ContactRegisterInTheAccount,
                Furnished= Furnished,
                Elevator= Elevator,
                Parking = Parking,
                ParkingSpace= ParkingSpace,
                Garden= Garden,
                GardenArea= GardenArea,
                Pool= Pool,
                Shop= Shop,
                ShopsNumber = ShopsNumber,
                ProximityToTheSea = ProximityToTheSea,
                Officies = Officies,
                AirConditioner = AirConditioner,
                DiningRoom = DiningRoom,
                Decoration = Decoration,
                Document = Document,
                Dinning = Dinning,
                Rent = Rent,
                ChaletRentType= ChaletRentType,
                ChaletRentValue = ChaletRentValue,
                NumOfMonths = NumOfMonths,
                MinTimeToBookForChaletId = MinTimeToBookForChaletId,
                Points= Points,
                DownPayment = DownPayment,
                MonthlyInstallment = MonthlyInstallment,
                YearlyInstallment = YearlyInstallment,
                NumOfYears = NumOfYears,
                DeliveryDate=DeliveryDate,
                ProjectId= ProjectId,
                IsEdited = IsEdited,
            };
            return Advertisement;
        }

    }

    
}