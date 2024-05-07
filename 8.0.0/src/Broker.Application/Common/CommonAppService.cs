using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Broker.Common.Dto;
using Broker.Datatable.Dtos;
using Abp.AutoMapper;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.Collections.Extensions;
using System.Linq.Dynamic.Core;
using Broker.Lookups;
using AutoMapper;
using Broker.Linq.Extensions;
using System.Threading;
using Broker.Lookups.Dto;
using Broker.Helpers;
using Microsoft.AspNetCore.Http;
using Broker.Common.Dto;
using Broker;
using Broker.Customers;

namespace Broker.Common
{
    public class CommonAppService : BrokerAppServiceBase, ICommonAppService
    {
        private readonly IRepository<Seeker, long> _seekerRepository;
        private readonly IRepository<Owner, long> _ownerRepository;
        private readonly IRepository<Company, long> _CompanyRepository;
        private readonly IRepository<BrokerPerson, long> _brokerRepository; 

        public CommonAppService(IRepository<Seeker, long> seekerRepository,
                IRepository<Owner, long> ownerRepository, IRepository<Company, long> CompanyRepository,
                IRepository<BrokerPerson, long> brokerRepository
              )
        {
            _seekerRepository = seekerRepository;
            _ownerRepository = ownerRepository;
            _CompanyRepository = CompanyRepository;
            _brokerRepository = brokerRepository;
        }
        public async Task<CountDto> GetCountAsync()
        {
            try
            {
                var countDto = new CountDto();
                countDto.SeekerCount = _seekerRepository.GetAll().Where(x => x.IsDeleted == false).ToList().Count;
                countDto.OwnerCount = _ownerRepository.GetAll().Where(x => x.IsDeleted == false).ToList().Count;
                countDto.CompanyCount = _CompanyRepository.GetAll().Where(x => x.IsDeleted == false&&!x.IsSponser).ToList().Count;
                countDto.SponsorCount = _CompanyRepository.GetAll().Where(x => x.IsDeleted == false && x.IsSponser).ToList().Count;
                countDto.BrokerCount = _brokerRepository.GetAll().Where(x => x.IsDeleted == false).ToList().Count;

                return countDto;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //public async Task<List<RequestExtraCostDto>> GetAllExtraTypes(long userId, RequestType requestType)
        //{
        //    try
        //    {
        //        List<RequestExtraCostDto> extraCostTypes = new List<RequestExtraCostDto>();
        //        var employee = await _employeeRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == userId);
        //        if ( requestType == Helpers.RequestType.FieldVisit)
        //        {
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType=Helpers.ExtraCostType.CarParking, Name = "Car Parking" });
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.TollGate,Name = "Toll Gate" });
        //        }
        //        else if ((employee.TitleId == 1 || employee.TitleId == 2) && requestType != Helpers.RequestType.FieldVisit)
        //        {
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.CarParking,Name = "Car Parking" });
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.TollGate, Name = "Toll Gate" });
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.Tickets,Name = "Tickets" });
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.Allowance,Name = "Allowance" });
        //        }
        //        //if ((employee.TitleId != 1 && employee.TitleId != 2) && requestType == Helpers.RequestType.FieldVisit)
        //        //{
        //        //    extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.CarParking, Name = "Car Parking" });
        //        //    extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.TollGate, Name = "Toll Gate" });
        //        //}
        //        else if ((employee.TitleId != 1 && employee.TitleId != 2) && requestType != Helpers.RequestType.FieldVisit)
        //        {
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.CarParking, Name = "Car Parking" });
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.TollGate, Name = "Toll Gate" });
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.Tickets, Name = "Tickets" });
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.Allowance, Name = "Allowance" });
        //            extraCostTypes.Add(new RequestExtraCostDto { ExtraCostType = Helpers.ExtraCostType.UberReceipts,Name = "UberReceipts" });
        //        }
        //        return extraCostTypes;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<IFormFile> ResizeImage(IFormFile file)
        //{
        //    //string imageFile = FileUpload1.FileName;
        //    //string path = Server.MapPath("~/images/galeria/" + imageFile);
        //    //FileUpload1.SaveAs(path);

        //    //IFormFile image = System.Drawing.Image.FromFile(path);
        //        float aspectRatio = file.Length / (float)image.Size.Height;
        //        int newHeight = 200;
        //        int newWidth = Convert.ToInt32(aspectRatio * newHeight);
        //        System.Drawing.Bitmap thumbBitmap = new System.Drawing.Bitmap(newWidth, newHeight);
        //        System.Drawing.Graphics thumbGraph = System.Drawing.Graphics.FromImage(thumbBitmap);
        //        thumbGraph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //        thumbGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //        thumbGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //        var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
        //        thumbGraph.DrawImage(image, imageRectangle);
        //        thumbBitmap.Save(Server.MapPath("~/images/galeria/thumb/" + FileUpload1.FileName), System.Drawing.Imaging.ImageFormat.Jpeg);
        //        thumbGraph.Dispose();
        //        thumbBitmap.Dispose();
        //        image.Dispose();
        //}
    }
}

