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
using System.Security.Cryptography.X509Certificates;
using Broker.Advertisements;

namespace Broker.Lookups
{
    public class CompanyAppService : BrokerAppServiceBase, ICompanyAppService
    {
        private readonly IRepository<Company, long> _companyRepository;
        private readonly UserManager _userManager;
        private readonly IUserAppService _userAppService;
        private readonly IPackageAppService _PackageAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Advertisement, long> _advertisementRepository;
        private readonly IRepository<AdFavorite, long> _adFavorite;
        public CompanyAppService( IRepository<Company, long> companyRepository, UserManager userManager, IUserAppService userAppService,
            IPackageAppService PackageAppService,
             IRepository<User, long> userRepository, IRepository<Advertisement, long> advertisementRepository, IRepository<AdFavorite, long> adFavorite)
        {
            _companyRepository = companyRepository;
            _userManager = userManager;
            _userAppService = userAppService;
            _PackageAppService = PackageAppService;
            _userRepository = userRepository;
            _advertisementRepository = advertisementRepository;
            _adFavorite = adFavorite;

        }

        public async Task<DataTableOutputDto<CompanyDto>> IsPaged(GetCompanyInput input)
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
                                var brokerPerson = await _companyRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                                if (brokerPerson != null)
                                {
                                    if (input.action == "Delete")
                                    {
                                        //await _companyRepository.DeleteAsync(brokerPerson);
                                        #region HardDelete

                                        brokerPerson.IsDeleted = true;
                                        await _companyRepository.UpdateAsync(brokerPerson);

                                        var seekerUser = await _userRepository.FirstOrDefaultAsync(Convert.ToInt32(brokerPerson.UserId));


                                        seekerUser.EmailAddress = "Deleted@Deleted.Deleted";
                                        seekerUser.UserName = "Deleted@Deleted.Deleted";
                                        seekerUser.PhoneNumber = "Deleted@Deleted.Deleted";
                                        seekerUser.IsDeleted = true;
                                        await _userRepository.UpdateAsync(seekerUser);

                                        var advertisments = _advertisementRepository.GetAll().Where(x => x.CompanyId == brokerPerson.Id);

                                        foreach (var advertisment in advertisments)
                                        {
                                            advertisment.CompanyId = null;
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
                                var brokerPerson = await _companyRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                                if (brokerPerson != null)
                                {
                                    if (input.action == "Delete")//Delete
                                    {
                                        //await _companyRepository.DeleteAsync(brokerPerson);

                                        #region HardDelete

                                        brokerPerson.IsDeleted = true;
                                        await _companyRepository.UpdateAsync(brokerPerson);

                                        var seekerUser = await _userRepository.FirstOrDefaultAsync(Convert.ToInt32(brokerPerson.UserId));


                                        seekerUser.EmailAddress = "Deleted@Deleted.Deleted";
                                        seekerUser.UserName = "Deleted@Deleted.Deleted";
                                        seekerUser.PhoneNumber = "Deleted@Deleted.Deleted";
                                        seekerUser.IsDeleted = true;
                                        await _userRepository.UpdateAsync(seekerUser);

                                        var advertisments = _advertisementRepository.GetAll().Where(x => x.CompanyId == brokerPerson.Id);

                                        foreach (var advertisment in advertisments)
                                        {
                                            advertisment.CompanyId = null;
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
                    else if (input.action == "active" || input.action == "deactivate"|| input.action == "approve" || input.action == "decline")
                    {
                        if (input.actionType == "GroupAction")
                        {
                            for (int i = 0; i < input.ids.Length; i++)
                            {
                                var brokerPerson = await _companyRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));

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


                                    if (brokerPerson.IsSponser == true)
                                    {
                                        if (input.action == "decline")
                                        {

                                            brokerPerson.IsSponser = false;
                                        }

                                    }
                                    else if (brokerPerson.IsSponser == false)
                                    {
                                        if (input.action == "approve")
                                        {

                                            brokerPerson.IsSponser = true;
                                        }

                                    }

                                    await _companyRepository.UpdateAsync(brokerPerson);
                                }


                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                        else if (input.actionType == "SingleAction")
                        {
                            if (input.ids.Length > 0)
                            {
                                var brokerPerson = await _companyRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));

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


                                    if (input.action == "approve")
                                    {

                                        brokerPerson.IsSponser = true;
                                    }

                                    else if (input.action == "decline")
                                    {

                                        brokerPerson.IsSponser = false;
                                    }
                                }

                                await _companyRepository.UpdateAsync(brokerPerson);
                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }
                    CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);

                    var query = _companyRepository.GetAllIncluding(x => x.User).Where(x => x.User.PhoneNumber != "Deleted@Deleted.Deleted");
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.User.Name.Contains(input.Name) || at.User.EmailAddress.Contains(input.Name)
                                                                            || at.User.PhoneNumber.Contains(input.Name));
                    int filteredCount = await query.CountAsync();
                    var companys = await query
                        .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .OrderBy(string.Format("  {0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<CompanyDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<CompanyDto>>(companys),
                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<CompanyDto> Manage(CompanyDto input)
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

                    var company = await _companyRepository.GetAsync(input.Id);
                    
                    company.IsSponser = input.IsSponser;
                    company.IsActive = input.IsActive;
                    company.SecondMobile = input.SecondMobile;
                    company.Logo = input.Logo;
                    company.BWLogo = input.BWLogo;
                    company.CommericalAvatar = input.CommericalAvatar;
                    company.About = input.About;
                    company.Latitude = input.Latitude;
                    company.Longitude = input.Longitude;
                    company.Facebook = input.Facebook;
                    company.Instagram = input.Instagram;
                    company.Snapchat = input.Snapchat;
                    company.Tiktok = input.Tiktok;
                    company.Website = input.Website;

                    if (company.PackageId == null || company.PackageId != input.PackageId)
                    {
                        var package = await _PackageAppService.GetById(new EntityDto<long>((long)input.PackageId));
                        if (package != null)
                            company.Balance = package.Points;
                    }
                    else
                        company.Balance = input.Balance;

                    company.PackageId = input.PackageId;

                    //company.PackageId = input.PackageId > 0 ? input.PackageId :2;

                    var updatedCompany = await _companyRepository.UpdateAsync(company);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    var user = new User();
                    //CurrentUnitOfWork.EnableFilter(AbpDataFilters.MayHaveTenant);
                    user = await _userManager.FindByIdAsync(updatedCompany.UserId.ToString());
                    user.Name = input.UserName;
                    user.EmailAddress = input.UserEmailAddress;
                    user.PhoneNumber = input.UserPhoneNumber;
                    user.UserName = input.UserEmailAddress;
                    user.Surname = input.UserSurname;
                    user.IsActive = input.User.IsActive;
                    CheckErrors(await _userManager.UpdateAsync(user));
                    return ObjectMapper.Map<CompanyDto>(company);
                }
                else
                {
                    //input.PackageId = input.PackageId > 0 ? input.PackageId : 2;
                    
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
                    var company = Company.Create(user1.Id, input.IsSponser, input.IsActive, input.SecondMobile, input.Logo, input.BWLogo, input.CommericalAvatar,
                       input.About, input.Latitude, input.Longitude,input.Facebook,input.Instagram,input.Snapchat,input.Tiktok,input.Website,input.PackageId) ;
                    var res = await _companyRepository.InsertAsync(company);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<CompanyDto>(res);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<CompanyDto> GetById(EntityDto<long> input)
        {
            try
            {
                CompanyDto companyinfo = new CompanyDto();
                if (input.Id > 0)
                {
                    var company = await _companyRepository.GetAllIncluding(x => x.User).Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                    if (company != null)
                    {
                        companyinfo = ObjectMapper.Map<CompanyDto>(company);
                    }
                }
                return companyinfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<CompanyDto> GetByUserId(EntityDto<long> input)
        {
            try
            {
                CompanyDto companyinfo = new CompanyDto();
                if (input.Id > 0)
                {
                    var company = await _companyRepository.GetAllIncluding(x => x.User,x=>x.Package).Where(x => x.UserId == input.Id).FirstOrDefaultAsync();
                    if (company != null)
                    {
                        companyinfo = ObjectMapper.Map<CompanyDto>(company);
                    }
                }
                return companyinfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<GetCompanyOutput> GetAll(PagedCompanyResultRequestDto input)
        {
            try
            {
                var query = _companyRepository.GetAllIncluding(x=>x.User);
                query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.User.Name.Contains(input.Name) || at.User.Surname.Contains(input.Name));

                query = query.WhereIf(input.IsSponsor.HasValue, x => x.IsSponser == input.IsSponsor);
                //if (input.MaxResultCount > 0)
                //    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                int count = await query.CountAsync();
                var list = await query.ToListAsync();
                return new GetCompanyOutput { Companys = ObjectMapper.Map<List<CompanyDto>>(list) };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
        public async Task Delete(EntityDto<long> input)
        {
            var company = await _companyRepository.GetAsync(input.Id);
            await _companyRepository.DeleteAsync(company);
        }
        public async Task<CompanyDto> UpdateCompanyFromMobile(CompanyDto input)
        {
            try
            {

                var company = await _companyRepository.GetAsync(input.Id);
                
                company.SecondMobile = input.SecondMobile;
                company.Logo = input.Logo;
                company.BWLogo = input.BWLogo;
                company.CommericalAvatar = input.CommericalAvatar;
                company.About = input.About;
                company.Longitude = input.Longitude;
                company.Latitude = input.Latitude;
                company.Facebook = input.Facebook;
                company.Instagram = input.Instagram;
                company.Snapchat = input.Snapchat;
                company.Tiktok = input.Tiktok;
                company.Website = input.Website;

                if (company.PackageId == null || company.PackageId != input.PackageId)
                {
                    var package =input.PackageId!=null? await _PackageAppService.GetById(new EntityDto<long>((long)input.PackageId)):null;
                    if (package != null)
                        company.Balance+= package.Points;
                }
                else
                    company.Balance = input.Balance;

                company.PackageId = input.PackageId??company.PackageId;

                var updatedCompany = await _companyRepository.UpdateAsync(company);
                await CurrentUnitOfWork.SaveChangesAsync();

                var user = new User();

                user = await _userManager.FindByIdAsync(updatedCompany.UserId.ToString());
                user.Name = input.UserName;
                user.EmailAddress = input.UserEmailAddress;
                user.PhoneNumber = input.UserPhoneNumber;
                user.UserName = input.UserEmailAddress;
                user.Surname = input.UserSurname;
                user.IsWhatsApped = input.UserIsWhatsApped;
                CheckErrors(await _userManager.UpdateAsync(user));
                return ObjectMapper.Map<CompanyDto>(company);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}