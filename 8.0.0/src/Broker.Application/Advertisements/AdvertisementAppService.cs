using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Broker.Authorization.Users;
using Broker.Customers.Dto;
using Broker.Customers;
using Broker.Datatable.Dtos;
using Broker.Users.Dto;
using Broker.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.Advertisements.Dto;
using Microsoft.EntityFrameworkCore;


using Abp.Collections.Extensions;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Broker.Linq.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Broker.Helpers;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using static Broker.Authorization.PermissionNames;
using Broker.Lookups;
using Broker.Lookups.Dto;
using Broker.Notifications.Dto;
using Broker.Notifications;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Broker.Advertisements
{
    public class AdvertisementAppService : BrokerAppServiceBase, IAdvertisementAppService
    {
        private readonly IRepository<Advertisement, long> _advertisementRepository;

        //private readonly IRepository<AdvertisementDecoration, long> _advertisementDecorationRepository;
        //private readonly IRepository<AdvertisementDocument, long> _advertisementDocumentRepository;
        private readonly IRepository<AdvertisementFacility, long> _advertisementFacilityRepository;
        //private readonly IRepository<AdvertisementImage, long> _advertisementImageRepository;
        private readonly IRepository<Photo, long> _photoRepository;
        private readonly IRepository<Layout, long> _layoutRepository;
        private readonly IRepository<AdvertisementBooking, long> _advertisementBookingRepository;
        private readonly IRepository<AdFavorite, long> _adFavoriteRepository;
        private readonly IRepository<AdView, long> _adVieweRepository;
        private readonly IDefinitionAppService _definitionAppService;
        private readonly IDurationAppService _durationAppService;
        private readonly INotificationAppService _notificationAppService;
        private readonly IRepository<UserDevice, long> _userDeviceRepository;
        public AdvertisementAppService(
            IRepository<Advertisement, long> advertisementRepository,
            //IRepository<AdvertisementDecoration, long> advertisementDecorationRepository,
            //IRepository<AdvertisementDocument, long> advertisementDocumentRepository,
            IRepository<AdvertisementFacility, long> advertisementFacilityRepository,
            //IRepository<AdvertisementImage, long> advertisementImageRepository,
            IRepository<AdFavorite, long> adFavoriteRepository,
            IRepository<AdView, long> adVieweRepository,
            IRepository<Photo, long> photoRepository,
            IRepository<Layout, long> layoutRepository,
            IRepository<AdvertisementBooking, long> advertisementBookingRepository,
            IDefinitionAppService definitionAppService, IDurationAppService durationAppService, NotificationAppService notificationAppService
, IRepository<UserDevice, long> userDeviceRepository

            )
        {
            _advertisementRepository = advertisementRepository;
            // _advertisementDecorationRepository = advertisementDecorationRepository;
            // _advertisementDocumentRepository = advertisementDocumentRepository;
            _advertisementFacilityRepository = advertisementFacilityRepository;
            //_advertisementImageRepository = advertisementImageRepository;
            _adFavoriteRepository = adFavoriteRepository;
            _adVieweRepository = adVieweRepository;
            _photoRepository = photoRepository;
            _advertisementBookingRepository = advertisementBookingRepository;
            _definitionAppService = definitionAppService;
            _durationAppService = durationAppService;
            _layoutRepository = layoutRepository;
            _notificationAppService = notificationAppService;
            _userDeviceRepository = userDeviceRepository;
        }

        public async Task<DataTableOutputDto<AdvertisementDto>> IsPaged(GetAdvertisementInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            //var advertisement = await _advertisementRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            var advertisement = _advertisementRepository.GetAllIncluding(x => x.views, x => x.Photos,
                              x => x.Layouts, x => x.AdvertisementFavorites,
                              y => y.AdvertisementFacilites, y => y.AdvertisementBookingsList
                              , x => x.BrokerPerson, x => x.Seeker, x => x.Owner, x => x.Company
                              ).Where(x => x.Id == Convert.ToInt32(input.ids[0])).FirstOrDefault();
                            if (advertisement != null)
                            {


                                if (input.action == "Delete")
                                    await _advertisementRepository.DeleteAsync(advertisement);
                                else if (input.action == "Restore")
                                {
                                    advertisement.IsDeleted = false;
                                    advertisement.IsEdited = true;
                                }

                                if (input.action == "Approve")
                                {
                                    advertisement.IsApprove = true;
                                    advertisement.IsEdited = false;

                                    var notfi = new CreateNotificationsDto();
                                    notfi.BrokerId = advertisement.BrokerPersonId;
                                    notfi.SeekerId = advertisement.SeekerId;
                                    notfi.OwnerId = advertisement.OwnerId;
                                    notfi.CompanyId = advertisement.CompanyId;
                                    notfi.AdId = advertisement.Id;
                                    notfi.UserId = (long)advertisement.CreatorUserId;
                                    notfi.Type = 0;
                                    notfi.IsRead = false;
                                    notfi.Description = L("Common.Advertisement") + advertisement.Title +" #" + advertisement.Id + L("Common.isApproved");
                                    await _notificationAppService.CreateAsync(notfi);
                                }
                                else if (input.action == "Decline")
                                {
                                    //var favourites = await _adFavoriteRepository.GetAllListAsync();

                                    //var filteredList = favourites.Where(x => x.AdvertisementId == advertisement.Id );
                                    if (advertisement.AdvertisementFavorites != null)
                                        foreach (var v in advertisement.AdvertisementFavorites)
                                        {
                                            await _adFavoriteRepository.DeleteAsync(v);
                                        }
                                    await CurrentUnitOfWork.SaveChangesAsync();

                                    advertisement.IsApprove = false;
                                    
                                    advertisement.IsEdited = false;

                                    var notfi = new CreateNotificationsDto();
                                    notfi.BrokerId = advertisement.BrokerPersonId;
                                    notfi.SeekerId = advertisement.SeekerId;
                                    notfi.OwnerId = advertisement.OwnerId;
                                    notfi.CompanyId = advertisement.CompanyId;
                                    notfi.AdId = advertisement.Id;
                                    notfi.UserId = (long)advertisement.CreatorUserId;
                                    notfi.Type = 0;
                                    notfi.IsRead = false;
                                    notfi.Description = L("Common.Advertisement") + advertisement.Title + " #" + advertisement.Id + L("Common.isDeclined");
                                    await _notificationAppService.CreateAsync(notfi);
                                }


                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            //var advertisement = await _advertisementRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            var advertisement = _advertisementRepository.GetAllIncluding(x => x.views, x => x.Photos,
                              x => x.Layouts, x => x.AdvertisementFavorites,
                              y => y.AdvertisementFacilites, y => y.AdvertisementBookingsList
                              , x => x.BrokerPerson, x => x.Seeker, x => x.Owner, x => x.Company
                              ).Where(x => x.Id== Convert.ToInt32(input.ids[0])).FirstOrDefault();

                            if (advertisement != null)
                            {

                                

                                if (input.action == "Delete")//Delete
                                    await _advertisementRepository.DeleteAsync(advertisement);
                                else if (input.action == "Restore")
                                {
                                    advertisement.IsDeleted = false;
                                    advertisement.IsEdited = true;
                                }

                                if (input.action == "Approve")
                                {
                                    advertisement.IsApprove = true;
                                    advertisement.IsEdited = false;

                                    var notfi = new CreateNotificationsDto();
                                    notfi.BrokerId = advertisement.BrokerPersonId;
                                    notfi.SeekerId = advertisement.SeekerId;
                                    notfi.OwnerId = advertisement.OwnerId;
                                    notfi.CompanyId = advertisement.CompanyId;
                                    notfi.AdId = advertisement.Id;
                                    notfi.UserId = (long)advertisement.CreatorUserId;
                                    notfi.Type = 0;
                                    notfi.IsRead = false;
                                    notfi.Description = L("Common.Advertisement") + advertisement.Title + " #" + advertisement.Id + L("Common.isApproved");
                                    await _notificationAppService.CreateAsync(notfi);
                                }
                                else if (input.action == "Decline")
                                {
                                   
                                    //var favourites = await _adFavoriteRepository.GetAllListAsync();

                                    //var filteredList = favourites.Where(x => x.AdvertisementId == advertisement.Id );
                                    if (advertisement.AdvertisementFavorites!=null)
                                    foreach (var v in advertisement.AdvertisementFavorites)
                                    {
                                        await _adFavoriteRepository.DeleteAsync(v);
                                    }
                                    await CurrentUnitOfWork.SaveChangesAsync();

                                    advertisement.IsApprove = false;
                                    advertisement.IsEdited = false;

                                    var notfi = new CreateNotificationsDto();
                                    notfi.BrokerId = advertisement.BrokerPersonId;
                                    notfi.SeekerId = advertisement.SeekerId;
                                    notfi.OwnerId = advertisement.OwnerId;
                                    notfi.CompanyId = advertisement.CompanyId;
                                    notfi.AdId = advertisement.Id;
                                    notfi.UserId = (long)advertisement.CreatorUserId;
                                    notfi.Type = 0;
                                    notfi.IsRead = false;
                                    notfi.Description = L("Common.Advertisement") + advertisement.Title + " #" + advertisement.Id + L("Common.isDeclined");
                                    await _notificationAppService.CreateAsync(notfi);
                                }


                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);

                    var query = _advertisementRepository.GetAllIncluding(x => x.City, x => x.Governorate, x => x.Duration,
                        x => x.Seeker, x => x.Owner, x => x.Company, x => x.BrokerPerson, x => x.MinTimeToBookForChalet)
                                    .Where(x => x.IsPublish == true && ( x.SeekerId!=null || x.BrokerPersonId!=null || x.OwnerId !=null || x.CompanyId!=null));
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.Title.Contains(input.Name) || at.Id.ToString().Contains(input.Name));
                    if (input.IsApprove != null)
                    {
                        switch (input.IsApprove)
                        {
                            case ApprovalStatus.Pending:
                                query = query.Where(x => x.IsApprove == null&&(x.IsEdited==null||x.IsEdited == false));
                                ; break;
                            case ApprovalStatus.Accepted:
                                query = query.Where(x => x.IsApprove == true && (x.IsEdited == null || x.IsEdited == false));
                                ; break;
                            case ApprovalStatus.Rejected:
                                query = query.Where(x => x.IsApprove == false && (x.IsEdited == null || x.IsEdited == false));
                                ; break;
                        }
                    }
                    //else
                    //    query = query.Where(x => x.IsApprove != null);
                    if (input.IsEdited != null)
                    {
                        query = query.WhereIf(input.IsEdited != null, x => x.IsEdited == input.IsEdited);
                    }
                    else
                        query = query.Where(x => x.IsEdited == null || x.IsEdited == false);
                    //query = input.Newer != null && input.Newer == false ? query.OrderBy(x => x.CreationTime) : query.OrderByDescending(x => x.CreationTime);
                    int filteredCount = await query.CountAsync();
                    var advertisements = await query.OrderByDescending(x => x.CreationTime)
                        .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    var listDto = ObjectMapper.Map<List<AdvertisementDto>>(advertisements);
                   
                    return new DataTableOutputDto<AdvertisementDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = listDto,
                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<AdvertisementDto> Manage(AdvertisementDto input)
        {
            try
            {
                input.SeekerId = input.SeekerId <= 0 ? null : input.SeekerId;
                input.OwnerId = input.OwnerId <= 0 ? null : input.OwnerId;
                input.CompanyId = input.CompanyId <= 0 ? null : input.CompanyId;
                input.BrokerPersonId = input.BrokerPersonId <= 0 ? null : input.BrokerPersonId;
                input.Decoration = input.Decoration <= 0 ? null : input.Decoration;
                input.Document = input.Document <= 0 ? null : input.Document;

                if (input.Id > 0)
                {

                    var advertisement = await _advertisementRepository.GetAsync(input.Id);
                    advertisement.Title = input.Title;
                    advertisement.Type = input.Type;

                    advertisement.Compound = input.Compound;
                    advertisement.Street = input.Street;
                    advertisement.BuildingNumber = input.BuildingNumber;
                    advertisement.Latitude = input.Latitude;
                    advertisement.Longitude = input.Longitude;
                    advertisement.FloorsNumber = input.FloorsNumber;
                    advertisement.Area = input.Area;
                    advertisement.BuildingArea = input.BuildingArea;
                    advertisement.ChaletType = input.ChaletType;
                    advertisement.AgreementStatus = input.AgreementStatus;
                    advertisement.BuildingStatus = input.BuildingStatus;
                    advertisement.LandingStatus = input.LandingStatus;
                    advertisement.UsingFor = input.UsingFor;
                    advertisement.StreetWidth = input.StreetWidth;
                    advertisement.Width = input.Width;
                    advertisement.Length = input.Length;
                    advertisement.BuildingDate = input.BuildingDate;
                    advertisement.Rooms = input.Rooms;
                    advertisement.Reception = input.Reception;
                    advertisement.Balcony = input.Balcony;
                    advertisement.Kitchen = input.Kitchen;
                    advertisement.Toilet = input.Toilet;
                    advertisement.NumUnits = input.NumUnits;
                    advertisement.NumPartitions = input.NumPartitions;
                    advertisement.OfficesNum = input.OfficesNum;
                    advertisement.OfficesFloors = input.OfficesFloors;

                    
                    advertisement.IsPublish = input.IsPublish;
                    advertisement.IsApprove = advertisement.IsApprove == input.IsApprove ? null : input.IsApprove;// input.IsApprove;
                    advertisement.Description = input.Description;
                    advertisement.FeaturedAd = input.FeaturedAd;
                    advertisement.Price = input.Price;
                    advertisement.PaymentFacility = input.PaymentFacility;
                    advertisement.MrMrs = input.MrMrs;
                    advertisement.AdvertiseMakerName = input.AdvertiseMakerName;
                    advertisement.AdvertiseMaker = input.AdvertiseMaker;
                    advertisement.MobileNumber = input.MobileNumber;
                    advertisement.IsWhatsApped = input.IsWhatsApped;
                    advertisement.SecondMobileNumber = input.SecondMobileNumber;
                    advertisement.ContactRegisterInTheAccount = input.ContactRegisterInTheAccount;
                    advertisement.Furnished = input.Furnished;
                    advertisement.Elevator = input.Elevator;
                    advertisement.Parking = input.Parking;
                    advertisement.ParkingSpace = input.ParkingSpace;
                    advertisement.Garden = input.Garden;
                    advertisement.GardenArea = input.GardenArea;
                    advertisement.Pool = input.Pool;
                    advertisement.Shop = input.Shop;
                    advertisement.ShopsNumber = input.ShopsNumber;
                    advertisement.ProximityToTheSea = input.ProximityToTheSea;
                    advertisement.Officies = input.Officies;
                    advertisement.AirConditioner = input.AirConditioner;
                    advertisement.DiningRoom = input.DiningRoom;

                    advertisement.Dinning = input.Dinning;
                    advertisement.SeekerId = input.SeekerId;
                    advertisement.OwnerId = input.OwnerId;
                    advertisement.CompanyId = input.CompanyId;
                    advertisement.BrokerPersonId = input.BrokerPersonId;
                    advertisement.Decoration = input.Decoration;
                    advertisement.Document = input.Document;

                    advertisement.CityId = input.CityId;
                    advertisement.GovernorateId = input.GovernorateId;
                    advertisement.DurationId = input.DurationId;
                    advertisement.Rent = input.Rent;
                    advertisement.ChaletRentType = input.ChaletRentType;
                    advertisement.ChaletRentValue = input.ChaletRentValue;
                    advertisement.NumOfMonths = input.NumOfMonths;
                    advertisement.MinTimeToBookForChaletId = input.MinTimeToBookForChaletId;
                    advertisement.IsEdited = true;
                    //advertisement.IsEdited = (advertisement.IsApprove==input.IsApprove)?true:false;

                    // advertisement.ProjectId = input.ProjectId;

                    //   var durationPrice = input.DurationId > 0 ?
                    //_durationAppService.GetById(new EntityDto<long>(input.DurationId ?? 0)).Result.Amount
                    //: 0;
                    //   var pointObject = _definitionAppService.GetAll(new PagedDefinitionResultRequestDto { EnumCategory = DefinitionTypes.Points });
                    //   var pointsUnit = Convert.ToInt32(pointObject.Result.Definitions[0].NameEn);
                    //   var pointsmoney = Convert.ToInt32(pointObject.Result.Definitions[1].NameEn);
                    //
                    //   var points = (durationPrice > 0 && pointsUnit > 0 && pointsmoney > 0)
                    //       ? Convert.ToInt32((durationPrice / pointsmoney) * pointsUnit)
                    //       : 0
                    //       ;
                    //




                    advertisement.DownPayment = input.DownPayment;
                    advertisement.MonthlyInstallment = input.MonthlyInstallment;
                    advertisement.YearlyInstallment = input.YearlyInstallment;
                    advertisement.NumOfYears = input.NumOfYears;
                    advertisement.DeliveryDate = input.DeliveryDate;
              //   if (advertisement.IsApprove==true)
              //   {
              //       var notfi = new CreateNotificationsDto();
              //       if(advertisement.Seeker != null)
              //       notfi.UserId = advertisement.Seeker.UserId ;
              //       else if(advertisement.BrokerPerson != null)
              //       notfi.UserId = advertisement.BrokerPerson.UserId ;
              //       else if(advertisement.Owner != null)
              //       notfi.UserId = advertisement.Owner.UserId ;
              //       else if(advertisement.Company != null)
              //       notfi.UserId = advertisement.Company.UserId ;
              //
              //       notfi.AdId = advertisement.Id;
              //       notfi.Type = NotificationTypes.AcceptAd;
              //       notfi.IsRead = false;
              //       notfi.Description = "Pages.Notifications.Adv.Accepted";
              //       await _notificationAppService.CreateAsync(notfi);
              //   }
              //   else if (advertisement.IsApprove==false)
              //   {
              //       var notfi = new CreateNotificationsDto();
              //       if(advertisement.Seeker != null)
              //       notfi.UserId = advertisement.Seeker.UserId ;
              //       else if(advertisement.BrokerPerson != null)
              //       notfi.UserId = advertisement.BrokerPerson.UserId ;
              //       else if(advertisement.Owner != null)
              //       notfi.UserId = advertisement.Owner.UserId ;
              //       else if(advertisement.Company != null)
              //       notfi.UserId = advertisement.Company.UserId ;
              //
              //       notfi.AdId = advertisement.Id;
              //       notfi.Type = NotificationTypes.RejectAd;
              //       notfi.IsRead = false;
              //       notfi.Description = "Pages.Notifications.Adv.Rejected";
              //       await _notificationAppService.CreateAsync(notfi);
              //   }
                    advertisement = await _advertisementRepository.UpdateAsync(advertisement);


                    //collections

                    //Decorations
                    //   input.AdvertisementDecorations.Where(x => !advertisement.AdvertisementDecorations.Any(b => b.DecorationId == x.DecorationId)).ToList()
                    //           .ForEach(async item =>
                    //             {
                    //                 var advertisementDecoration = AdvertisementDecoration.Create(item.DecorationId, advertisement.Id);
                    //                 await _advertisementDecorationRepository.InsertAsync(advertisementDecoration);
                    //             });
                    //
                    //   advertisement.AdvertisementDecorations.Where(x => !input.AdvertisementDecorations.Any(b => b.DecorationId == x.DecorationId)).ToList()
                    //     .ForEach(async item =>
                    //      {
                    //          await _advertisementDecorationRepository.HardDeleteAsync(item);
                    //      });
                    //
                    //
                    //   //Documents
                    //   input.AdvertisementDocuments.Where(x => !advertisement.AdvertisementDocuments.Any(b => b.DocumentId == x.DocumentId)).ToList()
                    //           .ForEach(async item =>
                    //            {
                    //                 var advertisementDocument = AdvertisementDocument.Create(item.DocumentId, advertisement.Id);
                    //                 await _advertisementDocumentRepository.InsertAsync(advertisementDocument);
                    //             });
                    //
                    //   advertisement.AdvertisementDocuments.Where(x => !input.AdvertisementDocuments.Any(b => b.DocumentId == x.DocumentId)).ToList()
                    //     .ForEach(async item =>
                    //      {
                    //          await _advertisementDocumentRepository.HardDeleteAsync(item);
                    //      });
                    //
                    //Facilities
                    //  input.AdvertisementFacilites.Where(x => !advertisement.AdvertisementFacilites.Any(b => b.FacilityId == x.FacilityId)).ToList()
                    var facilitiesDb = _advertisementFacilityRepository.GetAll().Where(a => a.AdvertisementId == advertisement.Id).ToList();
                    if (input.AdvertisementFacilitesList != null)

                    {
                        input.AdvertisementFacilitesList.Where(x => !facilitiesDb.Any(b => b.FacilityId == x)).ToList()
                                .ForEach(async item =>
                                {
                                    // var advertisementFacility = AdvertisementFacility.Create(item.FacilityId, advertisement.Id);
                                    var advertisementFacility = AdvertisementFacility.Create(item, advertisement.Id);
                                    await _advertisementFacilityRepository.InsertAsync(advertisementFacility);
                                });

                        //  advertisement.AdvertisementFacilites.Where(x => !input.AdvertisementFacilites.Any(b => b.FacilityId == x.FacilityId)).ToList()
                        facilitiesDb.Where(x => !input.AdvertisementFacilitesList.Any(b => b == x.FacilityId)).ToList()
                          .ForEach(async item =>
                          {
                              await _advertisementFacilityRepository.HardDeleteAsync(item);
                          });
                    }

                    //Photos
                    var PhotosDb = _photoRepository.GetAll().Where(a => a.AdvertisementId == advertisement.Id).ToList();
                    // input.Photos.Where(x => !PhotosDb.Any(b => b.Avatar == x.Avatar)).ToList()
                    if (input.PhotosList != null)
                    {
                        input.PhotosList.Where(x => !PhotosDb.Any(b => b.Avatar == x)).ToList()
                                .ForEach(async item =>
                                {
                                    //  var photo = Photo.Create(item.Avatar, advertisement.Id);
                                    var photo = Photo.Create(item, advertisement.Id);
                                    await _photoRepository.InsertAsync(photo);
                                });

                        //PhotosDb.Where(x => !input.Photos.Any(b => b.Avatar == x.Avatar)).ToList()
                        PhotosDb.Where(x => !input.PhotosList.Any(b => b == x.Avatar)).ToList()
                              .ForEach(async item =>
                              {
                                  await _photoRepository.HardDeleteAsync(item);
                              });
                    }


                    //Layouts
                    var LayoutsDb = _layoutRepository.GetAll().Where(a => a.AdvertisementId == advertisement.Id).ToList();
                    //input.Layouts.Where(x => !LayoutsDb.Any(b => b.Avatar == x.Avatar)).ToList()
                    if (input.LayoutsList != null)
                    {
                        input.LayoutsList.Where(x => !LayoutsDb.Any(b => b.Avatar == x)).ToList()
                               .ForEach(async item =>
                               {
                                   //  var layout = Layout.Create(item.Avatar, advertisement.Id);
                                   var layout = Layout.Create(item, advertisement.Id);
                                   await _layoutRepository.InsertAsync(layout);
                               });

                        //  LayoutsDb.Where(x => !input.Layouts.Any(b => b.Avatar == x.Avatar)).ToList()
                        LayoutsDb.Where(x => !input.LayoutsList.Any(b => b == x.Avatar)).ToList()
                              .ForEach(async item =>
                              {
                                  await _layoutRepository.HardDeleteAsync(item);
                              });
                    }


                    //AdvertisementBookings
                    var bookingsDb = _advertisementBookingRepository.GetAll().Where(a => a.AdvertisementId == advertisement.Id).ToList();
                    if (input.AdvertisementBookings != null)
                    {
                        input.AdvertisementBookings.Where(x => !bookingsDb.Any(b => b.BookingDate == x)).ToList()
                           .ForEach(async item =>
                           {
                               var advertisementBooking = AdvertisementBooking.Create(item, advertisement.Id);
                               await _advertisementBookingRepository.InsertAsync(advertisementBooking);
                           });

                        bookingsDb.Where(x => !input.AdvertisementBookings.Any(b => b == x.BookingDate)).ToList()
                              .ForEach(async item =>
                              {
                                  await _advertisementBookingRepository.HardDeleteAsync(item);
                              });
                    }
                    await CurrentUnitOfWork.SaveChangesAsync();

                    return ObjectMapper.Map<AdvertisementDto>(advertisement);

                }
                else
                {

                    input.SeekerId = input.SeekerId <= 0 ? null : input.SeekerId;
                    input.OwnerId = input.OwnerId <= 0 ? null : input.OwnerId;
                    input.CompanyId = input.CompanyId <= 0 ? null : input.CompanyId;
                    input.BrokerPersonId = input.BrokerPersonId <= 0 ? null : input.BrokerPersonId;
                    input.Decoration = input.Decoration <= 0 ? null : input.Decoration;
                    input.Document = input.Document <= 0 ? null : input.Document;

                    var durationPrice = input.DurationId > 0 ?
                    _durationAppService.GetById(new EntityDto<long>(input.DurationId ?? 0)).Result.Amount
                    : 0;
                    var pointObject = _definitionAppService.GetAll(new PagedDefinitionResultRequestDto { EnumCategory = DefinitionTypes.Points });
                    var pointsUnit = Convert.ToInt32(pointObject.Result.Definitions[0].NameEn);
                    var pointsmoney = Convert.ToInt32(pointObject.Result.Definitions[1].NameEn);

                    var points = (durationPrice > 0 && pointsUnit > 0 && pointsmoney > 0)
                        ? Convert.ToInt32((durationPrice / pointsmoney) * pointsUnit)
                        : 0
                        ;


                    var advertisement = Advertisement.Create(
                        input.Title, input.Type, input.CityId, input.GovernorateId, input.Compound, input.Street, input.BuildingNumber, input.Latitude
                        , input.Longitude, input.FloorsNumber, input.Area, input.BuildingArea, input.ChaletType, input.AgreementStatus
                        , input.BuildingStatus, input.LandingStatus, input.UsingFor, input.StreetWidth, input.Width, input.Length,
                        input.BuildingDate, input.Rooms, input.Reception, input.Balcony, input.Kitchen, input.Toilet, input.NumUnits
                        , input.NumPartitions, input.OfficesNum, input.OfficesFloors, input.DurationId, input.SeekerId, input.OwnerId
                        , input.CompanyId, input.BrokerPersonId, input.IsPublish, null, input.Description, input.FeaturedAd
                        , input.Price, input.PaymentFacility, input.MrMrs, input.AdvertiseMakerName, input.AdvertiseMaker,
                        input.MobileNumber, input.IsWhatsApped, input.SecondMobileNumber, input.ContactRegisterInTheAccount,
                        input.Furnished, input.Elevator, input.Parking, input.ParkingSpace, input.Garden, input.GardenArea,
                        input.Pool, input.Shop, input.ShopsNumber, input.ProximityToTheSea, input.Officies, input.AirConditioner,
                        input.DiningRoom, input.Decoration, input.Document, input.Dinning, input.Rent, input.ChaletRentType,
                        input.ChaletRentValue, input.NumOfMonths, input.MinTimeToBookForChaletId, points, input.DownPayment, input.MonthlyInstallment,
                        input.YearlyInstallment, input.NumOfYears, input.DeliveryDate, input.ProjectId,/*input.IsEdited*/ false);
                    var res = await _advertisementRepository.InsertAsync(advertisement);

                    await CurrentUnitOfWork.SaveChangesAsync();

                    //collections

                    //Decorations
                    //  if (input.AdvertisementDecorations!=null)
                    //  input.AdvertisementDecorations.ForEach(
                    //      async x =>
                    //      {
                    //          var advertisementDecoration = AdvertisementDecoration.Create(x.DecorationId, advertisement.Id);
                    //          await _advertisementDecorationRepository.InsertAsync(advertisementDecoration);
                    //      });
                    //
                    //  //Documents
                    //  if (input.AdvertisementDocuments != null)
                    //      input.AdvertisementDocuments.ForEach(
                    //      async x =>
                    //      {
                    //          var advertisementDocument = AdvertisementDocument.Create(x.DocumentId, advertisement.Id);
                    //          await _advertisementDocumentRepository.InsertAsync(advertisementDocument);
                    //      });

                    //Facilities
                   // using (var uow = UnitOfWorkManager.Begin())
                  //  {
                        if (input.AdvertisementFacilitesList != null)
                            input.AdvertisementFacilitesList.ForEach(
                       async x =>
                       {
                           // var advertisementFacility = AdvertisementFacility.Create(x.FacilityId, advertisement.Id);
                           var advertisementFacility = AdvertisementFacility.Create(x, advertisement.Id);
                           await _advertisementFacilityRepository.InsertAsync(advertisementFacility);
                       });

                     
                    //foreach (var x in input.AdvertisementFacilitesList)
                    //    {
                    //        // var advertisementFacility = AdvertisementFacility.Create(x.FacilityId, advertisement.Id);
                    //        var advertisementFacility = AdvertisementFacility.Create(x, advertisement.Id);
                    //        await _advertisementFacilityRepository.InsertAsync(advertisementFacility);
                    //    };





                    //Images
                    if (input.PhotosList != null)
                        input.PhotosList.ForEach(
                        async x =>
                        {
                            // var photo = Photo.Create(x.Avatar, advertisement.Id);
                            var photo = Photo.Create(x, advertisement.Id);
                            await _photoRepository.InsertAsync(photo);
                        });
                    //layouts
                    if (input.LayoutsList != null)
                        input.LayoutsList.ForEach(
                        async x =>
                        {
                            //  var layout = Layout.Create(x.Avatar, advertisement.Id);
                            var layout = Layout.Create(x, advertisement.Id);
                            await _layoutRepository.InsertAsync(layout);
                        });

                    //AdvertisementBookings
                    if (input.AdvertisementBookings != null)
                        input.AdvertisementBookings.ForEach(
                        async x =>
                        {
                            var booking = AdvertisementBooking.Create(x, advertisement.Id);
                            await _advertisementBookingRepository.InsertAsync(booking);
                        });
                      //  uow.Complete();
                   // }
                    await CurrentUnitOfWork.SaveChangesAsync();

                    return ObjectMapper.Map<AdvertisementDto>(advertisement);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<AdvertisementDto> GetById(EntityDto<long> input)
        {
            try
            {
                AdvertisementDto advertisementInfo = new AdvertisementDto();
                if (input.Id > 0)
                {
                    var advertisement = await _advertisementRepository
                        .GetAll().Include(x => x.City).Include(x => x.Duration).Include(x => x.Governorate)
                        //.Include(x => x.Document)
                        //.Include(x => x.Decoration)
                        .Include(x => x.views)
                        .Include(x => x.AdvertisementFacilites).ThenInclude(x => x.Facility)
                        .Include(x => x.Layouts)
                        .Include(x => x.Photos)
                        .Include(x => x.BrokerPerson).ThenInclude(x => x.User)
                        .Include(x => x.Seeker).ThenInclude(x => x.User)
                        .Include(x => x.Owner).ThenInclude(x => x.User)
                        .Include(x => x.Company).ThenInclude(x => x.User)
                        .Include(x => x.AdvertisementBookingsList)
                        .Include(x => x.MinTimeToBookForChalet)
                        .Include(x=>x.Project)
                        .Where(x => x.Id == input.Id).FirstOrDefaultAsync();

                    if (advertisement != null)
                    {
                        advertisementInfo = ObjectMapper.Map<AdvertisementDto>(advertisement);
                        advertisementInfo.AdvertisementBookings = new List<DateTime>();
                        foreach (var item in advertisement.AdvertisementBookingsList)
                        {
                            advertisementInfo.AdvertisementBookings.Add(item.BookingDate);
                        }
                    }
                }
                return advertisementInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<GetAdvertisementOutput> GetAll(PagedAdvertisementResultRequestDto input)
        {
            try
            {
                var query = _advertisementRepository.GetAllIncluding(x => x.City, x => x.Governorate, x => x.Duration, x => x.views
                , x => x.BrokerPerson.User, x => x.Seeker.User, x => x.Owner.User, x => x.Company.User,
                y => y.Photos, y => y.Layouts, x => x.AdvertisementFacilites, m => m.MinTimeToBookForChalet);
                query = query.WhereIf(input.AgreementStatus.HasValue && input.AgreementStatus > 0, x => x.AgreementStatus == input.AgreementStatus);

                int count = await query.CountAsync();
                var list = await query.ToListAsync();
                return new GetAdvertisementOutput { Advertisements = ObjectMapper.Map<List<AdvertisementDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetAdvertisementOutput { Error = ex.Message };
            }
        }

        public async Task<GetAdvertisementOutput> GetAllAdsByUserId(PagedAdvertisementResultRequestForUserDto input)
        {
            try
            {
                var query = _advertisementRepository.GetAllIncluding(x => x.City, x => x.Governorate, x => x.Duration, x => x.views
                , x => x.BrokerPerson.User, x => x.Seeker.User, x => x.Owner.User, x => x.Company.User, y => y.Photos, y => y.Layouts, x => x.AdvertisementFacilites);
                //x => x.City, x => x.Duration, x => x.views

                query = query.WhereIf(input.BrokerID.HasValue && input.BrokerID > 0, x => x.BrokerPersonId == input.BrokerID);
                query = query.WhereIf(input.SeekerID.HasValue && input.SeekerID > 0, x => x.SeekerId == input.SeekerID);
                query = query.WhereIf(input.OwnerID.HasValue && input.OwnerID > 0, x => x.OwnerId == input.OwnerID);
                query = query.WhereIf(input.CompanyID.HasValue && input.CompanyID > 0, x => x.CompanyId == input.CompanyID);

                int count = await query.CountAsync();
                if (input.MaxResultCount > 0)
                    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetAdvertisementOutput { Advertisements = ObjectMapper.Map<List<AdvertisementDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetAdvertisementOutput { Error = ex.Message };
            }
        }

        public async Task Delete(EntityDto<long> input)
        {
            var advertisement = await _advertisementRepository.GetAsync(input.Id);
            await _advertisementRepository.DeleteAsync(advertisement);

            //send notification after delete advertisment
            if (advertisement != null)
            {
                var notfi = new CreateNotificationsDto();

                if (advertisement.SeekerId > 0)
                    notfi.UserId = advertisement.Seeker.UserId;
                else if (advertisement.OwnerId > 0)
                    notfi.UserId = advertisement.Owner.UserId;
                else if (advertisement.BrokerPersonId > 0)
                    notfi.UserId = advertisement.BrokerPerson.UserId;
                else if (advertisement.CompanyId > 0)
                    notfi.UserId = advertisement.Company.UserId;

                notfi.BrokerId = advertisement.BrokerPersonId;
                notfi.SeekerId = advertisement.SeekerId;
                notfi.OwnerId = advertisement.OwnerId;
                notfi.CompanyId = advertisement.CompanyId;

                notfi.AdId = advertisement.Id;
                notfi.IsRead = false;
                notfi.Type = NotificationTypes.RejectAd;

                notfi.Description = L("Notification.YouradvertisemenIsDeleted") + $" {L("Common.Advertisement")}:{advertisement.Title}"; //input.Delete


                var notification = await _notificationAppService.CreateAsync(notfi);

                var Title = L("Notification.DeleteAdvertisement");
                notfi.Description = notfi.Description.Trim('[', ']');
                var registeredDevices = await _userDeviceRepository.GetAll().Where(x => x.UserId == notification.UserId).ToListAsync();

                var RegistrationTokens = new List<string>();
                if (registeredDevices != null)
                {
                    foreach (var registeredDevice in registeredDevices)
                    {
                        RegistrationTokens.Add(registeredDevice.RegistrationToken);
                    }

                    FCMPushNotification fcm = new FCMPushNotification();
                    fcm.SendNotification(new FcmNotificationInput()
                    {
                        RegistrationTokens = RegistrationTokens,
                        //RegistrationToken = registeredDevice.RegistrationToken,
                        Body = notification.Description,
                        Title = Title,
                        AdId = notification.AdId,
                    });

                }
            }
        }
        public async Task<AdFavoriteDto> CreateFavorite(AdFavoriteDto input)
        {
            try
            {
                var adFavorite = await _adFavoriteRepository.GetAll().Where(x => x.UserId == input.UserId && x.AdvertisementId == input.AdvertisementId)
                    .FirstOrDefaultAsync();
                if (adFavorite == null)
                {
                    var favorite = AdFavorite.Create(input.UserId, input.AdvertisementId);
                    var res = await _adFavoriteRepository.InsertAsync(favorite);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<AdFavoriteDto>(favorite);
                }
                else
                    throw new UserFriendlyException("it is already added");
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task DeleteFavorite(long favoriteId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var adFavorite = await _adFavoriteRepository.GetAll().Where(x => x.Id == favoriteId)
                        .FirstOrDefaultAsync();
                if (adFavorite != null)
                    await _adFavoriteRepository.HardDeleteAsync(adFavorite);
            }

        }
        public async Task DeleteFavoriteByAddId(long addId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var adFavorite = await _adFavoriteRepository.GetAll().Where(x => x.AdvertisementId == addId)
                        .FirstOrDefaultAsync();
                if (adFavorite != null)
                    await _adFavoriteRepository.HardDeleteAsync(adFavorite);
            }

        }
        public async Task<GetFavoriteOutput> GetFavoritesForUser(long userId)
        {
            try
            {
                var query = _adFavoriteRepository.GetAll().Include(x => x.Advertisement)
                    .ThenInclude(x => x.City).Include(x => x.Advertisement).ThenInclude(x => x.Photos)
                    .Include(x => x.Advertisement).ThenInclude(x => x.Governorate).Include(x => x.Advertisement)
                    .Where(x => x.UserId == userId);
                //.ThenInclude(x => x.use)
                var list = await query.ToListAsync();

                return new GetFavoriteOutput { AdFavorites = ObjectMapper.Map<List<AdFavoriteDto>>(list) };

            }

            catch (Exception ex)
            {
                return new GetFavoriteOutput { Error = ex.Message };
            }

        }
        public async Task<AdViewDto> CreateView(AdViewDto input)
        {
            try
            {
                /*var adView = await _adVieweRepository.GetAll().Where(x => (x.UserId == input.UserId && x.AdvertisementId == input.AdvertisementId) || (x.UserId == input.UserId && x.DeviceToken == input.DeviceToken && x.AdvertisementId == input.AdvertisementId))
                    .FirstOrDefaultAsync();*/
                var adView = await _adVieweRepository.GetAll().Where(x => (x.UserId == input.UserId && x.AdvertisementId == input.AdvertisementId)).FirstOrDefaultAsync();
                if (adView == null)
                {
                    var view = AdView.Create(/*input.DeviceToken,*/ input.UserId, input.AdvertisementId);
                    var res = await _adVieweRepository.InsertAsync(view);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<AdViewDto>(view);
                }
                else
                    throw new UserFriendlyException("it is already added");
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<GetViewOutput> GetViewsForAdvertisement(long advertisementId)
        {
            try
            {
                var query = _adVieweRepository.GetAll()
                    .Where(x => x.AdvertisementId == advertisementId);

                var list = await query.ToListAsync();

                return new GetViewOutput { AdViews = ObjectMapper.Map<List<AdViewDto>>(list) };

            }

            catch (Exception ex)
            {
                return new GetViewOutput { Error = ex.Message };
            }

        }
        public async Task<AdvertisementDto> ChangeStatus(long advertiseId)
        {
            try
            {

                var advertisement = await _advertisementRepository
                    .GetAllIncluding(x => x.Duration).Where(x => x.Id == advertiseId).FirstOrDefaultAsync();
                //.GetAsync((Int64)advertiseId);
                if (advertisement.DurationId > 0 && DateTime.Now < advertisement.CreationTime.AddMonths(advertisement.Duration.Period))
                {
                    advertisement.IsPublish = !advertisement.IsPublish;

                    advertisement = await _advertisementRepository.UpdateAsync(advertisement);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<AdvertisementDto>(advertisement);
                }
                else
                    throw new UserFriendlyException(this.L("Common.Message.ObjectNotInDuration"));
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> CheckAdvertisementStatus(long advertiseId)
        {
            try
            {

                var advertisement = await _advertisementRepository
                    .GetAllIncluding(x => x.Duration).Where(x => x.Id == advertiseId).FirstOrDefaultAsync();

                if (advertisement.DurationId > 0 && DateTime.Now < advertisement.CreationTime.AddMonths(advertisement.Duration.Period))
                {
                    return true;
                }
                else
                    return false;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task DeleteAllByUserId(PagedAdvertisementResultRequestForUserDto input)
        {
            var query = _advertisementRepository.GetAll();

            query = query.WhereIf(input.BrokerID.HasValue && input.BrokerID > 0, x => x.BrokerPersonId == input.BrokerID);
            query = query.WhereIf(input.SeekerID.HasValue && input.SeekerID > 0, x => x.SeekerId == input.SeekerID);
            query = query.WhereIf(input.OwnerID.HasValue && input.OwnerID > 0, x => x.OwnerId == input.OwnerID);
            query = query.WhereIf(input.CompanyID.HasValue && input.CompanyID > 0, x => x.CompanyId == input.CompanyID);

            foreach (var item in query)
            {
                await _advertisementRepository.DeleteAsync(item);
            }

        }
        public async Task<GetAdvertisementOutput> SearchForApi(GetAdvertisementSearchInput input)
        {
            try
            {
                var query = _advertisementRepository.GetAllIncluding(x => x.City, x => x.Governorate, x => x.Duration
                , x => x.Seeker, x => x.Owner, x => x.Company, x => x.BrokerPerson, x => x.views);
                query = query.FilterDataTable((DataTableInputDto)input);
                query = query.WhereIf(input.Type.HasValue && input.Type > 0, at => at.Type == input.Type)
                .WhereIf(input.CityId.HasValue && input.CityId > 0, at => at.CityId == input.CityId)
                .WhereIf(input.GovernorateId.HasValue && input.GovernorateId > 0, at => at.GovernorateId == input.GovernorateId)
                .WhereIf(!string.IsNullOrEmpty(input.StreetOrCompund), at => at.Street.Contains(input.StreetOrCompund) || at.Compound.Contains(input.StreetOrCompund))
                .WhereIf(input.Rooms.HasValue && input.Rooms > 0, at => at.Rooms == input.Rooms)
                .WhereIf(!string.IsNullOrEmpty(input.Area), at => at.Area.Contains(input.Area))
                .WhereIf(input.Decoration.HasValue && input.Decoration > 0, at => at.Decoration == input.Decoration)
                .WhereIf(input.Furnished.HasValue, at => at.Furnished == true)
                .WhereIf(input.Parking.HasValue, at => at.Parking == true)
                .WhereIf(input.AgreementStatus.HasValue && input.AgreementStatus > 0, at => at.AgreementStatus == input.AgreementStatus)
                .WhereIf(input.PriceFrom.HasValue && input.PriceTo.HasValue && input.PriceFrom >= 0 && input.PriceTo > 0
                , at => input.PriceFrom <= at.Price && at.Price <= input.PriceTo)
                .WhereIf(input.CompanyId.HasValue && input.CompanyId > 0, at => at.CompanyId == input.CompanyId);
                query = query.WhereIf(input.AreaFrom.HasValue && input.AreaTo.HasValue
                    && input.AreaFrom > 0 && input.AreaTo > 0
                    , at => (input.AreaFrom <= at.BuildingArea && at.BuildingArea <= input.AreaTo)
                    || (input.AreaFrom <= at.Width * at.Length && at.Width * at.Length <= input.AreaTo));

                query = query.WhereIf(input.IsEdited.HasValue, at => at.IsEdited == input.IsEdited);
                if (input.Latitude.HasValue && input.Longitude.HasValue && input.Latitude>0 && input.Longitude>0)
                {
                    double minLat = (double)input.Latitude - .15;
                    double maxLat = (double)input.Latitude + .15;
                    double minLong = (double)input.Longitude - .15;
                    double maxLong = (double)input.Longitude + .15;

                    query = query.Where(
                        x => x.Latitude >= minLat && x.Latitude <= maxLat 
                        && x.Longitude >= minLong && x.Longitude <= maxLong  );
                }
                   

                int count = await query.CountAsync();
                var list = await query.ToListAsync();
                return new GetAdvertisementOutput { Advertisements = ObjectMapper.Map<List<AdvertisementDto>>(list) };

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<AdvertisementDto> GetLastAdvertiseInsertedToDB()
        {
            try
            {
                AdvertisementDto advertisementInfo = new AdvertisementDto();
  
                    var advertisement =  _advertisementRepository
                        .GetAll().Include(x => x.City).Include(x => x.Duration).Include(x => x.Governorate)
                        .Include(x => x.views)
                        .Include(x => x.AdvertisementFacilites).ThenInclude(x => x.Facility)
                        .Include(x => x.Layouts)
                        .Include(x => x.Photos)
                        .Include(x => x.BrokerPerson).ThenInclude(x => x.User)
                        .Include(x => x.Seeker).ThenInclude(x => x.User)
                        .Include(x => x.Owner).ThenInclude(x => x.User)
                        .Include(x => x.Company).ThenInclude(x => x.User)
                        .Include(x => x.AdvertisementBookingsList)
                        .Include(x => x.MinTimeToBookForChalet).OrderByDescending(x=>x.Id).FirstOrDefault();
                    if (advertisement != null)
                    {
                        advertisementInfo = ObjectMapper.Map<AdvertisementDto>(advertisement);
                        advertisementInfo.AdvertisementBookings = new List<DateTime>();
                        foreach (var item in advertisement.AdvertisementBookingsList)
                        {
                            advertisementInfo.AdvertisementBookings.Add(item.BookingDate);
                        }
                    }
                
                return advertisementInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<AdvertisementDto> ManageReject(AdvertisementDto input)
        {
            try
            {
                var advertisement = new Advertisement();
                if (input.Id > 0)
                {
                    var advertisementDto = await GetById(new EntityDto<long> { Id=input.Id});

                    //advertisement = await _advertisementRepository.GetAsync(input.Id);

                     advertisement = _advertisementRepository.GetAllIncluding(x => x.views, x => x.Photos,
                            x => x.Layouts, x => x.AdvertisementFavorites,
                            y => y.AdvertisementFacilites, y => y.AdvertisementBookingsList
                            , x => x.BrokerPerson, x => x.Seeker, x => x.Owner, x => x.Company
                            ).Where(x => x.Id == Convert.ToInt32(input.Id)).FirstOrDefault();
                    advertisement.RejectReason = input.RejectReason;
                    advertisement.IsApprove = false;

                    if (advertisement.AdvertisementFavorites != null)
                        foreach (var v in advertisement.AdvertisementFavorites)
                        {
                            await _adFavoriteRepository.DeleteAsync(v);
                        }

                    advertisement = await _advertisementRepository.UpdateAsync(advertisement);

                    await CurrentUnitOfWork.SaveChangesAsync();

                    //send notification after reject advertisment
                    if (advertisement != null)
                    {
                        var notfi = new CreateNotificationsDto();

                        if(advertisementDto.SeekerId>0)
                                 notfi.UserId = advertisementDto.Seeker.UserId;
                        else if(advertisementDto.OwnerId>0)
                                notfi.UserId = advertisementDto.Owner.UserId;
                        else if (advertisementDto.BrokerPersonId  >0)
                                 notfi.UserId = advertisementDto.BrokerPerson.UserId;
                        else if (advertisementDto.CompanyId>0)
                                notfi.UserId = advertisementDto.Company.UserId;
                        
                        notfi.BrokerId = advertisementDto.BrokerPersonId;
                        notfi.SeekerId = advertisementDto.SeekerId;
                        notfi.OwnerId = advertisementDto.OwnerId;
                        notfi.CompanyId = advertisementDto.CompanyId;

                        notfi.AdId = advertisementDto.Id;
                        notfi.IsRead = false;
                        notfi.Type = NotificationTypes.RejectAd;

                        notfi.Description =L("Notification.Youradvertisementwasrejectedbecause", input.RejectReason); //input.RejectReason


                        var notification = await _notificationAppService.CreateAsync(notfi);

                        var Title = L("Notification.RejectAdvertisement") ;
                        notfi.Description = notfi.Description.Trim('[', ']');
                        var registeredDevices = await _userDeviceRepository.GetAll().Where(x => x.UserId == notification.UserId).ToListAsync();

                        var RegistrationTokens = new List<string>();
                        if (registeredDevices != null)
                        {
                            foreach (var registeredDevice in registeredDevices)
                            {
                                RegistrationTokens.Add(registeredDevice.RegistrationToken);
                            }

                            FCMPushNotification fcm = new FCMPushNotification();
                            fcm.SendNotification(new FcmNotificationInput()
                            {
                                RegistrationTokens = RegistrationTokens,
                                //RegistrationToken = registeredDevice.RegistrationToken,
                                Body = notification.Description,
                                Title = Title,
                                AdId = notification.AdId,
                            });

                        }
                   }

                    return ObjectMapper.Map<AdvertisementDto>(advertisement);
                }
                return ObjectMapper.Map<AdvertisementDto>(advertisement);
           
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
