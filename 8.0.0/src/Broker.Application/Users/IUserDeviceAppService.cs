using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Authorization.Users;
using Broker.Datatable.Dtos;
using Broker.Users.Dto;
using System.Threading.Tasks;

namespace Broker.Users
{
    public interface IUserDeviceAppService : IApplicationService
    {
        Task<GetUserDevicesOutput> GetAll(PagedUserDeviceResultRequestDto input);
        Task<UserDeviceDto> GetById(EntityDto<int> input);
        Task<UserDeviceDto> Manage(UserDeviceDto input);
        Task Delete(EntityDto<int> input);
        Task<UserDevice> CheckDevice(CheckDeviceInput input);
        Task<bool> Check(CheckRegisteredDeviceInput input);
        Task<bool> SendPushNotification(SendPushNotificationInput input);

    }
}
