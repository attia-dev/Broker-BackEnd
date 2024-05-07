using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Helpers
{
    public enum BuildingType
    {
        Apartment = 1,
        Villa = 2,
        ChaletForSummer = 3,
        Building = 4,
        AdminOffice = 5,
        Shop = 6,
        Land = 7,
    }
    public enum AgreementStatus
    {
        Sell = 1,
        Rent = 2,
        
    }
   
    public enum BuildingStatus
    {
        New = 1,
        Used = 2,
        Renewed = 3,
    }
    public enum LandingStatus
    {
        Empety = 1,
        Used = 2,
    }
    public enum ChaletType
    {
        Chalet = 1,
        Villa = 2,
    }
 
    public enum UsingFor
    {
        Buildings = 1,
        Industrial = 2,
        Agriculture = 3,
        Investment = 4,
        Residential= 5,
        Commercial=6
    }

    public enum Numbers
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        FivePlus = 5,
    }
    public enum DefinitionTypes
    {
        Facilities = 1,
        Documents = 2,
        Decorations = 3,
        About = 4,
        Terms = 5,
        Privacy = 6,
        Contact = 7,
        Points=8,
        FeatureAds =9,
        Property = 10,
        MinTimeToBookForChalet = 11,
    } 
    public enum TransactionType
    {
        InCome = 1,
        OutCome = 2,
    }

    public enum PaymentFacilitiesType
    {
        Allowed = 1,
        NotAllowed = 2,
    }

    public enum MrMrsType
    {
        Mr = 1,
        Mrs = 2,
    }

    public enum AdvertiseMakerType
    {
        Owner = 1,
        Broker = 2,
    }

    public enum UserType
    {
        Broker = 1,
        Seeker = 2,
        Owner = 3,
    }
    public enum AvatarType
    {
        Photo = 1,
        Layout = 2,
    }

    public enum ProximityToTheSeaType
    {
        M100 = 1,
        M500 = 2,
        M500To1KM = 3,
        km1To5KM = 4,
    }

    public enum OfficiesType
    {
        Company = 1,
        Factory = 2,
    }
    public enum RentType
    {
        Monthly = 1,
        MidTerm = 2,
        Annual = 3,
    }
    public enum ChaletRentType
    {
        Day = 1,
        Week = 2,
    }
    public enum MinTimeToBookType
    {
        Day1 = 1,
        Days2 = 2,
        Days3 = 3,
        Week1 = 4,
        Days10 = 5,
    }
    public enum DecorationStatus
    {
         Without = 1,
         SemiFinished = 2,
         Full = 3,
         HighQuality = 4,
    }
    public enum DocumentStatus
    {
        Registered = 1,
        Unregistered = 2,
        Registerable = 3,
        Unregisterable = 4,
    }
    public enum ApprovalStatus
    {
        Pending = 1,
        Accepted = 2,
        Rejected = 3,
    }

    public enum ExpiredStatus
    {
        Pending = 1,
        NotExpired = 2,
        Expired = 3,
    }
    //  public enum Finishing
    //  {
    //      Without = 1,
    //      SemiFinished = 2,
    //      Full = 3,
    //      HighQuality = 4,
    //  }
    // public enum Documents
    // {
    //    
    //     Registered = 1,
    //     Unregistered = 2,
    //     Registerable = 3,
    //     Unregisterable = 4,
    // } 
    public enum DeviceTypes
    {
        WEB = 0,
        ANDROID = 1,
        IOS = 2
    }
    public enum NotificationTypes
    {
        NewAd = 0,
        AcceptAd = 1,
        RejectAd = 2,
        //UpdatePrice = 3,
        //UploadFile = 4,
        //UploadComment = 5,
        //SubscriptionExpiration = 6,
    }
}
