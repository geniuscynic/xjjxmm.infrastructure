using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XjjXmm.Infrastructure.Common;
using XjjXmm.Infrastructure.Constant;
using XjjXmm.Infrastructure.ToolKit;
using XjjXmm.Infrastructure.Webs.Clients;

namespace XjjXmm.Infrastructure.Configuration
{
	public class ConfigClient
	{
		private readonly string? appId;
		private readonly string? host;

		public string? AppId => appId;
		public string? Host => host;

		public ConfigClient()
		{
			host = ConfigHelper.GetConfig(CommonConstant.ConfigHost);
			appId = ConfigHelper.GetConfig(CommonConstant.AppId);
		}

		public ConfigClient(string appId, string host)
		{
			this.appId = appId;
			this.host = host;
		}

		public async Task<IEnumerable<ConfigDto>?> Get()
		{
			if (host.IsNullOrEmpty() || appId.IsNullOrEmpty())
			{
				return default;
			}

			var res = await new WebClient().Get($"{host}/api/XjjXmmConfig/{appId}")
				.ResultFromJsonAsync<Response<IEnumerable<ConfigDto>>>();

			return res.Result;

		}

		// GET api/<XjjXmmConfigController>/5
		public async Task<string?> Get(string key)
		{
			if (host.IsNullOrEmpty() || appId.IsNullOrEmpty() || key.IsNullOrEmpty())
			{
				return default;
			}

			var res = await new WebClient().Get($"{host}/api/XjjXmmConfig/{appId}/{key}")
				.ResultFromJsonAsync<Response<string>>();

			return res.Result;
		}


		public async Task UpdateValue(string group, string key, string value)
		{
			if(group == null)
			{
				group = "";
			}

			if (host.IsNullOrEmpty() || appId.IsNullOrEmpty() || key.IsNullOrEmpty())
			{
				return;
			}

			var res = await new WebClient().Put($"{host}/api/XjjXmmConfig/{appId}/{group}_{key}")
				.JsonData(value)
				.ResultFromJsonAsync<Response<string>>();

			var a = "";
		}
	}
}
