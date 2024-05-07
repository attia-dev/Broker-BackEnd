using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Broker.Authorization;
using Broker.Authorization.Accounts;
using Broker.Authorization.Roles;
using Broker.Authorization.Users;
using Broker.Datatable.Dtos;
using Broker.Linq.Extensions;
using Broker.Lookups.Dto;
using Broker.Lookups;
using Broker.Roles.Dto;
using Broker.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Abp.Collections.Extensions;

using System.Linq.Dynamic.Core;
using System.Text;
using Broker.Advertisements;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Abp.EntityFrameworkCore.Repositories;

namespace Broker.Users
{
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IRepository<User, long> _repository;
        private readonly IRepository<Advertisement, long> _advertisementRepository;

        
        private readonly IRepository<AdView, long> _adViewRepository;
        private readonly IRepository<Photo, long> _photoRepository;
        private readonly IRepository<Layout, long> _layoutRepository;
        private readonly IRepository<AdvertisementFacility, long> _advertisementFacilityRepository;
        private readonly IRepository<AdvertisementBooking, long> _advertisementBookingRepository;

        private readonly IRepository<AdFavorite, long> _adFavoriteRepository;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager, IRepository<Advertisement, long> advertisementRepository,
            IRepository<AdView, long> adViewRepository, IRepository<Photo, long> photoRepository, IRepository<Layout, long> layoutRepository,
            IRepository<AdvertisementFacility, long> advertisementFacilityRepository, IRepository<AdvertisementBooking,
                long> advertisementBookingRepository, IRepository<AdFavorite, long> adFavoriteRepository)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _repository = repository;

            _advertisementRepository = advertisementRepository;

            _adViewRepository = adViewRepository;
            _photoRepository = photoRepository;
            _layoutRepository = layoutRepository;
            _advertisementFacilityRepository = advertisementFacilityRepository;
            _advertisementBookingRepository = advertisementBookingRepository;
            _adFavoriteRepository = adFavoriteRepository;

        }

        public async Task<DataTableOutputDto<UserDto>> IsPaged(GetUsersInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var user = await Repository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (user != null)
                            {
                                if (input.action == "Delete")
                                {
                                    var adsList = _advertisementRepository.GetAllIncluding(x => x.views, x => x.Photos, x => x.Layouts,
                                        y => y.AdvertisementFacilites, y => y.AdvertisementBookingsList
                                    , x => x.BrokerPerson, x => x.Seeker, x => x.Owner, x => x.Company
                                    ).Where(x => x.BrokerPerson.UserId == user.Id || x.Seeker.UserId == user.Id
                                    || x.Owner.UserId == user.Id || x.Company.UserId == user.Id);

                                    foreach (var item in adsList)
                                    {
                                        var favourites = _adFavoriteRepository.GetAll().Where(x=>x.AdvertisementId==item.Id);
                                        foreach (var v in favourites)
                                        {
                                            await _adFavoriteRepository.DeleteAsync(v);
                                        }
                                        foreach (var v in item.views)
                                        {
                                            await _adViewRepository.DeleteAsync(v);
                                        }
                                        foreach (var v in item.Layouts)
                                        {
                                            await _layoutRepository.DeleteAsync(v);
                                        }

                                        foreach (var v in item.Photos)
                                        {
                                            await _photoRepository.DeleteAsync(v);
                                        }

                                        foreach (var v in item.AdvertisementFacilites)
                                        {
                                            await _advertisementFacilityRepository.DeleteAsync(v);
                                        }

                                        foreach (var v in item.AdvertisementBookingsList)
                                        {
                                            await _advertisementBookingRepository.DeleteAsync(v);
                                        }
                                        
                                        await _advertisementRepository.DeleteAsync(item);
                                    }
                                    await Repository.DeleteAsync(user);
                                }
                                
                                else if (input.action == "Restore")
                                {
                                    user.IsDeleted = false;
                                    user.DeleterUserId = null;
                                    user.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var user = await Repository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (user != null)
                            {
                                if (input.action == "Delete")//Delete
                                {

                                    var adsList = _advertisementRepository.GetAllIncluding(x => x.views, x => x.Photos, x => x.Layouts,
                                        y => y.AdvertisementFacilites, y => y.AdvertisementBookingsList
                                    , x => x.BrokerPerson, x => x.Seeker, x => x.Owner, x => x.Company
                                    ).Where(x => x.BrokerPerson.UserId == user.Id || x.Seeker.UserId == user.Id
                                    || x.Owner.UserId == user.Id || x.Company.UserId == user.Id);

                                    foreach (var item in adsList)
                                    {
                                        var favourites = _adFavoriteRepository.GetAll().Where(x => x.AdvertisementId == item.Id);
                                        foreach (var v in favourites)
                                        {
                                            await _adFavoriteRepository.DeleteAsync(v);
                                        }

                                        foreach (var v in item.views)
                                        {
                                            await _adViewRepository.DeleteAsync(v);
                                        }
                                        foreach (var v in item.Layouts)
                                        {
                                            await _layoutRepository.DeleteAsync(v);
                                        }

                                        foreach (var v in item.Photos)
                                        {
                                            await _photoRepository.DeleteAsync(v);
                                        }

                                        foreach (var v in item.AdvertisementFacilites)
                                        {
                                            await _advertisementFacilityRepository.DeleteAsync(v);
                                        }

                                        foreach (var v in item.AdvertisementBookingsList)
                                        {
                                            await _advertisementBookingRepository.DeleteAsync(v);
                                        }

                                        await _advertisementRepository.DeleteAsync(item);
                                    }
                                    await Repository.DeleteAsync(user);

                                }
                                
                                
                                else if (input.action == "Restore")
                                {
                                    user.IsDeleted = false;
                                    user.DeleterUserId = null;
                                    user.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }

                    //var query = Repository.GetAllIncluding(z => z.Roles).Where(z => z.TenantId == _abpSession.TenantId && z.Roles.Any(r => r.RoleId == 2));
                    var query = Repository.GetAll().Where(x=>x.IsMobileUser==false && x.PhoneNumber!= "Deleted@Deleted.Deleted");
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.Name.Contains(input.Name));
                    int filteredCount = await query.CountAsync();
                    List<User> banners = await query
                          .OrderBy(string.Format("{0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<UserDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        //aaData = MapperService.MapList<User, UserDto>(banners)
                        aaData = ObjectMapper.Map<List<UserDto>>(banners)


                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [AbpAuthorize]
        public async Task SavePermissions(SaveUserPermissionsInput input)
        {
            try
            {
                input.TenantId = input.TenantId ?? _abpSession.TenantId;
                using (UnitOfWorkManager.Current.SetTenantId(input.TenantId))
                {
                    var data = Repository.GetAll().Where(x => x.TenantId == input.TenantId && x.IsDeleted == false);
                    var user = await Repository.FirstOrDefaultAsync(input.userId);
                    if (user != null)
                    {
                        var permissionsList = new List<Permission>();
                        foreach (string permissionName in input.grantedPermissions)
                        {
                            permissionsList.Add(PermissionManager.GetPermission(permissionName));
                        }
                        await _userManager.SetGrantedPermissionsAsync(user, permissionsList);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        [AbpAuthorize]
        public async Task<GetUserWithPermissionsOutput> GetWithPermissionsById(EntityDto<long> input)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(usr => usr.Id == input.Id);
                var grantedPermissions = await _userManager.GetGrantedPermissionsAsync(user);
                var grantedPermissionsDto = grantedPermissions
                .Select(x => new PermissionCustomDto()
                {
                    Name = x.Name,
                    Description = x.Description?.Localize(LocalizationManager),
                    DisplayName = x.DisplayName?.Localize(LocalizationManager),
                    SystemName = (x.DisplayName as LocalizableString).Name.ToString(),
                    MultiTenancySides = x.MultiTenancySides
                }).ToList();
                var allPermissions = PermissionManager.GetAllPermissions();
                var allPermissionsDto = allPermissions.Where(x => x.Parent == null)
                .Select(x => new PermissionCustomDto()
                {
                    Name = x.Name,
                    Description = x.Description?.Localize(LocalizationManager),
                    DisplayName = x.DisplayName?.Localize(LocalizationManager),
                    SystemName = (x.DisplayName as LocalizableString).Name.ToString(),
                    MultiTenancySides = x.MultiTenancySides,
                    Children = x.Children.Select(y => new PermissionCustomDto()
                    {
                        Name = y.Name,
                        Description = y.Description?.Localize(LocalizationManager),
                        DisplayName = y.DisplayName?.Localize(LocalizationManager),
                        SystemName = (y.DisplayName as LocalizableString).Name.ToString(),
                        MultiTenancySides = y.MultiTenancySides
                    }).ToList()
                }).ToList();
               
                return new GetUserWithPermissionsOutput { User = ObjectMapper.Map<UserDto>(user), GrantedPermissions = grantedPermissionsDto, AllPermissions = allPermissionsDto };
            }
            catch (Exception ex)
            {
                return new GetUserWithPermissionsOutput { User = new UserDto(), GrantedPermissions = new List<PermissionCustomDto>(), AllPermissions = null };
            }
        }
        [AbpAuthorize]
        public async Task<GetUserWithPermissionsOutput> ClearExplicitPermissions(EntityDto<long> input)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == input.Id);
                await _userManager.ResetAllPermissionsAsync(user);
                await CurrentUnitOfWork.SaveChangesAsync();
                var grantedPermissions = await _userManager.GetGrantedPermissionsAsync(user);
                var grantedPermissionsDto = grantedPermissions
                .Select(x => new PermissionCustomDto()
                {
                    Name = x.Name,
                    Description = x.Description?.Localize(LocalizationManager),
                    DisplayName = x.DisplayName?.Localize(LocalizationManager),
                    MultiTenancySides = x.MultiTenancySides
                }).ToList();
                var allPermissions = PermissionManager.GetAllPermissions();
                var allPermissionsDto = allPermissions
                .Select(x => new PermissionCustomDto()
                {
                    Name = x.Name,
                    Description = x.Description?.Localize(LocalizationManager),
                    DisplayName = x.DisplayName?.Localize(LocalizationManager),
                    MultiTenancySides = x.MultiTenancySides
                }).ToList();
                return new GetUserWithPermissionsOutput { User = ObjectMapper.Map<UserDto>(user), GrantedPermissions = grantedPermissionsDto, AllPermissions = allPermissionsDto };
            }
            catch (Exception ex)
            {
                return new GetUserWithPermissionsOutput { User = new UserDto(), GrantedPermissions = new List<PermissionCustomDto>(), AllPermissions = null };
            }
        }

        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
                await SavePermissions(new SaveUserPermissionsInput
                {
                    userId = user.Id,
                    grantedPermissions = new string[]
                         {
                            Authorization.PermissionNames.Admins.Read,
                            Authorization.PermissionNames.Admins.Write,
                            Authorization.PermissionNames.Admins.Delete,
                            Authorization.PermissionNames.UserPermissions.Read,
                            Authorization.PermissionNames.UserPermissions.Write,
                             //Authorization.PermissionNames.ActivityLog.Read,
                         }
                });
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            //var user = await _userManager.GetUserByIdAsync(input.Id);
            //await _userManager.DeleteAsync(user);

            var user = await Repository.FirstOrDefaultAsync(Convert.ToInt32(input.Id));


            if (user != null)
            {
                var adsList = _advertisementRepository.GetAllIncluding(x => x.views, x => x.Photos, x => x.Layouts, x => x.AdvertisementFavorites,
                    y => y.AdvertisementFacilites, y => y.AdvertisementBookingsList
                , x => x.BrokerPerson, x => x.Seeker, x => x.Owner, x => x.Company
                ).Where(x => x.BrokerPerson.UserId == user.Id || x.Seeker.UserId == user.Id
                || x.Owner.UserId == user.Id || x.Company.UserId == user.Id);


                foreach (var item in adsList)
                {
                    //var query = _adFavoriteRepository.GetAll().Where(x => x.AdvertisementId == item.Id || x.UserId == input.Id);

                    //var list = await query.ToListAsync();

                    foreach (var v in item.AdvertisementFavorites)
                    {
                        await _adFavoriteRepository.DeleteAsync(v);
                    }

                    foreach (var v in item.views)
                    {
                        await _adViewRepository.DeleteAsync(v);
                    }
                    foreach (var v in item.Layouts)
                    {
                        await _layoutRepository.DeleteAsync(v);
                    }

                    foreach (var v in item.Photos)
                    {
                        await _photoRepository.DeleteAsync(v);
                    }

                    foreach (var v in item.AdvertisementFacilites)
                    {
                        await _advertisementFacilityRepository.DeleteAsync(v);
                    }

                    foreach (var v in item.AdvertisementBookingsList)
                    {
                        await _advertisementBookingRepository.DeleteAsync(v);
                    }

                    await _advertisementRepository.DeleteAsync(item);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
                await Repository.DeleteAsync(user);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        //[AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }

        //[AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception(L("Common.Thereisnocurrentuser"));
            }
            
            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = L("Common.Incorrectpassword")
                }));
            }

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to reset password.");
            }
            
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            if (currentUser == null)
                currentUser = await _userManager.Users.FirstOrDefaultAsync(x => x.EmailAddress == input.UsernameOrEmailAddress || x.PhoneNumber == input.UsernameOrEmailAddress);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> ResetPasswordForForget(ResetPasswordDto input)
        {
            
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(x => x.EmailAddress == input.UsernameOrEmailAddress || x.PhoneNumber == input.UsernameOrEmailAddress);

            if (currentUser == null || currentUser.IsDeleted || !currentUser.IsActive || currentUser.PasswordResetCode != input.ResetCode)
            {
                return false;
            }

            else
            {
                currentUser.Password = _passwordHasher.HashPassword(currentUser, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return true;
        }

        public string GenerateOtp()
        {
            char[] charArr = "0123456789".ToCharArray();
            string strrandom = string.Empty;
            Random objran = new Random();
            for (int i = 0; i < 4; i++)
            {
                //It will not allow Repetation of Characters
                int pos = objran.Next(1, charArr.Length);
                if (!strrandom.Contains(charArr.GetValue(pos).ToString())) strrandom += charArr.GetValue(pos);
                else i--;
            }
            return strrandom;
        }
        public async Task<string> SendSMSOTP(string number, string msg)
        {
            var baseAddress = new Uri("https://smsmisr.com");

            var httpClient = new System.Net.Http.HttpClient { BaseAddress = baseAddress };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                environment = 1, // 1 For Live , 2 For Test
               
                userName = "2f3a159f3995afce62ba2176ddb7b2f86047827b0c2b1439396f78ead4f2d6a8",
                                  //2f3a159f3995afce62ba2176ddb7b2f86047827b0c2b1439396f78ead4f2d6a8
                // password = "da592ec85cb5615c40bba9324b59d536ba85b99b62af27bdcd5349fbfde498ba",

                //password = "0c956b579f579c8f6b742ff19d82fed90a8e1f3fb25b8a03bbbf4b43879bf1b4",
                password = "be241463898558adf8f7131a5e975a51daebef66ede133dfc92a7c635d97dd93",
                //sender = "b611afb996655a94c8e942a823f1421de42bf8335d24ba1f84c437b2ab11ca27",

                sender = "ef02c1139e1ea3ac083e695d38f63e21b072909fe7860767294b3d8ff6716039",
               // sender = "fa908c0c972d91e8c026f02e7352f604afb549d9be8e86e05109c3f7fcd02c77",//Etisalat - Orange
                mobile = number,
                template = "e83faf6025ec41d0f40256d2812629f5fa9291d05c8322f31eea834302501da8",
                otp = msg,
            });
            var content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync("/api/OTP/?", content))
            {
                string responseHeaders = response.Headers.ToString();
                string responseData = await response.Content.ReadAsStringAsync();
                return responseData;
            }
        }
        public async Task<string> SendWessageWhatsupLink(string number, string msg)
        {
            var baseAddress = new Uri("https://wasage.com");

            var httpClient = new System.Net.Http.HttpClient { BaseAddress = baseAddress };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {

                userName = "5082d80cea23d475e547d5c09bcab1bc943ab8914b892f53a1f27c53f2b23199",
                password = "c2f6539be392c645fc26f3f212b4c59f1d8bf2f4e34dc181d3a1421d40684836",
                reference = "user1",
                 mobile = number,
                message = $"Dear BrokerApp Customer, your Otp is {msg}",
            });
            var content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync("/api/OTP/?", content))
            {
                string responseHeaders = response.Headers.ToString();
                string responseData = await response.Content.ReadAsStringAsync();
                return responseData;
            }
        }

        public async Task<string> ForgetPassword(ForgetPasswordDto input)
        {
            CurrentUnitOfWork.EnableFilter(AbpDataFilters.MustHaveTenant);
            //   CurrentUnitOfWork.SetTenantId(1); || x.PhoneNumber==input.UserNameOrEmailAddress
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.EmailAddress == input.UserNameOrEmailAddress);
            if (user == null)
                return "Failed";
            user.PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(8);
            await _userManager.UpdateAsync(user);
            await CurrentUnitOfWork.SaveChangesAsync();
            return user.PasswordResetCode;
        }
    }
}

