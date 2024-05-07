using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Broker.Authorization.Users;
using Broker.Customers.Dto;
using Broker.Customers;
using Broker.Datatable.Dtos;
using Broker.Users.Dto;
using Broker.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.Projects.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.Collections.Extensions;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Broker.Linq.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Broker.Helpers;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using static Broker.Authorization.PermissionNames;
using Broker.Lookups;
using Broker.Lookups.Dto;
using Broker.Advertisements;
using Broker.Advertisements.Dto;
using Broker.Advertisements;

namespace Broker.Projects
{
    public class ProjectAppService : BrokerAppServiceBase, IProjectAppService
    {
        private readonly IRepository<Project, long> _ProjectRepository;
        private readonly IRepository<Advertisement, long> _AdvertisementRepository;
        private readonly IRepository<ProjectPhoto, long> _ProjectPhotoRepository;
        private readonly IRepository<ProjectLayout, long> _ProjectLayoutRepository;
        private readonly IDefinitionAppService _DefinitionAppService;
        private readonly IDurationAppService _DurationAppService;
        private readonly IAdvertisementAppService _AdvertisementAppService;

        public ProjectAppService(
           IRepository<Project, long> ProjectRepository,
           IRepository<Advertisement, long> AdvertisementRepository,
           IRepository<ProjectPhoto, long> ProjectPhotoRepository,
           IRepository<ProjectLayout, long> ProjectLayoutRepository,
           IDefinitionAppService DefinitionAppService,
           IDurationAppService DurationAppService,
           IAdvertisementAppService AdvertisementAppService
            )
        {
            _ProjectRepository = ProjectRepository;
            _AdvertisementRepository = AdvertisementRepository;
            _ProjectPhotoRepository = ProjectPhotoRepository;
            _ProjectLayoutRepository = ProjectLayoutRepository;
            _DefinitionAppService = DefinitionAppService;
            _DurationAppService = DurationAppService;
            _AdvertisementAppService = AdvertisementAppService;
        }

