using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Helpers;
using Broker.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Advertisements.Dto
{
    [AutoMapFrom(typeof(ProjectPhoto))]
    public class ProjectPhotoDto : FullAuditedEntityDto<long>
    {
        public string Avatar { get; set; }
        public long ProjectId { get; set; }
    }
}
