﻿using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
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
    [Table("ProjectLayouts")]
    public class ProjectLayout : FullAuditedEntity<long, User>
    {
        public string Avatar { get; set; }
        public long ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public static ProjectLayout Create( string Avatar,  long ProjectId)
        {
            var ProjectLayout = new ProjectLayout
            {
                Avatar = Avatar,
                ProjectId = ProjectId
            };
            return ProjectLayout;
        }

    }
}
