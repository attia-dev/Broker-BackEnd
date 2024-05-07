using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Common.Dto;
using Broker.Datatable.Dtos;
using Broker.Helpers;
//using Broker.Requests.Dto;

namespace Broker.Common
{
    public interface ICommonAppService : IApplicationService
    {
        Task<CountDto> GetCountAsync();
        //Task<List<RequestExtraCostDto>> GetAllExtraTypes(long userId, RequestType requestType);

    }
}
