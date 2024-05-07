using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Broker.Lookups;
using Broker.Datatable.Dtos;
using Broker.Linq.Extensions;
using Broker.Lookups.Dto;
using Broker.Customers;
using Broker.Customers.Dto;
using Broker.Authorization.Users;
using Castle.Core.Resource;
using Broker.Users.Dto;
using Broker.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Broker.Advertisements;
using static Broker.Authorization.PermissionNames;

namespace Broker.Customers
{
    public class BrokerPersonAppService : BrokerAppServiceBase, IBrokerPersonAppService
    {
        private readonly IRepository<BrokerPerson, long> _brokerPersonRepository;
        private readonly UserManager _userManager;
        private readonly IUserAppService _userAppService;
        private readonly IPackageAppService _PackageAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Advertisement, long> _advertisementRepository;
        private readonly IRepository<AdFavorite, long> _adFavorite;
        public BrokerPersonAppService( IRepository<BrokerPerson, long> brokerPersonRepository, UserManager userManager,
            IUserAppService userAppService, IPackageAppService PackageAppService,
           IRepository<User, long> userRepository, IRepository<Advertisement, long> advertisementRepository, IRepository<AdFavorite, long> adFavorite)
        {
            _brokerPersonRepository = brokerPersonRepository;
            _userManager = userManager;
            _userAppService = userAppService;
            _PackageAppService = PackageAppService;
            _userRepository = userRepository;
            _advertisementRepository = advertisementRepository;
            _adFavorite = adFavorite;
        }

