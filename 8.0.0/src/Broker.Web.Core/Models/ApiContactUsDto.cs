using Abp.Application.Services.Dto;
using Broker.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker.Models
{
  
    public class ApiContactUsDto : EntityDto<long>
    {
        public string EmailAddress { get; set; }
        public string EmailSubject { get; set; }
        public string AttachmentPath { get; set; }
        public long? UserId { get; set; }

    }
    public class ApiCreateContactUsOut
    {
        public ApiCreateContactUsOut()
        {
        }
        public long?ContactUsId { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ApiCreateContactUsDto
    {
        public string EmailAddress { get; set; }
        public string EmailSubject { get; set; }
        public string AttachmentPath { get; set; }
        public long? UserId { get; set; }

    }

    public class GetAllContactUsesOutput
    {
        public GetAllContactUsesOutput()
        {
            ContactUses = new List<ApiContactUsDto>();
        }
        public List<ApiContactUsDto> ContactUses { get; set; }
       // public long? Count { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class GetAllContactUsesInput
    {
        public long Keyword { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }

    }
}
