using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.UI;
using Broker.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Broker.Users.Dto;
using Broker.Notifications.Dto;
using Broker.Helpers;
using Broker.Security;
using System.Threading;
using System.Globalization;
using Broker.Lookups;
using Broker.Lookups.Dto;
using Broker.Advertisements.Dto;

namespace Broker.Notifications
{
    public class NotificationAppService : AsyncCrudAppService<Notification, NotificationDto, long, PagedNotificationResultRequestDto, CreateNotificationsDto, NotificationDto>, INotificationAppService
    {
        private readonly UserManager _userManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Notification, long> _notificationRepository;
        private readonly AppSession _AppSession;
        private readonly IRepository<UserDevice, long> _userDeviceRepository;
       // private readonly ISubscriptionProviderAppService _SubscriptionProviderAppService;
        //private readonly ISubscriptionAppService _SubscriptionAppService;
        public NotificationAppService(IRepository<Notification, long> notificationRepository/*, ISubscriptionAppService SubscriptionAppService, ISubscriptionProviderAppService SubscriptionProviderAppService*/, IRepository<Notification, long> repository, UserManager userManager, IRepository<User, long> userRepository, AppSession appSession, IRepository<UserDevice, long> userDeviceRepository) : base(repository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _AppSession = appSession;
            _userDeviceRepository = userDeviceRepository;
            LocalizationSourceName = BrokerConsts.LocalizationSourceName;
            //_SubscriptionProviderAppService = SubscriptionProviderAppService;
            //_SubscriptionAppService = SubscriptionAppService;
            _notificationRepository = notificationRepository;
        }

