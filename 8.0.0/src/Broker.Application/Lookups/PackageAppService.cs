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
    public class PackageAppService : BrokerAppServiceBase, IPackageAppService
    {
        private readonly IRepository<Package, long> _packageRepository;

        public PackageAppService(IRepository<Package, long> PackageRepository)
        {
            _packageRepository = PackageRepository;
        }

        public async Task<DataTableOutputDto<PackageDto>> IsPaged(GetPackagesInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var Package = await _packageRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (Package != null)
                            {
                                if (input.action == "Delete")
                                    await _packageRepository.DeleteAsync(Package);
                                else if (input.action == "Restore")
                                {
                                    Package.IsDeleted = false;
                                    Package.DeleterUserId = null;
                                    Package.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var Package = await _packageRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (Package != null)
                            {
                                if (input.action == "Delete")//Delete
                                    await _packageRepository.DeleteAsync(Package);
                                else if (input.action == "Restore")
                                {
                                    Package.IsDeleted = false;
                                    Package.DeleterUserId = null;
                                    Package.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var query = _packageRepository.GetAll();
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
                    return new DataTableOutputDto<PackageDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<PackageDto>>(countries)
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PackageDto> Manage(PackageDto input)
        {
            try
            {
                var Package = new Package();
                if (input.Id > 0)
                {
                    Package = await _packageRepository.GetAsync(input.Id);
                    Package.NameAr = input.NameAr;
                    Package.NameEn = input.NameEn;
                    Package.Price = input.Price;
                    Package.Points = input.Points;
                    Package = await _packageRepository.UpdateAsync(Package);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<PackageDto>(Package);
                }
                else
                {
                    Package = Package.Create(input.NameAr, input.NameEn, input.Price, input.Points);
                    Package = await _packageRepository.InsertAsync(Package);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<PackageDto>(Package);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<long> input)
        {
            var Package = await _packageRepository.GetAsync(input.Id);
            await _packageRepository.DeleteAsync(Package);
        }

        public async Task<PackageDto> GetById(EntityDto<long> input)
        {
            try
            {
                PackageDto Packageinfo = new PackageDto();
                if (input.Id > 0)
                {
                    var Package = await _packageRepository.GetAsync(input.Id);
                    if (Package != null)
                    {
                        Packageinfo = ObjectMapper.Map<PackageDto>(Package);
                    }
                }
                return Packageinfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GetPackagesOutput> GetAll(PagedPackageResultRequestDto input)
        {
            try
            {
                var query = _packageRepository.GetAll();
                query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name));
                int count = await query.CountAsync();
                //if (input.MaxResultCount > 0)
                //    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetPackagesOutput { Packages = ObjectMapper.Map<List<PackageDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetPackagesOutput { Error = ex.Message };
            }
        }

    }
}
