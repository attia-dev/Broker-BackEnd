using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using Broker.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Lookups
{
    [Table("Definitions")]
    public class Definition : FullAuditedEntity<int, User>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public DefinitionTypes Type { get; set; }
        public string Avatar { get; set; }
        public int? Value { get; set; }
        //   public int? ParentId { get; set; }
        //   [ForeignKey("ParentId")]
        //   public Definition Parent { get; set; }
        //   public string Avatar { get; set; }
        //   public DefinitionTypes Type { get; set; }
        //   public string Key { get; set; }
        //   public int? Order { get; set; }
        public static Definition Create(string NameAr, string NameEn, string DescriptionAr, string DescriptionEn, DefinitionTypes Type , string Avatar, int? Value)
        {
            var definition = new Definition
            {
                NameAr = NameAr,
                NameEn = NameEn,
                DescriptionAr = DescriptionAr,
                DescriptionEn = DescriptionEn,
                Type= Type,
                Avatar = Avatar,
                Value = Value,
            };
            return definition;
        }
    }
}
