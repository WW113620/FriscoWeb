using FriscoDev.Application.Common;
using FriscoDev.Data.Page;
using FriscoDev.Data.Services;
using FriscoTab;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Test()
        {
            string name = "BIKE_LANE.T12";
            string fontFilename = "Font.txt";
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/fonts");
            var fontFile = Path.Combine(filePath, fontFilename);

            if (System.IO.File.Exists(fontFile))
            {
                textPageDisplay = new TextPageDisplay(fontFilename);
            }

            int displaySize = FriscoDev.Application.Interface.PacketProtocol.GetPMDDisplaySize(name);
            PMDInterface.PageTextFile pageFile = new PMDInterface.PageTextFile();
            string username = "lidar";
            var page = this._service.GetDisplayPagesByPageName(name, displaySize, 0, username);
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

                Bitmap[] images = textPageDisplay.getTextDisplayBitmap((PMDDisplaySize)displaySize,
                                         pageFile.line1, pageFile.line2,
                                         fontType, scrollType,
                                         (TextPageScrollStart)pageFile.scrollStart,
                                         (TextPageScrollEnd)pageFile.scrollEnd);

            }




            return View();
        }

    }
}