using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Broker.Customers.Dto;
using Broker.Datatable.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.ContactUs.Dto;

namespace Broker.ContactUs
{
    public interface IContactUsAppService : IApplicationService
    {
        Task<DataTableOutputDto<ContactUsDto>> IsPaged(GetContactUsInput input);
        Task<ContactUsDto> Manage(ContactUsDto input);
        Task Delete(EntityDto<long> input);
        Task<ContactUsDto> GetById(EntityDto<long> input);
        Task<PagedResultDto<ContactUsDto>> GetAll(PagedContactUsResultRequestDto input);

    }
}
