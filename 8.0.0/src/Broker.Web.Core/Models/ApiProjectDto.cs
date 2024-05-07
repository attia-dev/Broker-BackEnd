using Abp.Application.Services.Dto;
using Broker.Advertisements.Dto;
using Broker.Customers.Dto;
using Broker.Helpers;
using Broker.Lookups.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApiProjectDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public long? DurationId { get; set; }
        public bool? FeaturedAd { get; set; }

        public long? CompanyId { get; set; }
        public bool IsPublish { get; set; }
        public bool? IsApprove { get; set; }

        public List<string> PhotosList { get; set; }
        public List<string> LayoutsList { get; set; }
        public List<ApiCreateAdvertisementDto> Advertisements { get; set; }
       // public List<AdvertisementDto> Advertisements { get; set; }

    }


    //public class ApiCreateProjectDto 
    //{
    //    public string Name { get; set; }
    //    public string Description { get; set; }

    //    public double? Latitude { get; set; }
    //    public double? Longitude { get; set; }

    //    public long? DurationId { get; set; }
    //    public bool? FeaturedAd { get; set; }

    //    public long? CompanyId { get; set; }
    //    public bool IsPublish { get; set; }
    //    //public bool IsApprove { get; set; }

    //    public List<string> PhotosList { get; set; }
    //    public List<string> LayoutsList { get; set; }
    //    public List<ApiCreateAdvertisementDto> Advertisements { get; set; }

    //   // public List<AdvertisementDto> Advertisements { get; set; }

    //}

    public class ApiProjectOut
    {
        public ApiProjectOut()
        {
        }
        public long? ProjectId { get; set; }
        public object Project { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiPlaceDurationOut
    {
        public bool Success { get; set; }
        public string Error { get; set; }
    }


    public class ApiProjectDetailsDtoOut
    {
        public ApiProjectDetailsDtoOut()
        {
            Details = new ApiProjectDto();
        }
        public ApiProjectDto Details { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiProjectDetailsInput
    {
        public long? ProjectId { get; set; }
    }
    public class ApiDeleteProjectDto
    {
        public long ProjectId { get; set; }

    }
    public class ApiDeleteProjectOut
    {
        public ApiDeleteProjectOut()
        {
        }
        public string msg { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class ApiProjectInput
    {
       public string Keyword { get; set; }
        public long? UserId { get; set; }
        public long? CompanyId { get; set; }
    }

    public class PlaceDurationInput
    {
        public long? ProjectId { get; set; }
        public int? DurationId { get; set; }

        public bool? IsPublish { get; set; }
    }


    public class GetAllProjectsOutput
    {
        public GetAllProjectsOutput()
        {
            projects = new List<ApiProjectDto>();
        }
        public List<ApiProjectDto> projects { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

}
