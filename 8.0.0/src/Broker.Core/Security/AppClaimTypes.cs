using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Security
{
    public static class AppClaimTypes
    {
        public const string BrokerPersonId = "http://www.broker.com/identity/claims/brokerPersonId";
        public const string SeekerId = "http://www.broker.com/identity/claims/seekerId";
        public const string OwnerId = "http://www.broker.com/identity/claims/ownerId";
        public const string CompanyId = "http://www.broker.com/identity/claims/companyId"; 
    }
}