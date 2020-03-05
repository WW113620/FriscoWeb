using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FriscoDev.UI.Common
{
    public static class ObjectExpansion
    {
        public static string ToJson(this object obj, string dateFormate = "")
        {

            if (!string.IsNullOrEmpty(dateFormate))
            {
                IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
                timeConverter.DateTimeFormat = dateFormate;
                return JsonConvert.SerializeObject(obj, timeConverter);
            }
            return JsonConvert.SerializeObject(obj);
        }
        public static PropertyInfo[] GetPropertyInfos(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
        public static void AutoMapping<S, T>(S s, T t)
        {
            // get source PropertyInfos
            PropertyInfo[] pps = GetPropertyInfos(s.GetType());
            // get target type
            Type target = t.GetType();

            foreach (var pp in pps)
            {
                PropertyInfo targetPp = target.GetProperty(pp.Name);
                object value = pp.GetValue(s, null);
                //if (targetPp != null && value != null)
                if (targetPp != null)
                {
                    try
                    {
                        if (targetPp.PropertyType.Name != typeof(List<>).Name)
                        {
                            targetPp.SetValue(t, value, null);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
    }
}