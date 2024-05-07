using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Datatable.Dtos;
using Broker.Payments.Dto;
using Broker.RateUs.Dto;
using Broker.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.RateUs
{
    public interface IRateAppService : IApplicationService
    {
        Task<DataTableOutputDto<RateDto>> IsPaged(GetRatesInput input);
        Task<GetRatesOutput> GetAll(PagedRateResultRequestDto input);
        Task<RateDto> GetById(EntityDto<long> input);
        Task<RateDto> Manage(RateDto input);
        Task Delete(EntityDto<long> input);
    }
}
