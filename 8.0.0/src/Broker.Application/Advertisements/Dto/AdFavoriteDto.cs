using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Authorization.Users;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Advertisements.Dto
{
    [AutoMapFrom(typeof(AdFavorite))]
    public class AdFavoriteDto : FullAuditedEntityDto<long>
    {
        public long UserId { get; set; }
        public UserDto User { get; set; }
        public long AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }
    }
    public class GetFavoriteOutput
    {
        public List<AdFavoriteDto> AdFavorites { get; set; }
        public string Error { get; set; }
    }

}
