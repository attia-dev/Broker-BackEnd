using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;
using System.Linq.Dynamic.Core;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using Broker.Linq.Extensions;
//using Broker.Doctors.Dto;
using Broker.Authorization.Users;
using Broker.Users.Dto;
//using Broker.Security;
using Microsoft.AspNetCore.Http;
using Abp.UI;
using Broker.Helpers;
using Broker.Security;

namespace Broker.Users
{
    public class UserDeviceAppService : BrokerAppServiceBase, IUserDeviceAppService
    {
        private readonly IRepository<UserDevice, long> _userDeviceRepository;
        private readonly AppSession _AppSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserDeviceAppService(IRepository<UserDevice, long> repository,IRepository<UserDevice, long> userDeviceRepository,AppSession appSession, IHttpContextAccessor httpContextAccessor) : base()
        {
            _userDeviceRepository = userDeviceRepository;
            _httpContextAccessor = httpContextAccessor;
            _AppSession = appSession;
        }


        public async Task<UserDeviceDto> Manage(UserDeviceDto input)
        {
            try
            {
                var userDevice = new UserDevice();
                if (input.Id > 0)
                {
                    userDevice.UserId = input.UserId;
                    userDevice.Type = input.Type;
                    userDevice.DeviceName = input.DeviceName;
                    userDevice.IpAddress = input.IpAddress;
                    userDevice.RegistrationToken = input.RegistrationToken;
                    userDevice = await _userDeviceRepository.UpdateAsync(userDevice);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<UserDeviceDto>(userDevice);
                }
                else
                {
                    userDevice = UserDevice.Create(input.UserId, input.Type, input.DeviceName, input.IpAddress,input.RegistrationToken);
                    userDevice = await _userDeviceRepository.InsertAsync(userDevice);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<UserDeviceDto>(userDevice);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<int> input)
        {
            var userDevice = await _userDeviceRepository.GetAsync(input.Id);
            await _userDeviceRepository.DeleteAsync(userDevice);
        }

        public async Task<UserDeviceDto> GetById(EntityDto<int> input)
        {
            try
            {
                UserDeviceDto userDeviceinfo = new UserDeviceDto();
                if (input.Id > 0)
                {
                    CurrentUnitOfWork.SetTenantId(1);
                    //  var userDevice = await _userDeviceRepository.GetAsync(input.Id);
                    var userDevice = await _userDeviceRepository.GetAllIncluding(x => x.User).Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                    if (userDevice != null)
                    {
                        userDeviceinfo = ObjectMapper.Map<UserDeviceDto>(userDevice);
                    }
                }
                return userDeviceinfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GetUserDevicesOutput> GetAll(PagedUserDeviceResultRequestDto input)
        {
            try
            {
                CurrentUnitOfWork.SetTenantId(1);
                var query = _userDeviceRepository.GetAllIncluding(x=>x.User);
                int count = await query.CountAsync();
                if (input.MaxResultCount > 0)
                    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetUserDevicesOutput { UserDevices = ObjectMapper.Map<List<UserDeviceDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetUserDevicesOutput { Error = ex.Message };
            }
        }
        public async Task<UserDevice> CheckDevice(CheckDeviceInput input)
        {
            try
            {
                CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
                CurrentUnitOfWork.SetTenantId(1);
                input.IpAddress = GetClientIpAddress();
                input.DeviceName = GetBrowserInfo();
                var device = _userDeviceRepository.GetAll().Where(x => x.IpAddress.Equals(input.IpAddress) || x.DeviceName.Equals(input.DeviceName)).FirstOrDefault();
                if (device == null)
                {
                    device = new UserDevice();
                    device.UserId = _AppSession.UserId.Value;
                    device.IpAddress = input.IpAddress;
                    device.DeviceName = input.DeviceName;
                    device = await _userDeviceRepository.InsertAsync(device);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
                else
                {
                    if (_AppSession.UserId.HasValue)
                    {
                        if (device.UserId != _AppSession.UserId.Value)
                        {
                            device.UserId = _AppSession.UserId.Value;
                            device = await _userDeviceRepository.UpdateAsync(device);
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }
                }

                return device;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string GetBrowserInfo()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return httpContext?.Request?.Headers?["User-Agent"];
        }

        private string GetClientIpAddress()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                return httpContext?.Connection?.RemoteIpAddress?.ToString();

            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
            }

            return null;
        }
        public async Task<bool> Check(CheckRegisteredDeviceInput input)
        {
            CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
            CurrentUnitOfWork.SetTenantId(1);
            var query = _userDeviceRepository.GetAll()
                .Where(x => x.RegistrationToken == input.RegistrationToken && x.Type == input.Type);
            var registeredDevice = await query.FirstOrDefaultAsync();
            if (registeredDevice == null)
                registeredDevice = new UserDevice(input.RegistrationToken, input.Type);

            registeredDevice.Type = input.Type;


            if (input.UserId.HasValue && input.UserId.Value > 0)
                registeredDevice.UserId = input.UserId;
            else
                registeredDevice.UserId = AbpSession.UserId;



            await _userDeviceRepository.InsertOrUpdateAsync(registeredDevice);
            return true;
        }
        public async Task<bool> SendPushNotification(SendPushNotificationInput input)
        {
            try
            {
                var RegistrationTokens = new List<string>();
                if (input.RegisteredDeviceId > 0)
                {
                    var registeredDevice = await _userDeviceRepository.GetAsync(input.RegisteredDeviceId);
                    if (registeredDevice != null)
                        RegistrationTokens.Add(registeredDevice.RegistrationToken);
                    else
                        throw new UserFriendlyException(L("Common.ErrorOccurred"));
                }
                else
                {
                    var registeredDevices = await _userDeviceRepository.GetAll().ToListAsync();
                    foreach (var registeredDevice in registeredDevices)
                    {
                        RegistrationTokens.Add(registeredDevice.RegistrationToken);
                    }
                    FCMPushNotification fcm = new FCMPushNotification();
                    fcm.SendNotification(new FcmNotificationInput()
                    {
                        RegistrationTokens = RegistrationTokens,
                        Body = input.Message,
                        Title = L("Common.SystemTitle")
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error when send push notification is : {ex.Message}");
                throw ex;
            }
        }

    }
}



//using Abp.Application.Services.Dto;
//using Abp.Domain.Repositories;
//using Abp.Domain.Uow;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Abp.Linq.Extensions;
//using Abp.Collections.Extensions;
//using System.Linq.Dynamic.Core;
//using Broker.Datatable.Dtos;
//using Broker.Lookups.Dto;
//using Broker.Linq.Extensions;
//using Broker.Doctors.Dto;
//using Broker.Authorization.Users;
//using Broker.Users.Dto;

//namespace Broker.Users
//{
//    public class UserDeviceAppService : BrokerAppServiceBase, IUserDeviceAppService
//    {
//        private readonly IRepository<UserDevice, long> _UserDeviceRepository;

//        public UserDeviceAppService(IRepository<UserDevice, long> UserDeviceRepository)
//        {
//            _UserDeviceRepository = UserDeviceRepository;
//        }

//        public async Task<DataTableOutputDto<UserDeviceDto>> IsPaged(GetUserDeviceInput input)
//        {
//            try
//            {
//                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
//                {
//                    if (input.actionType == "GroupAction")
//                    {
//                        for (int i = 0; i < input.ids.Length; i++)
//                        {
//                            var UserDevice = await _UserDeviceRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
//                            if (UserDevice != null)
//                            {
//                                if (input.action == "Delete")
//                                    await _UserDeviceRepository.DeleteAsync(UserDevice);
//                                else if (input.action == "Restore")
//                                {
//                                    UserDevice.IsDeleted = false;
//                                    UserDevice.DeleterUserId = null;
//                                    UserDevice.DeletionTime = null;
//                                }
//                            }
//                        }
//                        await CurrentUnitOfWork.SaveChangesAsync();
//                    }
//                    else if (input.actionType == "SingleAction")
//                    {
//                        if (input.ids.Length > 0)
//                        {
//                            var UserDevice = await _UserDeviceRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
//                            if (UserDevice != null)
//                            {
//                                if (input.action == "Delete")//Delete
//                                    await _UserDeviceRepository.DeleteAsync(UserDevice);
//                                else if (input.action == "Restore")
//                                {
//                                    UserDevice.IsDeleted = false;
//                                    UserDevice.DeleterUserId = null;
//                                    UserDevice.DeletionTime = null;
//                                }
//                            }
//                        }
//                        await CurrentUnitOfWork.SaveChangesAsync();
//                    }
//                    CurrentUnitOfWork.SetTenantId(1);
//                    var query = _UserDeviceRepository.GetAll().Include(x => x.Customer).ThenInclude(x => x.User).Include(x => x.Doctor).ThenInclude(x => x.User).Where(x => 1 == 1); ;
//                    int count = await query.CountAsync();
//                    query = query.FilterDataTable((DataTableInputDto)input);
//                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.Customer.User.Name.Contains(input.Name) || at.Customer.User.Surname.Contains(input.Name)
//                                                                                   ||at.Doctor.User.Name.Contains(input.Name) || at.Doctor.User.Surname.Contains(input.Name));
//                    int filteredCount = await query.CountAsync();
//                    var UserDevices = await query
//                          .OrderBy(string.Format("{0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
//                          .Skip(input.start)
//                          .Take(input.length)
//                          .ToListAsync();
//                    return new DataTableOutputDto<UserDeviceDto>
//                    {
//                        draw = input.draw,
//                        iTotalDisplayRecords = filteredCount,
//                        iTotalRecords = count,
//                        aaData = ObjectMapper.Map<List<UserDeviceDto>>(UserDevices)
//                    };
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task<UserDeviceDto> Manage(UserDeviceDto input)
//        {
//            try
//            {
//                var UserDevice = new UserDevice();
//                if (input.Id > 0)
//                {
//                    UserDevice = await _UserDeviceRepository.GetAsync(input.Id);
//                    UserDevice.UserId = input.UserId;
//                    UserDevice.Type = input.Type;
//                    UserDevice.DeviceName = input.DeviceName;
//                    UserDevice.IpAddress = input.IpAddress;
//                    UserDevice = await _UserDeviceRepository.UpdateAsync(UserDevice);
//                    await CurrentUnitOfWork.SaveChangesAsync();
//                    return ObjectMapper.Map<UserDeviceDto>(UserDevice);
//                }
//                else
//                {                                                            
//                    UserDevice = UserDevice.Create(input.UserId, input.Type, input.DeviceName,input.IpAddress);
//                    UserDevice = await _UserDeviceRepository.InsertAsync(UserDevice);
//                    await CurrentUnitOfWork.SaveChangesAsync();
//                    return ObjectMapper.Map<UserDeviceDto>(UserDevice);
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public async Task Delete(EntityDto<int> input)
//        {
//            var UserDevice = await _UserDeviceRepository.GetAsync(input.Id);
//            await _UserDeviceRepository.DeleteAsync(UserDevice);
//        }

//        public async Task<UserDeviceDto> GetById(EntityDto<int> input)
//        {
//            try
//            {
//                UserDeviceDto UserDeviceinfo = new UserDeviceDto();
//                if (input.Id > 0)
//                {
//                    CurrentUnitOfWork.SetTenantId(1);
//                  //  var UserDevice = await _UserDeviceRepository.GetAsync(input.Id);
//                    var UserDevice = await _UserDeviceRepository.GetAll().Include(x => x.Customer).ThenInclude(x => x.User).Include(x => x.Doctor).ThenInclude(x => x.User).Where(x => x.Id == input.Id).FirstOrDefaultAsync();
//                    if (UserDevice != null)
//                    {
//                        UserDeviceinfo = ObjectMapper.Map<UserDeviceDto>(UserDevice);
//                    }
//                }
//                return UserDeviceinfo;
//            }
//            catch (Exception ex)
//            {

//                throw;
//            }
//        }

//        public async Task<GetUserDevicesOutput> GetAll(PagedUserDeviceResultRequestDto input)
//        {
//            try
//            {
//                CurrentUnitOfWork.SetTenantId(1);
//                var query = _UserDeviceRepository.GetAll().Include(x=>x.Customer).ThenInclude(x=>x.User).Include(x=>x.Doctor).ThenInclude(x => x.User).Where(x=>1==1);
//                int count = await query.CountAsync();
//                if (input.MaxResultCount > 0)
//                    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
//                var list = await query.ToListAsync();
//                return new GetUserDevicesOutput { UserDevices = ObjectMapper.Map<List<UserDeviceDto>>(list) };
//            }
//            catch (Exception ex)
//            {
//                return new GetUserDevicesOutput { Error = ex.Message };
//            }
//        }

//    }
//}
