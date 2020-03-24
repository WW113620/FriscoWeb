using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Diagnostics;

namespace FriscoTab
{
    public class Period
    {
        public DateTime StartDateTime { get; private set; }
        public DateTime EndDateTime { get; private set; }

        public Period(DateTime StartDateTime, DateTime EndDateTime)
        {
            if (StartDateTime > EndDateTime)
                throw new InvalidPeriodException("End DateTime Must Be Greater Than Start DateTime!");
            this.StartDateTime = StartDateTime;
            this.EndDateTime = EndDateTime;
        }

        public bool Overlaps(Period anotherPeriod)
        {
            return (this.StartDateTime < anotherPeriod.EndDateTime && this.EndDateTime > anotherPeriod.StartDateTime);
        }

        public TimeSpan GetDuration()
        {
            return EndDateTime - StartDateTime;
        }

    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }
        public Boolean Enabled { get; set; }
        public Brush FontColor { get; set; }
        public ComboboxItem(string TextIn, object valueIn, Boolean EnabledIn = true)
        {
            Text = TextIn;
            Value = valueIn;
            Enabled = EnabledIn;

            FontColor = Brushes.Black;      
        }
        public override string ToString()
        {
            return Text;
        }
    }

    public class InvalidPeriodException : Exception
    {
        public InvalidPeriodException(string Message) : base(Message) { }
    }


    class Utils
    {
        //public static byte [] ConvertBitmapDataInOneDimentionArray(byte[,] mBitmapData)
        //{
        //    int x, y;
        //    int w, h;
        //    int byteIdx, bitIdx;

        //    w = mBitmapData.GetLength(0);
        //    h = mBitmapData.GetLength(1);

        //    int totalByte = (w * h + 7) / 8;

        //    byte[] data = new byte[totalByte];
        //    int pixelNo;

        //    for (y = 1; y <= h; y++)
        //    {
        //        for (x = 1; x <= w; x++)
        //        {
        //            if (mBitmapData[x - 1, y - 1] == 0)
        //                continue;

        //            pixelNo = (y - 1) * w + x;

        //            byteIdx = (pixelNo - 1) / 8;
        //            bitIdx = 8 - (pixelNo - byteIdx * 8);

        //            data[byteIdx] += (byte)(1 << bitIdx);
        //        }
        //    }

        //    return data;
        //}

        public static string GetCallStacks()
        {
            string s = "Call Stack Dump\n" ;

            System.Diagnostics.StackTrace st = new StackTrace(true);

            int stackDepth = st.FrameCount;

            if (stackDepth > 6)
                stackDepth = 6;

            for (int i = 1; i < stackDepth; i++)
            {
                // Note that high up the call stack, there is only
                // one stack frame.
                StackFrame sf = st.GetFrame(i);
                s += (sf.GetMethod() + "\n");
                s += (sf.GetFileName() + ":" + sf.GetFileLineNumber() + "\n\n");
            }

            return s;
        }
        public static string ToBitsString(byte value)
        {
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }

        public static string ToHexString(UInt32 value)
        {
            return value.ToString("X");
        }

        public static DateTime GetLinkerTime()
        {
            var filePath = Assembly.GetExecutingAssembly().Location;

            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            // Convert UTC time to US central time
            TimeZoneInfo myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            DateTime myDateTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, myTimeZone);

            return myDateTime;
        }

        public static string GetAssemblyVersion()
        {

            //return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string versionString = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            string[] fileVersionInfo = versionString.Split('.');

            if (fileVersionInfo.Length >= 4)
            {
                if (fileVersionInfo[3] == "0")
                {
                    versionString = string.Format("{0}.{1}.{2}", fileVersionInfo[0], fileVersionInfo[1], fileVersionInfo[2]);
                }
            }

            return versionString;

        }

    
        public unsafe static void SetIndexedPixel(int x, int y, BitmapData bmd, bool pixel)
        {
            byte* p = (byte*)bmd.Scan0.ToPointer();

            int index = y * bmd.Stride + (x >> 3);

            byte mask = (byte)(0x80 >> (x & 0x7));

            if (pixel)
                p[index] |= mask;
            else
                p[index] &= (byte)(mask ^ 0xff);
        }

        public static byte [,] GetBitmapDataFromBitmap(Bitmap bit)
        {
            byte[,] bitmapData = new byte[bit.Width, bit.Height];

            int x, y;

            Color color;

            for (x = 0; x < bit.Width; x++)
            {
                for (y = 0; y < bit.Height; y++)
                {
                    color = bit.GetPixel(x, y);

                    if (color.ToArgb() == Color.White.ToArgb())
                    {
                        bitmapData[x, y] = 0;
                    }
                    else
                        bitmapData[x, y] = 1;
                }
            }

            return bitmapData;
        }

        public static byte[] AppendArray(byte[] originalArray, byte[] addedArray)
        {
            byte[] newArray = new byte[originalArray.Length + addedArray.Length];

            Buffer.BlockCopy(originalArray, 0, newArray, 0, originalArray.Length);
            Buffer.BlockCopy(addedArray, 0, newArray, originalArray.Length, addedArray.Length);

            return newArray;
        }

        public static string ReplaceNonAsciiWithDot(string value)
        {
            string pattern = "[^ -~]+";
            Regex reg_exp = new Regex(pattern);
            return reg_exp.Replace(value, ".");
        }

        public static string ByteArrayToHexString(byte[] byteData)
        {
            if (byteData == null || byteData.Length == 0)
                return string.Empty;

            string s = BitConverter.ToString(byteData);

            return s;
        }

        public static string ByteArrayToHexString(byte[] byteData, int startPos, 
                                int dataLength, Boolean printAscii = true)
        {
            string strResult = "";
            string s;

            int count = 0;

            for (int i = startPos; i < startPos + dataLength; i++)
            {
                count++;
                strResult += byteData[i].ToString("X2") + " ";

                if (count % 16 == 0)
                {
                    if (printAscii)
                    {                  
                        s = System.Text.Encoding.Default.GetString(byteData, i - 15, 16);
                        s = ReplaceNonAsciiWithDot(s);

                        strResult += ("     " + s + "\n");
                    }               
                }      
            }

            // Deal with last line
            int len = dataLength - ((dataLength / 16) * 16);

            if (len != 0 && printAscii)
            {
                // First append space
                strResult = strResult.PadRight(strResult.Length + (3 * (16 - len)), ' ');

                s = System.Text.Encoding.Default.GetString(byteData, startPos + dataLength - len, len);
                s = ReplaceNonAsciiWithDot(s);
                strResult += ("     " + s);
            }
        
            return strResult;
        }
        static public string GetCurrentDateTime()
        {
            string s = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            return s;
        }  
          
        static public int GetLast7Digits(string id)
        {
            int len = id.Length;
            int value = 0;

            if (len > 7)
                id = id.Remove(0, len - 7);

            try
            { 
                value = Convert.ToInt32(id);
            }
            catch (Exception)
            {
                return 0;
            }

            return value;
        }
        public static int ElapsedTimeInSeconds(DateTime time)
        {
            TimeSpan elapsedTime = DateTime.Now - time;
            return (int)elapsedTime.TotalSeconds;
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                return null;

            int len = hex.Length >> 1;

            byte[] arr = new byte[len];

            for (int i = 0; i < len; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static Boolean AlmostEquals(float float1, float float2, int precision = 1)
        {
            return (Math.Round(float1 - float2, precision) == 0);
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
        public static Boolean GetNameValue(string s, ref string name, ref string value)
        {
            int idx = s.IndexOf('=');

            if (idx == -1)
                return false;

            value = string.Empty;
            name = s.Substring(0, idx);

            if (idx+1 < s.Length)
            {
                value = s.Substring(idx + 1, s.Length - idx - 1);
            }

            name = name.Trim();
            value = value.Trim();

            return true;
        }

        //
        // This is to remove wired character in the port name retrived
        // from system call
        //
        public static string GetCorrectCOMPortName(string nameIn)
        {
            string name = nameIn;

            for (int i = 3; i < name.Length; i++)
            {
                if (!Char.IsNumber(name[i]))
                {
                    name = name.Remove(i);
                    return name;
                }
            }

            return name;
        }

        public static string RemoveCOMPortDescription(string portName)
        {
            int idx = portName.IndexOf('-');

            if (idx != -1)
            {
                portName = portName.Remove(idx);
                portName = portName.Trim();
            }

            return portName;
        }

        public static void AddArrayToList(ref List<byte> byteList, byte [] byteArray)
        {
            if (byteArray == null || byteList == null)
                return;

            for (int i = 0; i < byteArray.Length; i++)
            {
                byteList.Add(byteArray[i]);
            }
        }

        public static void RemoveTraingFilenameData(ref byte [] filenameData)
        {
            if (filenameData == null)
                return;

            for (int i = 0; i < filenameData.Length; i++)
            {
                if (filenameData[i] == '\0')
                {
                    for (int j = i+1; j < filenameData.Length; j++)
                    {
                        filenameData[j] = 0;
                    }
                }
            }
        }

        public static Boolean IsFilesTheSame(string sourceFilename, string targetFilename)
        {
            try
            {
                if (!File.Exists(sourceFilename) || !File.Exists(targetFilename))
                    return false;

                bool bFilesAreEqual = 
                    File.ReadAllBytes(sourceFilename).SequenceEqual(File.ReadAllBytes(targetFilename));

                return bFilesAreEqual;
            }
            catch (Exception e)
            {
                Console.WriteLine("Fail to do file comparison : " + e.Message);
                return false;
            }
        }

        public static string GenerateIPAddressFromArray(byte [] dataIn, Boolean useHex = true, 
                                                       Boolean reverseByte = true)
        {
            string s = string.Empty;
            int i;

            if (dataIn == null)
                return string.Empty;

            byte[] data = dataIn;

            if (reverseByte)
            {
                data = new byte[dataIn.Length];

                for (i = 0; i < dataIn.Length; i++)
                    data[i] = dataIn[dataIn.Length - i - 1];
            }
            
            for (i = 0; i < data.Length; i++)
            {
                if (useHex)
                    s += (data[i].ToString("X2"));
                else
                    s += (data[i]);

                if (i < data.Length - 1)
                    s += (".");
            }

            return s;
        }

        public static void DeleteAllFilesInDirectory(string directory)
        {
            foreach (string fileToDelete in System.IO.Directory.GetFiles(directory))
            {
                System.IO.File.Delete(fileToDelete);
            }
            //foreach (string subDirectoryToDeleteToDelete in System.IO.Directory.GetDirectories(directory))
            //{
            //    System.IO.Directory.Delete(subDirectoryToDeleteToDelete, true);
            //}
        }

    

        // Newly added
        static public int RetriveInteger(char[] data, int startIndex, int length)
        {
            string s = new string(data, startIndex, length);

            int value = Convert.ToInt32(s);
            return value;
        }

        static public int RetriveInteger(byte[] data, int startIndex, int length)
        {
            int value = 0;

            for (int i = 0; i < length; i++)
            {
                value += (data[startIndex + i] << (i * 8));
            }

            return value;
        }

        static public decimal RetriveDecimal(char[] data, int startIndex, int length)
        {
            string s = new string(data, startIndex, length);

            decimal value = Convert.ToDecimal(s);
            return value;
        }



        public static DateTime CreateDateTime(byte month, byte day, byte year)
        {
            int year2 = 2000 + year;

            if (month > 12 || month <= 0 || day <= 0 || day > 31)
                return new DateTime(1900, 1, 1);

            return new DateTime(year2, (int)month, (int)day);
        }

        public static string getValueFromXMLElement(string line)
        {
            int startIdx = line.IndexOf(">");
            int endIdx = line.IndexOf("</");

            if (startIdx != -1 && endIdx != -1 && endIdx > startIdx && ((endIdx - startIdx - 1) > 0))
            {
                string value = line.Substring(startIdx + 1, endIdx - startIdx - 1);
                return value;
            }

            return string.Empty;
        }



        public static string GetPMDStatsRecordDirectory(string pmdIMSI)
        {
            string directory = Environment.CurrentDirectory + "\\StatsRecord\\" + pmdIMSI;
            return directory;

        }

        public static string [] GetMachineIPV4Address()
        {
            List<string> ipv4AddressList = new List<string> ();

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipv4AddressList.Add(ip.ToString());
                }
            }

            if (ipv4AddressList.Count > 0)
                return ipv4AddressList.ToArray();
            else
                return null;
        }

        //
        // The following function is used to solve Windows file system case insensitive
        // problem. 
        //
        //1) Write file normally if no collision(e.g.Slow_Down.Txx).
        //
        //2) If collision, append @1, @2, @3, etc as needed(e.g.SLOW_DOWN@1.Txx)
        //    --Windows will maintain the case of the new file.
        //
        //3) In PCA tool file lists, show only the filename without the part with 
        //   the @ up to the '.' (e.g.SLOW_DOWN.Txx).
        //
        //4) Do not allow users to use @ as part of the filename--i.e.treat 
        //   it the same as *, /, \.
        //
        //  Note:
        //
        //  (1) filenameAcutal: If the filename exist, the filenameAcutal will 
        //      include the special @num and it is the actual filename which should be used.
        //
        //  (2) filenameNew: If the file doesn't exist, the filenameNew will be returned
        //      

        public static Boolean IsCaseSensitiveFileExist(string filename, ref string filenameActual, 
                                                       ref string filenameNew)
        {
            int num = 0;
            //
            // If filename already has special symbol, we remove it
            //
            filename = RemoveSpecialSymbolFromFilename(filename, ref num);
        
            string pureFilename = Path.GetFileNameWithoutExtension(filename);
            string filenameWithExt = Path.GetFileName(filename);
            string ext = Path.GetExtension(filename);
            string directory = Path.GetDirectoryName(filename);   
            string filter = pureFilename + "*" + ext;   
            string [] files = Directory.GetFiles(directory, filter).Select(Path.GetFileName).ToArray();
            string[] filesProcessed = new string[files.Length];
            Boolean findCaseInsenstiveFilenameMatch = false;

            int count = 0;

            for (int i = 0; i < files.Length; i++)
            {
                //
                // Now, we remove symbol @num from the filename
                // 
                filesProcessed[i] = RemoveSpecialSymbolFromFilename(files[i], ref num);

                // Get the lastest filename index number
                if (num > count)
                    count = num;
            }
     
            for (int i = 0; i < filesProcessed.Length; i++)
            {       
                if (filesProcessed[i].Equals(filenameWithExt))
                {
                    filenameActual = directory + "\\" + files[i];
                    return true;
                }

                // We have one filename case insensitive match
                if (filesProcessed[i].Length == filenameWithExt.Length)
                {
                    findCaseInsenstiveFilenameMatch = true;
                }
            }

            if (count == 0 && !findCaseInsenstiveFilenameMatch)
                filenameNew = directory + "\\" + pureFilename + ext;
            else
                filenameNew = directory + "\\" + pureFilename + "@" + (count+1) + ext;
        
            return false;
        }

        public static Boolean GetCaseSensitiveFilename(string filename, ref string filenameActual)
                                                    
        {
            int num = 0;
            //
            // If filename already has special symbol, we remove it
            //
            filename = RemoveSpecialSymbolFromFilename(filename, ref num);

            string pureFilename = Path.GetFileNameWithoutExtension(filename);
            string filenameWithExt = Path.GetFileName(filename);
            string ext = Path.GetExtension(filename);
            string directory = Path.GetDirectoryName(filename);
            string filter = pureFilename + "*" + ext;
            string[] files = Directory.GetFiles(directory, filter).Select(Path.GetFileName).ToArray();
            string[] filesProcessed = new string[files.Length];
            Boolean findCaseInsenstiveFilenameMatch = false;

            int count = 0;

            for (int i = 0; i < files.Length; i++)
            {
                //
                // Now, we remove symbol @num from the filename
                // 
                filesProcessed[i] = RemoveSpecialSymbolFromFilename(files[i], ref num);

                // Get the lastest filename index number
                if (num > count)
                    count = num;
            }

            for (int i = 0; i < filesProcessed.Length; i++)
            {
                if (filesProcessed[i].Equals(filenameWithExt))
                {
                    filenameActual = directory + "\\" + files[i];
                    return true;
                }

                // We have one filename case insensitive match
                if (filesProcessed[i].Length == filenameWithExt.Length)
                {
                    findCaseInsenstiveFilenameMatch = true;
                }
            }

            if (count == 0 && !findCaseInsenstiveFilenameMatch)
                filenameActual = directory + "\\" + pureFilename + ext;
            else
                filenameActual = directory + "\\" + pureFilename + "@" + (count + 1) + ext;

            return false;
        }

        // If we successfully remove the spcial symbol, the count will store
        // the number removed
        public static string RemoveSpecialSymbolFromFilename(string filename, ref int count)
        {
            int idx1, idx2;   
                   
            idx1 = filename.IndexOf('@');
            idx2 = filename.IndexOf('.');

            count = 0;
            
            if (idx1 == -1 || idx2 == -1)
                return filename;

            string str = filename;

            str = filename.Remove(idx2);
            str = str.Remove(0, idx1+1);

            if (str.Length > 0)
                count = Int32.Parse(str);

            return filename.Remove(idx1, (idx2 - idx1));
        }

        public static byte [] CalculateMD5(string filename)
        {
            if (!File.Exists(filename))
                return null;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        //
        // Filename example:
        // “5543_Frisco_Controller_18_Inch_Rev_1_Release_v0.118.7.4.bin”
        //
        public static byte[] GetFirmwareVersions(string filename)
        {
            if (!File.Exists(filename))
                return null;

            int idx = filename.IndexOf("_v");

            if (idx == -1)
                return null;

            string name = filename.Remove(0, idx+2);

            string[] segArray = name.Split('.');

            if (segArray.Length < 4)
                return null;

            byte[] versions = new byte[4];

            try
            { 
                for (int i = 0; i < 4; i++)
                {
                    versions[i] = Convert.ToByte(segArray[i]);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return versions;
        }

        // Filename example:
        // “5543_Frisco_Controller_18_Inch_Rev_1_Release_v0.118.7.4.bin”
        //
        public static int GetFirmwareModelNumber(string filename)
        {
            if (!File.Exists(filename))
                return 0;

            filename = Path.GetFileName(filename);

            int idx = filename.IndexOf("_");

            if (idx == -1)
                return 0;

            string inputString = filename.Remove(idx);

            int modelNumber;
            bool parsed = Int32.TryParse(inputString, out modelNumber);

            if (parsed)
                return modelNumber;
            else
                return 0;
        }

        // Retrieve version number array (max is 4)
        public static byte[] GetVersions(string versionString)
        {
            string[] segArray = versionString.Split('.');

            if (segArray.Length == 0)
                return null;

            int num = segArray.Length;

            if (num > 4)
                num = 4;

            byte[] versions = new byte[num];

            try
            {
                for (int i = 0; i < num; i++)
                {
                    versions[i] = Convert.ToByte(segArray[i]);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return versions;
        }

    }
}
