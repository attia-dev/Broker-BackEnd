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
using System.Threading;

namespace Broker.Lookups
{
    public class DefinitionAppService : BrokerAppServiceBase, IDefinitionAppService
    {
        private readonly IRepository<Definition, int> _definitionRepository;

        public DefinitionAppService(IRepository<Definition, int> definitionRepository)
        {
            _definitionRepository = definitionRepository;
        }

        public async Task<DataTableOutputDto<DefinitionDto>> IsPaged(GetDefinitionsInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var definition = await _definitionRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (definition != null)
                            {
                                if (input.action == "Delete")
                                    await _definitionRepository.DeleteAsync(definition);
                                else if (input.action == "Restore")
                                {
                                    definition.IsDeleted = false;
                                    definition.DeleterUserId = null;
                                    definition.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var definition = await _definitionRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (definition != null)
                            {
                                if (input.action == "Delete")//Delete
                                    await _definitionRepository.DeleteAsync(definition);
                                else if (input.action == "Restore")
                                {
                                    definition.IsDeleted = false;
                                    definition.DeleterUserId = null;
                                    definition.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var query = _definitionRepository.GetAll();
                    query = query.WhereIf(input.EnumCategory > 0, at => at.Type == input.EnumCategory);
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name));
                    query = query.WhereIf(input.EnumCategory.HasValue && input.EnumCategory > 0, at => at.Type == input.EnumCategory);
                    int filteredCount = await query.CountAsync();
                    var definitions = await query
                          .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .OrderBy(string.Format("{0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<DefinitionDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<DefinitionDto>>(definitions)
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DefinitionDto> Manage(DefinitionDto input)
        {
            try
            {
                var definition = new Definition();
                if (input.Id > 0)
                {
                    definition = await _definitionRepository.GetAsync(input.Id);
                    definition.NameAr = input.NameAr;
                    definition.NameEn = input.NameEn;
                    definition.DescriptionAr = input.DescriptionAr;
                    definition.DescriptionEn = input.DescriptionEn;
                    definition.Avatar = input.Avatar;
                    definition.Type = input.Type;
                    definition.Value = input.Value;
                    definition = await _definitionRepository.UpdateAsync(definition);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<DefinitionDto>(definition);
                }
                else
                {
                    definition = Definition.Create(input.NameAr, input.NameEn, input.DescriptionAr, input.DescriptionEn, input.Type, input.Avatar, input.Value);
                    definition = await _definitionRepository.InsertAsync(definition);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<DefinitionDto>(definition);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<int> input)
        {
            var definition = await _definitionRepository.GetAsync(input.Id);
            await _definitionRepository.DeleteAsync(definition);
        }

        public async Task<DefinitionDto> GetById(EntityDto<int> input)
        {
            try
            {
                DefinitionDto definitioninfo = new DefinitionDto();
                if (input.Id > 0)
                {
                   // var definition = await _definitionRepository.GetAllIncluding(input.Id);
                     var definition = await _definitionRepository.GetAsync(input.Id);
                    if (definition != null)
                    {
                        definitioninfo = ObjectMapper.Map<DefinitionDto>(definition);
                    }
                }
                return definitioninfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GetDefinitionsOutput> GetAll(PagedDefinitionResultRequestDto input)
        {
            try
            {
                var query = _definitionRepository.GetAll();
                query = query.WhereIf(input.EnumCategory > 0, at => at.Type == input.EnumCategory);
                query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), at => at.NameAr.Contains(input.keyword) || at.NameEn.Contains(input.keyword));
                
               // query = Thread.CurrentThread.CurrentUICulture.Name.Contains("ar") ? query.OrderBy(x => x.NameAr) : query.OrderBy(x => x.NameEn);
                int count = await query.CountAsync();
                if (input.MaxResultCount > 0)
                    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetDefinitionsOutput { Definitions = ObjectMapper.Map<List<DefinitionDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetDefinitionsOutput { Error = ex.Message };
            }
        }
      
    }
}
