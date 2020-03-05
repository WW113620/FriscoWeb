using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FriscoDev.UI.Models;

namespace FriscoDev.UI.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        #region upload
        [HttpPost]
        public JsonResult UploadImage()
        {
            ResultEntity result = new ResultEntity() { errorCode = 500, errorStr = "" };
            if (HttpContext.Request.Files.Count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                string fileName = Path.GetFileName(file.FileName);
                string fileExtension = Path.GetExtension(file.FileName);
                Random rd = new Random();
                int num = rd.Next(100000, 1000000);
                string saveName = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), num, fileExtension);

                string savaFile = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Images");
                if (!Directory.Exists(savaFile))
                {
                    Directory.CreateDirectory(savaFile);
                }
                var filePath = Path.Combine(savaFile, saveName);
                file.SaveAs(filePath);
                result.errorCode = 200;
                result.errorStr = saveName;
            }
            else
            {
                result.errorCode = 100;
                result.errorStr = "Please select a picture to upload";
            }
            return Json(result);
        }
        #endregion
    }
}