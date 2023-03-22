using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XjjXmm.Infrastructure.ToolKit
{
	public class FileKit
	{
		public static void SpiteFile(string path)
		{
			var rows = 1000;
			var current = 0;
			var index = 1;
			var fileStream = new FileStream(path, FileMode.Open);
			StreamReader sr = new StreamReader(fileStream);

			List<string> lines = new List<string>();
			var line = sr.ReadLine();


			while (line != null)
			{
				current++;
				lines.Add(line);

				line = sr.ReadLine();

				if (current >= rows || line == null)
				{
					File.WriteAllLines($"note/{index.ToString().PadLeft(6, '0')}.txt", lines);
					current = 0;
					lines = new List<string>();
					index++;
				}
			}
		}

		// 声明INI文件的写操作函数 WritePrivateProfileString()
		[System.Runtime.InteropServices.DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		// 声明INI文件的读操作函数 GetPrivateProfileString()
		[System.Runtime.InteropServices.DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);


		/// 写入INI的方法
		public static void INIWrite(string section, string key, string value, string path)
		{
			// section=配置节点名称，key=键名，value=返回键值，path=路径
			WritePrivateProfileString(section, key, value, path);
		}

		//读取INI的方法
		public static string INIRead(string section, string key, string path)
		{
			// 每次从ini中读取多少字节
			System.Text.StringBuilder temp = new System.Text.StringBuilder(255);

			// section=配置节点名称，key=键名，temp=上面，path=路径
			GetPrivateProfileString(section, key, "", temp, 255, path);
			return temp.ToString();

		}

		//删除一个INI文件
		public static void INIDelete(string FilePath)
		{
			File.Delete(FilePath);
		}

	}
}
