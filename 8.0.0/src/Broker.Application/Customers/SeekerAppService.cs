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
    public class SeekerAppService : BrokerAppServiceBase, ISeekerAppService
    {

        private readonly IRepository<Seeker, long> _seekerRepository;
        private readonly UserManager _userManager;
        private readonly IUserAppService _userAppService;
        private readonly IPackageAppService _PackageAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Advertisement, long> _advertisementRepository;
        private readonly IRepository<AdFavorite, long> _adFavorite;
        public SeekerAppService( IRepository<Seeker, long> seekerRepository, UserManager userManager, IUserAppService userAppService,
            IPackageAppService PackageAppService, IRepository<User, long> userRepository 
            ,IRepository<Advertisement, long> advertisementRepository ,IRepository<AdFavorite, long> adFavorite)
        {
            _seekerRepository = seekerRepository;
            _userManager = userManager;
            _userAppService = userAppService;
            _PackageAppService = PackageAppService;
            _userRepository = userRepository;
            _advertisementRepository = advertisementRepository;
            _adFavorite = adFavorite;
        }

        public async Task<DataTableOutputDto<SeekerDto>> IsPaged(GetSeekerInput input)
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
                                var seeker = await _seekerRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                                if (seeker != null)
                                {
                                    if (input.action == "Delete")
                                    {
                                        //await _seekerRepository.DeleteAsync(brokerPerson);
                                        #region HardDelete

                                        seeker.IsDeleted = true;
                                        await _seekerRepository.UpdateAsync(seeker);
 
                                        var seekerUser = await _userRepository.FirstOrDefaultAsync(Convert.ToInt32(seeker.UserId));


                                        seekerUser.EmailAddress = "Deleted@Deleted.Deleted";
                                        seekerUser.UserName = "Deleted@Deleted.Deleted";
                                        seekerUser.PhoneNumber = "Deleted@Deleted.Deleted";
                                        seekerUser.IsDeleted = true;
                                        await _userRepository.UpdateAsync(seekerUser);
 
                                        var advertisments = _advertisementRepository.GetAll().Where(x => x.SeekerId == seeker.Id);

                                        foreach (var advertisment in advertisments)
                                        {
                                            advertisment.SeekerId = null;
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
                                        seeker.IsDeleted = false;
                                    }
                                }
                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                        else if (input.actionType == "SingleAction")
                        {
                            if (input.ids.Length > 0)
                            {
                                var seeker = await _seekerRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                                if (seeker != null)
                                {
                                    if (input.action == "Delete")//Delete
                                    {
                                        #region HardDelete

                                     
                                        //Delete Seeker   -->  Seeker
                                        //IsPaged

                                        // seeker.UserId = 0;
                                        seeker.IsDeleted = true;
                                        await _seekerRepository.UpdateAsync(seeker);
                                        //await CurrentUnitOfWork.SaveChangesAsync();
                                        // await _seekerRepository.DeleteAsync(seeker);

                                        //Delete User   -->  AbpUser
                                        //IsPaged
                                        var seekerUser = await _userRepository.FirstOrDefaultAsync(Convert.ToInt32(seeker.UserId));
                                                 

                                             seekerUser.EmailAddress = "Deleted@Deleted.Deleted";
                                             seekerUser.UserName = "Deleted@Deleted.Deleted";
                                             seekerUser.PhoneNumber = "Deleted@Deleted.Deleted";
                                             seekerUser.IsDeleted = true;
                                        await _userRepository.UpdateAsync(seekerUser);
                                      //  await CurrentUnitOfWork.SaveChangesAsync();

                                        //Delete Advertisment   -->  Advertisment
                                        //IsPaged advertisment - Project
                                        var advertisments =  _advertisementRepository.GetAll().Where(x=>x.SeekerId==seeker.Id);
                                        
                                        foreach (var advertisment in advertisments)
                                        {
                                            advertisment.SeekerId = null;
                                            advertisment.ProjectId = null;
                                            advertisment.IsDeleted = true;
                                            await _advertisementRepository.UpdateAsync(advertisment);
                                        }
                                     //   await CurrentUnitOfWork.SaveChangesAsync();
                                        //Delete AdFavorite   -->  AdFavorite (HardDelete)
                                        //IsPaged--->harddelete 
                                        var adfavorites = _adFavorite.GetAll().Where(x => x.UserId == seekerUser.Id);

                                        foreach (var adfavorite in adfavorites)
                                        {
                                             _adFavorite.HardDelete(adfavorite);
                                      //     await CurrentUnitOfWork.SaveChangesAsync();
                                           
                                            //adfavorite.UserId = null;
                                            //adfavorite.AdvertisementId = false;
                                            //await _advertisementRepository.UpdateAsync(advertisment);
                                        }
                                        
                                        #endregion
                                    }
                                    else if (input.action == "Restore")
                                    {
                                        seeker.IsDeleted = false;
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
                                var brokerPerson = await _seekerRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));

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

                                    await _seekerRepository.UpdateAsync(brokerPerson);
                                }


                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                        else if (input.actionType == "SingleAction")
                        {
                            if (input.ids.Length > 0)
                            {
                                var brokerPerson = await _seekerRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));

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

                                await _seekerRepository.UpdateAsync(brokerPerson);
                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }
                    CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);

                    var query = _seekerRepository.GetAllIncluding(x => x.User).Where(x=>x.User.PhoneNumber!= "Deleted@Deleted.Deleted");
                  
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.User.Name.Contains(input.Name) || at.User.EmailAddress.Contains(input.Name)
                                                                            || at.User.PhoneNumber.Contains(input.Name));
                    int filteredCount = await query.CountAsync();
                    var seekers = await query
                        .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .OrderBy(string.Format("  {0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<SeekerDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<SeekerDto>>(seekers),
                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<SeekerDto> Manage(SeekerDto input)
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

                    var seeker = await _seekerRepository.GetAsync(input.Id);
                    seeker.Avatar = input.Avatar;
                    seeker.SecondMobile = input.SecondMobile;
                    seeker.IsActive = input.IsActive;


                    if (seeker.PackageId == null || seeker.PackageId != input.PackageId)
                    {
                        var package = await _PackageAppService.GetById(new EntityDto<long>((long)input.PackageId));
                        if (package != null)
                            seeker.Balance = package.Points;
                    }
                    else
                        seeker.Balance = input.Balance;

                    seeker.PackageId = input.PackageId;

                    var updatedSeeker = await _seekerRepository.UpdateAsync(seeker);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    var user = new User();
                    //CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
                    user = await _userManager.FindByIdAsync(updatedSeeker.UserId.ToString());
                    user.Name = input.UserName;
                    user.EmailAddress = input.UserEmailAddress;
                    user.PhoneNumber = input.UserPhoneNumber;
                    user.UserName = input.UserEmailAddress;
                    user.Surname = input.UserSurname;
                    user.IsActive = input.User.IsActive;

                    CheckErrors(await _userManager.UpdateAsync(user));
                    return ObjectMapper.Map<SeekerDto>(seeker);
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
                    var seeker = Seeker.Create(user1.Id, input.SecondMobile, input.Avatar, input.IsActive);
                    var res = await _seekerRepository.InsertAsync(seeker);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<SeekerDto>(res);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<SeekerDto> GetById(EntityDto<long> input)
        {
            try
            {
                SeekerDto seekerinfo = new SeekerDto();
                if (input.Id > 0)
                {
                    var seeker = await _seekerRepository.GetAllIncluding(x => x.User).Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                    if (seeker != null)
                    {
                        seekerinfo = ObjectMapper.Map<SeekerDto>(seeker);
                    }
                }
                return seekerinfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<SeekerDto> GetByUserId(EntityDto<long> input)
        {
            try
            {
                SeekerDto seekerinfo = new SeekerDto();
                if (input.Id > 0)
                {
                    var seeker = await _seekerRepository.GetAllIncluding(x => x.User).Where(x => x.UserId == input.Id).FirstOrDefaultAsync();
                    if (seeker != null)
                    {
                        seekerinfo = ObjectMapper.Map<SeekerDto>(seeker);
                    }
                }
                return seekerinfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<GetSeekerOutput> GetAll(PagedSeekerResultRequestDto input)
        {
            try
            {
                var query = _seekerRepository.GetAll();
                //var query = _seekerRepository.GetAllIncluding(x => x.User).Where(x => x.IsDeleted == false);
                //query = query.WhereIf(input.GovernorateId.HasValue&& input.GovernorateId>0, x => x.GovernorateId == input.GovernorateId);
                //query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name));
                //.Where(x => x.GovernorateId == input.GovernorateId);
                //var query = _seekerRepository.GetAll();
                int count = await query.CountAsync();
                //if (input.MaxResultCount > 0)
                //    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetSeekerOutput { Seekers = ObjectMapper.Map<List<SeekerDto>>(list) };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
        public async Task Delete(EntityDto<long> input)
        {
            var seeker = await _seekerRepository.GetAsync(input.Id);
            await _seekerRepository.DeleteAsync(seeker);
        }

        public async Task<SeekerDto> UpdateSeekerFromMobile(SeekerDto input)
        {
            try
            {
                var seeker = await _seekerRepository.GetAsync(input.Id);
               
                seeker.Avatar = input.Avatar;
                seeker.SecondMobile = input.SecondMobile;
                seeker.IsActive = input.IsActive;

                if (seeker.PackageId != null || seeker.PackageId != input.PackageId)///
                {
                    var package = await _PackageAppService.GetById(new EntityDto<long>((long)input.PackageId));
                    if (package != null)
                        seeker.Balance = package.Points;
                }
                else
                    seeker.Balance = input.Balance;

                seeker.PackageId = input.PackageId;

                var updatedSeeker = await _seekerRepository.UpdateAsync(seeker);
                await CurrentUnitOfWork.SaveChangesAsync();

                var user = new User();
                //CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
                //CurrentUnitOfWork.SetTenantId(1);
                user = await _userManager.FindByIdAsync(updatedSeeker.UserId.ToString());
                user.Name = input.UserName;
                user.EmailAddress = input.UserEmailAddress;
                user.PhoneNumber = input.UserPhoneNumber;
                user.UserName = input.UserEmailAddress;
                user.Surname = input.UserSurname;
                user.IsWhatsApped = input.UserIsWhatsApped;
                CheckErrors(await _userManager.UpdateAsync(user));
                return ObjectMapper.Map<SeekerDto>(seeker);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}