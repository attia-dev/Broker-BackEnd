using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Customers.Dto;
using Broker.Datatable.Dtos;
using Broker.Helpers;
using Broker.Localization;
using Broker.Lookups;
using Broker.Lookups.Dto;
using Broker.Payments;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Payments.Dto
{
    [AutoMapFrom(typeof(Wallet))]
    public class WalletDto : FullAuditedEntityDto<int>
    {
        public long CompanyId { get; set; }
        public CompanyDto Company { get; set; }
        public TransactionType Type { get; set; }
        public decimal? Amount { get; set; }
        public int? Points { get; set; }
        public DateTime TransactionTime { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto LastModifierUser { get; set; }
        public UserDto DeleterUser { get; set; }
    }
    public class GetWalletsInput : DataTableInputDto
    {
        public string Name { get; set; }
        public long? CompanyId { get; set; }
        public TransactionType EnumTransaction { get; set; }
    }
    public class PagedWalletResultRequestDto : PagedResultRequestDto
    {
        public string Name { get; set; }
        public long? CompanyId { get; set; }
        public TransactionType EnumTransaction { get; set; }
    }
    public class GetWalletsOutput
    {
        public List<WalletDto> Wallets { get; set; }
        public string Error { get; set; }
    }
}
