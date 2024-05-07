using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Datatable.Dtos;
using Broker.Lookups;
using Broker.Lookups.Dto;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Advertisements.Dto
{
    [AutoMapFrom(typeof(AdvertisementBooking))]
    public class AdvertisementBookingDto : FullAuditedEntityDto<long>
    {
        public DateTime BookingDate { get; set; }
        public long AdvertisementId { get; set; }
        public AdvertisementDto Advertisement { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto LastModifierUser { get; set; }
        public UserDto DeleterUser { get; set; }

    }
    public class GetAdvertisementBookingInput : DataTableInputDto
    {
        public string Name { get; set; }
    }
    public class PagedAdvertisementBookingResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
        public long? AdvertisementId { get; set; }
    }
}
