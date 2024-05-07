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
    [Table("DurationBuildingTypes")]
    public class DurationBuildingType : FullAuditedEntity<long, User>
    {
        public long DurationId { get; set; }
        [ForeignKey("DurationId")]
        public Duration Duration { get; set; }
        public BuildingType Type { get; set; }
        public static DurationBuildingType Create(long DurationId, BuildingType Type)
        {
            var DurationBuildingType = new DurationBuildingType
            {
                DurationId = DurationId,
                Type = Type,
            };
            return DurationBuildingType;
        }
    }
}
