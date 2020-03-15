using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    }
}
