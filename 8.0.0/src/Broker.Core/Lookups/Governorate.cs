using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Broker.Lookups
{
    [Table("Governorates")]
    public class Governorate : FullAuditedEntity<long, User>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public long CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }
        public static Governorate Create(string NameAr,string NameEn, long CountryId)
        {
            var Governorate = new Governorate
            {
                NameAr = NameAr,
                NameEn = NameEn,
                CountryId = CountryId
            };
            return Governorate;
        }
    }
}
