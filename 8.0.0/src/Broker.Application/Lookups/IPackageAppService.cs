using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Datatable.Dtos;
using Broker.Lookups.Dto;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    public interface IPackageAppService : IApplicationService
    {
        Task<DataTableOutputDto<PackageDto>> IsPaged(GetPackagesInput input);
        Task<GetPackagesOutput> GetAll(PagedPackageResultRequestDto input);
        Task<PackageDto> GetById(EntityDto<long> input);
        Task<PackageDto> Manage(PackageDto input);
        Task Delete(EntityDto<long> input);

    }
}
