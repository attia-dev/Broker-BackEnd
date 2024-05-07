using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Broker.Controllers
{
    public abstract class BrokerControllerBase: AbpController
    {
        protected BrokerControllerBase()
        {
            LocalizationSourceName = BrokerConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
