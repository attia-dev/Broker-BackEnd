using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;
using System.Linq.Dynamic.Core;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using Broker.Linq.Extensions;
using static Broker.Authorization.PermissionNames;

namespace Broker.Lookups
{
    public class GovernorateAppService : BrokerAppServiceBase, IGovernorateAppService
    {
        private readonly IRepository<Governorate, long> _governorateRepository;

        public GovernorateAppService(IRepository<Governorate, long> governorateRepository)
        {
            _governorateRepository = governorateRepository;
        }

        public async Task<DataTableOutputDto<GovernorateDto>> IsPaged(GetGovernoratesInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var governorate = await _governorateRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (governorate != null)
                            {
                                if (input.action == "Delete")
                                    await _governorateRepository.DeleteAsync(governorate);
                                else if (input.action == "Restore")
                                {
                                    governorate.IsDeleted = false;
                                    governorate.DeleterUserId = null;
                                    governorate.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var governorate = await _governorateRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (governorate != null)
                            {
                                if (input.action == "Delete")//Delete
                                    await _governorateRepository.DeleteAsync(governorate);
                                else if (input.action == "Restore")
                                {
                                    governorate.IsDeleted = false;
                                    governorate.DeleterUserId = null;
                                    governorate.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var query = _governorateRepository.GetAllIncluding(x => x.Country);
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name));
                    int filteredCount = await query.CountAsync();
                    var cities = await query
                          .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .OrderBy(string.Format("{0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<GovernorateDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<GovernorateDto>>(cities)
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GovernorateDto> Manage(GovernorateDto input)
        {
            try
            {
                var governorate = new Governorate();
                if (input.Id > 0)
                {
                    governorate = await _governorateRepository.GetAsync(input.Id);
                    governorate.NameAr = input.NameAr;
                    governorate.NameEn = input.NameEn;
                    governorate.CountryId = input.CountryId;
                    governorate = await _governorateRepository.UpdateAsync(governorate);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<GovernorateDto>(governorate);
                }
                else
                {
                    governorate = Governorate.Create(input.NameAr, input.NameEn,input.CountryId);
                    governorate = await _governorateRepository.InsertAsync(governorate);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<GovernorateDto>(governorate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<long> input)
        {
            var governorate = await _governorateRepository.GetAsync(input.Id);
            await _governorateRepository.DeleteAsync(governorate);
        }

        public async Task<GovernorateDto> GetById(EntityDto<long> input)
        {
            try
            {
                GovernorateDto governorateinfo = new GovernorateDto();
                if (input.Id > 0)
                {
                    var governorate = await _governorateRepository.GetAllIncluding(x => x.Country).Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                    //GetAsync(input.Id);
                    if (governorate != null)
                    {
                        governorateinfo = ObjectMapper.Map<GovernorateDto>(governorate);
                    }
                }
                return governorateinfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GetGovernoratesOutput> GetAll(PagedGovernorateResultRequestDto input)
        {
            try
            {
                var query = _governorateRepository.GetAllIncluding(x=>x.Country);
                query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name));
                query =query.WhereIf(input.CountryId.HasValue && input.CountryId>0, at=>at.CountryId==input.CountryId); 
                int count = await query.CountAsync();
                if (input.MaxResultCount > 0)
                  query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetGovernoratesOutput { Governorates = ObjectMapper.Map<List<GovernorateDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetGovernoratesOutput { Error = ex.Message };
            }
        }

    }
}