        public async Task<DataTableOutputDto<ProjectDto>> IsPaged(GetProjectInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var Project = await _ProjectRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (Project != null)
                            {


                                if (input.action == "Delete")
                                    await _ProjectRepository.DeleteAsync(Project);
                                else if (input.action == "Restore")
                                {
                                    Project.IsDeleted = false;
                                }

                                if (input.action == "Approve")
                                {
                                    Project.IsApprove = true;
                                }
                                else if (input.action == "Decline")
                                {
                                    Project.IsApprove = false;
                                }


                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var Project = await _ProjectRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (Project != null)
                            {



                                if (input.action == "Delete")//Delete
                                    await _ProjectRepository.DeleteAsync(Project);
                                else if (input.action == "Restore")
                                {
                                    Project.IsDeleted = false;
                                }

                                if (input.action == "Approve")
                                {
                                    Project.IsApprove = true;
                                }
                                else if (input.action == "Decline")
                                {
                                    Project.IsApprove = false;
                                }


                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);

                    //var query = _ProjectRepository.GetAllIncluding(x => x.Duration, , x => x.Photos, x => x.Layouts, x => x.Company, x => x.Advertisements).Where(x => x.IsPublish == true);
                    var query = _ProjectRepository.GetAll().Include(x=>x.Duration).Include(x => x.Company).ThenInclude(x=>x.User)
                        .Include(x => x.Photos).Include(x => x.Layouts).Include(x => x.Advertisements).Where(x => x.IsPublish == true);

                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.Name.Contains(input.Name) || at.Description.Contains(input.Name) || at.Id.ToString().Contains(input.Name));
                    if (input.IsApprove != null)
                    {
                        switch (input.IsApprove)
                        {
                            case ApprovalStatus.Pending:
                                query = query.Where(x => x.IsApprove == null);
                                ; break;
                            case ApprovalStatus.Accepted:
                                query = query.Where(x => x.IsApprove == true);
                                ; break;
                            case ApprovalStatus.Rejected:
                                query = query.Where(x => x.IsApprove == false);
                                ; break;
                        }
                    }
                    //query = query.WhereIf(input.IsApprove != null, x => x.IsApprove == input.IsApprove);
                    query = input.Newer != null && input.Newer == false ? query.OrderBy(x => x.CreationTime) : query.OrderByDescending(x => x.CreationTime);
                    int filteredCount = await query.CountAsync();
                    var Projects = await query
                        .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    var dto = ObjectMapper.Map<List<ProjectDto>>(Projects);

                    return new DataTableOutputDto<ProjectDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = dto,
                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<ProjectDto> Manage(ProjectDto input)
        {
            try
            {
                //input.SeekerId = input.SeekerId <= 0 ? null : input.SeekerId;
                //input.OwnerId = input.OwnerId <= 0 ? null : input.OwnerId;
                //input.CompanyId = input.CompanyId <= 0 ? null : input.CompanyId;
                //input.BrokerPersonId = input.BrokerPersonId <= 0 ? null : input.BrokerPersonId;
                //input.Decoration = input.Decoration <= 0 ? null : input.Decoration;
                //input.Document = input.Document <= 0 ? null : input.Document;

                if (input.Id > 0)
                {

                    var Project = await _ProjectRepository.GetAsync(input.Id);
                    Project.Name = input.Name;
                    Project.Description = input.Description;
                    Project.Latitude = input.Latitude;
                    Project.Longitude = input.Longitude;
                    Project.DurationId = input.DurationId;
                    Project.FeaturedAd = input.FeaturedAd;
                    Project.CompanyId = input.CompanyId;
                    Project.IsPublish = input.IsPublish;
                    Project.IsApprove = input.IsApprove;
                    

                    Project = await _ProjectRepository.UpdateAsync(Project);

                    

                    //Photos
                    var PhotosDb = _ProjectPhotoRepository.GetAll().Where(a => a.ProjectId == Project.Id).ToList();
                    if (input.PhotosList != null)
                    {
                        input.PhotosList.Where(x => !PhotosDb.Any(b => b.Avatar == x)).ToList()
                                .ForEach(async item =>
                                {
                                    var photo = ProjectPhoto.Create(item, Project.Id);
                                    await _ProjectPhotoRepository.InsertAsync(photo);
                                });

                        PhotosDb.Where(x => !input.PhotosList.Any(b => b == x.Avatar)).ToList()
                              .ForEach(async item =>
                              {
                                  await _ProjectPhotoRepository.HardDeleteAsync(item);
                              });
                    }


                    //Layouts
                    var LayoutsDb = _ProjectLayoutRepository.GetAll().Where(a => a.ProjectId == Project.Id).ToList();
                    if (input.LayoutsList != null)
                    {
                        input.LayoutsList.Where(x => !LayoutsDb.Any(b => b.Avatar == x)).ToList()
                               .ForEach(async item =>
                               {
                                   var layout = ProjectLayout.Create(item, Project.Id);
                                   await _ProjectLayoutRepository.InsertAsync(layout);
                               });

                        LayoutsDb.Where(x => !input.LayoutsList.Any(b => b == x.Avatar)).ToList()
                              .ForEach(async item =>
                              {
                                  await _ProjectLayoutRepository.HardDeleteAsync(item);
                              });
                    }

                    //Advertisements
                    var AdvertisementsDb = _AdvertisementRepository.GetAll().Where(a => a.ProjectId == Project.Id).ToList();
                    if (input.Advertisements != null)
                    {
                       var tempAdvertisements= input.Advertisements.Where(x => !AdvertisementsDb.Any(b => b.CreationTime == x.CreationTime)).ToList();
                              
                        foreach(var item in tempAdvertisements)
                        {
                                  item.ProjectId = Project.Id;
                                  var advertisement = await _AdvertisementAppService.Manage(item);
                        }

                        //old
                        //.ForEach(async item =>
                        //   {
                        //       item.ProjectId = Project.Id;
                        //       var advertisement = await _AdvertisementAppService.Manage(item);
                        //   });
                        //old

                        //AdvertisementsDb.Where(x => !input.Advertisements.Any(b => b.CreationTime == x.CreationTime)).ToList()
                        //      .ForEach(async item =>
                        //      {
                        //          await _AdvertisementRepository.HardDeleteAsync(item);
                        //      });
                    }


                    await CurrentUnitOfWork.SaveChangesAsync();

                    return ObjectMapper.Map<ProjectDto>(Project);

                }
                else
                {

                    var Project = Broker.Advertisements.Project.Create(input.Name,input.Description,
                        input.DurationId, input.FeaturedAd, input.CompanyId, false, null,
                        input.Latitude,input.Longitude
                       );
                    var res = await _ProjectRepository.InsertAsync(Project);

                    await CurrentUnitOfWork.SaveChangesAsync();

                    //Photos
                    if (input.PhotosList != null)
                        input.PhotosList.ForEach(
                        async x =>
                        {
                            var photo = ProjectPhoto.Create(x, Project.Id);
                            await _ProjectPhotoRepository.InsertAsync(photo);
                        });

                    //layouts
                    if (input.LayoutsList != null)
                        input.LayoutsList.ForEach(
                        async x =>
                        {
                            var layout = ProjectLayout.Create(x, Project.Id);
                            await _ProjectLayoutRepository.InsertAsync(layout);
                        });
                    await CurrentUnitOfWork.SaveChangesAsync();
                     
                    
                    //Advertisements
                    if (input.Advertisements != null)
                        foreach(var x in input.Advertisements)
                        {
                            x.ProjectId = res.Id;
                            var Advertisement = await _AdvertisementAppService.Manage(x);
                        };

                       // input.Advertisements.ForEach(
                       // async x =>
                       // {
                       //     x.ProjectId = res.Id;
                       //     var Advertisement = await _AdvertisementAppService.Manage(x);
                       // });

                    await CurrentUnitOfWork.SaveChangesAsync();

                    return ObjectMapper.Map<ProjectDto>(Project);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<ProjectDto> GetById(EntityDto<long> input)
        {
            try
            {
                ProjectDto ProjectInfo = new ProjectDto();
                if (input.Id > 0)
                {
                    var Project = await _ProjectRepository
                        .GetAll().Include(x => x.Duration).Include(x => x.Company).ThenInclude(x=>x.User).Include(x => x.Photos)
                        .Include(x => x.Layouts).Include(x => x.Advertisements).ThenInclude(x => x.Photos).Include(x => x.Advertisements).ThenInclude(x => x.Layouts)

                        .Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                    if (Project != null)
                    {
                        ProjectInfo = ObjectMapper.Map<ProjectDto>(Project);
                        //ProjectInfo = ObjectMapper.Map<ProjectDto>(Project);
                        //ProjectInfo.Advertisements = new List<AdvertisementDto>();
                        //foreach (var item in Project.Advertisements)
                        //{
                        //    ProjectInfo.Advertisements.Add(ObjectMapper.Map<AdvertisementDto>(item));
                        //}
                    }
                }
                return ProjectInfo;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<GetProjectOutput> GetAll(PagedProjectResultRequestDto input)
        {
            try
            {
                var query = _ProjectRepository.GetAllIncluding(x => x.Duration, x => x.Photos, x => x.Layouts, x => x.Advertisements).Include(x => x.Company).ThenInclude(x=>x.User).Where(x=>1==1);
                query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.Name.Contains(input.Name)|| at.Description.Contains(input.Name));
                query = query.WhereIf(input.UserId > 0, x => x.Company.UserId == input.UserId);
                query = query.WhereIf(input.CompanyId > 0, x => x.CompanyId == input.CompanyId);
                int count = await query.CountAsync();
                var list = await query.ToListAsync();
                var projects = ObjectMapper.Map<List<ProjectDto>>(list);

                return new GetProjectOutput { Projects = projects };
            }
            catch (Exception ex)
            {
                return new GetProjectOutput { Error = ex.Message };
            }
        }
        public async Task Delete(EntityDto<long> input)
        {
            var Project = await _ProjectRepository.GetAsync(input.Id);
            await _ProjectRepository.DeleteAsync(Project);
        }

        public async Task<ProjectDto> ManageReject(ProjectDto input)
        {
            try
            {
                var project = new Project();
                if (input.Id > 0)
                {
                    project = await _ProjectRepository.GetAsync(input.Id);
                    project.RejectReason = input.RejectReason;
                    project.IsApprove = false;

                    project = await _ProjectRepository.UpdateAsync(project);

                    await CurrentUnitOfWork.SaveChangesAsync();

                    return ObjectMapper.Map<ProjectDto>(project);
                }
                return ObjectMapper.Map<ProjectDto>(project);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<GetProjectOutput> GetAllAdsByUserId(PagedProjectResultRequestForUserDto input)
        //{
        //    try
        //    {
        //        var query = _ProjectRepository.GetAllIncluding(x => x.City, x => x.Duration, x => x.views
        //        , x => x.BrokerPerson.User, x => x.Seeker.User, x => x.Owner.User, x => x.Company.User, y => y.Photos, y => y.Layouts, x => x.ProjectFacilites);
        //        //x => x.City, x => x.Duration, x => x.views

        //        query = query.WhereIf(input.BrokerID.HasValue && input.BrokerID > 0, x => x.BrokerPersonId == input.BrokerID);
        //        query = query.WhereIf(input.SeekerID.HasValue && input.SeekerID > 0, x => x.SeekerId == input.SeekerID);
        //        query = query.WhereIf(input.OwnerID.HasValue && input.OwnerID > 0, x => x.OwnerId == input.OwnerID);
        //        query = query.WhereIf(input.CompanyID.HasValue && input.CompanyID > 0, x => x.CompanyId == input.CompanyID);

        //        int count = await query.CountAsync();
        //        if (input.MaxResultCount > 0)
        //            query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
        //        var list = await query.ToListAsync();
        //        return new GetProjectOutput { Projects = ObjectMapper.Map<List<ProjectDto>>(list) };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new GetProjectOutput { Error = ex.Message };
        //    }
        //}

        //public async Task<AdFavoriteDto> CreateFavorite(AdFavoriteDto input)
        //{
        //    try
        //    {
        //        var adFavorite = await _adFavoriteRepository.GetAll().Where(x => x.UserId == input.UserId && x.ProjectId == input.ProjectId)
        //            .FirstOrDefaultAsync();
        //        if (adFavorite == null)
        //        {
        //            var favorite = AdFavorite.Create(input.UserId, input.ProjectId);
        //            var res = await _adFavoriteRepository.InsertAsync(favorite);
        //            await CurrentUnitOfWork.SaveChangesAsync();
        //            return ObjectMapper.Map<AdFavoriteDto>(favorite);
        //        }
        //        else
        //            throw new UserFriendlyException("it is already added");
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //public async Task DeleteFavorite(long favoriteId)
        //{
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
        //    {
        //        var adFavorite = await _adFavoriteRepository.GetAll().Where(x => x.Id == favoriteId)
        //                .FirstOrDefaultAsync();
        //        if (adFavorite != null)
        //            await _adFavoriteRepository.HardDeleteAsync(adFavorite);
        //    }

        //}
        //public async Task<GetFavoriteOutput> GetFavoritesForUser(long userId)
        //{
        //    try
        //    {
        //        var query = _adFavoriteRepository.GetAll().Include(x => x.Project)
        //            .ThenInclude(x => x.City).Include(x => x.Project).ThenInclude(x => x.Photos)
        //            .Where(x => x.UserId == userId);

        //        var list = await query.ToListAsync();

        //        return new GetFavoriteOutput { AdFavorites = ObjectMapper.Map<List<AdFavoriteDto>>(list) };

        //    }

        //    catch (Exception ex)
        //    {
        //        return new GetFavoriteOutput { Error = ex.Message };
        //    }

        //}
        //public async Task<AdViewDto> CreateView(AdViewDto input)
        //{
        //    try
        //    {
        //        /*var adView = await _adVieweRepository.GetAll().Where(x => (x.UserId == input.UserId && x.ProjectId == input.ProjectId) || (x.UserId == input.UserId && x.DeviceToken == input.DeviceToken && x.ProjectId == input.ProjectId))
        //            .FirstOrDefaultAsync();*/
        //        var adView = await _adVieweRepository.GetAll().Where(x => (x.UserId == input.UserId && x.ProjectId == input.ProjectId)).FirstOrDefaultAsync();
        //        if (adView == null)
        //        {
        //            var view = AdView.Create(/*input.DeviceToken,*/ input.UserId, input.ProjectId);
        //            var res = await _adVieweRepository.InsertAsync(view);
        //            await CurrentUnitOfWork.SaveChangesAsync();
        //            return ObjectMapper.Map<AdViewDto>(view);
        //        }
        //        else
        //            throw new UserFriendlyException("it is already added");
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //public async Task<GetViewOutput> GetViewsForProject(long ProjectId)
        //{
        //    try
        //    {
        //        var query = _adVieweRepository.GetAll()
        //            .Where(x => x.ProjectId == ProjectId);

        //        var list = await query.ToListAsync();

        //        return new GetViewOutput { AdViews = ObjectMapper.Map<List<AdViewDto>>(list) };

        //    }

        //    catch (Exception ex)
        //    {
        //        return new GetViewOutput { Error = ex.Message };
        //    }

        //}
        //public async Task<ProjectDto> ChangeStatus(long advertiseId)
        //{
        //    try
        //    {

        //        var Project = await _ProjectRepository
        //            .GetAllIncluding(x => x.Duration).Where(x => x.Id == advertiseId).FirstOrDefaultAsync();
        //        //.GetAsync((Int64)advertiseId);
        //        if (Project.DurationId > 0 && DateTime.Now < Project.CreationTime.AddMonths(Project.Duration.Period))
        //        {
        //            Project.IsPublish = !Project.IsPublish;

        //            Project = await _ProjectRepository.UpdateAsync(Project);
        //            await CurrentUnitOfWork.SaveChangesAsync();
        //            return ObjectMapper.Map<ProjectDto>(Project);
        //        }
        //        else
        //            throw new UserFriendlyException(this.L("Common.Message.ObjectNotInDuration"));
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //public async Task DeleteAllByUserId(PagedProjectResultRequestForUserDto input)
        //{
        //    var query = _ProjectRepository.GetAll();

        //    query = query.WhereIf(input.BrokerID.HasValue && input.BrokerID > 0, x => x.BrokerPersonId == input.BrokerID);
        //    query = query.WhereIf(input.SeekerID.HasValue && input.SeekerID > 0, x => x.SeekerId == input.SeekerID);
        //    query = query.WhereIf(input.OwnerID.HasValue && input.OwnerID > 0, x => x.OwnerId == input.OwnerID);
        //    query = query.WhereIf(input.CompanyID.HasValue && input.CompanyID > 0, x => x.CompanyId == input.CompanyID);

        //    foreach (var item in query)
        //    {
        //        await _ProjectRepository.DeleteAsync(item);
        //    }

        //}
        //public async Task<GetProjectOutput> SearchForApi(GetProjectSearchInput input)
        //{
        //    try
        //    {
        //        var query = _ProjectRepository.GetAllIncluding(x => x.City, x => x.Duration, x => x.Seeker, x => x.Owner, x => x.Company, x => x.BrokerPerson, x => x.views);
        //        query = query.FilterDataTable((DataTableInputDto)input);
        //        query = query.WhereIf(input.Type.HasValue && input.Type > 0, at => at.Type == input.Type)
        //        .WhereIf(input.CityId.HasValue && input.CityId > 0, at => at.CityId == input.CityId)
        //        .WhereIf(!string.IsNullOrEmpty(input.StreetOrCompund), at => at.Street.Contains(input.StreetOrCompund) || at.Compound.Contains(input.StreetOrCompund))
        //        .WhereIf(input.Rooms.HasValue && input.Rooms > 0, at => at.Rooms == input.Rooms)
        //        .WhereIf(!string.IsNullOrEmpty(input.Area), at => at.Area.Contains(input.Area))
        //        .WhereIf(input.Decoration.HasValue && input.Decoration > 0, at => at.Decoration == input.Decoration)
        //        .WhereIf(input.Furnished.HasValue, at => at.Furnished == true)
        //        .WhereIf(input.Parking.HasValue, at => at.Parking == true)
        //        .WhereIf(input.AgreementStatus.HasValue && input.AgreementStatus > 0, at => at.AgreementStatus == input.AgreementStatus)
        //        .WhereIf(input.PriceFrom.HasValue && input.PriceTo.HasValue && input.PriceFrom >= 0 && input.PriceTo > 0
        //        , at => input.PriceFrom <= at.Price && at.Price <= input.PriceTo);
        //        query = query.WhereIf(input.AreaFrom.HasValue && input.AreaTo.HasValue
        //            && input.AreaFrom > 0 && input.AreaTo > 0
        //            , at => (input.AreaFrom <= at.BuildingArea && at.BuildingArea <= input.AreaTo)
        //            || (input.AreaFrom <= at.Width * at.Length && at.Width * at.Length <= input.AreaTo));
        //        int count = await query.CountAsync();
        //        var list = await query.ToListAsync();
        //        return new GetProjectOutput { Projects = ObjectMapper.Map<List<ProjectDto>>(list) };

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}
    }
}
