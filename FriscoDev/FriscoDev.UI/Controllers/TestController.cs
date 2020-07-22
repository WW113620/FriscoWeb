using FriscoDev.Application.Common;
using FriscoDev.Data.Page;
using FriscoDev.Data.Services;
using FriscoTab;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace FriscoDev.UI.Controllers
{
    public class TestController : Controller
    {
        private readonly IPMGConfigurationService _service;
        private TextPageDisplay textPageDisplay = null;
        public TestController(IPMGConfigurationService service)
        {
            this._service = service;
        }

        public ActionResult Index(string to= "WW113620@163.com")
        {
            string errorMsg = "";
            string body = @"<div style='padding:10px;'>User Email: <span style='margin-left: 5px;font-size: 16px;'>test@163.com</span></div>
                              <div style='padding:10px;'>New Password:<span style='margin-left: 5px;font-size: 16px;'>test@163.com</span></div>";
            bool bo = SendMail.Send("stalkerradarsoftware@gmail.com", to, "Stalker Pole Mount Display Product Message Test", body, out errorMsg);
            return Content("Result=" + errorMsg);
        }



        public FileResult Test(string name = "BIKE_LANE.T12")
        {

            string fontFilename = "Font.txt";
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/fonts");
            fontFilename = Path.Combine(filePath, fontFilename);

            if (System.IO.File.Exists(fontFilename))
            {
                textPageDisplay = new TextPageDisplay(fontFilename);
            }

            int displaySize = FriscoDev.Application.Interface.PacketProtocol.GetPMDDisplaySize(name);
            PMDInterface.PageTextFile pageFile = new PMDInterface.PageTextFile();
            string username = "lidar";
            var page = this._service.GetDisplayPagesByPageName(name, displaySize, 0, username);

            Bitmap[] images = null;
            if (page != null)
            {
                Boolean status = pageFile.fromString(page.Content);

                TextPageScrollType scrollType = (TextPageScrollType)pageFile.scrollType;

                FontType fontType = FontType.Regular;

                if (displaySize == (int)PMDDisplaySize.EighteenInchPMD)
                {
                    if (scrollType != TextPageScrollType.No_Scrolling)
                        fontType = FontType.Large;
                }

                images = textPageDisplay.getTextDisplayBitmap((PMDDisplaySize)displaySize,
                                        pageFile.line1, pageFile.line2,
                                        fontType, scrollType,
                                        (TextPageScrollStart)pageFile.scrollStart,
                                        (TextPageScrollEnd)pageFile.scrollEnd, margin: 10);


                Bitmap bmp = images[0];

                var byData = Bitmap2Byte(bmp);

                return File(byData, "image/jpg");
            }

            return null;

        }


        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        public ActionResult Autocomplete()
        {
            return View();
        }


        public ActionResult ZipCode()
        {
            return View();
        }

    }
}