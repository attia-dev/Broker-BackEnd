using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Customers;
using Castle.Core.Resource;
using Castle.MicroKernel.Internal;
using System.Numerics;

namespace Broker.Sessions.Dto
{
    public class GetCurrentLoginInformationsOutput
    {
        public ApplicationInfoDto Application { get; set; }

        public UserLoginInfoDto User { get; set; }

        public TenantLoginInfoDto Tenant { get; set; }

        public BrokerPersonLoginInfoDto BrokerPerson { get; set; }
        public SeekerLoginInfoDto Seeker { get; set; }
        public OwnerLoginInfoDto Owner { get; set; }
        public CompanyLoginInfoDto Company { get; set; }
    }
    [AutoMapFrom(typeof(BrokerPerson))]
    public class BrokerPersonLoginInfoDto : EntityDto
    {
        public string Name { get; set; }
    }
    [AutoMapFrom(typeof(Seeker))]
    public class SeekerLoginInfoDto : EntityDto
    {
        public string Name { get; set; }
    }
    [AutoMapFrom(typeof(Owner))]
    public class OwnerLoginInfoDto : EntityDto
    {
        public string Name { get; set; }
    }
    [AutoMapFrom(typeof(Company))]
    public class CompanyLoginInfoDto : EntityDto
    {
        public string Name { get; set; }
    }
}
