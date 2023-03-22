using System;
using System.Linq;
using System.Reflection;

namespace XjjXmm.Infrastructure.ToolKit
{
    public sealed class TypeKit
    {

		/// <summary>
		/// 获取类型
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		public static Type GetType<T>()
		{
			return GetType(typeof(T));
		}

		/// <summary>
		/// 获取类型
		/// </summary>
		/// <param name="type">类型</param>
		public static Type GetType(Type type)
		{
			return Nullable.GetUnderlyingType(type) ?? type;
		}
		
		// private static Logger logger = Logger.GetLogger(typeof(TypeKit));
		/// <summary>
		/// 检验是否是数值型
		/// </summary>
		/// <param name="type">类型</param>
		/// <returns></returns>
		public static bool IsNumeric(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type");

            return IsFloat(type) || IsInteger(type);
        }

        /// <summary>
        /// 检验是否是字符
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsString(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type");

            Type temp = type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                temp = type.GetGenericArguments()[0];
            }
            return temp == typeof(string)
                    || temp == typeof(string)
                    || temp == typeof(char)
                    || temp == typeof(char)
                   ;
        }

        /// <summary>
        /// 检验是否是布尔型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsBoolean(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type");

            Type temp = type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                temp = type.GetGenericArguments()[0];
            }
            return temp == typeof(bool)
                    || temp == typeof(bool)
                   ;
        }

        /// <summary>
        /// 检验是否是整型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsInteger(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type");
            Type temp = type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                temp = type.GetGenericArguments()[0];
            }
            return temp == typeof(short)
                    || temp == typeof(int)
                    || temp == typeof(long)
                    || temp == typeof(short)
                    || temp == typeof(int)
                    || temp == typeof(long)
                    || temp == typeof(ushort)
                    || temp == typeof(uint)
                    || temp == typeof(ulong)
                    || temp == typeof(ushort)
                    || temp == typeof(uint)
                    || temp == typeof(ulong)
                    || temp == typeof(sbyte)
                    || temp == typeof(sbyte)
                    || temp == typeof(byte)
                    || temp == typeof(byte)
                   ;
        }

        /// <summary>
        /// 检验是否是浮点型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsFloat(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type");

            Type temp = type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                temp = type.GetGenericArguments()[0];
            }
            return temp == typeof(double)
                    || temp == typeof(double)
                    || temp == typeof(decimal)
                    || temp == typeof(decimal)
                    || temp == typeof(float)
                    || temp == typeof(float)
                   ;
        }

        /// <summary>
        /// 检验是否是数值，字符，布尔型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsSimpleType(Type type)
        {
            return IsNumeric(type) || IsString(type) || IsBoolean(type);
        }

        /// <summary>
        /// 检验是否是日期型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsDateTime(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Type");

            Type temp = type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                temp = type.GetGenericArguments()[0];
            }
            return temp == typeof(DateTime);
        }

        /// <summary>
        /// 是否可空类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否可空</returns>
        public static bool CanNotNull(Type type)
        {
            return IsBoolean(type)
                || IsNumeric(type)
                || type == typeof(char)
                || type == typeof(char)
                || type.IsValueType && !IsNullable(type);
        }

        /// <summary>
        /// 获取第一个实现该接口的子类
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="parentType">父类型</param>
        /// <returns>类型</returns>
        public static Type? GetFirstSubClass(Assembly assembly, Type parentType)
        {
            if (parentType == null)
            {
                throw new Exception("Invalidate parent type.");
            }
            return assembly.GetTypes()?.FirstOrDefault(m => m.IsSubclassOf(parentType));
        }

        /// <summary>
        /// 是否是可空类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>判断结果</returns>
        public static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 取得可空类型的原始类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>原始类型</returns>
        public static Type GetNullableType(Type type)
        {
            if (IsNullable(type))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }

        /// <summary>
        /// 获取类型的值
        /// </summary>
        /// <param name="type">格式化类型</param>
        /// <param name="obj">转换源</param>
        /// <returns>转换目标</returns>
        public static object? FormatValue(Type type, object obj)
        {
            if (obj == null) return default;
            if (type.IsAssignableFrom(obj.GetType())) return obj;
            Type t = GetNullableType(type);
            if (t == typeof(short))
            {
                return Convert.ToInt16(obj);
            }
            else if (t == typeof(int))
            {
                return Convert.ToInt32(obj);
            }
            else if (t == typeof(long))
            {
                return Convert.ToInt64(obj);
            }
            else if (t == typeof(decimal))
            {
                return Convert.ToDecimal(obj);
            }
            else if (t == typeof(string))
            {
                return Convert.ToString(obj);
            }
            else if (t == typeof(DateTime))
            {
                return Convert.ToDateTime(obj);
            }
            else if (t == typeof(Guid))
            {
                return new Guid(BitConverter.ToString((byte[])obj).Replace("-", ""));
            }
            return obj;
        }
    }
}
