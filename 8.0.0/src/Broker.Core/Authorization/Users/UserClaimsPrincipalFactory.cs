using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Broker.Authorization.Roles;
using Abp.Domain.Uow;
using Abp.Domain.Repositories;
using Broker.Customers;
using System.Threading.Tasks;
using System.Security.Claims;
//using Agenda.Security;
using System.Linq;
using System.Globalization;
using Broker.Security;
namespace Broker.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        private readonly IRepository<BrokerPerson, long> _brokerPersonRepository;
        private readonly IRepository<Seeker, long> _seekerRepository;
        private readonly IRepository<Owner, long> _ownerRepository;
        private readonly IRepository<Company, long> _companyRepository;
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor,
        IRepository<BrokerPerson, long> brokerPersonRepository,
            IRepository<Seeker, long> seekerRepository,
        IRepository<Owner, long> ownerRepository,
            IRepository<Company, long> companyRepository,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor,
                  unitOfWorkManager)
        {
            _brokerPersonRepository = brokerPersonRepository;
            _seekerRepository = seekerRepository;
            _ownerRepository = ownerRepository;
            _companyRepository = companyRepository;
        }
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var claim = await base.CreateAsync(user);
            var brokerPerson = await _brokerPersonRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (brokerPerson != null)
            {
                claim.Identities.First().AddClaim(new Claim(AppClaimTypes.BrokerPersonId, brokerPerson.Id.ToString(CultureInfo.InvariantCulture)));
            }
            var owner = await _ownerRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (owner != null)
            {
                claim.Identities.First().AddClaim(new Claim(AppClaimTypes.OwnerId, owner.Id.ToString(CultureInfo.InvariantCulture)));
            }
            var seeker = await _seekerRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (seeker != null)
            {
                claim.Identities.First().AddClaim(new Claim(AppClaimTypes.SeekerId, seeker.Id.ToString(CultureInfo.InvariantCulture)));
            }


            var company = await _companyRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (company != null)
            {
                claim.Identities.First().AddClaim(new Claim(AppClaimTypes.CompanyId, company.Id.ToString(CultureInfo.InvariantCulture)));
            }



            return claim;
        }
    }
}
