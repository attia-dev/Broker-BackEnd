using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Broker.Lookups;
using Broker.Datatable.Dtos;
using Broker.Linq.Extensions;
using Broker.Lookups.Dto;

namespace Broker.Lookups
{
    public class DiscountCodeAppService : BrokerAppServiceBase, IDiscountCodeAppService
    {
        private readonly IRepository<DiscountCode, long> _discountCodeRepository;
        public DiscountCodeAppService(IRepository<DiscountCode, long> discountCodeRepository)
        {
            _discountCodeRepository = discountCodeRepository;
        }

        public async Task<DataTableOutputDto<DiscountCodeDto>> IsPaged(GetDiscountCodesInput input)
        {
            try

            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var discountCode = await _discountCodeRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (discountCode != null)
                            {
                                if (input.action == "Delete")
                                    await _discountCodeRepository.DeleteAsync(discountCode);
                                else if (input.action == "Restore")
                                {
                                    discountCode.IsDeleted = false;
                                    discountCode.DeleterUserId = null;
                                    discountCode.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var discountCode = await _discountCodeRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (discountCode != null)
                            {
                                if (input.action == "Delete")//Delete
                                    await _discountCodeRepository.DeleteAsync(discountCode);
                                else if (input.action == "Restore")
                                {
                                    discountCode.IsDeleted = false;
                                    discountCode.DeleterUserId = null;
                                    discountCode.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }

                    var query = _discountCodeRepository.GetAll();
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    int filteredCount = await query.CountAsync();
                    var discountCodes = await query
                          .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();

                    return new DataTableOutputDto<DiscountCodeDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<DiscountCodeDto>>(discountCodes)
                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<DiscountCodeDto> Manage(DiscountCodeDto input)
        {
            try
            {
                if (input.Id > 0)
                {
                    var discountCode = await _discountCodeRepository.GetAsync(input.Id);
                    discountCode.Code = input.Code;
                    discountCode.Percentage = input.Percentage;
                    discountCode.FixedAmount = input.FixedAmount;
                    discountCode.IsPublish = input.IsPublish;
                    discountCode.From = input.From;
                    discountCode.To = input.To;
                    discountCode = await _discountCodeRepository.UpdateAsync(discountCode);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<DiscountCodeDto>(discountCode);
                }
                else
                {
                    var discountCode = DiscountCode.Create(input.Code, input.Percentage, input.FixedAmount, input.IsPublish
                        ,input.From,input.To);
                    var res = await _discountCodeRepository.InsertAsync(discountCode);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<DiscountCodeDto>(discountCode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<DiscountCodeDto> GetById(EntityDto<long> input)
        {
            try
            {
                DiscountCodeDto discountCodeInfo = new DiscountCodeDto();
                if (input.Id > 0)
                {
                    var discountCode = await _discountCodeRepository.GetAsync(input.Id);
                    if (discountCode != null)
                    {
                        discountCodeInfo = ObjectMapper.Map<DiscountCodeDto>(discountCode);
                    }
                }
                return discountCodeInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<PagedResultDto<DiscountCodeDto>> GetAll(PagedDiscountCodeResultRequestDto input)
        {
            try
            {
                var query = _discountCodeRepository.GetAll();
                int count = await query.CountAsync();
                var list = await query.ToListAsync();
                return new PagedResultDto<DiscountCodeDto>
                {
                    TotalCount = count,
                    Items = ObjectMapper.Map<List<DiscountCodeDto>>(list)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task Delete(EntityDto<long> input)
        {
            var city = await _discountCodeRepository.GetAsync(input.Id);
            await _discountCodeRepository.DeleteAsync(city);
        }
        public async Task<DiscountCodeDto> GetByCode(GetDiscountCodeByCodeInput input)
        {
            try
            {
                DiscountCodeDto discountCodeInfo = new DiscountCodeDto();
                if (input.Code !=null)
                {
                    var discountCode = _discountCodeRepository.GetAll().Where(x=>x.Code==input.Code && x.IsPublish==true &&x.IsDeleted==false ).FirstOrDefault();
                   // && x.From <= DateTime.Now && x.To >= DateTime.Now
                    if (discountCode != null)
                    {
                        discountCodeInfo = ObjectMapper.Map<DiscountCodeDto>(discountCode);
                    }
                }
                return discountCodeInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}