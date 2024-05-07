using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using Broker.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    [Table("Durations")]
    public class Duration : FullAuditedEntity<long, User>
    {
        public int Period { get; set; }
        public decimal Amount { get; set; }
       // public BuildingType? Type { get; set; }
        public bool IsPublish { get; set; }
        public ICollection<DurationBuildingType> DurationBuildingTypes { get; set; }
        public static Duration Create(int Period, decimal Amount, bool IsPublish)
        {
            var Duration = new Duration
            {
                Period = Period,
                Amount = Amount,
                //Type = Type,
                IsPublish = IsPublish
            };
            return Duration;
        }
    }
}
