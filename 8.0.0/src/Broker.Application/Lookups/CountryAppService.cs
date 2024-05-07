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

namespace Broker.Lookups
{
    public class CountryAppService : BrokerAppServiceBase, ICountryAppService
    {
        private readonly IRepository<Country, long> _countryRepository;

        public CountryAppService(IRepository<Country, long> CountryRepository)
        {
            _countryRepository = CountryRepository;
        }

        public async Task<DataTableOutputDto<CountryDto>> IsPaged(GetCountriesInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var Country = await _countryRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (Country != null)
                            {
                                if (input.action == "Delete")
                                    await _countryRepository.DeleteAsync(Country);
                                else if (input.action == "Restore")
                                {
                                    Country.IsDeleted = false;
                                    Country.DeleterUserId = null;
                                    Country.DeletionTime = null;
                                }
                                if (input.action == "Enable")
                                {
                                    Country.IsEnabled = true;
                                }
                                else if (input.action == "Disable")
                                {
                                    Country.IsEnabled = false;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var Country = await _countryRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (Country != null)
                            {
                                if (input.action == "Delete")//Delete
                                    await _countryRepository.DeleteAsync(Country);
                                else if (input.action == "Restore")
                                {
                                    Country.IsDeleted = false;
                                    Country.DeleterUserId = null;
                                    Country.DeletionTime = null;
                                }
                                if (input.action == "Enable")
                                {
                                    Country.IsEnabled = true;
                                }
                                else if (input.action == "Disable")
                                {
                                    Country.IsEnabled = false;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var query = _countryRepository.GetAll();
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name));
                    int filteredCount = await query.CountAsync();
                    var countries = await query
                          .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .OrderBy(string.Format("{0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<CountryDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<CountryDto>>(countries)
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CountryDto> Manage(CountryDto input)
        {
            try
            {
                var Country = new Country();
                if (input.Id > 0)
                {
                    Country = await _countryRepository.GetAsync(input.Id);
                    Country.NameAr = input.NameAr;
                    Country.NameEn = input.NameEn;
                    Country = await _countryRepository.UpdateAsync(Country);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<CountryDto>(Country);
                }
                else
                {
                    Country = Country.Create(input.NameAr, input.NameEn,true);
                    Country = await _countryRepository.InsertAsync(Country);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<CountryDto>(Country);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<long> input)
        {
            var Country = await _countryRepository.GetAsync(input.Id);
            await _countryRepository.DeleteAsync(Country);
        }

        public async Task<CountryDto> GetById(EntityDto<long> input)
        {
            try
            {
                CountryDto Countryinfo = new CountryDto();
                if (input.Id > 0)
                {
                    var Country = await _countryRepository.GetAsync(input.Id);
                    if (Country != null)
                    {
                        Countryinfo = ObjectMapper.Map<CountryDto>(Country);
                    }
                }
                return Countryinfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GetCountriesOutput> GetAll(PagedCountryResultRequestDto input)
        {
            try
            {
                var query = _countryRepository.GetAll();
                int count = await query.CountAsync();
                if (input.MaxResultCount > 0)
                    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetCountriesOutput { Countries = ObjectMapper.Map<List<CountryDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetCountriesOutput { Error = ex.Message };
            }
        }

    }
}