        public async Task<DataTableOutputDto<BrokerPersonDto>> IsPaged(GetBrokerPersonInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {


                    if ((input.action == "Delete" || input.action == "Restore"))
                    {
                        if (input.actionType == "GroupAction")
                        {
                            for (int i = 0; i < input.ids.Length; i++)
                            {
                                var brokerPerson = await _brokerPersonRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                                if (brokerPerson != null)
                                {
                                    if (input.action == "Delete")
                                    {
                                        //await _brokerPersonRepository.DeleteAsync(brokerPerson);
                                        #region HardDelete

                                        brokerPerson.IsDeleted = true;
                                        await _brokerPersonRepository.UpdateAsync(brokerPerson);

                                        var seekerUser = await _userRepository.FirstOrDefaultAsync(Convert.ToInt32(brokerPerson.UserId));


                                        seekerUser.EmailAddress = "Deleted@Deleted.Deleted";
                                        seekerUser.UserName = "Deleted@Deleted.Deleted";
                                        seekerUser.PhoneNumber = "Deleted@Deleted.Deleted";
                                        seekerUser.IsDeleted = true;
                                        await _userRepository.UpdateAsync(seekerUser);

                                        var advertisments = _advertisementRepository.GetAll().Where(x => x.BrokerPersonId == brokerPerson.Id);

                                        foreach (var advertisment in advertisments)
                                        {
                                            advertisment.BrokerPersonId = null;
                                            advertisment.ProjectId = null;
                                            advertisment.IsDeleted = true;
                                            await _advertisementRepository.UpdateAsync(advertisment);
                                        }

                                        var adfavorites = _adFavorite.GetAll().Where(x => x.UserId == seekerUser.Id);

                                        foreach (var adfavorite in adfavorites)
                                        {
                                            _adFavorite.HardDelete(adfavorite);
                                        }

                                        #endregion
                                    }
                                    else if (input.action == "Restore")
                                    {
                                        brokerPerson.IsDeleted = false;
                                    }
                                }
                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                        else if (input.actionType == "SingleAction")
                        {
                            if (input.ids.Length > 0)
                            {
                                var brokerPerson = await _brokerPersonRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                                if (brokerPerson != null)
                                {
                                    if (input.action == "Delete")//Delete
                                    {
                                        //await _brokerPersonRepository.DeleteAsync(brokerPerson);
                                        #region HardDelete

                                        brokerPerson.IsDeleted = true;
                                        await _brokerPersonRepository.UpdateAsync(brokerPerson);

                                        var seekerUser = await _userRepository.FirstOrDefaultAsync(Convert.ToInt32(brokerPerson.UserId));


                                        seekerUser.EmailAddress = "Deleted@Deleted.Deleted";
                                        seekerUser.UserName = "Deleted@Deleted.Deleted";
                                        seekerUser.PhoneNumber = "Deleted@Deleted.Deleted";
                                        seekerUser.IsDeleted = true;
                                        await _userRepository.UpdateAsync(seekerUser);

                                        var advertisments = _advertisementRepository.GetAll().Where(x => x.BrokerPersonId == brokerPerson.Id);

                                        foreach (var advertisment in advertisments)
                                        {
                                            advertisment.BrokerPersonId = null;
                                            advertisment.ProjectId = null;
                                            advertisment.IsDeleted = true;
                                            await _advertisementRepository.UpdateAsync(advertisment);
                                        }

                                        var adfavorites = _adFavorite.GetAll().Where(x => x.UserId == seekerUser.Id);

                                        foreach (var adfavorite in adfavorites)
                                        {
                                            _adFavorite.HardDelete(adfavorite);
                                        }

                                        #endregion

                                    }
                                    else if (input.action == "Restore")
                                    {
                                        brokerPerson.IsDeleted = false;
                                    }
                                }
                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }
                    else if (input.action == "active" || input.action == "deactivate")
                    {
                        if (input.actionType == "GroupAction")
                        {
                            for (int i = 0; i < input.ids.Length; i++)
                            {
                                var brokerPerson = await _brokerPersonRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));

                                if (brokerPerson != null)
                                {

                                    if (brokerPerson.IsActive == true)
                                    {
                                        if (input.action == "deactivate")
                                        {

                                            brokerPerson.IsActive = false;
                                        }

                                    }
                                    else if (brokerPerson.IsActive == false)
                                    {
                                        if (input.action == "active")
                                        {

                                            brokerPerson.IsActive = true;
                                        }

                                    }

                                    await _brokerPersonRepository.UpdateAsync(brokerPerson);
                                }


                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                        else if (input.actionType == "SingleAction")
                        {
                            if (input.ids.Length > 0)
                            {
                                var brokerPerson = await _brokerPersonRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));

                                if (brokerPerson != null)
                                {
                                    if (input.action == "active")
                                    {

                                        brokerPerson.IsActive = true;
                                    }

                                    else if (input.action == "deactivate")
                                    {

                                        brokerPerson.IsActive = false;
                                    }
                                }

                                await _brokerPersonRepository.UpdateAsync(brokerPerson);
                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }

                    CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);

                    var query = _brokerPersonRepository.GetAllIncluding(x => x.User).Where(x => x.User.PhoneNumber != "Deleted@Deleted.Deleted");
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.User.Name.Contains(input.Name) || at.User.EmailAddress.Contains(input.Name)
                                                                            || at.User.PhoneNumber.Contains(input.Name));
                    int filteredCount = await query.CountAsync();
                    var brokerPersons = await query
                        .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .OrderBy(string.Format("  {0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<BrokerPersonDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<BrokerPersonDto>>(brokerPersons),
                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<BrokerPersonDto> Manage(BrokerPersonDto input)
        {
            try
            {
                if (input.Id > 0)
                {
                    User userExists = await _userManager.Users.Where(at => at.EmailAddress == input.UserEmailAddress && at.Id != input.UserId).FirstOrDefaultAsync();
                    if (userExists != null)
                    {
                        if (userExists.IsDeleted)
                            throw new UserFriendlyException(L("Pages.Users.Error.AlreadyExistInDeleted"));
                        else
                            throw new UserFriendlyException(L("Pages.Users.Error.AlreadyExist"));
                    }
                    userExists = await _userManager.Users.Where(at => at.PhoneNumber == input.UserPhoneNumber && at.Id != input.UserId).FirstOrDefaultAsync();
                    if (userExists != null)
                    {
                        if (userExists.IsDeleted)
                            throw new UserFriendlyException(L("Pages.Users.Error.PhoneNumberAlreadyExistInDeleted"));
                        else
                            throw new UserFriendlyException(L("Pages.Users.Error.PhoneNumberAlreadyExist"));
                    }

                    var brokerPerson = await _brokerPersonRepository.GetAsync(input.Id);
                    brokerPerson.Avatar = input.Avatar;
                    brokerPerson.SecondMobile = input.SecondMobile;
                    brokerPerson.IsActive = input.IsActive;

                    if (brokerPerson.PackageId == null || brokerPerson.PackageId != input.PackageId)
                    {
                        var package = await _PackageAppService.GetById(new EntityDto<long>((long)input.PackageId));
                        if (package != null)
                            brokerPerson.Balance = package.Points;
                    }
                    else
                        brokerPerson.Balance = input.Balance;

                    brokerPerson.PackageId = input.PackageId;

                    var updatedBrokerPerson = await _brokerPersonRepository.UpdateAsync(brokerPerson);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    var user = new User();
                    //CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
                    user = await _userManager.FindByIdAsync(updatedBrokerPerson.UserId.ToString());
                    user.Name = input.UserName;
                    user.EmailAddress = input.UserEmailAddress;
                    user.PhoneNumber = input.UserPhoneNumber;
                    user.UserName = input.UserEmailAddress;
                    user.Surname = input.UserSurname;
                    user.IsActive = input.User.IsActive;
                    CheckErrors(await _userManager.UpdateAsync(user));
                    return ObjectMapper.Map<BrokerPersonDto>(brokerPerson);
                }
                else
                {
                    //User user = new User();
                    User userExists = await _userManager.Users.Where(at => at.EmailAddress == input.UserEmailAddress).FirstOrDefaultAsync();
                    if (userExists != null)
                    {
                        if (userExists.IsDeleted)
                            throw new UserFriendlyException(L("Pages.Users.Error.AlreadyExistInDeleted"));
                        else
                            throw new UserFriendlyException(L("Pages.Users.Error.AlreadyExist"));
                    }
                    userExists = await _userManager.Users.Where(at => at.PhoneNumber == input.UserPhoneNumber).FirstOrDefaultAsync();
                    if (userExists != null)
                    {
                        if (userExists.IsDeleted)
                            throw new UserFriendlyException(L("Pages.Users.Error.PhoneNumberAlreadyExistInDeleted"));
                        else
                            throw new UserFriendlyException(L("Pages.Users.Error.PhoneNumberAlreadyExist"));
                    }
                    CreateUserDto user = new CreateUserDto();
                    user.Name = input.UserName;
                    user.Surname = input.UserSurname;
                    user.EmailAddress = input.UserEmailAddress;
                    user.PhoneNumber = input.UserPhoneNumber;
                    user.UserName = input.UserEmailAddress;
                    user.IsActive = true;
                    user.RoleNames = new string[] { "Admin" };
                    user.Password = input.UserPassword;
                    var user1 = await _userAppService.CreateAsync(user);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    //CheckErrors(await _userManager.CreateAsync(user, user.Password));
                    await CurrentUnitOfWork.SaveChangesAsync();
                    var brokerPerson = BrokerPerson.Create(user1.Id, input.SecondMobile, input.Avatar, input.IsActive);
                    var res = await _brokerPersonRepository.InsertAsync(brokerPerson);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<BrokerPersonDto>(res);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<BrokerPersonDto> GetById(EntityDto<long> input)
        {
            try
            {
                BrokerPersonDto brokerPersoninfo = new BrokerPersonDto();
                if (input.Id > 0)
                {
                    var brokerPerson = await _brokerPersonRepository.GetAllIncluding(x => x.User).Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                    if (brokerPerson != null)
                    {
                        brokerPersoninfo = ObjectMapper.Map<BrokerPersonDto>(brokerPerson);
                    }
                }
                return brokerPersoninfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<BrokerPersonDto> GetByUserId(EntityDto<long> input)
        {
            try
            {
                BrokerPersonDto brokerPersoninfo = new BrokerPersonDto();
                if (input.Id > 0)
                {
                    var brokerPerson = await _brokerPersonRepository.GetAllIncluding(x => x.User).Where(x => x.UserId == input.Id).FirstOrDefaultAsync();
                    if (brokerPerson != null)
                    {
                        brokerPersoninfo = ObjectMapper.Map<BrokerPersonDto>(brokerPerson);
                    }
                }
                return brokerPersoninfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<GetBrokerPersonOutput> GetAll(PagedBrokerPersonResultRequestDto input)
        {
            try
            {
                var query = _brokerPersonRepository.GetAll();
                //var query = _brokerPersonRepository.GetAllIncluding(x => x.User).Where(x => x.IsDeleted == false);
                //query = query.WhereIf(input.GovernorateId.HasValue&& input.GovernorateId>0, x => x.GovernorateId == input.GovernorateId);
                //query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name));
                //.Where(x => x.GovernorateId == input.GovernorateId);
                //var query = _brokerPersonRepository.GetAll();
                int count = await query.CountAsync();
                //if (input.MaxResultCount > 0)
                //    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetBrokerPersonOutput { BrokerPersons = ObjectMapper.Map<List<BrokerPersonDto>>(list) };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
        public async Task Delete(EntityDto<long> input)
        {
            var brokerPerson = await _brokerPersonRepository.GetAsync(input.Id);
            await _brokerPersonRepository.DeleteAsync(brokerPerson);
        }

        public async Task<BrokerPersonDto> UpdateBrokerPersonFromMobile(BrokerPersonDto input)
        {
            try
            {
                var brokerPerson = await _brokerPersonRepository.GetAsync(input.Id);
                brokerPerson.Avatar = input.Avatar;
                brokerPerson.SecondMobile = input.SecondMobile;
                brokerPerson.IsActive = input.IsActive;

                if (brokerPerson.PackageId == null || brokerPerson.PackageId != input.PackageId)
                {
                    var package = await _PackageAppService.GetById(new EntityDto<long>((long)input.PackageId));
                    if (package != null)
                        brokerPerson.Balance = package.Points;
                }
                else
                    brokerPerson.Balance = input.Balance;

                brokerPerson.PackageId = input.PackageId;

                var updatedBrokerPerson = await _brokerPersonRepository.UpdateAsync(brokerPerson);
                await CurrentUnitOfWork.SaveChangesAsync();

                var user = new User();

                user = await _userManager.FindByIdAsync(updatedBrokerPerson.UserId.ToString());
                user.Name = input.UserName;
                user.EmailAddress = input.UserEmailAddress;
                user.PhoneNumber = input.UserPhoneNumber;
                user.UserName = input.UserEmailAddress;
                user.Surname = input.UserSurname;
                user.IsWhatsApped = input.UserIsWhatsApped;
                CheckErrors(await _userManager.UpdateAsync(user));
                return ObjectMapper.Map<BrokerPersonDto>(brokerPerson);
             
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}