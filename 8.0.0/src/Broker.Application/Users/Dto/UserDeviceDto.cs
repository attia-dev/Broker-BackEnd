using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Datatable.Dtos;
using Broker.Helpers;
using System.Collections.Generic;
using Broker.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace Broker.Users.Dto
{
    [AutoMapFrom(typeof(UserDevice))]
    public class UserDeviceDto : FullAuditedEntity<long, User>
    {
        public long? UserId { get; set; }
        public UserDto User { get; set; }
        public DeviceTypes Type { get; set; }
        public string DeviceName { get; set; }
        public string IpAddress { get; set; }
        public string RegistrationToken { get; set; }
    }

    public class PagedUserDeviceResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
    }
    public class GetUserDevicesOutput
    {
        public List<UserDeviceDto> UserDevices { get; set; }
        public string Error { get; set; }
    }
    public class CheckDeviceInput
    {
        public DeviceTypes Type { get; set; }
        public string DeviceName { get; set; }
        public string IpAddress { get; set; }
    }
   
    public class CheckRegisteredDeviceInput
    {
        [Required]
        public virtual string RegistrationToken { get; set; }
        public virtual DeviceTypes Type { get; set; }
        public virtual string OSVersion { get; set; }
        // public CurrentSystemLanguage SystemLanguage { get; set; }
        public long? UserId { get; set; }
        //  public bool IgnoreUser { get; set; }
        public bool CheckForUnique { get; set; }
    }
    public class SendPushNotificationInput
    {
        public long RegisteredDeviceId { get; set; }
        public string Message { get; set; }
    }
}
