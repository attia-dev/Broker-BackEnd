using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Advertisements.Dto
{
    [AutoMapFrom(typeof(AdView))]
    public class AdViewDto : FullAuditedEntityDto<long>
    {
        //public string DeviceToken { get; set; }
        public long UserId { get; set; }
        public UserDto User { get; set; }
        public long AdvertisementId { get; set; }
        //public AdvertisementDto Advertisement { get; set; }

    }
    public class GetViewOutput
    {
        public List<AdViewDto> AdViews { get; set; }
        public string Error { get; set; }
    }
}
