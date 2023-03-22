using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XjjXmm.Infrastructure.ToolKit
{
	public class AppKit
	{
		public static string GetCurrentDirectory()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}

		public static void AddEnvironmentPaths(IEnumerable<string> paths)
		{
			var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

			string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(paths));

			Environment.SetEnvironmentVariable("PATH", newPath);
		}

		public static void AddEnvironmentPaths(string paths)
		{
			var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

			if (path.Any(t => t.Contains(paths)))
			{
				return;
			}

			string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(new string[] { paths }));

			Environment.SetEnvironmentVariable("PATH", newPath);
		}
	}
}
