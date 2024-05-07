using Abp.Application.Services;
using System.Threading.Tasks;
using Broker.Notifications.Dto;
using Broker.Advertisements.Dto;

namespace Broker.Notifications
{
   public interface INotificationAppService : IAsyncCrudAppService<NotificationDto, long, PagedNotificationResultRequestDto, CreateNotificationsDto, NotificationDto>
    {
        Task MarkAsRead();
        Task<GetNotificationListOutput> GetAllNotificationsByUserId(PagedNotificationResultRequestForUserDto input);
        //  Task<bool> GetCheckSubscribtionExpiration();
    }
}
