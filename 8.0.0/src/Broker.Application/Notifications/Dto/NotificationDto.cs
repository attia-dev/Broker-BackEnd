using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Advertisements;
using Broker.Advertisements.Dto;
using Broker.Datatable.Dtos;

using Broker.Helpers;

using Broker.Users.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Broker.Notifications.Dto
{
    [AutoMapFrom(typeof(Notification))]
    public class NotificationDto : FullAuditedEntityDto<long>
    {
        public long UserId { get; set; }
        public UserDto User { get; set; }
        public long? AdId { get; set; }
        public AdvertisementDto Advertisement { get; set; }
        public NotificationTypes Type { get; set; }
        public string Description { get; set; }
        public bool? IsRead { get; set; }
        public string Time { get; set; }

    }
    public class GetNotificationsInput : DataTableInputDto
    {
        public long UserId { get; set; }
        public long? AdId { get; set; }
        public NotificationTypes Type { get; set; }
        public string Description { get; set; }
        public bool? IsRead { get; set; }
    }
    public class GetNotificationByIdInput
    {
        [Required]
        public long NotificationId { get; set; }
    }
    public class PagedNotificationResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        public double? UserId { get; set; }
    }
    public class GetNotificationOutput
    {
        public NotificationDto Notification { get; set; }
        public string Error { get; set; }
    }
    public class ManageNotificationInput
    {
        public long UserId { get; set; }
        public long? AdId { get; set; }
        public NotificationTypes Type { get; set; }
        public string Description { get; set; }
        public bool? IsRead { get; set; }

    }

    public class GetNotificationsOutput
    {
        public NotificationDto Notifications { get; set; }
        public string Error { get; set; }
    }


    public class CreateNotificationsDto
    {

        public long UserId { get; set; }
        public long? AdId { get; set; }
        public NotificationTypes Type { get; set; }
        public string Description { get; set; }
        public bool? IsRead { get; set; }
        public long? CreatedUserId { get; set; }

        public long? BrokerId { get; set; }
        public long? SeekerId { get; set; }
        public long? OwnerId { get; set; }
        public long? CompanyId { get; set; }
    }

    public class PagedNotificationResultRequestForUserDto /*: PagedResultRequestDto*/
    {
        public long? UserId { get; set; }
    }

    public class GetNotificationListOutput
    {
        public List<NotificationDto> Notifications { get; set; }
        public string Error { get; set; }
    }


}