        public override async Task<PagedResultDto<NotificationDto>> GetAllAsync(PagedNotificationResultRequestDto input)
        {
            try
            {
                CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.SetTenantId(1);

                var query = Repository.GetAllIncluding(y => y.Advertisement, x => x.CreatorUser);
                query = query.WhereIf(input.UserId.HasValue, x => x.UserId == input.UserId);
                int count = await query.CountAsync();
                
                if (input.SkipCount >= 0 && input.MaxResultCount > 0)
                    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
            
                query = query.OrderByDescending(x => x.CreationTime);
                var list = await query.ToListAsync();
                var items = ObjectMapper.Map<List<NotificationDto>>(list);
                var notifications = new List<NotificationDto>();
                var notif = new NotificationDto();
                foreach (var item in list)
                {
                    notif = new NotificationDto();
                    notif.Id = item.Id;
                    notif.UserId = item.UserId;
                    notif.IsRead = item.IsRead;
                    notif.AdId = item.AdId;
                    //notif.LawyerBookingId = item.LawyerBookingId;
                    //notif.TeacherBookingId = item.TeacherBookingId;
                    notif.Type = item.Type;
                    notif.Advertisement = ObjectMapper.Map<AdvertisementDto>(item.Advertisement);
                    //notif.LawyerBooking = ObjectMapper.Map<LawyerBookingDto>(item.LawyerBooking);
                    //notif.TeacherBooking = ObjectMapper.Map<TeacherBookingDto>(item.TeacherBooking);
                    notif.Description = getLocalizedDescription(item);
                    notif.Time = await GetTimeFromNow(item.CreationTime.AddHours(-2));
                    if (!string.IsNullOrEmpty(notif.Description))
                        notifications.Add(notif);
                }
                return new PagedResultDto<NotificationDto>
                {
                    TotalCount = count,
                    Items = notifications
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async override Task<NotificationDto> GetAsync(EntityDto<long> input)
        {
            var Notification = await Repository.GetAllIncluding(x => x.User, y => y.Advertisement).Where(usr => usr.Id == input.Id).FirstOrDefaultAsync();
            return ObjectMapper.Map<NotificationDto>(Notification);
        }
        public override async Task<NotificationDto> CreateAsync(CreateNotificationsDto input)
        {
            try
            {
                CheckCreatePermission();
                CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.SetTenantId(1);
                var notification = new Notification();
                notification = Notification.Create(input.UserId,
                         input.AdId, input.Type, input.Description, input.IsRead,input.BrokerId,input.SeekerId
                         ,input.OwnerId,input.CompanyId);
                var result =  _notificationRepository.Insert(notification);
                await CurrentUnitOfWork.SaveChangesAsync();

                var registeredDevices = await _userDeviceRepository.GetAll().Where(x => x.UserId == input.UserId).ToListAsync();

                //   if (registeredDevicestemp != null)
                //   {
                //      registeredDevicestemp = registeredDevicestemp.WhereIf(input.UserId > 0, x => x.UserId == input.UserId);
                //       var registeredDevices = registeredDevicestemp.ToList();

                if (registeredDevices != null && registeredDevices.Count > 0)
                {

                    var item = await Repository.GetAllIncluding(y => y.Advertisement, x => x.CreatorUser).Where(x => x.Id == result.Id).FirstOrDefaultAsync();
                    FCMPushNotification fcm = new FCMPushNotification();
                    foreach (var token in registeredDevices)
                    {
                        fcm.SendNotification(new FcmNotificationInput()
                        {
                            RegistrationToken = token.RegistrationToken,/* registeredDevices.Select(x => x.RegistrationToken).ToList()*/
                            Body = getLocalizedDescription(item),
                            Title = "",
                            BrokerId = input.BrokerId,
                            SeekerId = input.SeekerId,
                            OwnerId = input.OwnerId,
                            CompanyId = input.CompanyId,
                        });
                    }

                }

            
                  

            return ObjectMapper.Map<NotificationDto>(notification);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string getLocalizedDescription(Notification item)
        {
            try
            {
                var desc = L(item.Description);
                var convDate = "";
                if (item.Advertisement != null)
                {
                    /////////////////////////
                    convDate = item.Advertisement.CreationTime.ToString("yyyy-MM-dd", new CultureInfo("ar-JO"));

                    var timing = L("Pages.Notifications.Timing");
                    var Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? item.CreatorUser?.Name : item.CreatorUser?.Surname;
                    var timings = convDate + " " + timing + " " + item.Advertisement.CreationTime.ToString("HH:mm") + " ";
                    desc = L(item.Description, Name, timings);
                }
              //  else if (item.LawyerBooking != null)
              //  {
              //      convDate = item.LawyerBooking.BookingTime.ToString("yyyy-MM-dd", new CultureInfo("ar-JO"));
              //
              //      var timing = L("Pages.Notifications.Timing");
              //      var Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? item.CreatorUser?.Name : item.CreatorUser?.Surname;
              //      var timings = convDate + " " + timing + " " + item.LawyerBooking.BookingTime.ToString("HH:mm") + " ";
              //      desc = L(item.Description, Name, timings);
              //  }
              //  else if (item.TeacherBooking != null)
              //  {
              //      convDate = item.TeacherBooking.BookingTime.ToString("yyyy-MM-dd", new CultureInfo("ar-JO"));
              //
              //      var timing = L("Pages.Notifications.Timing");
              //      var Name = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? item.CreatorUser?.Name : item.CreatorUser?.Surname;
              //      var timings = convDate + " " + timing + " " + item.TeacherBooking.BookingTime.ToString("HH:mm") + " ";
              //      desc = L(item.Description, Name, timings);
              //  }

                return desc;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        private async Task<string> GetTimeFromNow(DateTime yourDate)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - yourDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? L("Pages.ReviewsTime.OneSecond") : L("Pages.ReviewsTime.Seconds", ts.Seconds);

            if (delta < 2 * MINUTE)
                return L("Pages.ReviewsTime.OneMinute");

            if (delta < 45 * MINUTE)
                return L("Pages.ReviewsTime.Minutes", ts.Minutes);

            if (delta < 90 * MINUTE)
                return L("Pages.ReviewsTime.OneHour");

            if (delta < 24 * HOUR)
                return L("Pages.ReviewsTime.Hours", ts.Hours);

            if (delta < 48 * HOUR)
                return L("Pages.ReviewsTime.Yesterday");

            if (delta < 30 * DAY)
                return L("Pages.ReviewsTime.Days", ts.Days);

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? L("Pages.ReviewsTime.OneMonth") : L("Pages.ReviewsTime.Months", months);
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? L("Pages.ReviewsTime.OneYear") : L("Pages.ReviewsTime.Years", years);
            }

        }
        public async Task MarkAsRead()
        {
            try
            {
                await Repository.GetAll().Where(x => x.UserId == _AppSession.UserId).ForEachAsync(x => x.IsRead = true);
            }
            catch (Exception ex)
            {
            }
        }
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public override Task<NotificationDto> UpdateAsync(NotificationDto input)
        {
            return base.UpdateAsync(input);
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var Notification = await GetById(new GetNotificationByIdInput { NotificationId = input.Id });
            await Repository.DeleteAsync(x => x.Id == input.Id);
        }

        public async Task<Notification> GetById(GetNotificationByIdInput input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.SetTenantId(1);
                var Notification = await Repository.GetAllIncluding(x => x.User).Where(usr => usr.Id == input.NotificationId).FirstOrDefaultAsync();
                return ObjectMapper.Map<Notification>(Notification);

            }
        }

        protected override IQueryable<Notification> CreateFilteredQuery(PagedNotificationResultRequestDto input)
        {
            return (IQueryable<Notification>)Repository.GetAllIncluding()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.User.Name.Contains(input.Keyword));
        }


        //public async Task<bool> GetCheckSubscribtionExpiration()
        //{
        //    if (_AppSession.DoctorId != null)
        //    {
        //        var subscriptionProvider = _SubscriptionProviderAppService.GetByDoctorId(_AppSession.DoctorId);

        //        if (subscriptionProvider != null)
        //        {
        //            var checkTime = subscriptionProvider.Result.EndDate.AddDays(-2);
        //            if (checkTime.Date <= DateTime.Now)
        //            {
        //                var createVotfi = new CreateNotificationsDto();
        //                createVotfi.UserId = subscriptionProvider.Result.Doctor.UserId;
        //                createVotfi.Type = NotificationTypes.SubscriptionExpiration;
        //                createVotfi.Description = "Pages.Notifications.Admin.SubscriptionExpiration";

        //                var data = await CreateAsync(createVotfi);
        //                if (data != null)
        //                    return true;
        //                else
        //                    return false;
        //            }
        //        }
        //        return false;
        //    }
        //    else if(_AppSession.TeacherId != null)
        //    {
        //        var subscriptionProvider = _SubscriptionProviderAppService.GetByTeacherId(_AppSession.TeacherId);

        //        if (subscriptionProvider != null)
        //        {
        //            var checkTime = subscriptionProvider.Result.EndDate.AddDays(-2);
        //            if (checkTime.Date <= DateTime.Now)
        //            {
        //                var createVotfi = new CreateNotificationsDto();
        //                createVotfi.UserId = subscriptionProvider.Result.Teacher.UserId;
        //                createVotfi.Type = NotificationTypes.SubscriptionExpiration;
        //                createVotfi.Description = "Pages.Notifications.Admin.SubscriptionExpiration";

        //                var data = await CreateAsync(createVotfi);
        //                if (data != null)
        //                    return true;
        //                else
        //                    return false;
        //            }
        //        }
        //        return false;

        //    }
        //    else if(_AppSession.LawyerId != null)
        //    {
        //        var subscriptionProvider = _SubscriptionProviderAppService.GetByLawyerId(_AppSession.TeacherId);

        //        if (subscriptionProvider != null)
        //        {
        //            var checkTime = subscriptionProvider.Result.EndDate.AddDays(-2);
        //            if (checkTime.Date <= DateTime.Now)
        //            {
        //                var createVotfi = new CreateNotificationsDto();
        //                createVotfi.UserId = subscriptionProvider.Result.Lawyer.UserId;
        //                createVotfi.Type = NotificationTypes.SubscriptionExpiration;
        //                createVotfi.Description = "Pages.Notifications.Admin.SubscriptionExpiration";

        //                var data = await CreateAsync(createVotfi);
        //                if (data != null)
        //                    return true;
        //                else
        //                    return false;
        //            }
        //        }
        //        return false;

        //    }
        //    return false;

        //    //  else{
        //    //      var subscriptionProvider = _SubscriptionProviderAppService.GetByDoctorId(Id);
        //    //
        //    //      if (subscriptionProvider != null)
        //    //      {
        //    //          var checkTime = subscriptionProvider.Result.EndDate.AddDays(-2);
        //    //          if (checkTime.Date <= DateTime.Now)
        //    //          {
        //    //              var createVotfi = new CreateNotificationsDto();
        //    //              createVotfi.UserId = subscriptionProvider.Result.Doctor.UserId;
        //    //              createVotfi.Type = NotificationTypes.SubscriptionExpiration;
        //    //              createVotfi.Description = "SubscriptionExpiration less Than 2 Days";
        //    //
        //    //              var data = await CreateAsync(createVotfi);
        //    //              if (data != null)
        //    //                  return true;
        //    //              else
        //    //                  return false;
        //    //           
        //    //              //public long UserId { get; set; }
        //    //              //public long? DoctorBookingId { get; set; }
        //    //              //public long? LawyerBookingId { get; set; }
        //    //              //public long? TeacherBookingId { get; set; }
        //    //              //public NotificationTypes Type { get; set; }
        //    //              //public string Description { get; set; }
        //    //              //public bool? IsRead { get; set; }
        //    //              //public long? CreatedUserId { get; set; }
        //    //          }
        //    //          // var subscription = _SubscriptionAppService.GetById(new EntityDto<long> { Id = subscriptionProvider.Result.SubscriptionId });
        //    //      }
        //    //      return false;
        //    // }
        //    //
        //}

        public async Task<GetNotificationListOutput> GetAllNotificationsByUserId(PagedNotificationResultRequestForUserDto input)
        {
            try
            {
                var query = _notificationRepository.GetAll();
                query = query.WhereIf(input.UserId.HasValue && input.UserId > 0, x => x.UserId == input.UserId);
                int count = await query.CountAsync();
                //if (input.MaxResultCount > 0)
                //    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                query = query.OrderByDescending(x => x.CreationTime);
                var list = await query.ToListAsync();
                return new GetNotificationListOutput { Notifications = ObjectMapper.Map<List<NotificationDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetNotificationListOutput { Error = ex.Message };
            }
        }

    }
}