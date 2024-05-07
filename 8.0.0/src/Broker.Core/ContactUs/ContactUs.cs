using Abp.Domain.Entities.Auditing;
using Broker.Authorization.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.ContactUs
{
    

    [Table("ContactUses")]
    public class ContactUs : FullAuditedEntity<long, User>
    {
        public string EmailAddress { get; set; }
        public string EmailSubject { get; set; }
        //public IFormFile Attachment { get; set; }
        public string AttachmentPath { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        public static ContactUs Create(string EmailAddress, string EmailSubject, string AttachmentPath, long? UserId)
        {
            var ContactUs = new ContactUs
            {
                EmailAddress = EmailAddress,
                EmailSubject = EmailSubject,
                AttachmentPath = AttachmentPath,
                UserId= UserId
            };
            return ContactUs;
        }
    }
}
