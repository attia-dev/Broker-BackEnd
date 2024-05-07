using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;
using System.Linq.Dynamic.Core;
using Broker.Advertisements;
using Broker.Advertisements.Dto;

namespace Broker.Lookups
{
    public class AdvertisementBookingAppService : BrokerAppServiceBase, IAdvertisementBookingAppService
    {
        private readonly IRepository<AdvertisementBooking, long> _advertisementBookingRepository;
       
        public AdvertisementBookingAppService(IRepository<AdvertisementBooking, long> advertisementBookingRepository)
        {
            _advertisementBookingRepository = advertisementBookingRepository;
        }

        public async Task<AdvertisementBookingDto> Manage(AdvertisementBookingDto input)
        {
            try
            {
                var advertisementBooking = new AdvertisementBooking();
                if (input.Id > 0)
                {
                    advertisementBooking = await _advertisementBookingRepository.GetAsync(input.Id);
                    advertisementBooking.AdvertisementId = input.AdvertisementId;
                    advertisementBooking.BookingDate = input.BookingDate;

                    advertisementBooking = await _advertisementBookingRepository.UpdateAsync(advertisementBooking);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<AdvertisementBookingDto>(advertisementBooking);
                }
                else
                {
                    advertisementBooking = AdvertisementBooking.Create(input.BookingDate, input.AdvertisementId);
                    advertisementBooking = await _advertisementBookingRepository.InsertAsync(advertisementBooking);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<AdvertisementBookingDto>(advertisementBooking);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<long> input)
        {
            var Country = await _advertisementBookingRepository.GetAsync(input.Id);
            await _advertisementBookingRepository.DeleteAsync(Country);
        }

        public async Task<AdvertisementBookingDto> GetById(EntityDto<long> input)
        {
            try
            {
                AdvertisementBookingDto advertisementBookingInfo = new AdvertisementBookingDto();
                if (input.Id > 0)
                {
                    var advertisementBooking = await _advertisementBookingRepository.GetAll()
                        .Where(x=>x.Id==input.Id).FirstOrDefaultAsync();
                    if (advertisementBooking != null)
                    {
                        advertisementBookingInfo = ObjectMapper.Map<AdvertisementBookingDto>(advertisementBooking);
                        
                    }
                }
                return advertisementBookingInfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<PagedResultDto<AdvertisementBookingDto>> GetAll(PagedAdvertisementBookingResultRequestDto input)
        {
            try
            {
                var query = _advertisementBookingRepository.GetAll().Where(x=>x.IsDeleted==false);
                query = query.WhereIf(input.AdvertisementId != null && input.AdvertisementId > 0, x => x.AdvertisementId == input.AdvertisementId);
                int count = await query.CountAsync();
                var list = await query.ToListAsync();

                return new PagedResultDto<AdvertisementBookingDto>
                {
                    TotalCount = count,
                    Items = ObjectMapper.Map<List<AdvertisementBookingDto>>(list)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
