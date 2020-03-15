using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PMDInterface
{

    public enum TextPageScrollType
    {
        No_Scrolling = 0,
        Bottom_to_Top,
        Top_to_Bottom,
        Right_to_Left,
        Left_to_Right,
        Hybrid_Right_to_Top,
        Hybrid_Left_to_Top
    };

    public enum TextPageScrollStart
    {
        Full_Field = 0,
        Scroll_In
    };
    public enum TextPageScrollEnd
    {
        Full_Field = 0,
        Scroll_Out
    };

    public enum PMDDisplaySize
    {
        TwelveInchPMD = 12,
        FifteenInchPMD = 15,
        EighteenInchPMD = 18,
        AllSize
    };
    public enum PageType
    {
        Text = 0,
        Graphic = 1,
        Animation = 2,
        Sequence = 3,
        Composite = 4,
        Calendar = 5,
        Unknown
    };


    public class PageTextFile 
    {
        public PageType pageType = PageType.Text;

        public string selectedValue { get; set; }

        public string errorMsg = string.Empty;
        public PMDDisplaySize displayType { get; set; } = PMDDisplaySize.TwelveInchPMD;
        public Boolean isTxx { get; set; } = false;

        public string pageName { get; set; } = string.Empty;

        public string line1 { get; set; } = string.Empty;
        public string line2 { get; set; } = string.Empty;

        public TextPageScrollType scrollType { get; set; } = TextPageScrollType.No_Scrolling;
        public TextPageScrollStart scrollStart { get; set; } = TextPageScrollStart.Full_Field;
        public TextPageScrollEnd scrollEnd { get; set; } = TextPageScrollEnd.Full_Field;
        public byte startHold { get; set; } = 0;
        public byte endHold { get; set; } = 0;
        public byte framesPerPixel { get; set; } = 0;
        public byte scrollCyclesNumber { get; set; } = 0;


        public  Boolean fromString(string s)
        {

            #region
            string[] segList = Regex.Split(s, Environment.NewLine);
            string name = string.Empty, value = string.Empty;

            displayType = PMDDisplaySize.TwelveInchPMD;

            try
            {
                for (int i = 0; i < segList.Length; i++)
                {
                    if (!Utils.GetNameValue(segList[i], ref name, ref value))
                        continue;

                    if (name.Equals("Name"))
                        pageName = value;
                    else if (name.Equals("IsTxx"))
                        isTxx = Convert.ToBoolean(value);
                    else if (name.Equals("DisplayType"))
                        displayType = (PMDDisplaySize)Convert.ToInt16(value);
                    else if (name.Equals("ScrollType"))
                        scrollType = (TextPageScrollType)Enum.Parse(typeof(TextPageScrollType), value);
                    else if (name.Equals("ScrollStart"))
                        scrollStart = (TextPageScrollStart)Enum.Parse(typeof(TextPageScrollStart), value);
                    else if (name.Equals("ScrollEnd"))
                        scrollEnd = (TextPageScrollEnd)Enum.Parse(typeof(TextPageScrollEnd), value);
                    else if (name.Equals("StartHold"))
                        startHold = Convert.ToByte(value);
                    else if (name.Equals("EndHold"))
                        endHold = Convert.ToByte(value);
                    else if (name.Equals("FramesPerPixel"))
                        framesPerPixel = Convert.ToByte(value);
                    else if (name.Equals("ScrollCyclesNumber"))
                        scrollCyclesNumber = Convert.ToByte(value);
                    else if (name.Equals("Line1"))
                        line1 = value;
                    else if (name.Equals("Line2"))
                        line2 = value;

                    int maxNumChars = 254;

                    if (line1.Length > maxNumChars)
                        line1 = line1.Substring(0, maxNumChars);

                    if (line2.Length > maxNumChars)
                        line2 = line2.Substring(0, maxNumChars);
                }

                return true;
            }
            catch (Exception e)
            {
                errorMsg = "Exception = " + e.Message;
                return false;
            }
            #endregion
        }


        public string toString()
        {
            #region
            string s = string.Empty;

            s += ("Name=" + pageName + Environment.NewLine);
            s += ("DisplayType=" + (int)displayType + Environment.NewLine);
            s += ("IsTxx=" + isTxx.ToString() + Environment.NewLine);

            s += ("Line1=" + line1.ToString() + Environment.NewLine);

            if (scrollType == TextPageScrollType.No_Scrolling)
                s += ("Line2=" + line2.ToString() + Environment.NewLine);
            else
                s += ("Line2=" + Environment.NewLine);

            s += ("ScrollType=" + scrollType.ToString() + Environment.NewLine);
            s += ("ScrollStart=" + scrollStart.ToString() + Environment.NewLine);
            s += ("ScrollEnd=" + scrollEnd.ToString() + Environment.NewLine);
            s += ("StartHold=" + startHold + Environment.NewLine);
            s += ("EndHold=" + endHold + Environment.NewLine);
            s += ("FramesPerPixel=" + framesPerPixel + Environment.NewLine);
            s += ("ScrollCyclesNumber=" + scrollCyclesNumber);

            return s;
            #endregion
        }

        public string getFilename()
        {
            return pageName + PageTag.getFileExtension(pageType, displayType, isTxx);
        }

    }

    public class PageTag
    {
        public static string getFileExtension(PageType type,
                                             PMDDisplaySize displaySize = PMDDisplaySize.TwelveInchPMD,
                                             Boolean isTxx = false)
        {
            string appendix = string.Empty;
            string ext = string.Empty;

            if (displaySize == PMDDisplaySize.TwelveInchPMD)
                appendix = "12";
            else if (displaySize == PMDDisplaySize.FifteenInchPMD)
                appendix = "15";
            else if (displaySize == PMDDisplaySize.EighteenInchPMD)
                appendix = "18";

            if (type == PageType.Text)
            {
                ext = ".T" + appendix;

                if (isTxx)
                    ext = ".Txx";
            }
            else if (type == PageType.Graphic)
                ext = ".G" + appendix;
            else if (type == PageType.Animation)
                ext = ".A" + appendix;
            else if (type == PageType.Sequence)
                ext = ".O" + appendix;
            else if (type == PageType.Composite)
                ext = ".C" + appendix;
            else if (type == PageType.Calendar)
                ext = ".Jln";

            return ext;
        }



       
    }

}
