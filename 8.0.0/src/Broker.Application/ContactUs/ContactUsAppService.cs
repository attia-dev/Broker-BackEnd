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
using Broker.ContactUs.Dto;

namespace Broker.ContactUs
{
    public class ContactUsAppService : BrokerAppServiceBase, IContactUsAppService
    {
        private readonly IRepository<ContactUs, long> _contactUsRepository;

        public ContactUsAppService(IRepository<ContactUs, long> contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
        }

        public async Task<DataTableOutputDto<ContactUsDto>> IsPaged(GetContactUsInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var contactUs = await _contactUsRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (contactUs != null)
                            {
                                if (input.action == "Delete")
                                    await _contactUsRepository.DeleteAsync(contactUs);
                                else if (input.action == "Restore")
                                {
                                   contactUs.IsDeleted = false;
                                   contactUs.DeleterUserId = null;
                                   contactUs.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var contactUs = await _contactUsRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (contactUs != null)
                            {
                                if (input.action == "Delete")//Delete
                                    await _contactUsRepository.DeleteAsync(contactUs);
                                else if (input.action == "Restore")
                                {
                                   contactUs.IsDeleted = false;
                                   contactUs.DeleterUserId = null;
                                   contactUs.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var query = _contactUsRepository.GetAllIncluding(x=>x.User);
                    int count = await query.CountAsync();
                    query = query.FilterDataTable(input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.EmailAddress), at => at.EmailAddress.Contains(input.EmailAddress) || at.EmailSubject.Contains(input.EmailAddress));
                     int filteredCount = await query.CountAsync();
                    var contactUses = await query
                          .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<ContactUsDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<ContactUsDto>>(contactUses)
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ContactUsDto> Manage(ContactUsDto input)
        {
            try
            {
                var contactUs = new ContactUs();
                if (input.Id > 0)
                {
                    contactUs = await _contactUsRepository.GetAsync(input.Id);
                    contactUs.EmailAddress = input.EmailAddress;
                    contactUs.EmailSubject = input.EmailSubject;
                    contactUs.AttachmentPath = input.AttachmentPath;
                    contactUs.UserId = input.UserId??AbpSession.UserId;

                    contactUs = await _contactUsRepository.UpdateAsync(contactUs);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<ContactUsDto>(contactUs);
                }
                else
                {
                    contactUs.UserId = input.UserId ?? AbpSession.UserId;
                    contactUs = ContactUs.Create(input.EmailAddress, input.EmailSubject, input.AttachmentPath, input?.UserId);
                    contactUs = await _contactUsRepository.InsertAsync(contactUs);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<ContactUsDto>(contactUs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<long> input)
        {
            var contactUs = await _contactUsRepository.GetAsync(input.Id);
            await _contactUsRepository.DeleteAsync(contactUs);
        }

        public async Task<ContactUsDto> GetById(EntityDto<long> input)
        {
            try
            {
                ContactUsDto ContactUsInfo = new ContactUsDto();
                if (input.Id > 0)
                {
                    var contactUs = await _contactUsRepository.GetAsync(input.Id);
                    if (contactUs != null)
                    {
                        ContactUsInfo = ObjectMapper.Map<ContactUsDto>(contactUs);
                    }
                }
                return ContactUsInfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<PagedResultDto<ContactUsDto>> GetAll(PagedContactUsResultRequestDto input)
        {
            try
            {
                var query = _contactUsRepository.GetAllIncluding(x=>x.User);
                int count = await query.CountAsync();
                var list = await query.ToListAsync();
                return new PagedResultDto<ContactUsDto>
                {
                    TotalCount = count,
                    Items = ObjectMapper.Map<List<ContactUsDto>>(list)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
