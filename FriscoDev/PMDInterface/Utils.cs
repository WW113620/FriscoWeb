using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using uint8_t = System.Byte;
using uint32_t = System.UInt32;
using int16_t = System.Int16;
using uint16_t = System.UInt16;
using int8_t = System.Byte;

namespace PMDInterface
{
    public class Utils
    {
        public static Boolean GetNameValue(string s, ref string name, ref string value)
        {
            int idx = s.IndexOf('=');

            if (idx == -1)
                return false;

            value = string.Empty;
            name = s.Substring(0, idx);

            if (idx + 1 < s.Length)
            {
                value = s.Substring(idx + 1, s.Length - idx - 1);
            }

            name = name.Trim();
            value = value.Trim();

            return true;
        }

        public static void AddArrayToList(ref List<byte> byteList, byte[] byteArray)
        {
            if (byteArray == null || byteList == null)
                return;

            for (int i = 0; i < byteArray.Length; i++)
            {
                byteList.Add(byteArray[i]);
            }
        }

        static public uint16_t U16ComputeCRC(byte[] data, int startIdx, int len)
        {
            int u8Bit, i;
            uint16_t u16CRC = 0xFFFF;
            uint16_t u16Odd;

            for (i = startIdx; i < startIdx + len; i++)
            {
                u16CRC ^= ((uint16_t)(data[i] << 8));

                for (u8Bit = 0; u8Bit < 8; u8Bit++)
                {
                    u16Odd = (uint16_t)(u16CRC & 0x8000);

                    u16CRC <<= 1;

                    if (u16Odd == 0x8000)
                    {
                        u16CRC ^= 0x1021;  //C13 + C6 + C1
                    }
                }
            }

            return (u16CRC);
        }

        static public uint16_t U16ComputeCRC(uint16_t u16CRC, byte extraByteData)
        {
            int u8Bit;
            uint16_t u16Odd;

            u16CRC ^= ((uint16_t)(extraByteData << 8));

            for (u8Bit = 0; u8Bit < 8; u8Bit++)
            {
                u16Odd = (uint16_t)(u16CRC & 0x8000);

                u16CRC <<= 1;

                if (u16Odd == 0x8000)
                {
                    u16CRC ^= 0x1021;  //C13 + C6 + C1
                }
            }

            return (u16CRC);
        }


    }
}
