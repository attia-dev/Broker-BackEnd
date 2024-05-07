using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Broker.Customers.Dto;
using Broker.Datatable.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.Projects.Dto;

namespace Broker.Projects
{
    public interface IProjectAppService : IApplicationService
    {
        Task<DataTableOutputDto<ProjectDto>> IsPaged(GetProjectInput input);
        Task<ProjectDto> Manage(ProjectDto input);
        Task<ProjectDto> ManageReject(ProjectDto input);
        Task Delete(EntityDto<long> input);
        Task<ProjectDto> GetById(EntityDto<long> input);
        Task<GetProjectOutput> GetAll(PagedProjectResultRequestDto input);

        //Task<AdFavoriteDto> CreateFavorite(AdFavoriteDto input);
        //Task DeleteFavorite(long favoriteId);
        //Task<GetFavoriteOutput> GetFavoritesForUser(long userId);
        //Task<AdViewDto> CreateView(AdViewDto input);
        //Task<GetViewOutput> GetViewsForProject(long ProjectId);
        //Task<GetProjectOutput> GetAllAdsByUserId(PagedProjectResultRequestForUserDto input);
        //Task<ProjectDto> ChangeStatus(long advertiseId);
        //Task DeleteAllByUserId(PagedProjectResultRequestForUserDto input);
        //Task<GetProjectOutput> SearchForApi(GetProjectSearchInput input);

    }
}
