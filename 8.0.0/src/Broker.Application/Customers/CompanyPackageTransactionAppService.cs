using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Broker.Advertisements.Dto;
using Broker.Advertisements;
using Broker.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.Customers.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.Collections.Extensions;

namespace Broker.Customers
{
    public class CompanyPackageTransactionAppService : BrokerAppServiceBase, ICompanyPackageTransactionAppService
    {

        private readonly IRepository<CompanyPackageTransaction, long> _CompanyPackageTransactionRepository;

        public CompanyPackageTransactionAppService(IRepository<CompanyPackageTransaction, long> CompanyPackageTransactionRepository)
        {
            _CompanyPackageTransactionRepository = CompanyPackageTransactionRepository;
        }

        public async Task<CompanyPackageTransactionDto> Manage(CompanyPackageTransactionDto input)
        {
            try
            {
                var CompanyPackageTransaction = new CompanyPackageTransaction();
                if (input.Id > 0)
                {
                    CompanyPackageTransaction = await _CompanyPackageTransactionRepository.GetAsync(input.Id);
                    CompanyPackageTransaction.PackageId = input.PackageId;
                    CompanyPackageTransaction.CompanyId = input.CompanyId;

                    CompanyPackageTransaction = await _CompanyPackageTransactionRepository.UpdateAsync(CompanyPackageTransaction);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<CompanyPackageTransactionDto>(CompanyPackageTransaction);
                }
                else
                {
                    CompanyPackageTransaction = CompanyPackageTransaction.Create(input.PackageId, input.CompanyId);
                    CompanyPackageTransaction = await _CompanyPackageTransactionRepository.InsertAsync(CompanyPackageTransaction);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return ObjectMapper.Map<CompanyPackageTransactionDto>(CompanyPackageTransaction);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(EntityDto<long> input)
        {
            var Country = await _CompanyPackageTransactionRepository.GetAsync(input.Id);
            await _CompanyPackageTransactionRepository.DeleteAsync(Country);
        }

        public async Task<CompanyPackageTransactionDto> GetById(EntityDto<long> input)
        {
            try
            {
                CompanyPackageTransactionDto CompanyPackageTransactionInfo = new CompanyPackageTransactionDto();
                if (input.Id > 0)
                {
                    var CompanyPackageTransaction = await _CompanyPackageTransactionRepository.GetAll()
                        .Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                    if (CompanyPackageTransaction != null)
                    {
                        CompanyPackageTransactionInfo = ObjectMapper.Map<CompanyPackageTransactionDto>(CompanyPackageTransaction);

                    }
                }
                return CompanyPackageTransactionInfo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<PagedResultDto<CompanyPackageTransactionDto>> GetAll(PagedCompanyPackageTransactionResultRequestDto input)
        {
            try
            {
                var query = _CompanyPackageTransactionRepository.GetAll().Where(x => x.IsDeleted == false);
                query = (IQueryable<CompanyPackageTransaction>)query.WhereIf(input.CompanyId != null && input.CompanyId > 0, x => x.CompanyId == input.CompanyId);
                int count = await query.CountAsync();
                var list = await query.ToListAsync();

                return new PagedResultDto<CompanyPackageTransactionDto>
                {
                    TotalCount = count,
                    Items = ObjectMapper.Map<List<CompanyPackageTransactionDto>>(list)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
