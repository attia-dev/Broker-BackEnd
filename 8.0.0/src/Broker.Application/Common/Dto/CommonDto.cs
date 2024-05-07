using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Broker.Datatable.Dtos;
using Broker.Helpers;
using Broker.Localization;
using Broker.Lookups;
using Broker.Lookups.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Broker.Common.Dto
{
    public class CommonDto
    {

    }
    public class CountDto
    {
        public int SeekerCount { get; set; }
        public int OwnerCount { get; set; }
        public int CompanyCount { get; set; }
        public int SponsorCount { get; set; }
        public int BrokerCount { get; set; }
        

    }
    //public class ExtraCostTypeDto
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

}
