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
using Broker.Helpers;
using System.Numerics;

namespace Broker.Lookups
{
    public class DurationAppService : BrokerAppServiceBase, IDurationAppService
    {
        private readonly IRepository<Duration, long> _durationRepository;
        private readonly IRepository<DurationBuildingType, long> _durationBuildingTypeRepository;

        public DurationAppService(IRepository<Duration, long> durationRepository, IRepository<DurationBuildingType, long> durationBuildingTypeRepository)
        {
            _durationRepository = durationRepository;
            _durationBuildingTypeRepository = durationBuildingTypeRepository;
        }

        public async Task<DataTableOutputDto<DurationDto>> IsPaged(GetDurationInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var duration = await _durationRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (duration != null)
                            {
                                if (input.action == "Delete")
                                    await _durationRepository.DeleteAsync(duration);
                                else if (input.action == "Restore")
                                {
                                    duration.IsDeleted = false;
                                    duration.DeleterUserId = null;
                                    duration.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var duration = await _durationRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (duration != null)
                            {
                                if (input.action == "Delete")//Delete
                                    await _durationRepository.DeleteAsync(duration);
                                else if (input.action == "Restore")
                                {
                                    duration.IsDeleted = false;
                                    duration.DeleterUserId = null;
                                    duration.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var query = _durationRepository.GetAll().Include(x=>x.DurationBuildingTypes).Where(x=>1==1);
                    int count = await query.CountAsync();
                    query = query.FilterDataTable(input);
                    int filteredCount = await query.CountAsync();
                    var durations = await query
                          .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    var dto = ObjectMapper.Map<List<DurationDto>>(durations);
                  //  dto.ForEach(x =>
                  //  {
                  //      x.BuildingTypes = x.DurationBuildingTypes.Select(c => new BuildingType
                  //      {
                  //
                  //      }).ToList();
                  //
                  //  });

                    return new DataTableOutputDto<DurationDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = dto
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DurationDto> Manage(DurationDto input)
        {
            try
            {
                var duration = new Duration();
                if (input.Id > 0)
                {
                    duration = await _durationRepository.GetAsync(input.Id);
                    duration.Period = input.Period;
                    duration.Amount = input.Amount;
                    //duration.Type = input.Type;
                    duration.IsPublish = input.IsPublish;
                    duration = await _durationRepository.UpdateAsync(duration);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    var returnDurationBuildingTypes = await _durationBuildingTypeRepository.GetAll().Where(y => y.DurationId == duration.Id).ToListAsync();

                    //insert
                    input.BuildingTypes.Where(x => !returnDurationBuildingTypes.Any(b => b.Type == x)).ToList()
                            .ForEach(async item =>
                            {
                                var details = new DurationBuildingType();
                                details.Type = item;
                                details.DurationId = duration.Id;
                                await _durationBuildingTypeRepository.InsertAsync(details);
                            });
                    //delete
                    returnDurationBuildingTypes.Where(x => !input.BuildingTypes.Any(b => b == x.Type)).ToList()
                      .ForEach(async item =>
                      {
                          await _durationBuildingTypeRepository.HardDeleteAsync(x => x.Id == item.Id);
                      });
                    await CurrentUnitOfWork.SaveChangesAsync();


                    return ObjectMapper.Map<DurationDto>(duration);
                }
                else
                {
                    duration = Duration.Create(input.Period, input.Amount, /*input.Type,*/ input.IsPublish);
                    duration = await _durationRepository.InsertAsync(duration);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    input.BuildingTypes.ToList().ForEach(async item =>
                    {
                        var durationbuildingType = DurationBuildingType.Create(duration.Id, item);
                        durationbuildingType = await _durationBuildingTypeRepository.InsertAsync(durationbuildingType);
                    });
                    return ObjectMapper.Map<DurationDto>(duration);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<long> input)
        {
            var Country = await _durationRepository.GetAsync(input.Id);
            await _durationRepository.DeleteAsync(Country);
        }

        public async Task<DurationDto> GetById(EntityDto<long> input)
        {
            try
            {
                DurationDto durationInfo = new DurationDto();
                if (input.Id > 0)
                {
                    var duration = await _durationRepository.GetAll()
                        .Include(x=>x.DurationBuildingTypes)
                        .Where(x=>x.Id==input.Id).FirstOrDefaultAsync();
                    if (duration != null)
                    {
                        durationInfo = ObjectMapper.Map<DurationDto>(duration);
                        
                    }
                }
                return durationInfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<PagedResultDto<DurationDto>> GetAll(PagedDurationResultRequestDto input)
        {
            try
            {
                var query = _durationRepository.GetAll().Include(x=>x.DurationBuildingTypes).Where(x=>1==1);
                 query = query.WhereIf(input.Type.HasValue && input.Type>0,x=>x.DurationBuildingTypes.Any(y=>y.Type == input.Type) );
                query = query.WhereIf(input.IsPublish.HasValue,x=>x.IsPublish==input.IsPublish);
                int count = await query.CountAsync();
                var list = await query.ToListAsync();

                return new PagedResultDto<DurationDto>
                {
                    TotalCount = count,
                    Items = ObjectMapper.Map<List<DurationDto>>(list)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
