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
using static Broker.Authorization.PermissionNames;
using Broker.Payments.Dto;
using Broker.Helpers;
using Abp.Runtime.Session;

namespace Broker.Payments
{
    public class WalletAppService : BrokerAppServiceBase, IWalletAppService
    {
        private readonly IRepository<Wallet, long> _walletRepository;
        private readonly object _abpSession;

        public WalletAppService(IRepository<Wallet, long> walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<DataTableOutputDto<WalletDto>> IsPaged(GetWalletsInput input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    if (input.actionType == "GroupAction")
                    {
                        for (int i = 0; i < input.ids.Length; i++)
                        {
                            var Wallet = await _walletRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[i]));
                            if (Wallet != null)
                            {
                                if (input.action == "Delete")
                                    await _walletRepository.DeleteAsync(Wallet);
                                else if (input.action == "Restore")
                                {
                                    Wallet.IsDeleted = false;
                                    Wallet.DeleterUserId = null;
                                    Wallet.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    else if (input.actionType == "SingleAction")
                    {
                        if (input.ids.Length > 0)
                        {
                            var Wallet = await _walletRepository.FirstOrDefaultAsync(Convert.ToInt32(input.ids[0]));
                            if (Wallet != null)
                            {
                                if (input.action == "Delete")//Delete
                                    await _walletRepository.DeleteAsync(Wallet);
                                else if (input.action == "Restore")
                                {
                                    Wallet.IsDeleted = false;
                                    Wallet.DeleterUserId = null;
                                    Wallet.DeletionTime = null;
                                }
                            }
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    var query = _walletRepository.GetAll().Include(x => x.Company).Where(x => 1 == 1);
                    int count = await query.CountAsync();
                    query = query.FilterDataTable((DataTableInputDto)input);
                    query = query.WhereIf(input.CompanyId.HasValue && input.CompanyId > 0, at => at.CompanyId == input.CompanyId);

                    //query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name));
                    int filteredCount = await query.CountAsync();
                    var cities = await query
                          .Include(x => x.CreatorUser).Include(x => x.LastModifierUser).Include(x => x.DeleterUser)
                          .OrderBy(string.Format("{0} {1}", input.columns[input.order[0].column].name, input.order[0].dir))
                          .Skip(input.start)
                          .Take(input.length)
                          .ToListAsync();
                    return new DataTableOutputDto<WalletDto>
                    {
                        draw = input.draw,
                        iTotalDisplayRecords = filteredCount,
                        iTotalRecords = count,
                        aaData = ObjectMapper.Map<List<WalletDto>>(cities)
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WalletDto> Manage(WalletDto input)
        {
            try
            {
                var Wallet = new Wallet();
                if (input.Id > 0)
                {
                    Wallet = await _walletRepository.GetAsync(input.Id);
                    Wallet.Type = input.Type;
                    Wallet.Amount = input.Amount;
                    Wallet.Points = input.Points;
                    Wallet.TransactionTime = input.TransactionTime;
                    Wallet.CompanyId = input.CompanyId;
                    Wallet = await _walletRepository.UpdateAsync(Wallet);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<WalletDto>(Wallet);
                }
                else
                {
                    Wallet = Wallet.Create(input.Type, (decimal)input.Amount, (int)input.Points, input.TransactionTime, input.CompanyId);
                    Wallet = await _walletRepository.InsertAsync(Wallet);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<WalletDto>(Wallet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<long> input)
        {
            var Wallet = await _walletRepository.GetAsync(input.Id);
            await _walletRepository.DeleteAsync(Wallet);
        }

        public async Task<WalletDto> GetById(EntityDto<long> input)
        {
            try
            {
                WalletDto Walletinfo = new WalletDto();
                if (input.Id > 0)
                {
                    var Wallet = await _walletRepository.GetAllIncluding(x => x.Company).Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                    //GetAsync(input.Id);
                    if (Wallet != null)
                    {
                        Walletinfo = ObjectMapper.Map<WalletDto>(Wallet);
                    }
                }
                return Walletinfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<GetWalletsOutput> GetAll(PagedWalletResultRequestDto input)
        {
            try
            {
                
                var query = _walletRepository.GetAllIncluding(x=>x.Company);
                //query = query.WhereIf(!string.IsNullOrEmpty(input.Name), at => at.NameAr.Contains(input.Name) || at.NameEn.Contains(input.Name));
                query =query.WhereIf(input.CompanyId.HasValue && input.CompanyId>0, at=>at.CompanyId==input.CompanyId); 
                int count = await query.CountAsync();
                //if (input.MaxResultCount > 0)
                //    query = query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
                var list = await query.ToListAsync();
                return new GetWalletsOutput { Wallets = ObjectMapper.Map<List<WalletDto>>(list) };
            }
            catch (Exception ex)
            {
                return new GetWalletsOutput { Error = ex.Message };
            }
        }

    }
}
