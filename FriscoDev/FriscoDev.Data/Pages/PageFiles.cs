using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using FriscoDev.Data.PMGDataPacketProtocol;
using FriscoDev.Application.ViewModels;

namespace FriscoTab
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
    public enum PMDDisplaySize
    {
        TwelveInchPMD = 12,
        FifteenInchPMD = 15,
        EighteenInchPMD = 18,
        AllSize
    };

    //public enum PageScope
    //{
    //    System = 0,
    //    User = 1,
    //    All = 2
    //};

    public enum ActionType
    {
        None = 0,
        ShowSpeed,
        ShowText,
        ShowGraphics,
        ShowAnimation,
        ShowTime,
        ShowTemperature,
        ShowComposite
    };

    public enum AlertType
    {
        None = 0,
        Blink_Display,
        Strobes,
        Blink_and_Strobes,
        Camera_Trigger,
        Aux_Out_1
    };

    public enum DisplayAlertType
    {
        ShowNone = 0x00,
        ShowSpeed = 0x01,
        ShowText = 0x02,
        ShowGraphics = 0x03,
        ShowAnimation = 0x04,
        ShowTime = 0x05,
        ShowTemperature = 0x06,

        No_Alert = 0x80,
        Blink_Display = 0x81,
        Strobes = 0x82,
        Blink_and_Strobes = 0x83,
        Camera_Trigger = 0x84,

        GPIO_Port_1 = 0x85,
        GPIO_Port_2 = 0x86,
        GPIO_Port_3 = 0x87,
        GPIO_Port_4 = 0x88
    };

    public enum SpeedUnit
    {
        MPH = 0,
        KPH,
        Knots,
        MPS,
        FPS,
    };

    public enum TabIds
    {
        QuickSetup = 1,
        Configuration = 2,
        Text = 4,
        Graphics = 8,
        Animations = 16,
        ScheduledOperations = 32,
        AllTabs = 0xFF,
    }

    public class PagePacketData
    {
        public int pmgId = 0;
        public int no = 0;
        public byte[] packetData = null;

        public PagePacketData(int noIn, byte[] packetDataIn)
        {
            no = noIn;
            packetData = packetDataIn;
        }
        public PagePacketData()
        {
        }
        public byte[] encode()
        {
            List<byte> byteList = new List<byte>();
            byte[] idData = BitConverter.GetBytes(pmgId);

            Utils.AddArrayToList(ref byteList, idData);

            byteList.Add((byte)(no & 0xFF));
            byteList.Add((byte)((no >> 8) & 0xFF));

            Utils.AddArrayToList(ref byteList, packetData);

            return byteList.ToArray();
        }

        public Boolean decode(byte[] data)
        {
            if (data == null || data.Length <= 6)
                return false;

            pmgId = BitConverter.ToInt32(data, 0);

            no = ((byte)data[4]) + (data[5] << 8);

            packetData = new byte[data.Length - 6];

            Buffer.BlockCopy(data, 6, packetData, 0, packetData.Length);

            return true;
        }

        public Boolean IsValid()
        {
            if (pmgId != 0 && no != 0 && packetData != null)
                return true;
            else
                return false;

        }

        public PMGCommandID GetCommandID()
        {
            if (packetData != null && packetData.Length >= 10)
                return (PMGCommandID)packetData[2];
            else
                return PMGCommandID.Unknown;
        }
    }
    class PageTagComparer : IEqualityComparer<PageTag>
    {
        public bool Equals(PageTag x, PageTag y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.isEqual(y);
        }
        public int GetHashCode(PageTag pageTag)
        {
            if (Object.ReferenceEquals(pageTag, null)) return 0;

            string filename = pageTag.getPageFilename();

            return filename.GetHashCode();
        }
    }

    public class PageTag
    {
        public static Font boldFontForTxxPageTag = null;
        public static string defaultUserDirectory = string.Empty;

        public PageType pageType = PageType.Text;
        public string name = string.Empty;
        public PMDDisplaySize displaySize = PMDDisplaySize.TwelveInchPMD;
        public Boolean isTxx = true;  // Only for TextPage
        public PageTag() { }
        public PageTag(string nameIn, PageType typeIn,
                      PMDDisplaySize displaySizeIn, Boolean isTxxIn = false)
        {
            name = nameIn;
            pageType = typeIn;
            displaySize = displaySizeIn;
            isTxx = isTxxIn;
        }

        // No directory
        public PageTag(string filename)
        {
            int num = 0;

            if (filename.Length > 0)
            {
                if (getPageInfoFromFilename(filename, ref pageType, ref displaySize, ref isTxx))
                {
                    // Or the page name will be empty
                    filename = Utils.RemoveSpecialSymbolFromFilename(filename, ref num);
                    name = Path.GetFileNameWithoutExtension(filename);
                }
            }
        }

        public string getPageFilename(Boolean withDirectory = true)
        {
            string pageDir = string.Empty;

            if (name == string.Empty)
                return string.Empty;

            pageDir = getPagesDirectory();

            string ext = string.Empty;
            string appendix = string.Empty;
            string pageFilename;

            ext = getFileExtension(pageType, displaySize);

            //if (withDirectory)
            //{
            //    // pageFilename = pageDir + "\\" + name + ext;

            //    //
            //    // PENDINGCASE: we add special symbol to address file system case sensitivity issue
            //    //              on Window file system
            //    //
            //    // Utils.GetCaseSensitiveFilename(pageFilename, ref pageFilename);

            //    // return pageFilename;

            //}
            //else

            pageFilename = name + ext;
            return pageFilename;
        }

        public Boolean IsFileExist()
        {
            string filename = getPageFilename();

            string filenameActual = string.Empty;
            string filenameNew = string.Empty;

            if (Utils.IsCaseSensitiveFileExist(filename, ref filenameActual, ref filenameNew))
                return true;
            else
                return false;
        }


        public Brush getTextBrush()
        {
            return Brushes.Black;
        }

        public Color getTextColor()
        {
            return Color.Black;
        }

        override public string ToString()
        {
            return name;
        }

        public string toString()
        {
            string s = pageType.ToString() + "," +
                       name + "," + isTxx + "," + (int)displaySize;
            return s;
        }

        public bool fromString(string s)
        {
            //
            // type,name,systemPageFlag,displaysize
            //
            string[] segList = s.Split(',');

            if (segList.Length == 4)
            {
                Enum.TryParse(segList[0], out pageType);
                name = segList[1];

                Boolean.TryParse(segList[2], out isTxx);
                displaySize = (PMDDisplaySize)Convert.ToInt16(segList[3]);
            }
            else
            {
                return false;
            }

            return true;
        }


 

        public static string getPagesDirectory()
        {
            //string rootDir = getDefaultUserDirectory();

            //if (!Directory.Exists(rootDir))
            //{
            //    Directory.CreateDirectory(rootDir);
            //}

            //string pageDir = Path.Combine(rootDir, "pages");

            //if (!Directory.Exists(pageDir))
            //{
            //    Directory.CreateDirectory(pageDir);
            //}

            ////string pmgsDir = getConnectedPMGPagesDirectory();
            ////if (!Directory.Exists(pmgsDir))
            ////{
            ////    Directory.CreateDirectory(pmgsDir);
            ////}

            //return pageDir;
            return "";
        }

        public static string getFirmwareDirectory()
        {
            string rootDir = getDefaultUserDirectory();
            string firmwareDir = Path.Combine(rootDir, "firmware");

            return firmwareDir;
        }

        public static string getuSDFilesDirectory()
        {
            string rootDir = getDefaultUserDirectory();
            string uSDFilesDir = Path.Combine(rootDir, "www");

            return uSDFilesDir;
        }

        public static string getOtherFilesDirectory()
        {
            string rootDir = getDefaultUserDirectory();
            string othersDir = Path.Combine(rootDir, "others");

            return othersDir;
        }
        public static string getDefaultUserDirectory()
        {
            //if (defaultUserDirectory == string.Empty)
            //{
            //    defaultUserDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //    defaultUserDirectory = Path.Combine(defaultUserDirectory, "PMG");
            //}

            return defaultUserDirectory;
        }

        public static string getConfigurationFilesDirectory()
        {
            string rootDir = getDefaultUserDirectory();
            string othersDir = Path.Combine(rootDir, "config");

            return othersDir;
        }

        public static Boolean IsValidPageName(string pageName, ref string reason)
        {
            if (pageName == null || pageName.Length == 0)
            {
                reason = "Name is empty!";
                return false;
            }

            if (pageName.Length > 27)
            {
                reason = "Name cannot exceeds 27 characters!\n" +
                         "Current name has " + pageName.Length + " characters.";
                return false;
            }

            int idx = pageName.IndexOfAny(Path.GetInvalidFileNameChars());

            if (idx < 0)
                idx = pageName.IndexOf('@');

            if (idx < 0)
                return true;
            else
            {
                reason = "Name cannot contain following characters:\n\n" +
                          "\\ / : * ? \"< > | @";
                return false;
            }
        }
        public Boolean isEqual(PageTag otherPage)
        {
            if (otherPage.pageType != PageType.Text)
            {
                if (otherPage.name.Equals(name) &&
                    otherPage.pageType == pageType &&
                    otherPage.displaySize == displaySize)
                    return true;
            }
            //
            // isTxx is only applicatible to Text Page
            //
            else if (otherPage.name.Equals(name) &&
                    otherPage.pageType == pageType &&
                    otherPage.displaySize == displaySize &&
                    otherPage.isTxx == isTxx)
            {
                return true;
            }

            return false;
        }


        public override bool Equals(Object obj)
        {
            if (obj == null || obj.GetType() != typeof(PageTag))
                return false;

            PageTag otherPage = (PageTag)obj;

            return isEqual(otherPage);
        }


   

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

      

        public static Boolean isPageFileRequired(Action_Type_Enum_t actionType)
        {
            if (actionType == Action_Type_Enum_t.Graphic ||
                actionType == Action_Type_Enum_t.Text ||
                actionType == Action_Type_Enum_t.Animation ||
                actionType == Action_Type_Enum_t.Composite)
                return true;
            else
                return false;
        }

        public static Boolean getPageInfoFromFilename(string filename,
                                                      ref PageType type,
                                                      ref PMDDisplaySize displaySize,
                                                      ref Boolean isTxx)
        {
            string ext = Path.GetExtension(filename);

            // Get Page Type
            if (ext.Contains(".T"))
                type = PageType.Text;
            else if (ext.Contains(".G"))
                type = PageType.Graphic;
            else if (ext.Contains(".A"))
                type = PageType.Animation;
            else if (ext.Contains(".O"))
                type = PageType.Sequence;
            else if (ext.Contains(".C"))
                type = PageType.Composite;
            else if (ext.Contains(".J"))
                type = PageType.Calendar;
            else
            {
                type = PageType.Unknown;
                return false;
            }

            if (ext.Contains("15"))
                displaySize = PMDDisplaySize.FifteenInchPMD;
            else if (ext.Contains("18"))
                displaySize = PMDDisplaySize.EighteenInchPMD;
            else
                displaySize = PMDDisplaySize.TwelveInchPMD;

            if (ext.Contains(".Txx"))
                isTxx = true;
            else
                isTxx = false;

            return true;
        }

        public static int getZeroIndexFromDisplaySize(PMDDisplaySize size)
        {
            int idx = 0;

            if (size == PMDDisplaySize.FifteenInchPMD)
                idx = 1;
            else if (size == PMDDisplaySize.EighteenInchPMD)
                idx = 2;

            return idx;
        }

        public static PMDDisplaySize getDisplaySizeFromZeroIndex(int idx)
        {
            if (idx == 1)
                return PMDDisplaySize.FifteenInchPMD;
            else if (idx == 2)
                return PMDDisplaySize.EighteenInchPMD;
            else
                return PMDDisplaySize.TwelveInchPMD;
        }

        public static byte[] encodePageFilename(string filename)
        {
            List<byte> byteList = new List<byte>();

            // PENDING838
            byteList.Add(1);
            byteList.Add((byte)(filename.Length + 1)); // file length
            byte[] data = Encoding.ASCII.GetBytes(filename);
            Util.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            return byteList.ToArray();
        }
    };

    public abstract class PageFile
    {
        public string errorMsg = string.Empty;
        public PageType pageType = PageType.Text;
        public PMDDisplaySize displayType = PMDDisplaySize.TwelveInchPMD;
        public Boolean isTxx = false;

        public string pageName = string.Empty;

        public int durationHours = 0;
        public int durationMinutes = 0;
        public int durationSeconds = 10;

        //public PageFile() { }
        public PageFile(string nameIn, PageType pageTypeIn,
                        PMDDisplaySize panelSize = PMDDisplaySize.TwelveInchPMD)
        {
            pageName = nameIn;
            pageType = pageTypeIn;
            displayType = panelSize;
        }

        public static PageFile CreatePageFile(PageTag pageTag)
        {
            PageFile page = null;

            if (pageTag.pageType == PageType.Text)
                page = new PageTextFile(pageTag.name, pageTag.displaySize, pageTag.isTxx);
            else if (pageTag.pageType == PageType.Graphic)
                page = new PageGraphicFile(pageTag.name, pageTag.displaySize);
            else if (pageTag.pageType == PageType.Animation)
                page = new PageAnimationFile(pageTag.name, pageTag.displaySize);
            else if (pageTag.pageType == PageType.Composite)
                page = new PageCompositeFile(pageTag.name, pageTag.displaySize);
            else if (pageTag.pageType == PageType.Calendar)
                page = new PageCalendarFile(pageTag.name);
            else
                return null;

            string filename = string.Empty;
            string filenameActual = string.Empty;
            string filenameNew = string.Empty;

            filename = pageTag.getPageFilename();

            if (!Utils.IsCaseSensitiveFileExist(filename, ref filenameActual, ref filenameNew))
            {
                return null;
            }

            if (!page.loadPage(filenameActual))
                return null;

            return page;
        }
        public string getFilename()
        {
            return pageName + PageTag.getFileExtension(pageType, displayType, isTxx);
        }

        public PageTag getPageTag()
        {
            PageTag pageTag = new PageTag(pageName, pageType, displayType, isTxx);
            return pageTag;
        }

        // With directory full path
        public string getPageFilename()
        {
            PageTag pageTag = getPageTag();

            return pageTag.getPageFilename();
        }

        // Convert into binary data to be sent out to PMD
        public abstract byte[] encode();
        public abstract Boolean decode(byte[] data);

        // Read write to and from file
        public abstract Boolean loadPage(string pageFilename);
        public abstract Boolean savePage(string pageFilename);

        public abstract Boolean fromString(string s);
        public abstract string toString();
        public abstract Boolean isEqual(PageFile otherPage);
        public virtual UInt16 getHashValue()
        {
            byte[] data = encode();

            if (data != null)
            {
                int len = data.Length;

                UInt16 hashValue = (UInt16)((data[0] << 8) + data[1]);
                return hashValue;
            }
            return 0;
        }
    }

    public class PageTextFile : PageFile
    {
        public string line1 = string.Empty;
        public string line2 = string.Empty;

        public TextPageScrollType scrollType = TextPageScrollType.No_Scrolling;
        public TextPageScrollStart scrollStart = TextPageScrollStart.Full_Field;
        public TextPageScrollEnd scrollEnd = TextPageScrollEnd.Full_Field;
        public byte startHold = 0;
        public byte endHold = 0;
        public byte framesPerPixel = 0;
        public byte scrollCyclesNumber = 0;
        public byte font = 1;

        public PageTextFile(string nameIn, PMDDisplaySize panelSize, Boolean isTxxIn = false) :
                            base(nameIn, PageType.Text, panelSize)
        {
            isTxx = isTxxIn;
        }

        public override Boolean fromString(string s)
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
                        Enum.TryParse(value, out scrollType);
                    else if (name.Equals("ScrollStart"))
                        Enum.TryParse(value, out scrollStart);
                    else if (name.Equals("ScrollEnd"))
                        Enum.TryParse(value, out scrollEnd);
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

                    //
                    //
                    // Per Request (May 29, 2019), the text size limitation is removed
                    // 
                    // If there is no scrolling, limit chars length to 6 
                    // for 12 in and 8 for 15 and 18 inch PMG
                    //
                    //if (scrollType == TextPageScrollType.No_Scrolling)
                    //{
                    //    int maxNumChars = 6;

                    //    if (displayType == PMDDisplaySize.FifteenInchPMD ||
                    //        displayType == PMDDisplaySize.EighteenInchPMD)
                    //        maxNumChars = 8;

                    //    if (line1.Length > maxNumChars)
                    //        line1 = line1.Substring(0, maxNumChars);

                    //    if (line2.Length > maxNumChars)
                    //        line2 = line2.Substring(0, maxNumChars);
                    //}
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

        public override string toString()
        {
            #region
            string s = string.Empty;

            s += ("Name=" + pageName + Environment.NewLine);
            s += ("DisplayType=" + (int)displayType + Environment.NewLine);
            s += ("IsTxx=" + isTxx.ToString() + Environment.NewLine);

            s += ("Line1=" + line1.ToString() + Environment.NewLine);

            //
            // 2nd line is empty if we are scrolling
            //
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

        public override byte[] encode()
        {
            List<byte> byteList = new List<byte>();


            // Filename
            string filename = pageName + PageTag.getFileExtension(pageType, displayType, isTxx);

            byteList.Add((byte)(filename.Length + 1));
            byte[] data = Encoding.ASCII.GetBytes(filename);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            // Font
            byteList.Add(font);

            //
            // Only left to right and right to left scrolling is supported on PMG side
            //
            if (scrollType == TextPageScrollType.Bottom_to_Top ||
                scrollType == TextPageScrollType.Top_to_Bottom ||
                scrollType == TextPageScrollType.Hybrid_Right_to_Top ||
                scrollType == TextPageScrollType.Hybrid_Left_to_Top)
                scrollType = TextPageScrollType.No_Scrolling;

            // Scroll Type, Scroll Start, Scroll End, Start Hold to one byte
            int scrolldata = (byte)scrollType + ((byte)scrollStart << 3) +
                            ((byte)scrollEnd << 4) + (startHold << 5);

            byteList.Add((byte)scrolldata);

            // Additonal Scroll Data
            scrolldata = endHold + (framesPerPixel << 4);
            byteList.Add((byte)scrolldata);

            // Number Of lines
            byte numLines = 0;

            if (line1.Length > 0)
                numLines++;
            if (line2.Length > 0)
                numLines++;
            byteList.Add(numLines);

            //
            // Number Of Scroll Cycles
            //
            byteList.Add(scrollCyclesNumber);

            if (line1.Length > 0)
            {
                byteList.Add((byte)(line1.Length + 1));
                data = Encoding.ASCII.GetBytes(line1);
                Utils.AddArrayToList(ref byteList, data);
                byteList.Add(0);
            }

            if (line2.Length > 0)
            {
                byteList.Add((byte)(line2.Length + 1));
                data = Encoding.ASCII.GetBytes(line2);
                Utils.AddArrayToList(ref byteList, data);
                byteList.Add(0);
            }

            // Calculate Hash
            byte[] payload = byteList.ToArray();

            UInt16 hashValue = Util.U16ComputeCRC(payload, 0, payload.Length);

            // If payload length is not even, we add padding 0 to hash calculation
            if (payload.Length % 2 != 0)
            {
                hashValue = Util.U16ComputeCRC(hashValue, (byte)0);
            }

            byteList.Insert(0, (byte)((hashValue >> 8) & 0xFF));
            byteList.Insert(0, (byte)(hashValue & 0xFF));

            return byteList.ToArray();
        }

        public override Boolean decode(byte[] data)
        {
            int idx = 0;
            UInt16 hashValue = 0;

            hashValue = (UInt16)(data[idx] + (data[idx + 1] << 8));
            idx += 2;

            //
            // Filename Length + Filename --> Panel Size and Page Name
            //
            int len = data[idx++];
            byte[] filenameData = Util.GetByteArray(data, ref idx, len);

            string filename = Util.GetFilename(filenameData);

            if (!PageTag.getPageInfoFromFilename(filename,
                                                ref pageType,
                                                ref displayType, ref isTxx))
            {
                errorMsg = "Wrong page filename format!";
                return false;
            }

            pageName = Path.GetFileNameWithoutExtension(filename);

            // Font
            font = data[idx++];

            //
            // Scroll Type, Scroll Start, Scroll End, start Hold
            //
            byte scrolldata = data[idx++];

            scrollType = (TextPageScrollType)(0x07 & scrolldata);
            scrollStart = (TextPageScrollStart)((0x08 & scrolldata) >> 3);
            scrollEnd = (TextPageScrollEnd)((0x10 & scrolldata) >> 4);
            startHold = (byte)((0xE0 & scrolldata) >> 5);

            scrolldata = data[idx++];
            endHold = (byte)(0x0F & scrolldata);
            framesPerPixel = (byte)((0xF0 & scrolldata) >> 4);

            // Number Of Lines
            byte lineNum = data[idx++];
            byte lineLength;
            byte[] lineData;

            // 0xFF --> Continuous
            scrollCyclesNumber = data[idx++];
            if (scrollCyclesNumber == 0xFF)
                scrollCyclesNumber = 0;

            // 1st Line
            if (lineNum > 0)
            {
                lineLength = data[idx++];
                lineData = Util.GetByteArray(data, ref idx, lineLength);
                line1 = Util.GetAsciiString(lineData);
            }

            // 2nd Line
            if (lineNum > 1)
            {
                lineLength = data[idx++];
                lineData = Util.GetByteArray(data, ref idx, lineLength);
                line2 = Util.GetAsciiString(lineData);
            }

            //// 
            //byte[] data2 = encode();

            //LogForm.LogMessage("Original Text Page Encoded: \n" +
            //                   Utils.ByteArrayToHexString(data, 0, data.Length) + "\n Newly Encoded:\n" +
            //                   Utils.ByteArrayToHexString(data2, 0, data2.Length),
            //                    Color.Red, true, LogForm.Direction.Internal, 0);

            return true;
        }
        public override Boolean isEqual(PageFile otherPage)
        {
            if (otherPage == null ||
                otherPage.GetType() != typeof(PageTextFile))
                return false;

            PageTextFile other = (PageTextFile)otherPage;

            if (!line1.Equals(other.line1) ||
                !line2.Equals(other.line2) ||
                scrollType != other.scrollType ||
                scrollStart != other.scrollStart ||
                scrollEnd != other.scrollEnd ||
                startHold != other.startHold ||
                endHold != other.endHold ||
                framesPerPixel != other.framesPerPixel ||
                scrollCyclesNumber != other.scrollCyclesNumber)
                return false;
            else
                return true;
        }

        public override Boolean loadPage(string pageFilename)
        {
            if (!File.Exists(pageFilename))
                return false;

            try
            {
                string readText = File.ReadAllText(pageFilename);
                Boolean status = fromString(readText);

                //
                // This is to avoid case when user manually change the page filename
                // So, the page name will come from the filename
                //
                if (status)
                    pageName = Path.GetFileNameWithoutExtension(pageFilename);

                int idx = pageName.IndexOf('@');
                if (idx != -1)
                    pageName = pageName.Remove(idx);

                return status;
            }
            catch (Exception e)
            {
                errorMsg = "Load page failure :" + e.Message;
                return false;
            }
        }

        public override Boolean savePage(string pageFilename)
        {
            try
            {
                if (pageFilename == string.Empty)
                {
                    pageFilename = PageTag.getPagesDirectory() + "\\" + pageName +
                                   PageTag.getFileExtension(PageType.Text, displayType);

                    Utils.GetCaseSensitiveFilename(pageFilename, ref pageFilename);
                }

                using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(pageFilename))
                {
                    // Write Page Duration

                    string s = toString();

                    file.Write(s);

                    file.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                errorMsg = "Fail to save page! " + e.Message;
                return false;
            }
        }
    }
    public class PageGraphicFile : PageFile
    {
        public byte[,] mBitmapData = null;
        public PageGraphicFile(string nameIn, PMDDisplaySize panelSize) :
                               base(nameIn, PageType.Graphic, panelSize)
        {
            if (panelSize == PMDDisplaySize.EighteenInchPMD)
                mBitmapData = new byte[48, 31];

            else if (panelSize == PMDDisplaySize.FifteenInchPMD)
                mBitmapData = new byte[42, 26];
            else
                mBitmapData = new byte[36, 21];
        }

        public override byte[] encode()
        {
            List<byte> byteList = new List<byte>();

            // Filename
            string filename = getFilename();

            byteList.Add((byte)(filename.Length + 1));
            byte[] data = Encoding.ASCII.GetBytes(filename);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            //
            // Packed Image Data
            //
            byte[] imageData = Util.ConvertBitmapDataInOneDimentionArray(mBitmapData);
            Utils.AddArrayToList(ref byteList, imageData);

            // Calculate Hash
            byte[] payload = byteList.ToArray();
            UInt16 hashValue = Util.U16ComputeCRC(payload, 0, payload.Length);

            // If payload length is not even, we add padding 0 to hash calculation
            if (payload.Length % 2 != 0)
            {
                hashValue = Util.U16ComputeCRC(hashValue, (byte)0);
            }

            byteList.Insert(0, (byte)((hashValue >> 8) & 0xFF));
            byteList.Insert(0, (byte)(hashValue & 0xFF));

            return byteList.ToArray();
        }

        public override Boolean decode(byte[] data)
        {
            int idx = 0;

            UInt16 hashValue = 0;

            hashValue = (UInt16)(data[idx] + (data[idx + 1] << 8));
            idx += 2;

            // Filename Length + Filename --> Panel Size and Page Name
            //
            int len = data[idx++];
            byte[] filenameData = Util.GetByteArray(data, ref idx, len);

            string filename = Util.GetFilename(filenameData);

            Boolean isTxxDummy = false; // Only for Text Page

            if (!PageTag.getPageInfoFromFilename(filename,
                                                ref pageType,
                                                ref displayType,
                                                ref isTxxDummy))
            {
                errorMsg = "Wrong page filename format!";
                return false;
            }

            pageName = Path.GetFileNameWithoutExtension(filename);

            // Given the panel size, we should now know the packed image data size
            int packedImageDataLen = 0;

            if (displayType == PMDDisplaySize.EighteenInchPMD)
            {
                packedImageDataLen = 186;
                mBitmapData = new byte[48, 31];
            }

            else if (displayType == PMDDisplaySize.FifteenInchPMD)
            {
                packedImageDataLen = 137;
                mBitmapData = new byte[42, 26];
            }
            else
            {
                packedImageDataLen = 95;
                mBitmapData = new byte[36, 21];
            }

            byte[] imageData = Util.GetByteArray(data, ref idx, packedImageDataLen);

            if (!Util.ConvertFromOneDimentionArrayToBitmapData(imageData, ref mBitmapData))
            {
                errorMsg = "Fail to read packed graphic data!";
                return false;
            }

            // 
            //byte[] data2 = encode();
            //
            //LogForm.LogMessage("Original Graphic Page Encoded: \n" +
            //                   Utils.ByteArrayToHexString(data, 0, data.Length) + "\n Newly Encoded:\n" +
            //                   Utils.ByteArrayToHexString(data2, 0, data2.Length),
            //                   Color.Red, true, LogForm.Direction.Internal, 0);


            return true;
        }

        public override Boolean isEqual(PageFile otherPage)
        {
            if (otherPage == null ||
                otherPage.GetType() != typeof(PageGraphicFile))
                return false;

            PageGraphicFile other = (PageGraphicFile)otherPage;

            if (mBitmapData == null || other.mBitmapData == null)
                return false;

            if (mBitmapData.GetLength(0) != other.mBitmapData.GetLength(0) ||
               (mBitmapData.GetLength(1) != other.mBitmapData.GetLength(1)))
                return false;

            int w = mBitmapData.GetLength(0);
            int h = mBitmapData.GetLength(1);

            // Compare bitmap data
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (mBitmapData[x, y] != other.mBitmapData[x, y])
                        return false;
                }
            }

            return true;
        }

        public Boolean isEqual(byte[,] otherBitmap)
        {
            if (mBitmapData.GetLength(0) != mBitmapData.GetLength(0) ||
               (mBitmapData.GetLength(1) != otherBitmap.GetLength(1)))
                return false;

            int w = mBitmapData.GetLength(0);
            int h = mBitmapData.GetLength(1);

            // Compare bitmap data
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (mBitmapData[x, y] != otherBitmap[x, y])
                        return false;
                }
            }

            return true;
        }

        public override Boolean savePage(string pageFilename)
        {
            int w = mBitmapData.GetLength(0);
            int h = mBitmapData.GetLength(1);

            Bitmap bit = new Bitmap(w, h, PixelFormat.Format1bppIndexed);
            int x, y;

            BitmapData bmpData = bit.LockBits(new Rectangle(0, 0, w, h),
                        System.Drawing.Imaging.ImageLockMode.ReadWrite, bit.PixelFormat);

            if (pageFilename == string.Empty)
            {
                pageFilename = PageTag.getPagesDirectory() + "\\" + pageName +
                                PageTag.getFileExtension(PageType.Graphic, displayType);

                Utils.GetCaseSensitiveFilename(pageFilename, ref pageFilename);
            }

            for (y = 0; y < h; y++)
            {
                for (x = 0; x < w; x++)
                {
                    if (mBitmapData[x, y] == 0)
                    {
                        //Utils.SetIndexedPixel(x, y, bmpData, true);
                    }
                }
            }

            try
            {
                bit.Save(pageFilename);
            }
            catch (Exception ex)
            {
                errorMsg = "Error Detected! " + ex.Message;
                return false;
            }

            return true;
        }

        public override Boolean loadPage(string pageFilename)
        {

            if (!File.Exists(pageFilename))
                return false;

            displayType = PMDDisplaySize.TwelveInchPMD;

            Bitmap bit = new Bitmap(pageFilename);

            if ((bit.Width == 36 && bit.Height == 21) ||
                (bit.Width == 42 && bit.Height == 26) ||
                (bit.Width == 48 && bit.Height == 31))
            {
                if (bit.PixelFormat != PixelFormat.Format1bppIndexed)
                {
                    errorMsg = "Fail to load page " + pageFilename + ", only bitmap depth of 1 is supported!";
                    bit.Dispose();

                    return false;
                }

                if (bit.Width == 36)
                    displayType = PMDDisplaySize.TwelveInchPMD;
                else if (bit.Width == 42)
                    displayType = PMDDisplaySize.FifteenInchPMD;
                else
                    displayType = PMDDisplaySize.EighteenInchPMD;

                mBitmapData = Utils.GetBitmapDataFromBitmap(bit);

                bit.Dispose();

                return true;
            }
            else
            {
                String currentSize = bit.Width + "x" + bit.Height;
                errorMsg = "Bitmap dimention is " + currentSize + "! It needs to be either 36x21, 42x26 or 48x31!";
                bit.Dispose();
                return false;
            }
        }

        public override Boolean fromString(string s)
        {
            string[] segList = Regex.Split(s, Environment.NewLine);
            string name = string.Empty, value = string.Empty;
            int lineIdx = 0;
            int w = 0;
            int h = 0;

            displayType = PMDDisplaySize.TwelveInchPMD;

            for (int i = 0; i < segList.Length; i++)
            {
                if (!Utils.GetNameValue(segList[i], ref name, ref value))
                    continue;

                if (name.Equals("Name"))
                    pageName = value;
                else if (name.Equals("Size"))
                {
                    string[] dims = value.Split(',');

                    Int32.TryParse(dims[0], out w);
                    Int32.TryParse(dims[1], out h);

                    if (w == 36 && h == 21)
                        displayType = PMDDisplaySize.TwelveInchPMD;
                    else if (w == 42 && h == 26)
                        displayType = PMDDisplaySize.FifteenInchPMD;
                    else if (w == 48 && h == 31)
                        displayType = PMDDisplaySize.EighteenInchPMD;
                    else
                    {
                        errorMsg = "Wrong bitmap size! \nOnly 36x21, 42x26 or 48x31 are supported!";
                        return false;
                    }

                    mBitmapData = new byte[w, h];

                    //
                    // Now we read in the bitmap data
                    //
                    lineIdx = 0;

                    for (int idx = i; idx < segList.Length; idx++)
                    {
                        if (segList[idx].Length < 30)
                            continue;

                        string str = segList[idx].Trim();
                        char[] arr = str.ToCharArray();

                        for (int k = 0; k < arr.Length; k++)
                        {
                            if (arr[k] == 'o')
                                mBitmapData[k, lineIdx] = 1;
                        }

                        lineIdx++;
                    }
                }
            }

            return true;
        }
        public override string toString()
        {
            if (mBitmapData == null)
                return string.Empty;

            string content = string.Empty;

            int w = mBitmapData.GetLength(0);
            int h = mBitmapData.GetLength(1);
            int x, y;

            content += ("Name=" + pageName + Environment.NewLine);
            content += ("Size=" + w + "," + h + Environment.NewLine);
            content += ("DisplayType=" + (int)displayType + Environment.NewLine);

            string s;

            for (y = 0; y < h; y++)
            {
                s = string.Empty;

                for (x = 0; x < w; x++)
                {
                    if (mBitmapData[x, y] == 1)
                        s += "o";
                    else
                        s += ".";
                }

                content += s;

                if (y != h - 1)
                    content += Environment.NewLine;
            }

            return content;
        }
    }

    public class PageAnimationFile : PageFile
    {
        public int framesPerCell = 1;

        public List<PageTag> pageList = new List<PageTag>();
        public PageAnimationFile(string nameIn, PMDDisplaySize panelSize) :
                                base(nameIn, PageType.Animation, panelSize)
        {

        }

        public override byte[] encode()
        {
            // We use getPacketData() for animation page
            errorMsg = "PageAnimationFile: encode, not yet implemented!";
            return null;
        }

        public override Boolean decode(byte[] data)
        {
            errorMsg = "PageAnimationFile: decode not yet implemented!";
            return false;
        }

        public override Boolean isEqual(PageFile otherPage)
        {
            if (otherPage == null ||
                otherPage.GetType() != typeof(PageAnimationFile))
                return false;

            PageAnimationFile other = (PageAnimationFile)otherPage;

            if (pageList.Count != other.pageList.Count)
                return false;

            if (framesPerCell != other.framesPerCell)
                return false;

            for (int i = 0; i < pageList.Count; i++)
            {
                if (!pageList[i].isEqual(other.pageList[i]))
                    return false;
            }

            return true;
        }

        public override UInt16 getHashValue()
        {
            if (pageList.Count == 0)
                return 0;

            string filename = getFilename();
            List<byte> byteList = new List<byte>();
            int i;

            // Page name with nul term
            byteList.Add((byte)(filename.Length + 1));
            byte[] data = Encoding.ASCII.GetBytes(filename);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            // Frames Per Cell
            byteList.Add((byte)framesPerCell);

            // Number Of Cells
            byteList.Add((byte)pageList.Count);

            for (i = 0; i < pageList.Count; i++)
            {
                PageTag pageTag = pageList[i];

                PageGraphicFile grahicPageFile = new PageGraphicFile(pageTag.name, pageTag.displaySize);
                string pageFilename = pageTag.getPageFilename();

                if (!grahicPageFile.loadPage(pageFilename))
                    continue;

                byte[] imageData =
                    Util.ConvertBitmapDataInOneDimentionArray(grahicPageFile.mBitmapData);

                Utils.AddArrayToList(ref byteList, imageData);
            }

            if (byteList.Count % 2 != 0)
                byteList.Add(0);

            byte[] payload = byteList.ToArray();
            UInt16 hashValue = Util.U16ComputeCRC(payload, 0, payload.Length);

            return hashValue;
        }

        // 
        // Retrieve how many packets needed 
        // (This is not used temporary. In the release, we use one packet for each graphic)
        //
        public void getPacketInfo(ref int totalPacketNumber, ref int numCellsInEachPacket,
                                  ref int numCellsInLastPacket)
        {
            //
            // 4 for 12 Inch (95 Bytes Each Cell)
            // 3 for 15 Inch (137 Bytes Each Cell)
            // 2 for 18 Inch (186 Bytes Each Cell)
            //
            if (displayType == PMDDisplaySize.TwelveInchPMD)
                numCellsInEachPacket = 4;
            else if (displayType == PMDDisplaySize.FifteenInchPMD)
                numCellsInEachPacket = 3;
            else
                numCellsInEachPacket = 2;

            int numCellsInProceedingPacket = pageList.Count / numCellsInEachPacket;

            numCellsInLastPacket = pageList.Count - (numCellsInProceedingPacket * numCellsInEachPacket);

            if (numCellsInLastPacket != 0)
                totalPacketNumber = numCellsInProceedingPacket + 1;
            else
                totalPacketNumber = numCellsInProceedingPacket;
        }

        public byte[] getPacketData(int packetNumber, ref string errorMsg)
        {
            int totalPacketNumber = pageList.Count;

            if (packetNumber > totalPacketNumber)
            {
                errorMsg = "Packet number cannot exceed Total Packet Number";
                return null;
            }

            List<byte> byteList = new List<byte>();
            byte[] data;

            // Encode 1st Packet
            if (packetNumber == 1)
            {
                string filename = getFilename();

                // Two Hash bytes
                UInt16 hashValue = getHashValue();

                byteList.Add((byte)(hashValue & 0xFF));
                byteList.Add((byte)((hashValue >> 8) & 0xFF));

                // Page name with nul term
                byteList.Add((byte)(filename.Length + 1));
                data = Encoding.ASCII.GetBytes(filename);
                Utils.AddArrayToList(ref byteList, data);
                byteList.Add(0);

                // Frames Per Cell 
                byteList.Add((byte)framesPerCell);

                // Number Of Cells
                byteList.Add((byte)totalPacketNumber);
            }

            PageTag pageTag = pageList[packetNumber - 1];

            PageGraphicFile grahicPageFile = new PageGraphicFile(pageTag.name, pageTag.displaySize);
            string pageFilename = pageTag.getPageFilename();

            if (!grahicPageFile.loadPage(pageFilename))
            {
                errorMsg = grahicPageFile.errorMsg;
                return null;
            }

            byte[] imageData =
                Util.ConvertBitmapDataInOneDimentionArray(grahicPageFile.mBitmapData);

            Utils.AddArrayToList(ref byteList, imageData);

            return byteList.ToArray();
        }

        public override Boolean loadPage(string pageFilename)
        {
            if (!File.Exists(pageFilename))
                return false;

            try
            {
                string readText = File.ReadAllText(pageFilename);

                return fromString(readText);
            }
            catch (Exception e)
            {
                errorMsg = "Load page failure :" + e.Message;
                return false;
            }
        }

        public override Boolean savePage(string pageFilename)
        {
            try
            {
                if (pageFilename == string.Empty)
                {
                    pageFilename = PageTag.getPagesDirectory() + "\\" + pageName +
                                   PageTag.getFileExtension(PageType.Animation, displayType);

                    Utils.GetCaseSensitiveFilename(pageFilename, ref pageFilename);
                }

                using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(pageFilename))
                {
                    // Write Page Duration

                    string s = toString();

                    file.Write(s);
                }

                return true;
            }
            catch (Exception e)
            {
                errorMsg = "Fail to save page! " + e.Message;
                return false;
            }
        }

        public override Boolean fromString(string s)
        {
            string[] segList = Regex.Split(s, Environment.NewLine);

            string name = string.Empty, value = string.Empty;

            pageList.Clear();

            for (int i = 0; i < segList.Length; i++)
            {
                if (!Utils.GetNameValue(segList[i], ref name, ref value))
                    continue;

                if (name.Equals("Name"))
                    pageName = value;
                else if (name.Equals("DisplayType"))
                    displayType = (PMDDisplaySize)Convert.ToInt16(value);
                else if (name.Equals("FramesPerCell"))
                    framesPerCell = Convert.ToInt16(value);
                else if (name.Equals("PageName"))
                {
                    string[] values = value.Split(',');

                    if (values.Length >= 1)
                    {
                        PageTag page = new PageTag();

                        page.name = values[0];
                        page.pageType = PageType.Graphic;
                        page.displaySize = displayType;

                        //
                        // We check if the graphic exists
                        //
                        if (!File.Exists(page.getPageFilename()))
                            continue;

                        // We limit it to max total 127 Graphic Pages
                        if (pageList.Count < 127)
                            pageList.Add(page);
                    }
                }
            }

            return true;
        }
        public override string toString()
        {
            string content;

            content = ("Name=" + pageName + Environment.NewLine);
            content += ("DisplayType=" + (int)displayType + Environment.NewLine);
            content += ("FramesPerCell=" + framesPerCell + Environment.NewLine);

            //
            // Max number of graphic page is 127. Limited added by Mike
            //
            for (int i = 0; i < pageList.Count && i < 127; i++)
            {
                content += ("PageName=" + pageList[i].name);

                if (i != pageList.Count - 1)
                    content += Environment.NewLine;
            }

            return content;
        }
    }

    public class CompositeSequence
    {
        public int idx = 1;
        public float startTime = 0;
        public float duration = 2;
        public DisplayAlertType displayAlertType = DisplayAlertType.ShowText;
        public string filename = string.Empty;

        public string errorMsg = string.Empty;

        // Used for concurrent type
        public Boolean startTimeCalculated = true;

        // This is used internally for sorting
        // public float actualStartTimeDuringRuntime = 0;
        public CompositeSequence()
        {
        }

        public CompositeSequence(CompositeSequence seq)
        {
            startTime = seq.startTime;
            duration = seq.duration;
            displayAlertType = seq.displayAlertType;
            filename = seq.filename;
            startTimeCalculated = seq.startTimeCalculated;
        }

        public string toString()
        {
            string s = startTime.ToString("0.0") + "," +
                       duration.ToString("0.0") + "," +
                       displayAlertType.ToString() + "," +
                       filename + "," + startTimeCalculated.ToString();
            return s;
        }

        public Boolean fromString(string s)
        {
            #region
            string[] segList = s.Split(',');

            if (segList.Length < 4)
            {
                errorMsg = "Wrong format of Composite Sequence: " + s;
                return false;
            }

            try
            {
                startTime = Convert.ToSingle(segList[0]);
                duration = Convert.ToSingle(segList[1]);

                Enum.TryParse(segList[2], out displayAlertType);

                filename = segList[3];

                if (segList.Length > 4)
                    startTimeCalculated = Convert.ToBoolean(segList[4]);

                // Do some sanity checking
                if (displayAlertType == DisplayAlertType.Blink_and_Strobes ||
                    displayAlertType == DisplayAlertType.Strobes)
                    duration = (float)1.0;
                else if (displayAlertType == DisplayAlertType.Camera_Trigger)
                    duration = (float)2.4;

            }
            catch (Exception e)
            {
                errorMsg = "Exception = " + e.Message;
                return false;
            }

            return true;

            #endregion
        }

        public byte[] encode()
        {
            float val2;
            int val;

            List<byte> byteList = new List<byte>();

            //
            // Start Time:
            //
            //      Tenths of Seconds from beginning of sequence. 
            //      Setting this to 0 for non - concurrent operations
            //      will cause the start time to be calculated based on the duration 
            //      of the previous operation.
            //
            val2 = startTime * 10;
            val = (int)val2;

            byteList.Add((byte)(val & 0xff));
            byteList.Add((byte)((val >> 8) & 0xff));
            byteList.Add((byte)((val >> 16) & 0xff));
            byteList.Add((byte)((val >> 24) & 0xff));

            // Duration
            val2 = duration * 10;
            val = (int)val2;

            byteList.Add((byte)(val & 0xff));
            byteList.Add((byte)((val >> 8) & 0xff));

            byte current = 0;

            current = (byte)displayAlertType;

            //if (displayAlertType >= DisplayAlertType.ShowNone &&
            //    displayAlertType <= DisplayAlertType.ShowTemperature)
            //{
            //    current = (byte)displayAlertType;
            //}
            //else
            //{          
            //   current = (byte)(displayAlertType - DisplayAlertType.No_Alert);
            //   current += 0x08;
            //}

            byteList.Add(current);

            // Add page filename       
            byteList.Add((byte)(filename.Length + 1));

            if (filename.Length > 0)
            {
                byte[] data = Encoding.ASCII.GetBytes(filename);
                Utils.AddArrayToList(ref byteList, data);
            }

            byteList.Add(0);

            return byteList.ToArray();
        }

        public int decode(byte[] data, int idx)
        {
            // Start Time
            int val = data[idx] + (data[idx + 1] << 8) + (data[idx + 2] << 16) + (data[idx + 3] << 24);
            idx += 4;
            startTime = (float)val / 10;

            // Duration
            val = data[idx] + (data[idx + 1] << 8);
            idx += 2;
            duration = (float)val / 10;

            displayAlertType = (DisplayAlertType)data[idx];
            idx++;

            // Filename Length + Filename --> Panel Size and Page Name
            //
            int len = data[idx++];
            byte[] filenameData = Util.GetByteArray(data, ref idx, len);

            filename = Util.GetFilename(filenameData);

            return idx;
        }


        // 
        // This function will decode a block of sequences data. If we have successfully
        // decode number of sequences indicated in numSequences, then this function will
        // return true.
        //
        static public Boolean decodeSequenceBlock(byte[] data, int startIndex, int numSequences,
                                                  ref List<CompositeSequence> seqs)
        {
            int i;
            int idx = startIndex;

            for (i = 0; i < numSequences; i++)
            {

                // We should have at least min 6 bytes for a sequence
                if (idx + 6 > data.Length)
                    return false;

                CompositeSequence current = new CompositeSequence();

                idx = current.decode(data, idx);

                if (current.isValid())
                {
                    seqs.Add(current);
                }
            }

            return true;
        }

        public PageTag getReferencedPage()
        {
            if (filename == string.Empty)
                return null;

            if (displayAlertType == DisplayAlertType.ShowText ||
                displayAlertType == DisplayAlertType.ShowGraphics ||
                displayAlertType == DisplayAlertType.ShowAnimation)
            {
                PageTag pageTag = new PageTag(filename);

                return pageTag;
            }

            return null;
        }

        public string[] getDisplayColumnString()
        {
            string[] cols = new string[5];

            cols[0] = "0";

            if (startTime > 0)
                cols[1] = startTime.ToString("0.0");
            else
                cols[1] = "Dependant";

            cols[2] = duration.ToString("0.0");
            cols[3] = convertFromDisplayAlertTypeToString(displayAlertType);
            cols[4] = filename;

            return cols;
        }

        public static string convertFromDisplayAlertTypeToString(DisplayAlertType type)
        {
            switch (type)
            {
                case DisplayAlertType.ShowNone:
                    return "Display None";
                case DisplayAlertType.ShowSpeed:
                    return "Display Speed";
                case DisplayAlertType.ShowText:
                    return "Display Text";
                case DisplayAlertType.ShowGraphics:
                    return "Display Graphics";
                case DisplayAlertType.ShowAnimation:
                    return "Display Animation";
                case DisplayAlertType.ShowTime:
                    return "Display Time";
                case DisplayAlertType.ShowTemperature:
                    return "Display Temperature";
                case DisplayAlertType.No_Alert:
                    return "No Alert";
                case DisplayAlertType.Blink_Display:
                    return "Flash Display";
                case DisplayAlertType.Strobes:
                    return "Strobes";
                case DisplayAlertType.Blink_and_Strobes:
                    return "Flash + Strobes";
                case DisplayAlertType.Camera_Trigger:
                    return "Camera Trigger";
                case DisplayAlertType.GPIO_Port_1:
                    return "GPIO Port 1";
                case DisplayAlertType.GPIO_Port_2:
                    return "GPIO Port 2";
                case DisplayAlertType.GPIO_Port_3:
                    return "GPIO Port 3";
                case DisplayAlertType.GPIO_Port_4:
                    return "GPIO Port 4";
            }

            return "Unknown";
        }

        public Boolean isEqual(CompositeSequence seq)
        {
            if (Utils.AlmostEquals(seq.startTime, startTime) &&
                Utils.AlmostEquals(seq.duration, duration) &&
                seq.displayAlertType == displayAlertType &&
                seq.filename.Equals(filename) &&
                seq.startTimeCalculated == startTimeCalculated)
                return true;
            else
                return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || obj.GetType() != typeof(CompositeSequence))
                return false;

            CompositeSequence seq = (CompositeSequence)obj;

            return isEqual(seq);
        }


        public Boolean isValid()
        {
            return true;
        }

        public Boolean isConcurrentType()
        {
            if (displayAlertType == DisplayAlertType.Blink_Display ||
                // displayAlertType == DisplayAlertType.Blink_and_Strobes ||
                displayAlertType == DisplayAlertType.Camera_Trigger ||
                displayAlertType == DisplayAlertType.GPIO_Port_1 ||
                displayAlertType == DisplayAlertType.GPIO_Port_2 ||
                displayAlertType == DisplayAlertType.GPIO_Port_3 ||
                displayAlertType == DisplayAlertType.GPIO_Port_4)
                return true;
            else
                return false;
        }

        public Boolean isStartTimeCalculated()
        {
            if (!isConcurrentType())
                return true;
            else
                return startTimeCalculated;
        }
    }

    public class PageCompositeFile : PageFile
    {
        public List<CompositeSequence> sequences = new List<CompositeSequence>();
        public byte numCycles = 0;

        public PageCompositeFile(string nameIn, PMDDisplaySize panelSize) :
                                base(nameIn, PageType.Composite, panelSize)
        {

        }

        //
        // For Hash Value calculation only
        // Actual messages sent to PMG are encoded using 
        // function getPayloadDataList()
        //
        public override byte[] encode()
        {
            List<byte> byteList = new List<byte>();

            // Filename
            string filename = pageName + PageTag.getFileExtension(pageType, displayType);

            byteList.Add((byte)(filename.Length + 1));
            byte[] data = Encoding.ASCII.GetBytes(filename);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            // Number Of Segments
            byteList.Add((byte)sequences.Count);

            // Number Of Cycles
            byteList.Add(numCycles);

            // Reserved bytes
            byteList.Add(0);
            byteList.Add(0);

            // Encode sequences
            for (int i = 0; i < sequences.Count; i++)
            {
                byte[] seqData = sequences[i].encode();

                Utils.AddArrayToList(ref byteList, seqData);
            }

            // Calculate Hash
            byte[] payload = byteList.ToArray();

            UInt16 hashValue = Util.U16ComputeCRC(payload, 0, payload.Length);

            // If payload length is not even, we add padding 0 to hash calculation
            if (payload.Length % 2 != 0)
            {
                hashValue = Util.U16ComputeCRC(hashValue, (byte)0);
            }

            byteList.Insert(0, (byte)((hashValue >> 8) & 0xFF));
            byteList.Insert(0, (byte)(hashValue & 0xFF));

            return byteList.ToArray();
        }

        //
        // Note:
        //
        // (1) The actaul data has already been processed to remove
        //     all reserved bytes
        //
        // (2) If multi-packets message is received from PMG, the payload
        //     from each packet is retrieved and combined together as
        //     single message. 
        //
        public override Boolean decode(byte[] data)
        {
            int idx = 0;
            UInt16 hashValue = 0;

            hashValue = (UInt16)(data[idx] + (data[idx + 1] << 8));
            idx += 2;

            //
            // Filename Length + Filename --> Panel Size and Page Name
            //
            int len = data[idx++];
            byte[] filenameData = Util.GetByteArray(data, ref idx, len);

            string filename = Util.GetFilename(filenameData);

            if (!PageTag.getPageInfoFromFilename(filename,
                                                ref pageType,
                                                ref displayType, ref isTxx))
            {
                errorMsg = "Wrong page filename format!";
                return false;
            }
            pageName = Path.GetFileNameWithoutExtension(filename);

            // Number Of Segments
            int numSegments = data[idx++];
            numCycles = data[idx++];

            if (CompositeSequence.decodeSequenceBlock(data, idx, numSegments,
                                                     ref sequences))
                return true;
            else
                return false;
        }

        public override Boolean isEqual(PageFile otherPage)
        {
            if (otherPage == null ||
                otherPage.GetType() != typeof(PageCompositeFile))
                return false;

            PageCompositeFile other = (PageCompositeFile)otherPage;

            if (sequences.Count != other.sequences.Count)
                return false;

            if (numCycles != other.numCycles)
                return false;

            for (int i = 0; i < sequences.Count; i++)
            {
                if (!sequences[i].isEqual(other.sequences[i]))
                    return false;
            }

            return true;
        }

        public override UInt16 getHashValue()
        {
            List<PayloadData> packetDataList = getPayloadDataList();

            // 1st Packet's 1st two bytes are Hash value
            if (packetDataList.Count == 0)
                return 0;

            UInt16 hashValue = (UInt16)((packetDataList[0].data[0] << 8) +
                                         packetDataList[0].data[1]);
            return hashValue;
        }

        // 
        // Retrieve how many packets needed 
        //
        public void getPacketInfo(ref int totalPacketNumber, ref int numSegmentsInEachPacket,
                                  ref int numSegmentsInLastPacket)
        {
            const int MaxSegmentsInOnePacket = 10;

            if (sequences.Count == 0)
            {
                totalPacketNumber = 0;
                numSegmentsInEachPacket = 0;
                numSegmentsInLastPacket = 0;
                return;
            }

            // We put maximum "MaxSegmentsInOnePacket" segemetns in one packet
            totalPacketNumber = ((sequences.Count - 1) / MaxSegmentsInOnePacket) + 1;

            if (totalPacketNumber > 1)
            {
                numSegmentsInEachPacket = MaxSegmentsInOnePacket;
                numSegmentsInLastPacket = sequences.Count - ((totalPacketNumber - 1) * MaxSegmentsInOnePacket);
            }
            else
            {
                numSegmentsInEachPacket = sequences.Count;
                numSegmentsInLastPacket = sequences.Count;
            }
        }

        // 
        // Retrieve message payload list. Each entry in the list represent
        // a packet to be sent to PMG.
        //
        public List<PayloadData> getPayloadDataList()
        {
            List<PayloadData> list = new List<PayloadData>();
            int totalPacketNumber = 0;
            int numSegmentsInEachPacket = 0;
            int numSegmentsInLastPacket = 0;
            int i, k;
            int numSegmentsInThisPacket = 0;

            getPacketInfo(ref totalPacketNumber, ref numSegmentsInEachPacket,
                          ref numSegmentsInLastPacket);

            if (totalPacketNumber == 0)
                return list;

            for (i = 0; i < totalPacketNumber; i++)
            {
                List<byte> byteList = new List<byte>();

                //
                // We first encode header portion
                //
                // 1st Packet
                if (i == 0)
                {
                    //
                    // Two bytes of total segments size Hash Value.
                    // We need to encode the message using old
                    // deprecated single message to get this hash value
                    //
                    byte[] dataOfDeprecatedMessage = encode();

                    byteList.Add(dataOfDeprecatedMessage[0]);
                    byteList.Add(dataOfDeprecatedMessage[1]);

                    // Encode sequence data
                    List<byte> completeSeqData = new List<byte>();

                    for (k = 0; k < sequences.Count; k++)
                    {
                        byte[] seqData = sequences[k].encode();

                        Utils.AddArrayToList(ref completeSeqData, seqData);
                    }

                    // Filename
                    string filename = pageName + PageTag.getFileExtension(pageType, displayType);

                    byteList.Add((byte)(filename.Length + 1));
                    byte[] data = Encoding.ASCII.GetBytes(filename);
                    Utils.AddArrayToList(ref byteList, data);
                    byteList.Add(0);

                    // Total Number Of Segments
                    byteList.Add((byte)sequences.Count);

                    // Number Of Cycles
                    byteList.Add(numCycles);

                    // Total Segments Size
                    byteList.Add((byte)(completeSeqData.Count & 0xff));
                    byteList.Add((byte)((completeSeqData.Count >> 8) & 0xff));

                    // 3 bytes reserved
                    byteList.Add(0);
                    byteList.Add(0);
                    byteList.Add(0);

                    // Num Segments in 1st Packet
                    byteList.Add((byte)numSegmentsInEachPacket);

                    numSegmentsInThisPacket = numSegmentsInEachPacket;
                }
                else
                {
                    //
                    // Num Segments in 2nd and rest of Packets
                    //
                    if (i == totalPacketNumber - 1)
                        numSegmentsInThisPacket = numSegmentsInLastPacket;
                    else
                        numSegmentsInThisPacket = numSegmentsInEachPacket;

                    byteList.Add((byte)numSegmentsInThisPacket);

                    // Reserved bytes
                    byteList.Add(0);
                    byteList.Add(0);
                    byteList.Add(0);
                }

                // Now actual sequence data part
                int sequenceStartIndex = i * numSegmentsInEachPacket;

                List<byte> seqDataForPacket = new List<byte>();

                for (k = sequenceStartIndex; k < sequenceStartIndex + numSegmentsInThisPacket; k++)
                {
                    byte[] seqData = sequences[k].encode();
                    Utils.AddArrayToList(ref seqDataForPacket, seqData);
                }

                // Add sequence data portion to current packet data
                Utils.AddArrayToList(ref byteList, seqDataForPacket.ToArray());

                // Generate a packet
                list.Add(new PayloadData(byteList.ToArray()));
            }

            return list;
        }

        public override Boolean loadPage(string pageFilename)
        {
            if (!File.Exists(pageFilename))
            {
                errorMsg = "Fail to read file " + pageFilename;
                return false;
            }

            try
            {
                string readText = File.ReadAllText(pageFilename);

                return fromString(readText);
            }
            catch (Exception e)
            {
                errorMsg = "Load page failure :" + e.Message;
                return false;
            }
        }

        public override Boolean savePage(string pageFilename)
        {
            try
            {
                if (pageFilename == string.Empty)
                {
                    pageFilename = PageTag.getPagesDirectory() + "\\" + pageName +
                                    PageTag.getFileExtension(PageType.Composite, displayType);

                    Utils.GetCaseSensitiveFilename(pageFilename, ref pageFilename);
                }

                using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(pageFilename))
                {
                    // Write Page Duration

                    string s = toString();

                    file.Write(s);
                }

                return true;
            }
            catch (Exception e)
            {
                errorMsg = "Fail to save page! " + e.Message;
                return false;
            }
        }

        public static void calculateStartTime(ref List<CompositeSequence> seqs)
        {
            int i;
            float startTime = 0;

            for (i = 0; i < seqs.Count; i++)
            {
                if (seqs[i].isStartTimeCalculated())
                {
                    seqs[i].startTime = startTime;

                    if (!seqs[i].isConcurrentType())
                        startTime += seqs[i].duration;
                }
            }
        }

        public override Boolean fromString(string s)
        {
            string[] segList = Regex.Split(s, Environment.NewLine);

            string name = string.Empty, value = string.Empty;

            sequences.Clear();

            for (int i = 0; i < segList.Length; i++)
            {
                if (!Utils.GetNameValue(segList[i], ref name, ref value))
                    continue;

                if (name.Equals("Name"))
                    pageName = value;
                else if (name.Equals("DisplayType"))
                    displayType = (PMDDisplaySize)Convert.ToInt16(value);
                else if (name.Equals("NumCycles"))
                    numCycles = Convert.ToByte(value);

                else if (name.Equals("Sequence"))
                {
                    CompositeSequence entry = new CompositeSequence();

                    if (entry.fromString(value))
                        sequences.Add(entry);
                }
            }

            PageCompositeFile.calculateStartTime(ref sequences);

            return true;
        }
        public override string toString()
        {
            string content;

            content = ("Name=" + pageName + Environment.NewLine);
            content += ("DisplayType=" + (int)displayType + Environment.NewLine);
            content += ("NumCycles=" + numCycles + Environment.NewLine);

            for (int i = 0; i < sequences.Count; i++)
            {
                content += ("Sequence=" + sequences[i].toString());

                if (i != sequences.Count - 1)
                    content += Environment.NewLine;

            }

            return content;
        }

        public List<PageTag> getReferencedPages()
        {
            List<PageTag> referencePages = new List<PageTag>();

            for (int i = 0; i < sequences.Count; i++)
            {
                PageTag page = sequences[i].getReferencedPage();

                if (page != null)
                    referencePages.Add(page);
            }

            // Now, we remove the duplication
            referencePages = referencePages.Distinct(new PageTagComparer()).ToList();
            return referencePages;
        }
    }

    public class PageCalendarFile : PageFile
    {
        public List<DateTime> schoolDayList = new List<DateTime>();

        public PageCalendarFile(string nameIn) :
                               base(nameIn, PageType.Calendar)
        {

        }

        public PageCalendarFile() : base("Unknown", PageType.Calendar)
        {
        }

        public override byte[] encode()
        {
            #region
            List<byte> byteList = new List<byte>();

            // Filename
            string filename = getFilename();

            byteList.Add((byte)(filename.Length + 1));
            byte[] data = Encoding.ASCII.GetBytes(filename);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            // 4 bytes reserved
            byteList.Add(0);
            byteList.Add(0);
            byteList.Add(0);
            byteList.Add(0);

            //
            // First and last selected calendar day
            //
            UInt32 unixClock;
            byte[] unixClockData;

            DateTime firstCalendarDay = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTime lastCalendarDay = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

            if (schoolDayList.Count > 0)
            {
                firstCalendarDay = Util.GetDayTimestamp(schoolDayList[0]);
                lastCalendarDay = Util.GetDayTimestamp(schoolDayList[schoolDayList.Count - 1]);

                unixClock = Util.ConvertToUnixTimestamp(firstCalendarDay);
                unixClockData = BitConverter.GetBytes(unixClock);
                Util.AddArrayToList(ref byteList, unixClockData);

                unixClock = Util.ConvertToUnixTimestamp(lastCalendarDay);
                unixClockData = BitConverter.GetBytes(unixClock);
                Util.AddArrayToList(ref byteList, unixClockData);
            }
            else
            {
                //
                // This should never happen, we should have at least one school day
                //
                unixClock = Util.ConvertToUnixTimestamp(lastCalendarDay);
                unixClockData = BitConverter.GetBytes(unixClock);
                Util.AddArrayToList(ref byteList, unixClockData);
                Util.AddArrayToList(ref byteList, unixClockData);
            }

            byte[] daysData = encodeEventDays();

            if (daysData == null)
            {
                byteList.Add(0);
            }
            else
            {
                byteList.Add((byte)daysData.Length);
                Util.AddArrayToList(ref byteList, daysData);
            }

            // Calculate Hash
            byte[] payload = byteList.ToArray();
            UInt16 hashValue = Util.U16ComputeCRC(payload, 0, payload.Length);

            // If payload length is not even, we add padding 0 to hash calculation
            if (payload.Length % 2 != 0)
            {
                hashValue = Util.U16ComputeCRC(hashValue, (byte)0);
            }

            byteList.Insert(0, (byte)((hashValue >> 8) & 0xFF));
            byteList.Insert(0, (byte)(hashValue & 0xFF));

            return byteList.ToArray();
            #endregion
        }

        private byte[] encodeEventDays()
        {
            if (schoolDayList.Count == 0)
                return null;

            // Number of Calendar Bytes
            DateTime firstCalendarDay = schoolDayList[0];
            DateTime lastCalendarDay = schoolDayList[schoolDayList.Count - 1];

            int totalDays = 1 + (lastCalendarDay - firstCalendarDay).Days;
            int numCalendarBytes = (byte)((totalDays + 7) / 8);

            byte[] daysData = new byte[numCalendarBytes];

            int numDaySinceFirstCalendarDay = 0;
            int byteIdx, bitIndx;

            for (int i = 0; i < schoolDayList.Count; i++)
            {
                numDaySinceFirstCalendarDay = (schoolDayList[i].Date - firstCalendarDay.Date).Days;

                byteIdx = (numDaySinceFirstCalendarDay / 8) + 1;
                bitIndx = 8 - ((byteIdx * 8) - numDaySinceFirstCalendarDay);

                daysData[byteIdx - 1] = (byte)(daysData[byteIdx - 1] | (1 << bitIndx));
            }

            return daysData;
        }
        public override Boolean decode(byte[] data)
        {
            #region
            int idx = 0, i, j;

            UInt16 hashValue = 0;

            hashValue = (UInt16)(data[idx] + (data[idx + 1] << 8));
            idx += 2;

            // Filename Length + Filename --> Panel Size and Page Name
            //
            int len = data[idx++];
            byte[] filenameData = Util.GetByteArray(data, ref idx, len);

            string filename = Util.GetFilename(filenameData);

            Boolean isTxxDummy = false; // Only for Text Page

            if (!PageTag.getPageInfoFromFilename(filename,
                                                ref pageType,
                                                ref displayType,
                                                ref isTxxDummy))
            {
                errorMsg = "Wrong page filename format!";
                return false;
            }

            pageName = Path.GetFileNameWithoutExtension(filename);

            // Skip 4 bytes reserved
            idx += 4;

            // Start Date and End Date
            byte[] sectionData = Util.GetByteArray(data, ref idx, 4);
            UInt32 clockData = Util.GetUint32(sectionData, 0);
            DateTime startDate = Util.ConvertFromUnixTimestamp(clockData);

            sectionData = Util.GetByteArray(data, ref idx, 4);
            clockData = Util.GetUint32(sectionData, 0);
            DateTime endDate = Util.ConvertFromUnixTimestamp(clockData);

            int numCalendarBytes = data[idx++];

            byte[] daysData = Util.GetByteArray(data, ref idx, numCalendarBytes);
            int numDaySinceFirstCalendarDay = 0;

            schoolDayList.Clear();

            for (i = 0; i < numCalendarBytes; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (((daysData[i] >> j) & 0x01) != 0)
                    {
                        numDaySinceFirstCalendarDay = (i * 8) + j;

                        DateTime eventDate = startDate.AddDays(numDaySinceFirstCalendarDay);

                        schoolDayList.Add(eventDate);
                    }
                }
            }

            return true;
            #endregion
        }

        public override Boolean isEqual(PageFile otherPage)
        {
            #region
            if (otherPage == null ||
                otherPage.GetType() != typeof(PageCalendarFile))
                return false;

            PageCalendarFile other = (PageCalendarFile)otherPage;

            if (schoolDayList.Count != other.schoolDayList.Count)
                return false;

            for (int i = 0; i < schoolDayList.Count; i++)
            {
                if (schoolDayList[i].Year != other.schoolDayList[i].Year ||
                    schoolDayList[i].Month != other.schoolDayList[i].Month ||
                    schoolDayList[i].Day != other.schoolDayList[i].Day)
                    return false;
            }

            return true;
            #endregion
        }

        public override Boolean savePage(string pageFilename)
        {
            #region

            Boolean status = syntaxCheck(ref errorMsg);

            if (!status)
                return false;

            if (pageFilename == string.Empty)
            {
                pageFilename = PageTag.getPagesDirectory() + "\\" + pageName +
                PageTag.getFileExtension(PageType.Calendar, displayType);

                Utils.GetCaseSensitiveFilename(pageFilename, ref pageFilename);
            }

            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(pageFilename))
                {
                    // Write Page Duration

                    string s = toString();

                    file.Write(s);
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                errorMsg = "Error Detected! " + ex.Message;
                return false;
            }

            return true;
            #endregion
        }

        public override Boolean loadPage(string pageFilename)
        {
            #region
            if (!File.Exists(pageFilename))
                return false;

            try
            {
                string readText = File.ReadAllText(pageFilename);
                Boolean status = fromString(readText);

                //
                // This is to avoid case when user manually change the page filename
                // So, the page name will come from the filename
                //
                if (status)
                    pageName = Path.GetFileNameWithoutExtension(pageFilename);

                int idx = pageName.IndexOf('@');
                if (idx != -1)
                    pageName = pageName.Remove(idx);

                return status;
            }
            catch (Exception e)
            {
                errorMsg = "Load page failure :" + e.Message;
                return false;
            }
            #endregion
        }

        public override Boolean fromString(string s)
        {
            #region
            string[] segList = Regex.Split(s, Environment.NewLine);
            string name = string.Empty, value = string.Empty;
            int i, j, numDaySinceFirstCalendarDay;

            displayType = PMDDisplaySize.TwelveInchPMD;
            DateTime startDate = new DateTime(1970, 1, 1);

            int calendarBytes = 0;
            string eventDaysData = string.Empty;

            for (i = 0; i < segList.Length; i++)
            {
                if (!Utils.GetNameValue(segList[i], ref name, ref value))
                    continue;

                if (name.Equals("Name"))
                    pageName = value;
                else if (name.Equals("StartDate"))
                    startDate = Convert.ToDateTime(value);
                else if (name.Equals("CalendarBytes"))
                    calendarBytes = Convert.ToInt16(value);
                else if (name.Equals("EventDays"))
                    eventDaysData += value;
            }

            if (calendarBytes == 0 || eventDaysData == string.Empty)
            {
                schoolDayList.Clear();
                return true;
            }

            //
            // Now, we generate event day list
            //
            eventDaysData = Regex.Replace(eventDaysData, @"\s+", "");
            eventDaysData = Regex.Replace(eventDaysData, "[^A-Za-z0-9 _]", "");

            byte[] daysData = Utils.StringToByteArrayFastest(eventDaysData);

            if (daysData == null || daysData.Length != calendarBytes)
            {
                errorMsg = "Load page failure, error detected in calendar event day data !";
                schoolDayList.Clear();
                return false;
            }

            for (i = 0; i < daysData.Length; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    if (((daysData[i] >> j) & 0x01) != 0)
                    {
                        numDaySinceFirstCalendarDay = (i * 8) + j;

                        DateTime eventDate = startDate.AddDays(numDaySinceFirstCalendarDay);

                        schoolDayList.Add(eventDate);
                    }
                }
            }

            return true;
            #endregion
        }
        public override string toString()
        {
            #region
            string s = string.Empty;

            s += ("Name=" + pageName + Environment.NewLine);

            schoolDayList.Sort();

            if (schoolDayList.Count > 0)
            {
                s += ("StartDate=" + schoolDayList[0].ToShortDateString() + Environment.NewLine);
                s += ("EndDate=" + schoolDayList[schoolDayList.Count - 1].ToShortDateString() + Environment.NewLine);
            }

            byte[] daysData = encodeEventDays();

            if (daysData != null)
            {
                s += ("CalendarBytes=" + daysData.Length + Environment.NewLine);

                string eventSection = string.Empty;

                for (int i = 0; i < daysData.Length; i++)
                {
                    if (i % 16 == 0)
                    {
                        if (i == 0)
                            eventSection += "EventDays=";
                        else
                            eventSection += (Environment.NewLine + "EventDays=");
                    }

                    eventSection += (daysData[i].ToString("X2") + " ");
                }

                s += eventSection;
            }

            return s;
            #endregion
        }

        public Boolean syntaxCheck(ref string errorMsg)
        {
            //if (schoolDayList.Count == 0)
            //{
            //    errorMsg = "Calendar has no selected days !";
            //    return false;
            //}

            // Number of Calendar Bytes
            if (schoolDayList.Count > 0)
            {
                DateTime firstCalendarDay = schoolDayList[0];
                DateTime lastCalendarDay = schoolDayList[schoolDayList.Count - 1];

                int totalDays = 1 + (lastCalendarDay - firstCalendarDay).Days;

                if (totalDays > 255 * 8)
                {
                    errorMsg = "Days between first day and last day cannot\nexceed 5 years (2040 days) !";
                    return false;
                }
            }

            return true;
        }

        public void addEvents(List<DateTime> eventList)
        {

        }

        public DateTime getFirstEventDay()
        {
            if (schoolDayList.Count != 0)
            {
                return schoolDayList[0];
            }
            else
                return DateTime.Now.Date;
        }

    }

    public class ScheduledOperation
    {
        public enum DateRangeType
        {
            DateRange = 0x00, Calendar = 0x01
        };
        public string name { get; set; }
        public string errorMsg = string.Empty;
        public PMDDisplaySize displayType { get; set; } = PMDDisplaySize.TwelveInchPMD;

        public int PMGID { get; set; }
        public string operationName { get; set; } = string.Empty;

        public Action_Type_Enum_t idleDisplayMode { get; set; }
        public Action_Type_Enum_t limitDisplayMode { get; set; }
        public Action_Type_Enum_t alertDisplayMode { get; set; }

        public byte limitSpeed { get; set; }
        public byte alertSpeed { get; set; }


        public PageTag idleDisplayPage { get; set; } = new PageTag();
        public PageTag limitDisplayPage { get; set; } = new PageTag();
        public PageTag alertDisplayPage { get; set; } = new PageTag();

        public string idleDisplayPageName { get; set; }
        public string limitDisplayPageName { get; set; }
        public string alertDisplayPageName { get; set; }


        public PageTag GetPageTag(string pageName, Action_Type_Enum_t type)
        {
            bool isTxxIn = false;
            if (string.IsNullOrEmpty(pageName))
            {
                pageName = "";
                isTxxIn = true;
            }
            else
            {
                pageName = Path.GetFileNameWithoutExtension(pageName);
                isTxxIn = false;
            }
            PageType pageType = GetPageType(type);
            return new PageTag(pageName, pageType, displayType, isTxxIn);
        }


        public PageType GetPageType(Action_Type_Enum_t type)
        {
            if (type == Action_Type_Enum_t.Text)
                return PageType.Text;
            else if (type == Action_Type_Enum_t.Graphic)
                return PageType.Graphic;
            else if (type == Action_Type_Enum_t.Animation)
                return PageType.Animation;
            else if (type == Action_Type_Enum_t.Composite)
                return PageType.Composite;

            return PageType.Text;
        }

        public Alert_Type_Enum_t limitActionType { get; set; }
        public Alert_Type_Enum_t alertActionType { get; set; }


        public DateRangeType dateRangeType { get; set; } = DateRangeType.DateRange;
        public string calendarFilename { get; set; } = "";


        public DateTime startDate { get; set; } = DateTime.MinValue;
        public string strStartDate => startDate.Date.ToString("yyyy/MM/dd");
        public DateTime stopDate { get; set; } = DateTime.MinValue;
        public string strStopDate => stopDate.Date.ToString("yyyy/MM/dd");
        public DateTime startTime { get; set; } = DateTime.MinValue;
        public string strStartTime => startTime.ToString("HH:mm");
        public DateTime stopTime { get; set; } = DateTime.MinValue;
        public string strStopTime => stopTime.ToString("HH:mm");


        public byte days { get; set; } = 0;
        public string selectedDays { get; set; }

        public byte enableFlag { get; set; } = 1;

        public List<SelectOption> IdlePageList { get; set; }
        public List<SelectOption> LimitPageList { get; set; }
        public List<SelectOption> AlertPageList { get; set; }

        public byte userData = 0;

        public ScheduledOperation()
        {

        }

        public ScheduledOperation(ScheduledOperation others)
        {
            if (others != null)
            {
                byte[] data = others.encode();
                decode(data);
            }
        }

        public byte toDays()
        {
            byte day = 0x7F;
            if (string.IsNullOrEmpty(selectedDays))
                return day;
            var listArray = selectedDays.Split(',');
            for (int i = 1; i <= 7; i++)
            {
                if (listArray.Contains(i.ToString()))
                    day += (byte)(0x01 << (i - 1));
            }
            return day;
        }

        public string fromDays()
        {
            if (days == 0x7F)
                return "Daily";
            List<int> newList = new List<int>();
            for (int i = 1; i <= 7; i++)
            {

                if ((byte)((days >> (i - 1)) & 0x01) == 0x01)
                    newList.Add(i);

            }
            return string.Join(",", newList);
        }

        public string getFilename()
        {
            return operationName + PageTag.getFileExtension(PageType.Sequence, displayType);
        }

        public string toString()
        {
            string s = string.Empty;

            s += ("OperationName=" + operationName + Environment.NewLine);

            s += ("DateRange=" + dateRangeType.ToString() + Environment.NewLine);
            s += ("Calendar=" + calendarFilename + Environment.NewLine);

            s += ("StartDate=" + startDate.ToShortDateString() + "," + startDate.ToShortTimeString() + Environment.NewLine);
            s += ("StopDate=" + stopDate.ToShortDateString() + "," + stopDate.ToShortTimeString() + Environment.NewLine);

            s += ("Days= " + Convert.ToString(days, 2).PadLeft(8, '0') + Environment.NewLine);

            s += ("DisplayType=" + (int)displayType + Environment.NewLine);

            s += ("StartTime=" + startTime.ToShortTimeString() + Environment.NewLine);
            s += ("StopTime=" + stopTime.ToShortTimeString() + Environment.NewLine);

            s += ("LimitSpeed=" + limitSpeed + Environment.NewLine);
            s += ("AlertSpeed=" + alertSpeed + Environment.NewLine);

            s += ("IdleDisplayMode=" + (int)idleDisplayMode + Environment.NewLine);
            s += ("AlertDisplayMode=" + (int)alertDisplayMode + Environment.NewLine);
            s += ("LimitDisplayMode=" + (int)limitDisplayMode + Environment.NewLine);

            s += ("LimitActionType=" + (int)limitActionType + Environment.NewLine);
            s += ("AlertActionType=" + (int)alertActionType + Environment.NewLine);

            s += ("IdleDisplayPageTag=" + idleDisplayPage.toString() + Environment.NewLine);
            s += ("LimitDisplayPageTag=" + limitDisplayPage.toString() + Environment.NewLine);
            s += ("AlertDisplayPageTag=" + alertDisplayPage.toString() + Environment.NewLine);

            s += ("Enabled=" + enableFlag + Environment.NewLine);

            // Output Hash Value
            UInt16 hashValue = getHashValue();
            byte hashLSB = (byte)(hashValue & 0xFF);
            byte hashMSB = (byte)((hashValue >> 8) & 0xFF);

            s += ("HashValue=" + hashMSB.ToString("X2") + hashLSB.ToString("X2"));

            return s;
        }

        public bool fromString(string s)
        {
            #region
            string[] segList = Regex.Split(s, Environment.NewLine);
            string name = string.Empty, value = string.Empty;

            try
            {
                for (int i = 0; i < segList.Length; i++)
                {
                    if (!Util.GetNameValue(segList[i], ref name, ref value))
                        continue;

                    if (name.Equals("OperationName"))
                        operationName = value;
                    else if (name.Equals("DateRange"))
                    {
                        dateRangeType = (DateRangeType)Enum.Parse(typeof(DateRangeType), value);

                    }
                    else if (name.Equals("Calendar"))
                    {
                        calendarFilename = value;
                    }
                    else if (name.Equals("DisplayType"))
                        displayType = (PMDDisplaySize)Convert.ToInt16(value);

                    else if (name.Equals("StartDate"))
                        startDate = Convert.ToDateTime(value);
                    else if (name.Equals("StartTime"))
                        startTime = Convert.ToDateTime(value);
                    else if (name.Equals("StopDate"))
                        stopDate = Convert.ToDateTime(value);
                    else if (name.Equals("StopTime"))
                        stopTime = Convert.ToDateTime(value);
                    else if (name.Equals("Days"))
                        days = Convert.ToByte(value, 2);

                    else if (name.Equals("LimitSpeed"))
                        limitSpeed = Convert.ToByte(value);
                    else if (name.Equals("AlertSpeed"))
                        alertSpeed = Convert.ToByte(value);

                    else if (name.Equals("IdleDisplayMode"))
                        idleDisplayMode = (Action_Type_Enum_t)Convert.ToByte(value);
                    else if (name.Equals("AlertDisplayMode"))
                        alertDisplayMode = (Action_Type_Enum_t)Convert.ToByte(value);
                    else if (name.Equals("LimitDisplayMode"))
                        limitDisplayMode = (Action_Type_Enum_t)Convert.ToByte(value);
                    else if (name.Equals("LimitActionType"))
                        limitActionType = (Alert_Type_Enum_t)Convert.ToByte(value);
                    else if (name.Equals("AlertActionType"))
                        alertActionType = (Alert_Type_Enum_t)Convert.ToByte(value);

                    else if (name.Equals("IdleDisplayPageTag"))
                        idleDisplayPage.fromString(value);
                    else if (name.Equals("LimitDisplayPageTag"))
                        limitDisplayPage.fromString(value);
                    else if (name.Equals("AlertDisplayPageTag"))
                        alertDisplayPage.fromString(value);
                    else if (name.Equals("Enabled"))
                        enableFlag = Convert.ToByte(value);
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

       

        public Boolean isValidName(string name, ref string reason)
        {
            if (name == null || name.Length == 0)
            {
                reason = "Operation name is empty!";
                return false;
            }

            if (name.Length > 27)
            {
                reason = "Operation name cannot exceeds 27 characters!\n" +
                         "Current operation name has " + name.Length + " characters.";
                return false;
            }

            int idx = name.IndexOfAny(Path.GetInvalidFileNameChars());

            if (idx < 0)
                return true;
            else
            {
                reason = "Operation name cannot contain following characters:\n\n" +
                          "\\ / : * ? \"< > |";
                return false;
            }
        }

        public List<PageTag> getReferencedPageTags()
        {
            List<PageTag> referencePages = new List<PageTag>();

            if (PageTag.isPageFileRequired(idleDisplayMode))
                referencePages.Add(idleDisplayPage);

            if (PageTag.isPageFileRequired(limitDisplayMode))
                referencePages.Add(limitDisplayPage);

            if (PageTag.isPageFileRequired(alertDisplayMode))
                referencePages.Add(alertDisplayPage);

            return referencePages;
        }
        public Boolean syntaxCheck(ref string errorMsg)
        {
            if (!isValidName(operationName, ref errorMsg))
            {
                return false;
            }



            //if (PageTag.isPageFileRequired(idleDisplayMode) &&
            //    (idleDisplayPage == null || idleDisplayPage.name == string.Empty))
            //{
            //    errorMsg = "Idle Display Page is empty!";
            //    return false;
            //}

            //if (PageTag.isPageFileRequired(alertDisplayMode) &&
            //    (alertDisplayPage == null || alertDisplayPage.name == string.Empty))
            //{
            //    errorMsg = "Alert Display Page is empty!";
            //    return false;
            //}

            //if (PageTag.isPageFileRequired(limitDisplayMode) &&
            //   (limitDisplayPage == null || limitDisplayPage.name == string.Empty))
            //{
            //    errorMsg = "Limit Display Page is empty!";
            //    return false;
            //}

            //
            // Alert Speed should be larger than Speed Limit
            //      
            if (alertSpeed <= limitSpeed)
            {
                errorMsg = "Alert Speed should be set higher than Speed Limit!";
                return false;
            }

            if (dateRangeType == DateRangeType.DateRange)
            {
                // Valid Time Peroid
                if (stopDate.Date < startDate.Date)
                {
                    errorMsg = operationName + ": Stop Date cannot be earlier than Start Date!";
                    return false;
                }

                if (stopTime == startTime)
                {
                    errorMsg = operationName + ": Stop Time cannot be equal to Start Time!";
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(calendarFilename))
                {
                    errorMsg = "Calendar is empty!";
                    return false;
                }

            }

            return true;
        }

        public byte[] encode()
        {
            List<byte> byteList = new List<byte>();
            byte[] data;
            string idleFilename, limitFilename, alertFilename;
            int idleFilenameOffset, limitFilenameOffset, alertFilenameOffset;

            idleFilename = idleDisplayPage.getPageFilename(false);
            limitFilename = limitDisplayPage.getPageFilename(false);
            alertFilename = alertDisplayPage.getPageFilename(false);

            //
            // Scheduled filename
            //
            string scheduleFilename = getFilename();
            byteList.Add((byte)(scheduleFilename.Length + 1));
            data = Encoding.ASCII.GetBytes(scheduleFilename);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            // 4 bytes reserved
            byteList.Add(0);
            byteList.Add(0);
            byteList.Add(0);
            byteList.Add(0);

            // Select Calendar
            byteList.Add((byte)dateRangeType);

            // Calendar Name
            if (string.IsNullOrEmpty(calendarFilename)) calendarFilename = "";
            byteList.Add((byte)(calendarFilename.Length + 1));
            data = Encoding.ASCII.GetBytes(calendarFilename);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            //
            // Start Date
            //
            System.UInt32 unixClock;
            DateTime startDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day,
                                                  0, 0, 0, DateTimeKind.Local);

            unixClock = Util.ConvertToUnixTimestamp(startDateTime);
            byte[] unixClockData = BitConverter.GetBytes(unixClock);
            Util.AddArrayToList(ref byteList, unixClockData);

            //
            // Stop Date
            //
            DateTime stopDateTime = new DateTime(stopDate.Year, stopDate.Month, stopDate.Day, 0,
                                                 0, 0, DateTimeKind.Local);

            unixClock = Util.ConvertToUnixTimestamp(stopDateTime);
            unixClockData = BitConverter.GetBytes(unixClock);
            Util.AddArrayToList(ref byteList, unixClockData);

            // Start Time
            DateTime currentTime =
                new DateTime(1999, 12, 31, startTime.Hour, startTime.Minute, 0, DateTimeKind.Local);
            unixClock = Util.ConvertToUnixTimestamp(currentTime);
            unixClockData = BitConverter.GetBytes(unixClock);
            Util.AddArrayToList(ref byteList, unixClockData);

            // Stop Time
            currentTime =
                new DateTime(1999, 12, 31, stopTime.Hour, stopTime.Minute, 0, DateTimeKind.Local);
            unixClock = Util.ConvertToUnixTimestamp(currentTime);
            unixClockData = BitConverter.GetBytes(unixClock);
            Util.AddArrayToList(ref byteList, unixClockData);

            //
            // Selected days
            //
            byteList.Add(days);

            // Start Time
            //int secondsFromMidnight = (startTime.Hour * 3600) + (startTime.Minute * 60);
            //byte[] timeData = BitConverter.GetBytes(secondsFromMidnight);
            //Util.AddArrayToList(ref byteList, timeData);

            // Stop Time
            //secondsFromMidnight = (stopTime.Hour * 3600) + (stopTime.Minute * 60);
            //timeData = BitConverter.GetBytes(secondsFromMidnight);
            //Util.AddArrayToList(ref byteList, timeData);


            // Limit Speed
            byteList.Add(limitSpeed);

            // Alert Speed
            byteList.Add(alertSpeed);

            // Enabled flag (Reseved_2 according Message Definition Doc)

            // byteList.Add(enableFlag);
            byteList.Add(0);

            //
            // =====Start Idle Display ============================================
            //         
            byteList.Add((byte)idleDisplayMode);
            byteList.Add((byte)0);
            byteList.Add((byte)0);
            idleFilenameOffset = operationName.Length + 2;
            byteList.Add((byte)idleFilenameOffset);

            //
            // ===== Start Limit Display ============================================
            //           
            byteList.Add((byte)limitDisplayMode);
            byteList.Add((byte)limitActionType);
            byteList.Add((byte)1);
            limitFilenameOffset = idleFilenameOffset + idleFilename.Length + 2;
            byteList.Add((byte)limitFilenameOffset);

            //
            // ===== Start Alert Display ============================================
            //       
            byteList.Add((byte)alertDisplayMode);
            byteList.Add((byte)alertActionType);
            byteList.Add((byte)2);
            alertFilenameOffset = limitFilenameOffset + limitFilename.Length + 2;
            byteList.Add((byte)alertFilenameOffset);


            // Operation Name        
            byteList.Add((byte)(operationName.Length + 1));
            data = Encoding.ASCII.GetBytes(operationName);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            // Idle Display Filename
            byteList.Add((byte)(idleFilename.Length + 1));
            data = Encoding.ASCII.GetBytes(idleFilename);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            // Limit Display Filename
            byteList.Add((byte)(limitFilename.Length + 1));
            data = Encoding.ASCII.GetBytes(limitFilename);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            // Alert Display Filename
            byteList.Add((byte)(alertFilename.Length + 1));
            data = Encoding.ASCII.GetBytes(alertFilename);
            Utils.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            // Calculate Hash
            byte[] payload = byteList.ToArray();
            UInt16 hashValue = Util.U16ComputeCRC(payload, 0, payload.Length);

            // If payload length is not even, we add padding 0 to hash calculation
            if (payload.Length % 2 != 0)
            {
                hashValue = Util.U16ComputeCRC(hashValue, (byte)0);
            }

            byteList.Insert(0, (byte)((hashValue >> 8) & 0xFF));
            byteList.Insert(0, (byte)(hashValue & 0xFF));

            return byteList.ToArray();
        }

        public Boolean decode(byte[] data)
        {
            int idx = 0, len;
            byte[] sectionData = null;
            string filename;

            // Two byte hash
            UInt16 hashValue = 0;
            hashValue = (UInt16)(data[idx] + (data[idx + 1] << 8));
            idx += 2;

            // Schedule Operation Name
            len = data[idx++];
            byte[] filenameData = Util.GetByteArray(data, ref idx, len);
            operationName = Util.GetFilename(filenameData);

            // Skip 4 reserved bytes
            idx += 4;

            // Date Range Type
            dateRangeType = (DateRangeType)data[idx++];

            // Calendar Name
            len = data[idx++];
            filenameData = Util.GetByteArray(data, ref idx, len);
            calendarFilename = Util.GetFilename(filenameData);

            // Start Date
            sectionData = Util.GetByteArray(data, ref idx, 4);
            UInt32 clockData = Util.GetUint32(sectionData, 0);
            startDate = Util.ConvertFromUnixTimestamp(clockData);

            // Stop Date
            sectionData = Util.GetByteArray(data, ref idx, 4);
            clockData = Util.GetUint32(sectionData, 0);
            stopDate = Util.ConvertFromUnixTimestamp(clockData);

            // Start Time
            sectionData = Util.GetByteArray(data, ref idx, 4);
            clockData = Util.GetUint32(sectionData, 0);
            startTime = Util.ConvertFromUnixTimestamp(clockData);

            // Stop Time      
            sectionData = Util.GetByteArray(data, ref idx, 4);
            clockData = Util.GetUint32(sectionData, 0);
            stopTime = Util.ConvertFromUnixTimestamp(clockData);

            // Days
            days = data[idx++];

            // Limit Speed and Alert Speed
            limitSpeed = data[idx++];
            alertSpeed = data[idx++];

            // Enabled flag (Reserved_2)
            idx++;

            // enableFlag = data[idx++];


            // ===== Idle Display ============================================   
            idleDisplayMode = (Action_Type_Enum_t)data[idx++];
            idx += 3;

            // ===== Limit Display ============================================   
            limitDisplayMode = (Action_Type_Enum_t)data[idx++];
            limitActionType = (Alert_Type_Enum_t)data[idx++];
            idx += 2;

            // ===== Alert Display ============================================   
            alertDisplayMode = (Action_Type_Enum_t)data[idx++];
            alertActionType = (Alert_Type_Enum_t)data[idx++];
            idx += 2;

            // Operation Name
            len = data[idx++];
            filenameData = Util.GetByteArray(data, ref idx, len);
            operationName = Util.GetFilename(filenameData);

            // Idle Display Filename
            len = data[idx++];
            filenameData = Util.GetByteArray(data, ref idx, len);
            filename = Util.GetFilename(filenameData);
            idleDisplayPage = new PageTag(filename);

            // Limit Display Filename
            len = data[idx++];
            filenameData = Util.GetByteArray(data, ref idx, len);
            filename = Util.GetFilename(filenameData);
            limitDisplayPage = new PageTag(filename);

            // Alert Display Filename
            len = data[idx++];
            filenameData = Util.GetByteArray(data, ref idx, len);
            filename = Util.GetFilename(filenameData);
            alertDisplayPage = new PageTag(filename);

            return true;
        }

        public Boolean isEqual(ScheduledOperation other)
        {
            byte[] data1 = encode();
            byte[] data2 = other.encode();

            if (data1 != null && data2 != null)
                return Util.ByteArrayCompare(data1, data2);

            return false;
        }
        public string getDescription()
        {
            string s = string.Empty;

            if (idleDisplayMode != Action_Type_Enum_t.None)
            {
                s += ("Idle: " + idleDisplayMode.ToString());
                if (idleDisplayPage.name != string.Empty)
                    s += ("=" + idleDisplayPage.name);
            }

            if (limitDisplayMode != Action_Type_Enum_t.None)
            {
                if (s != string.Empty)
                    s += "; ";

                s += ("Limit: " + limitDisplayMode.ToString());
                if (limitDisplayPage.name != string.Empty)
                    s += ("=" + limitDisplayPage.name);
            }

            if (alertDisplayMode != Action_Type_Enum_t.None)
            {
                if (s != string.Empty)
                    s += "; ";

                s += ("Alert: " + alertDisplayMode.ToString());
                if (alertDisplayPage.name != string.Empty)
                    s += ("=" + alertDisplayPage.name);
            }

            return s;
        }

        public ScheduledOperationViewModel getDisplayColumnString()
        {
            ScheduledOperationViewModel model = new ScheduledOperationViewModel();

            model.DisplayType = (int)displayType;
            model.OperationName = operationName;

            if (dateRangeType == DateRangeType.Calendar)
                model.DatePeriod = "Calendar [" + Path.GetFileNameWithoutExtension(calendarFilename) + "]";
            else
                model.DatePeriod = startDate.ToShortDateString() + "~" + stopDate.ToShortDateString();

            model.TimePeriod = startTime.ToShortTimeString() + "~" + stopTime.ToShortTimeString();

            if (dateRangeType != DateRangeType.Calendar)
                model.Recurrence = Util.GetDaysRepresentative(days);

            model.Description = getDescription();

            return model;
        }

        public UInt16 getHashValue()
        {
            byte[] data = encode();

            if (data != null)
            {
                int len = data.Length;

                UInt16 hashValue = (UInt16)((data[0] << 8) + data[1]);
                return hashValue;
            }
            return 0;
        }


    }
}
