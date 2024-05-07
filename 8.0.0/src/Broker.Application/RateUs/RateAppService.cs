using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Broker.Datatable.Dtos;
using Broker.Payments;
using Broker.Payments.Dto;
using Broker.RateUs.Dto;
using Broker.SocialContact.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.Linq.Extensions;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;

namespace Broker.RateUs
{
    public class RateAppService : BrokerAppServiceBase, IRateAppService
    {
        private readonly IRepository<Rate, long> _rateRepository;
        public RateAppService(IRepository<Rate, long> rateRepository)
        {
            _rateRepository = rateRepository;
        }

        public async Task<DataTableOutputDto<RateDto>> IsPaged(GetRatesInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var rate = await _rateRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (rate != null)
                            {
                                if (input.action == "Delete")
                                    await _rateRepository.DeleteAsync(rate);
                                else if (input.action == "Restore")
                                {
                                    rate.IsDeleted = false;
                                    rate.DeleterUserId = null;
                                    rate.DeletionTime = null;
                                }
                                //if (input.action == "Enable")
                                //{
                                //    Country.IsEnabled = true;
                                //}
                                //else if (input.action == "Disable")
                                //{
                                //    Country.IsEnabled = false;
                                //}
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var rate = await _rateRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (rate != null)
                            {
                                if (input.action == "Delete")//Delete
                                    await _rateRepository.DeleteAsync(rate);
                                else if (input.action == "Restore")
                                {
                                    rate.IsDeleted = false;
                                    rate.DeleterUserId = null;
                                    rate.DeletionTime = null;
                                }
                                //if (input.action == "Enable")
                                //{
                                //    Country.IsEnabled = true;
                                //}
                                //else if (input.action == "Disable")
                                //{
                                //    Country.IsEnabled = false;
                                //}
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var query = _rateRepository.GetAllIncluding(x=>x.User);
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name)
                    , at => at.User.Name.Contains(input.Name) || at.User.Surname.Contains(input.Name));
            
                    int filteredCount = await query.CountAsync();
                    var rates = await query
                          .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .OrderBy(string.Format("{0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<RateDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<RateDto>>(rates)
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RateDto> Manage(RateDto input)
        {
            try
            {
                var rate = new Rate();
                if (input.Id > 0)
                {
                    rate = await _rateRepository.GetAsync(input.Id);
                    rate.UserId = input.UserId;
                    rate.UserRate = input.UserRate;
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<RateDto>(rate);
                }
                else
                {
                   var xRate = await _rateRepository.GetAll().Where(x => x.UserId == input.UserId).FirstOrDefaultAsync();
                    if (xRate == null)
                    {
                        rate = Rate.Create(input.UserId, input.UserRate);
                        rate = await _rateRepository.InsertAsync(rate);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                   else throw new UserFriendlyException(L("Common.AlreadyRated"));

                    return ObjectMapper.Map<RateDto>(rate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<long> input)
        {
            var rate = await _rateRepository.GetAsync(input.Id);
            await _rateRepository.DeleteAsync(rate);
        }

        public async Task<RateDto> GetById(EntityDto<long> input)
        {
            try
            {
                RateDto Rateinfo = new RateDto();
                if (input.Id > 0)
                {
                    var rate = await _rateRepository.GetAllIncluding(x => x.User).Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                    //GetAsync(input.Id);
                    if (rate != null)
                    {
                        Rateinfo = ObjectMapper.Map<RateDto>(rate);
                    }
                }
                return Rateinfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GetRatesOutput> GetAll(PagedRateResultRequestDto input)
        {
            try
            {
                var query = _rateRepository.GetAllIncluding(x => x.User);
                query = (IQueryable<Rate>)query.WhereIf(string.IsNullOrEmpty(input.Name) 
                    , at => at.User.Name.Contains(input.Name)|| at.User.Surname.Contains(input.Name));
                //int count = await query.CountAsync();
                if (input.MaxResultCount > 0)
                    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetRatesOutput { Rates = ObjectMapper.Map<List<RateDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetRatesOutput { Error = ex.Message };
            }
        }
    }
}
