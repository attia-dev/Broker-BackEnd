using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Broker.Customers.Dto;
using Broker.Datatable.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.Advertisements.Dto;

namespace Broker.Advertisements
{
    public interface IAdvertisementAppService : IApplicationService
    {
        Task<DataTableOutputDto<AdvertisementDto>> IsPaged(GetAdvertisementInput input);
        Task<AdvertisementDto> Manage(AdvertisementDto input);
        Task<AdvertisementDto> ManageReject(AdvertisementDto input);
        Task Delete(EntityDto<long> input);
        Task<AdvertisementDto> GetById(EntityDto<long> input);
        Task<GetAdvertisementOutput> GetAll(PagedAdvertisementResultRequestDto input);
        
       Task<AdFavoriteDto> CreateFavorite(AdFavoriteDto input);
        Task DeleteFavorite(long favoriteId);
        Task DeleteFavoriteByAddId(long addId);
        Task<GetFavoriteOutput> GetFavoritesForUser(long userId);
        Task<AdViewDto> CreateView(AdViewDto input);
        Task<GetViewOutput> GetViewsForAdvertisement(long advertisementId);
        Task<GetAdvertisementOutput> GetAllAdsByUserId(PagedAdvertisementResultRequestForUserDto input);
        Task<AdvertisementDto> ChangeStatus(long advertiseId);
        Task DeleteAllByUserId(PagedAdvertisementResultRequestForUserDto input);
        Task<GetAdvertisementOutput> SearchForApi(GetAdvertisementSearchInput input);
        Task<bool> CheckAdvertisementStatus(long advertiseId);
        Task<AdvertisementDto> GetLastAdvertiseInsertedToDB();

    }
}
