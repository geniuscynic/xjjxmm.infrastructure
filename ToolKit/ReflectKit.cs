// Copyright (c) 2020-2021 百小僧, Baiqian Co.,Ltd.
// Furion is licensed under Mulan PSL v2.
// You can use this software according to the terms and conditions of the Mulan PSL v2.
// You may obtain a copy of Mulan PSL v2 at:
//             https://gitee.com/dotnetchina/Furion/blob/master/LICENSE
// THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.
// See the Mulan PSL v2 for more details.

using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using XjjXmm.Infrastructure.Exceptions;

namespace XjjXmm.Infrastructure.ToolKit
{
    /// <summary>
    /// 内部反射静态类
    /// </summary>
    public static class ReflectKit
    {
        /// <summary>
        /// 获取入口程序集
        /// </summary>
        /// <returns></returns>
        public static Assembly GetEntryAssembly()
        {
	        return Assembly.GetExecutingAssembly();
	        //return Assembly.GetEntryAssembly();
        }

        //internal static IEnumerable<Assembly> AllAssemblies()
        //{
        //   return DependencyContext.Default.RuntimeLibraries
        //        .Where(u =>
        //        {
        //            //Serilog.Log.Debug(u.Type);
        //            //Console.WriteLine(u.Type);
        //            return u.Type == "project";
        //        })
        //        .Select(u => ReflectKit.GetAssembly(u.Name));
        //  // return AssemblyLoadContext.Default.Assemblies;
        //}

        /// <summary>
        /// 根据程序集名称获取运行时程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(string assemblyName)
        {
            // 加载程序集
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }

        /// <summary>
        /// 根据程序集名称获取运行时程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(Type type)
        {
            // 加载程序集
            return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(GetAssemblyName(type)));
        }

        /// <summary>
        /// 根据路径加载程序集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Assembly? LoadAssembly(string path)
        {
            if (!File.Exists(path)) return default;
            //return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            return Assembly.LoadFrom(path);
        }

        /// <summary>
        /// 通过流加载程序集
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        internal static Assembly LoadAssembly(MemoryStream assembly)
        {
            return Assembly.Load(assembly.ToArray());
        }

        /// <summary>
        /// 根据程序集名称、类型完整限定名获取运行时类型
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        internal static Type? GetType(string assemblyName, string typeFullName)
        {
            return GetAssembly(assemblyName)?.GetType(typeFullName);
        }

        /// <summary>
        /// 根据程序集和类型完全限定名获取运行时类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        internal static Type? GetType(Assembly assembly, string typeFullName)
        {
            return assembly.GetType(typeFullName);
        }

        /// <summary>
        /// 根据程序集和类型完全限定名获取运行时类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        internal static Type? GetType(MemoryStream assembly, string typeFullName)
        {
            return LoadAssembly(assembly).GetType(typeFullName);
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        internal static string? GetAssemblyName(Assembly assembly)
        {
            return assembly.GetName()?.Name;
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetAssemblyName(Type type)
        {
            return GetAssemblyName(type.GetTypeInfo());
        }

        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        internal static string? GetAssemblyName(TypeInfo typeInfo)
        {
            return GetAssemblyName(typeInfo.Assembly);
        }


		public static object? GetFieldValue(this object obj, string fieldName)
		{
			
				var type = TypeKit.GetType(obj.GetType());

				var propertyInfo = type.GetProperty(fieldName);
				
			if(propertyInfo == null)
			{
				Log.Information("{@Obj}里面找不到{fieldName}", obj, fieldName);
				throw new BussinessException(StatusCodes.Status404NotFound, $"找不到{fieldName}");
			}

			return propertyInfo.GetValue(obj);
			
		}

		public static void SetFieldValue(this object obj, string fieldName, object? value)
		{
			var type = TypeKit.GetType(obj.GetType());

			var propertyInfo = type.GetProperty(fieldName);

			if (propertyInfo == null)
			{
				Log.Information("{@Obj}里面找不到{fieldName}", obj, fieldName);
				throw new BussinessException(StatusCodes.Status404NotFound, $"找不到{fieldName}");
			}

			propertyInfo.SetValue(obj, value);
		}

	}
}
