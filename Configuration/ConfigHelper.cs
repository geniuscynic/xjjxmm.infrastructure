using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog.Core;
using Ubiety.Dns.Core.Records.NotUsed;
using XjjXmm.Infrastructure.Common;
using XjjXmm.Infrastructure.ToolKit;
using XjjXmm.Infrastructure.Webs.Clients;

namespace XjjXmm.Infrastructure.Configuration
{
	public static class ConfigHelper
	{
		// private static ILog<DefaultLogger> Logger => App.Logger;
		public static IConfiguration Configuration { get; set; } // = Scan();


		static ConfigHelper()
		{
			//new ConfigBuilder().Build();

			//ConfigBuilder.GetInstance().GetAwaiter().GetResult();
			//var filePath = AppContext.BaseDirectory;

			/*
			config = new ConfigurationBuilder()
				.SetBasePath(filePath);*/
			// Configuration = Scan();
		}


		public static string GetConfig(params string[] sections)
		{
			try
			{
				if (sections.Any())
				{
					return Configuration[string.Join(":", sections)];
				}
			}
			catch (Exception ex)
			{
				//Log.Error($"{ToKey(sections)} 不存在", ex);
				Serilog.Log.Error(ex, $"{ToKey(sections)} 不存在");
			}

			return "";
		}

		public static T GetSection<T>(params string[] keys) where T : class
		{
			try
			{
				if (keys.Any())
				{
					return Configuration.GetSection(ToKey(keys)).Get<T>();
				}
			}
			catch (Exception ex)
			{
				//Logger.Error($"{ToKey(keys)}: 不存在", ex);

				Serilog.Log.Error(ex, $"{ToKey(keys)} 不存在");
			}

			return null;
		}


		private static string ToKey(params string[] keys)
		{
			return string.Join(":", keys);
		}

		/// <summary>
		/// 加载配置文件
		/// </summary>
		/// <param name="fileName">文件名称</param>
		/// <param name="environmentName">环境名称</param>
		/// <param name="reloadOnChange">自动更新</param>
		/// <returns></returns>
		public static IConfigurationBuilder Scan(this IConfigurationBuilder configurationBuilder, string environmentName = "", bool reloadOnChange = false)
		{
			//var filePath = AppContext.BaseDirectory;

			/*config = new ConfigurationBuilder()
				.SetBasePath(filePath);*/


			var configFilePath = Path.Combine(AppContext.BaseDirectory, "configs");
			if (Directory.Exists(configFilePath))
			{
				
				foreach (var enumerateFile in Directory.EnumerateFiles(configFilePath, "*.json"))
				{
					

					var file = new FileInfo(enumerateFile);

					configurationBuilder.AddJsonFile("configs/" + file.Name, true, reloadOnChange);
					
				}
			}
			else
			{
				Directory.CreateDirectory(configFilePath);
			}

			configurationBuilder.AddJsonFile("appsettings.json", true, reloadOnChange);
			configurationBuilder.AddJsonFile($"appsettings.{environmentName}.json", true, reloadOnChange);
			return configurationBuilder;
			
		}

		private class ConfigBuilder
		{
			private IConfigurationBuilder config;

			private static ConfigBuilder configBuilder;
			
			public ConfigBuilder(string environmentName = "", bool reloadOnChange = false)
			{
				config = Scan(environmentName, reloadOnChange);
			}
			
