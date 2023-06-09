using System;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json;

namespace XjjXmm.Infrastructure.ToolKit
{
    public static class ObjectExtension
    {
		/// <summary>
		/// 安全转换为字符串，去除两端空格，当值为null时返回""
		/// </summary>
		/// <param name="input">输入值</param>
		public static string ToSafeString(this object input)
		{
			return input?.ToString()?.Trim() ?? string.Empty;
		}
		
		/// <summary>
		/// 是否空值
		/// </summary>
		/// <returns></returns>
		public static bool IsNullOrEmpty(object value, PropertyInfo property)
        {
            if (value == null) return true;

            if (value is ICollection array && array.Count == 0) return true;


            if (TypeKit.IsDateTime(property.PropertyType))
            {
                if ((DateTime)value == DateTime.MinValue)
                {
                    return true;
                }
            }


            if (string.IsNullOrEmpty(value.ToString())) return true;

            return false;
        }

        public static string ToValue(this object arg)
        {
            if (arg is DateTime || arg is DateTime?)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            if (arg is string || arg is ValueType || arg is Nullable)
                return arg.ToString();

            if (arg != null)
            {
                if (arg.GetType().IsClass)
                {
                    return JsonConvert.SerializeObject(arg);
                    //;.ToMD5Encrypt();
                }
                else
                {
                    //var res = arg.GetHashCode() + arg.ToString();
                    return arg.ToString();
                }
            }
            return string.Empty;
        }
       
    }
}
