using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Concurrent;
using XjjXmm.Infrastructure.Constant;
using XjjXmm.Infrastructure.ToolKit;

namespace XjjXmm.Infrastructure.Configuration
{
	public class CustomConfigurationProvider : ConfigurationProvider
	{

		private readonly string? appId;
		private readonly string? host;
		public CustomConfigurationProvider()
		{
			var jsonConfig = new JsonConfigurationSource();
			jsonConfig.FileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
			jsonConfig.Path = "appsettings.json";
			var jsonProvider = new JsonConfigurationProvider(jsonConfig);
			jsonProvider.Load();
			
			
			jsonProvider.TryGet(CommonConstant.ConfigHost, out string host);
			jsonProvider.TryGet(CommonConstant.AppId, out string appId);
			
			//jsonProvider.TryGet("myconfigServer", out string serverAddress);

			/*if (string.IsNullOrEmpty(host))
			{
				throw new Exception("Can not find configHost  from appsettings.json");
			}
			
			if (string.IsNullOrEmpty(appId))
			{
				throw new Exception("Can not find appId  from appsettings.json");
			}
			*/

			this.host = host;
			this.appId = appId;
		}
		
		public override void Load()
		{
			try
			{
				var configClient = new ConfigClient(appId, host);
				var configsResponseModel = configClient.Get().GetAwaiter().GetResult();

				if (configsResponseModel != null && configsResponseModel.Any())
				{
					var jsonConfig = configsResponseModel
						.ToDictionary(t => t.JsonKey,
							t => t.Value);

					WriteToLocal(jsonConfig);
					
					foreach (KeyValuePair<string, string> keyValuePair in jsonConfig)
					{
						Data.Add(keyValuePair);
					}
					// AddJsonConfig(configClient.AppId, jsonConfig);
				}
			}
			catch(Exception ex)
			{
				Serilog.Log.Error(ex, "LoadFromNetwork");
			}
			
			//var response = ReadFromLocal();
			
			//var configs = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(response);

			//Data = new ConcurrentDictionary<string, string>();

			/*configs.ForEach(c =>
			{
				Data.Add(c);
			});*/
			
		}
		
		private void WriteToLocal(IDictionary<string, string> dict)
		{
			//if(dict == null) return;

			//if (json is ICollection array && array.Count == 0) return;

				
			var jsonStr = DictionaryConvertToJson.ToJson(dict);

			var configFilePath = Path.Combine(AppContext.BaseDirectory, "configs", $"{appId}.json");
			File.WriteAllText(configFilePath, jsonStr);
		}

		private string ReadFromLocal()
		{
			var configFilePath = Path.Combine(AppContext.BaseDirectory, "configs", $"{appId}.json");

			return File.ReadAllText(configFilePath);
		}
	}
}