			private  async Task<ConfigBuilder> LoadFromNetwork()
			{
				try
				{
					/*var configHost = ConfigHelper.GetConfig("configHost");
					var appId = ConfigHelper.GetConfig("appId");

					if (configHost.IsNullOrEmpty() || appId.IsNullOrEmpty())
					{
						return this;
					}

					var configsResponseModel = new WebClient().Get($"{configHost}/api/XjjXmmConfig/{appId}").ResultFromJsonAsync<ResponseModel<IEnumerable<ConfigDto>>>().Result;
					//var jsonConfig = configsResponseModel.Data.Select(t=>new KeyValuePair<string, object?>( t.Key, JsonConvert.DeserializeObject(t.Value))).ToList();*/

					var configClient = new ConfigClient();
					var configsResponseModel = await configClient.Get();

					if (configsResponseModel != null && configsResponseModel.Any())
					{

						var jsonConfig = configsResponseModel.ToDictionary(t => t.Key, t => JsonConvert.DeserializeObject(t.Value));


						AddJsonConfig(configClient.AppId, jsonConfig);
					}
				}
				catch(Exception ex)
				{
					Serilog.Log.Error(ex, "LoadFromNetwork");
				}

					return this;
				
			}
			/// <summary>
			/// 加载配置文件
			/// </summary>
			/// <param name="fileName">文件名称</param>
			/// <param name="environmentName">环境名称</param>
			/// <param name="reloadOnChange">自动更新</param>
			/// <returns></returns>
			private IConfigurationBuilder Scan(string environmentName = "", bool reloadOnChange = false)
			{
				//var filePath = Path.Combine(AppContext.BaseDirectory, "configs");
				//if (!Directory.Exists(filePath))
				//    return;


				var filePath = AppContext.BaseDirectory;

				config = new ConfigurationBuilder()
					.SetBasePath(filePath);


				var configFilePath = Path.Combine(AppContext.BaseDirectory, "configs");
				if (Directory.Exists(configFilePath))
				{
					//var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
					//var environment = config["Environment"];
					foreach (var enumerateFile in Directory.EnumerateFiles(configFilePath, "*.json"))
					{
						//if(enumerateFile.ToLower().Contains(".development.") && environment?.ToLower() != "development")
						//{
						//      continue;
						//}
						//else if (!enumerateFile.ToLower().Contains(".development.") && environment?.ToLower() == "development")
						//{
						//    continue;
						//}
						//else  if (!enumerateFile.ToLower().EndsWith(".json"))
						//{
						//    continue;
						//}

						var file = new FileInfo(enumerateFile);
						//if (!file.Name.ToLower().EndsWith(".json"))
						//{
						//     continue;
						//}

						//var files = file.Name.Split(".");
						//if (files.Length == 2)
						//{
							config.AddJsonFile("configs/" + file.Name, true, reloadOnChange);
						//}
					}


					/*foreach (var enumerateFile in Directory.EnumerateFiles(configFilePath, "*.json"))
					{
						//if(enumerateFile.ToLower().Contains(".development.") && environment?.ToLower() != "development")
						//{
						//      continue;
						//}
						//else if (!enumerateFile.ToLower().Contains(".development.") && environment?.ToLower() == "development")
						//{
						//    continue;
						//}
						//else  if (!enumerateFile.ToLower().EndsWith(".json"))
						//{
						//    continue;
						//}

						var file = new FileInfo(enumerateFile);


						var files = file.Name.Split(".");
						if (files.Length == 3 && files[1] == environmentName)
						{
							config.AddJsonFile("configs/" + file.Name, true, reloadOnChange);
						}
					}*/
				}
				else
				{
					Directory.CreateDirectory(configFilePath);
				}

				config.AddJsonFile("appsettings.json", true, reloadOnChange);

				return config;
				//return config.Build();

				//var builder = new ConfigurationBuilder()
				//    .SetBasePath(filePath)
				//    .AddJsonFile(fileName.ToLower() + ".json", true, reloadOnChange);

				//if (environmentName.NotNull())
				//{
				//    builder.AddJsonFile(fileName.ToLower() + "." + environmentName + ".json", optional: true, reloadOnChange: reloadOnChange);
				//}

				//return builder.Build();
			}

			private ConfigBuilder AddJsonConfig(string appId ,object json)
			{
				if(json == null) return this;

				if (json is ICollection array && array.Count == 0) return this;

				
				var jsonStr = json.ToJson();

				var configFilePath = Path.Combine(AppContext.BaseDirectory, "configs", $"{appId}.json");
				File.WriteAllText(configFilePath, jsonStr);

				config.AddJsonFile(configFilePath, true, true);
				//byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);
				//MemoryStream mem = new MemoryStream();

				//mem.Write(bytes, 0, bytes.Length);

				//config.AddJsonStream(mem);

				//config.AddInMemoryCollection(json);

				return this;
			}

			public void Build()
			{
				//config = Scan(environmentName, reloadOnChange);
				
				ConfigHelper.Configuration = config.Build();
			}
		}
	}
}
