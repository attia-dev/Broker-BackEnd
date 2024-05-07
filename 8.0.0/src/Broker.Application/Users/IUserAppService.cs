using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Roles.Dto;
using Broker.Users.Dto;

namespace Broker.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task DeActivate(EntityDto<long> user);
        Task Activate(EntityDto<long> user);
        Task<ListResultDto<RoleDto>> GetRoles();
        Task ChangeLanguage(ChangeUserLanguageDto input);
        Task SavePermissions(SaveUserPermissionsInput input);
        Task<bool> ChangePassword(ChangePasswordDto input);
        string GenerateOtp();
        Task<string> SendSMSOTP(string number, string msg);
        Task<string> ForgetPassword(ForgetPasswordDto input);
        Task<string> SendWessageWhatsupLink(string number, string msg);
    }
}
