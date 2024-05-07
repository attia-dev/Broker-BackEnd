using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;

namespace Broker.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UploadController : BrokerControllerBase
    {
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                // var file = Request.Form.Files[0];
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("Resources", "UploadFiles");
                var pathToSave = Path.Combine(System.IO.Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> UploadMobile(IFormFile file)
        //{
        //    //Windows path
        //    var folderName = Path.Combine("Resources", "UploadFiles");
        //    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
      
        //    //Linux path
        //    //var uploadLocation = Path.Combine(_env.WebRootPath, "Uploads//UsersImg");
      
        //    var fileName = file.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
      
        //    if (file.Length > 0)
        //    {
        //        using (var stream = new FileStream(Path.Combine(pathToSave, fileName), FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //    }
        //    return Ok();
        //}

        [HttpPost]
        public async Task<IActionResult> UploadMobile (IFormFile file) //New One
        {
            // Windows path
            var folderName = Path.Combine("Resources", "UploadFiles");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            // Linux path
            // var uploadLocation = Path.Combine(_env.WebRootPath, "Uploads//UsersImg");

            var fileName = file.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();

            if (file.Length > 0)
            {
                
                using (var stream = new FileStream(Path.Combine(pathToSave, fileName), FileMode.Create))
                {
                    var fileBinary =await ConvertIFormFileToByteArray(file);
                    System.Drawing.Image image1 = System.Drawing.Image.FromStream(new System.IO.MemoryStream(fileBinary));//fileBinary
                    Image img = await ResizeImage(file, image1.Width, image1.Height); //image.width,image.height
                    img.Save(stream, ImageFormat.Jpeg); // SomeFormat
                    //await file.CopyToAsync(stream);
                }
            }
            return Ok();
        }
        [HttpGet]
        public async Task<byte[]> ConvertIFormFileToByteArray(IFormFile file)
        {
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        [HttpGet]
        public async Task<Image> ResizeImage(IFormFile file, int width, int height)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var img = Image.FromStream(memoryStream))
                {
                    return Resize(img, width, height);
                }
            }
        }
        [HttpGet]
        public Image Resize(Image image, int w, int h)
        {
            var res = new Bitmap(w, h);
            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, w, h);
            }
            return res;
        }



        //public async Task<IActionResult> AsyncUploadApi()
        //{
        //    try
        //    {
        //        var fileName = "";
        //        var contentType = "";
        //        var httpPostedFile = Request.Form.Files.FirstOrDefault();
        //        if (String.IsNullOrEmpty(Request.Form["qqfile"]))
        //        {
        //            // IE
        //            if (httpPostedFile == null)
        //                throw new ArgumentException("No file uploaded");

        //            fileName = Path.GetFileName(httpPostedFile.FileName);
        //            contentType = httpPostedFile.ContentType;
        //        }

        //        var fileBinary = _attachmentService.GetDownloadBits(httpPostedFile);

        //        var fileExtension = Path.GetExtension(fileName);
        //        if (!String.IsNullOrEmpty(fileExtension))
        //            fileExtension = fileExtension.ToLowerInvariant();
        //        //contentType is not always available 
        //        //that's why we manually update it here
        //        //http://www.sfsu.edu/training/mimetype.htm
        //        if (String.IsNullOrEmpty(contentType))
        //        {
        //            switch (fileExtension.ToLower())
        //            {
        //                case ".bmp":
        //                    contentType = MimeTypes.ImageBmp;
        //                    break;
        //                case ".gif":
        //                    contentType = MimeTypes.ImageGif;
        //                    break;
        //                case ".jpeg":
        //                case ".jpg":
        //                case ".jpe":
        //                case ".jfif":
        //                case ".pjpeg":
        //                case ".pjp":
        //                    contentType = MimeTypes.ImageJpeg;
        //                    break;
        //                case ".png":
        //                    contentType = MimeTypes.ImagePng;
        //                    break;
        //                case ".tiff":
        //                case ".tif":
        //                    contentType = MimeTypes.ImageTiff;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //        //   var pictureSmall = new Domain.Model.Attachment();
        //        //   string new_filenameSmall = "";
        //        // var picture = await _attachmentService.InsertPicture(httpPostedFile.Length, fileName, contentType, fileExtension);

        //        string new_filename = string.Format("{0}_{1}", Guid.NewGuid().ToString().Substring(0, 6), fileName);

        //        string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/AttachmentFiles",
        //            new_filename);

        //        using (var stream = new FileStream(SavePath,
        //   FileMode.Create))
        //        {
        //            httpPostedFile.CopyTo(stream);
        //        }

        //        System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(fileBinary));
        //        var orginalHeight = image.Height;
        //        var orginalWidth = image.Width;
        //        var prop = image.GetPropertyItem(0x0112);
        //        var orientation = (int)prop.Value[0];
        //        //using (var image = System.Drawing.Image.FromStream(stream))
        //        //{
        //        //        TargetSize targetSize = new TargetSize(image.Width, image.Height);

        //        //        // Calculate the resize factor
        //        //        var scaleFactor = targetSize.CalculateScaleFactor(image.Width, image.Height);
        //        //scaleFactor /= (int)4;
        //        //var newWidth = (int)Math.Floor(image.Width / scaleFactor);
        //        //var newHeight = (int)Math.Floor(image.Height / scaleFactor);
        //        //using (var newBitmap = new Bitmap(newWidth, newHeight))
        //        //{
        //        //    using (var imageScaler = Graphics.FromImage(newBitmap))
        //        //    {
        //        //        imageScaler.CompositingQuality = CompositingQuality.HighQuality;
        //        //        imageScaler.SmoothingMode = SmoothingMode.HighQuality;
        //        //        imageScaler.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //        //        var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
        //        //        imageScaler.DrawImage(image, imageRectangle);

        //        //        // Fix orientation if needed.
        //        //        if (image.PropertyIdList.Contains(OrientationKey))
        //        //        {
        //        //            var orientation = (int)image.GetPropertyItem(OrientationKey).Value[0];
        //        //            switch (orientation)
        //        //            {
        //        //                case NotSpecified: // Assume it is good.
        //        //                case NormalOrientation:
        //        //                    // No rotation required.
        //        //                    break;
        //        //                case MirrorHorizontal:
        //        //                    newBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
        //        //                    break;
        //        //                case UpsideDown:
        //        //                    newBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //        //                    break;
        //        //                case MirrorVertical:
        //        //                    newBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
        //        //                    break;
        //        //                case MirrorHorizontalAndRotateRight:
        //        //                    newBitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
        //        //                    break;
        //        //                case RotateLeft:
        //        //                    newBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //        //                    break;
        //        //                case MirorHorizontalAndRotateLeft:
        //        //                    newBitmap.RotateFlip(RotateFlipType.Rotate270FlipX);
        //        //                    break;
        //        //                case RotateRight:
        //        //                    newBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
        //        //                    break;
        //        //                default:
        //        //                    throw new NotImplementedException("An orientation of " + orientation + " isn't implemented.");
        //        //            }
        //        //        }
        //        //                httpPostedFile.CopyTo(stream);
        //        //                newBitmap.Save(stream, image.RawFormat);
        //        //    }
        //        //}

        //        //    }

        //        var newHeight = 0;
        //        var newWidth = 0;
        //        //var target_area = orginalHeight * orginalWidth;
        //        var bmpBig = new Bitmap(SavePath);
        //        //var new_width = Math.Sqrt((orginalWidth / orginalHeight) * target_area);
        //        //var new_height = target_area / new_width;
        //        //int w = Convert.ToInt32(new_width); // round to the nearest integer
        //        //int h = Convert.ToInt32(new_height - (w - new_width)); // adjust the rounded width with height    
        //        //            ///
        //        //            decimal new_width = square_root((original_width / original_height) * target_area)
        //        //decimal new_height = target_area / new_width

        //        //int w = Math.round(new_width) // round to the nearest integer
        //        //int h = Math.round(new_height - (w - new_width)) // adjust the rounded width with height    

        //        //return (w, h)
        //        //                ///
        //        int maxHeight = orginalHeight / 5;
        //        int maxWidth = orginalWidth / 5;

        //        if (maxHeight > maxWidth)
        //        {
        //            newHeight = maxHeight;
        //            newWidth = Convert.ToInt16(orginalWidth * (maxHeight / Convert.ToDecimal(orginalHeight)));
        //        }
        //        else
        //        {
        //            newWidth = maxWidth;
        //            newHeight = Convert.ToInt16(orginalHeight * (maxWidth / Convert.ToDecimal(orginalWidth)));
        //        }
        //        Bitmap bmpBigNew;

        //        bmpBigNew = new Bitmap(newWidth, newHeight);



        //        Graphics canvasBig = Graphics.FromImage(bmpBigNew);
        //        canvasBig.DrawImage(bmpBig, new Rectangle(0, 0, bmpBigNew.Width, bmpBigNew.Height), 0, 0, bmpBig.Width, bmpBig.Height, GraphicsUnit.Pixel);
        //        bmpBig = bmpBigNew;
        //        var pictureSmall = await _attachmentService.InsertPicture(httpPostedFile.Length, "sm" + new_filename, contentType, fileExtension);
        //        // string newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/AttachmentFiles",
        //        // "sm" + new_filename);
        //        var smallimagename = "sm" + new_filename;
        //        string newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/AttachmentFiles",
        //        string.Format("{0}_{1}", pictureSmall.AttachmentId, smallimagename));
        //        bmpBig.SetPropertyItem(prop);
        //        bmpBig.RotateFlip(RotateFlipType.Rotate90FlipNone);
        //        bmpBig.Save(newPath, System.Drawing.Imaging.ImageFormat.Png);

        //        var url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
        //        //var url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

        //        //}

        //        return Json(new
        //        {
        //            success = true,
        //            id = pictureSmall.AttachmentId,
        //            imagePath = url + "/AttachmentFiles/" + string.Format("{0}_{1}", pictureSmall.AttachmentId, smallimagename),
        //            message = "",
        //        });
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

































        //[HttpPost]
        //public ActionResult FileUpload(string qqfile)
        //{
        //    var path = @"C:\\Temp\\100\\";
        //    var file = string.Empty;

        //    try
        //    {
        //        var stream = Request.InputStream;
        //        if (String.IsNullOrEmpty(Request["qqfile"]))
        //        {
        //            // IE
        //            HttpPostedFileBase postedFile = Request.Files[0];
        //            stream = postedFile.InputStream;
        //            file = Path.Combine(path, System.IO.Path.GetFileName(Request.Files[0].FileName));
        //        }
        //        else
        //        {
        //            //Webkit, Mozilla
        //            file = Path.Combine(path, qqfile);
        //        }

        //        var buffer = new byte[stream.Length];
        //        stream.Read(buffer, 0, buffer.Length);
        //        System.IO.File.WriteAllBytes(file, buffer);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message }, "application/json");
        //    }

        //    return Json(new { success = true }, "text/html");
        //}


        //[HttpPost]
        //public async Task<IActionResult> UploadMobile(IFormFile file)
        //{
        //    //Windows path
        //    var folderName = Path.Combine("Resources", "UploadFiles");
        //    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        //    //Linux path
        //    //var uploadLocation = Path.Combine(_env.WebRootPath, "Uploads//UsersImg");

        //    var fileName = file.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();

        //    if (file.Length > 0)
        //    {
        //        using (var stream = new FileStream(Path.Combine(pathToSave, fileName), FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //    }
        //    return Ok();
        //}
    }
                }