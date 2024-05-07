using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using Broker.Lookups.Dto;
using Broker.Lookups;
using Broker.Payments;
using Broker.Payments.Dto;
using Broker.RateUs.Dto;
using Broker.SocialContact.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Broker.Datatable.Dtos;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Broker.Linq.Extensions;

namespace Broker.SocialContacts
{
    public class SocialContactAppService : BrokerAppServiceBase, ISocialContactAppService
    {
        private readonly IRepository<SocialContact, int> _socialContactRepository;
        public SocialContactAppService(IRepository<SocialContact, int> socialContactRepository)
        {
            _socialContactRepository = socialContactRepository;
        }

        public async Task<DataTableOutputDto<SocialContactDto>> IsPaged(GetSocialContactsInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var SocialContact = await _socialContactRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (SocialContact != null)
                            {
                                if (input.action == "Delete")
                                    await _socialContactRepository.DeleteAsync(SocialContact);
                                else if (input.action == "Restore")
                                {
                                    SocialContact.IsDeleted = false;
                                    SocialContact.DeleterUserId = null;
                                    SocialContact.DeletionTime = null;
                                }
                                //if (input.action == "Enable")
                                //{
                                //    Country.IsEnabled = true;
                                //}
                                //else if (input.action == "Disable")
                                //{
                                //    Country.IsEnabled = false;
                                //}
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var SocialContact = await _socialContactRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (SocialContact != null)
                            {
                                if (input.action == "Delete")//Delete
                                    await _socialContactRepository.DeleteAsync(SocialContact);
                                else if (input.action == "Restore")
                                {
                                    SocialContact.IsDeleted = false;
                                    SocialContact.DeleterUserId = null;
                                    SocialContact.DeletionTime = null;
                                }
                                //if (input.action == "Enable")
                                //{
                                //    Country.IsEnabled = true;
                                //}
                                //else if (input.action == "Disable")
                                //{
                                //    Country.IsEnabled = false;
                                //}
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var query = _socialContactRepository.GetAll();
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.SocialName.Contains(input.Name));
                    int filteredCount = await query.CountAsync();
                    var socialContacts = await query
                          .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .OrderBy(string.Format("{0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<SocialContactDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<SocialContactDto>>(socialContacts)
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SocialContactDto> Manage(SocialContactDto input)
        {
            try
            {
                var socialContact = new SocialContact();
                if (input.Id > 0)
                {
                    socialContact = await _socialContactRepository.GetAsync(input.Id);
                    socialContact.SocialName = input.SocialName;
                    socialContact.SocialValue = input.SocialValue;
                    socialContact.Avatar = input.Avatar;
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<SocialContactDto>(socialContact);
                }
                else
                {
                    socialContact = SocialContact.Create(input.SocialName, input.SocialValue,input.Avatar);
                    var res = await _socialContactRepository.InsertAsync(socialContact);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<SocialContactDto>(socialContact);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<int> input)
        {
            var socialContact = await _socialContactRepository.GetAsync(input.Id);
            await _socialContactRepository.DeleteAsync(socialContact);
        }

        public async Task<SocialContactDto> GetById(EntityDto<int> input)
        {
            try
            {
                SocialContactDto SocialContactInfo = new SocialContactDto();
                if (input.Id > 0)
                {
                    var socialContact = await _socialContactRepository.GetAsync(input.Id);
                    //GetAsync(input.Id);
                    if (socialContact != null)
                    {
                        SocialContactInfo = ObjectMapper.Map<SocialContactDto>(socialContact);
                    }
                }
                return SocialContactInfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GetSocialContactsOutput> GetAll(PagedSocialContactResultRequestDto input)
        {
            try
            {
                var query = _socialContactRepository.GetAll();
                query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.SocialName.Contains(input.Name));
                int count = await query.CountAsync();
                if (input.MaxResultCount > 0)
                    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetSocialContactsOutput { SocialContacts = ObjectMapper.Map<List<SocialContactDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetSocialContactsOutput { Error = ex.Message };
            }
        }
    }
}
