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
using Broker.Advertisements;

namespace Broker.Lookups
{
    public class CityAppService : BrokerAppServiceBase, ICityAppService
    {
        private readonly IRepository<City, long> _cityRepository;
        public CityAppService( IRepository<City, long> cityRepository)
        {
            _cityRepository = cityRepository;
        }

       public async Task<DataTableOutputDto<CityDto>> IsPaged(GetCityInput input)
          {
              try

              {
                  using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                  {
                      if (input.actionType == "GroupAction")
                      {
                          for (int i = 0; i < input.ids.Length; i++)
                          {
                              var city = await _cityRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                              if (city != null)
                              {
                                  if (input.action == "Delete")
                                      await _cityRepository.DeleteAsync(city);
                                  else if (input.action == "Restore")
                                  {
                                      city.IsDeleted = false;
                                      city.DeleterUserId = null;
                                      city.DeletionTime = null;
                                  }

                                if (input.action == "Enable")
                                {
                                    city.IsEnabled = true;
                                }
                                else if (input.action == "Disable")
                                {
                                    city.IsEnabled = false;
                                }
                            }
                          }
                          await CurrentUnitOfWork.SaveChangesAsync();
                      }
                      else if (input.actionType == "SingleAction")
                      {
                          if (input.ids.Length > 0)
                          {
                              var city = await _cityRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                              if (city != null)
                              {
                                  if (input.action == "Delete")//Delete
                                      await _cityRepository.DeleteAsync(city);
                                  else if (input.action == "Restore")
                                  {
                                      city.IsDeleted = false;
                                      city.DeleterUserId = null;
                                      city.DeletionTime = null;
                                  }
                                if (input.action == "Enable")
                                {
                                    city.IsEnabled = true;
                                }
                                else if (input.action == "Disable")
                                {
                                    city.IsEnabled = false;
                                }
                            }
                          }
                          await CurrentUnitOfWork.SaveChangesAsync();
                      }

                    var query = _cityRepository.GetAll().Include(x => x.Governorate).ThenInclude(x => x.Country).Where(x => 1 == 1);
                      int count = await query.CountAsync();
                      query = query.FilterDataTable((DataTableInputDto)input);
                      query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name) 
                                                                             ||at.Governorate.NameEn.Contains(input.Name) || at.Governorate.NameAr.Contains(input.Name));
                     // query = query.WhereIf(!string.IsNullOrEmpty(input.GovernorateName), at => at.Governorate.NameAr.Contains(input.GovernorateName));
                      int filteredCount = await query.CountAsync();
                      var citys = await query
                            .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                            .OrderBy(string.Format("{0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                            .Skip(input.start)
                            .Take(input.length)
                            .ToListAsync();

                      return new DataTableOutputDto<CityDto>
                      {
                          draw = input.draw,
                          iTotalDisplayRecords = filteredCount,
                          iTotalRecords = count,
                          aaData = ObjectMapper.Map<List<CityDto>>(citys)
                      };
                  }
              }
              catch (Exception ex)
              {

                  throw;
              }

          }
        public async Task<CityDto> Manage(CityDto input)
        {
            try
            {
                if (input.Id > 0)
                {

                    var city = await _cityRepository.GetAsync(input.Id);
                    city.GovernorateId = input.GovernorateId;
                    city.NameAr = input.NameAr;
                    city.NameEn = input.NameEn;
                    city = await _cityRepository.UpdateAsync(city);
                    await CurrentUnitOfWork.SaveChangesAsync();
                     return ObjectMapper.Map<CityDto>(city);
                }
                else
                {

                    var city = City.Create(input.NameAr,input.NameEn, input.GovernorateId,true);
                    var res = await _cityRepository.InsertAsync(city);
                    await CurrentUnitOfWork.SaveChangesAsync();
                     return ObjectMapper.Map<CityDto>(city);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<CityDto> GetById(EntityDto<long> input)
        {
            try
            {
                CityDto cityinfo = new CityDto();
                if (input.Id > 0)
                {
                    var city = await _cityRepository.GetAll().Include(x => x.Governorate).ThenInclude(x => x.Country).Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                    if (city != null)
                    {
                        cityinfo = ObjectMapper.Map<CityDto>(city);
                    }
                }
                return cityinfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<PagedResultDto<CityDto>> GetAll(PagedCityResultRequestDto input)
        {
            try
            {
                var query = _cityRepository.GetAllIncluding(x=>x.Governorate);
                query = query.WhereIf(input.GovernorateId.HasValue&& input.GovernorateId>0, x => x.GovernorateId == input.GovernorateId);
                query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name));
                //.Where(x => x.GovernorateId == input.GovernorateId);
                //var query = _cityRepository.GetAll();
                int count = await query.CountAsync();
                if (input.MaxResultCount > 0)
                    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new PagedResultDto<CityDto>
                {
                    TotalCount = count,
                    Items = ObjectMapper.Map<List<CityDto>>(list)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
        public async Task Delete(EntityDto<long> input)
        {
            var city = await _cityRepository.GetAsync(input.Id);
            await _cityRepository.DeleteAsync(city);
        }
    }
}