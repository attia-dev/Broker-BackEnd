using Abp.Authorization;
using Broker.Authorization.Roles;
using Broker.Authorization.Users;

namespace Broker.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
