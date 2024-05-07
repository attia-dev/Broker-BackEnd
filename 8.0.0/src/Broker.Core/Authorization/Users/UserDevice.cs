using Abp.Domain.Entities.Auditing;
using Broker.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Authorization.Users
{
    public class UserDevice : CreationAuditedEntity<long, User>
    {
        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public DeviceTypes Type { get; set; }
        public string DeviceName { get; set; }
        public string IpAddress { get; set; }
        public string RegistrationToken { get; set; }
        public UserDevice()
        {

        }
        public UserDevice(string registrationToken, DeviceTypes type)
        {
            RegistrationToken = registrationToken;
            Type = type;
        }
        public static UserDevice Create(long? UserId, DeviceTypes Type, string DeviceName, string IpAddress, string RegistrationToken)
        {
            var userDevice = new UserDevice
            {
                UserId = UserId,
                Type = Type,
                DeviceName = DeviceName,
                IpAddress = IpAddress,
                RegistrationToken = RegistrationToken,
            };
            return userDevice;
        }
      


    }
}
