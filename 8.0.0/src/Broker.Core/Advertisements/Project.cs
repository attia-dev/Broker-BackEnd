using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using Broker.Customers;
using Broker.Helpers;
using Broker.Lookups;
using Castle.MicroKernel.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Advertisements
{
    [Table("Projects")]
    public class Project : FullAuditedEntity<long, User>
    {
        public string Name { get; set; }
        public string Description { get; set; }


        public ICollection<ProjectPhoto> Photos { get; set; }
        public ICollection<ProjectLayout> Layouts { get; set; }
        public ICollection<Advertisement> Advertisements { get; set; }

        public long? DurationId { get; set; }
        [ForeignKey("DurationId")]
        public Duration Duration { get; set; }
        public bool? FeaturedAd { get; set; }

        public long? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public bool? IsPublish { get; set; }
        public bool? IsApprove { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string RejectReason { get; set; }
        public static Project Create(string Name, string Description, long? DurationId, 
                                    bool? FeaturedAd, long? CompanyId, bool? IsPublish,
                                    bool? IsApprove, double? Latitude, double? Longitude)
        {
            var Project = new Project
            {
                Name = Name,
                Description = Description,
                DurationId = DurationId,
                FeaturedAd = FeaturedAd,
                CompanyId = CompanyId,
                IsPublish = IsPublish,
                IsApprove = IsApprove,
                Latitude = Latitude,
                Longitude = Longitude,
            };
            return Project;
        }

    }
}
