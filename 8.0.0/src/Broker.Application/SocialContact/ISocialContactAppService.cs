using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Broker.Datatable.Dtos;
using Broker.Payments.Dto;
using Broker.SocialContact.Dto;
using Broker.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.SocialContacts
{
    public interface ISocialContactAppService : IApplicationService
    {
        Task<DataTableOutputDto<SocialContactDto>> IsPaged(GetSocialContactsInput input);
        Task<GetSocialContactsOutput> GetAll(PagedSocialContactResultRequestDto input);
        Task<SocialContactDto> GetById(EntityDto<int> input);
        Task<SocialContactDto> Manage(SocialContactDto input);
        Task Delete(EntityDto<int> input);
    }
}
