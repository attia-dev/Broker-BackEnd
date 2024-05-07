using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Advertisements;
using Broker.Advertisements.Dto;
using Broker.Customers;
using Broker.Customers.Dto;
using Broker.Datatable.Dtos;
using Broker.Helpers;
using Broker.Localization;
using Broker.Lookups;
using Broker.Lookups.Dto;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Broker.Projects.Dto
{
    [AutoMapFrom(typeof(Project))]
    public class ProjectDto : FullAuditedEntityDto<long>
   
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public long? DurationId { get; set; }
        public DurationDto Duration { get; set; }
        public bool? FeaturedAd { get; set; }

        public string RejectReason { get; set; }
        public long? CompanyId { get; set; }
        public CompanyDto Company { get; set; }
        public bool IsPublish { get; set; }
        public bool? IsApprove { get; set; }

        public List<string> PhotosList { get; set; }
        public List<string> LayoutsList { get; set; }
        public List<AdvertisementDto> Advertisements { get; set; }
        public List<ProjectPhotoDto> Photos { get; set; }
        public List<ProjectLayoutDto> Layouts { get; set; }
    }

    public class GetProjectInput : DataTableInputDto
    {
        public string Name { get; set; }
        public ApprovalStatus? IsApprove { get; set; }
        public bool? Newer { get; set; }
    }

    public class GetProjectSearchInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class PagedProjectResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
        public long? UserId { get; set; }
        public long? CompanyId { get; set; }


    }


    public class GetProjectOutput
    {
        public List<ProjectDto> Projects { get; set; }
        public string Error { get; set; }
    }

}
