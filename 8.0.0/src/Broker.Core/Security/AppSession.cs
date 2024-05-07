using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Security
{
    public class AppSession : ClaimsAbpSession, ITransientDependency
    {
        public AppSession(IPrincipalAccessor principalAccessor, IMultiTenancyConfig multiTenancy, Abp.MultiTenancy.ITenantResolver tenantResolver,
            Abp.Runtime.IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
            : base(principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        { }

        public long? BrokerPersonId
        {
            get
            {
                if (!(PrincipalAccessor.Principal is ClaimsPrincipal claimsPrincipal))
                {
                    return null;
                }

                Claim claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.BrokerPersonId);
                if (claim == null || string.IsNullOrEmpty(claim.Value))
                {
                    return null;
                }

                return Convert.ToInt64(claim.Value);
            }
        }
        public long? SeekerId
        {
            get
            {
                if (!(PrincipalAccessor.Principal is ClaimsPrincipal claimsPrincipal))
                {
                    return null;
                }

                Claim claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.SeekerId);
                if (claim == null || string.IsNullOrEmpty(claim.Value))
                {
                    return null;
                }

                return Convert.ToInt64(claim.Value);
            }
        }
        public long? OwnerId
        {
            get
            {
                if (!(PrincipalAccessor.Principal is ClaimsPrincipal claimsPrincipal))
                {
                    return null;
                }

                Claim claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.OwnerId);
                if (claim == null || string.IsNullOrEmpty(claim.Value))
                {
                    return null;
                }

                return Convert.ToInt64(claim.Value);
            }
        }
        public long? CompanyId
        {
            get
            {
                if (!(PrincipalAccessor.Principal is ClaimsPrincipal claimsPrincipal))
                {
                    return null;
                }

                Claim claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == AppClaimTypes.CompanyId);
                if (claim == null || string.IsNullOrEmpty(claim.Value))
                {
                    return null;
                }

                return Convert.ToInt64(claim.Value);
            }
        }

    }
}
