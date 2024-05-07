using Abp.Application.Services.Dto;
using Broker.Lookups.Dto;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
    public class ApiAdvertisementBookingDto : EntityDto<long>
    {
        public DateTime BookingDate { get; set; }
        public long AdvertisementId { get; set; }

    }

    public class GetAllAdvertisementBookingsOutput
    {
        public GetAllAdvertisementBookingsOutput()
        {
            advertisementBookings = new List<ApiAdvertisementBookingDto>();
        }
        public List<ApiAdvertisementBookingDto> advertisementBookings { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllAdvertisementBookingsInput
    {
        public string Keyword { get; set; }

    }

    public class ApiCreateAdvertisementBookingDto 
    {
        public DateTime BookingDate { get; set; }
        public long AdvertisementId { get; set; }

    }

    public class ApiCreateAdvertisementBookingOut
    {
        public ApiCreateAdvertisementBookingOut()
        {
        }
        public long? AdvertisementBookingId { get; set; }
        public object AdvertisementBooking { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }


    public class ApiAdvertisementBookingDetailsDto
    {
        public DateTime BookingDate { get; set; }
        public long AdvertisementId { get; set; }
    }
    public class ApiAdvertisementBookingDetailsDtoOut
    {
        public ApiAdvertisementBookingDetailsDtoOut()
        {
            Details = new ApiAdvertisementBookingDetailsDto();
        }
        public ApiAdvertisementBookingDetailsDto Details { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiAdvertisementBookingDetailsInput
    {
        public long? UserId { get; set; }
    }

    public class ApiUpdateAdvertisementBookingDto 
    {
        public long Id { get; set; }
        public DateTime BookingDate { get; set; }
        public long AdvertisementId { get; set; }
    }

    public class ApiUpdateAdvertisementBookingOut
    {
        public ApiUpdateAdvertisementBookingOut()
        {
        }
        public long? AdvertisementBookingId { get; set; }
        public object AdvertisementBooking { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
    public class ApiDeleteAdvertisementBookingOut
    {
        public ApiDeleteAdvertisementBookingOut()
        {
        }
        public string msg { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }
    public class ApiDeleteAdvertisementBookingDto
    {
        public long AdvertisementBookingId { get; set; }

    }
}
