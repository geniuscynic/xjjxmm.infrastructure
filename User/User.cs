using EasyCaching.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XjjXmm.Infrastructure.ToolKit;
using XjjXmm.Infrastructure.Cache;
using XjjXmm.Infrastructure.Constant;
using System.Security.Claims;

namespace XjjXmm.Infrastructure.User
{
	public class UserContext : IUserContext
	{
		private readonly IHttpContextAccessor _accessor;

		private readonly IEasyCachingProvider _cache;

		public UserContext(IHttpContextAccessor accessor, IEasyCachingProvider cache)
		{
			//_cache = cache;

			//cache.Get<l>($"user_{Id}");
			//Id = accessor.HttpContext.User.Identity?.Name?.ToLong() ?? 0;
			//UserName = cache.Get<String>($"{CacheKey.UserName}:{Id}");
			//NickName = cache.Get<String>($"{CacheKey.NickName}:{NickName}");
			_accessor = accessor;
			_cache = cache;
		}

		public string Id => _accessor?.HttpContext?.User.Claims?.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value??"";

		//return _accessor?.HttpContext?.User.Identity?.Name?.ToLong() ?? 0;
		public bool IsAccess => _accessor?.HttpContext?.User.Claims?.FirstOrDefault(t => t.Type == "IsAccess")?.Value != "0";

		public string ClientId => _accessor?.HttpContext?.User.Claims?.FirstOrDefault(t => t.Type == "ClientId")?.Value ?? "";

		public string Token => _accessor?.HttpContext?.Request?.Headers["Authorization"].ToString()?.Replace("Bearer", "") ?? "";

		//public string UserName => "";

		//public string NickName => "";


		public string LoginName
		{
			get
			{
				try
				{
					return _cache?.Get<String>($"{CacheKey.UserName}:{Id}")?.Value ?? "";
				}
				catch
				{
					return "";
				}
			}
		}

		public string NickName
		{
			get
			{
				try
				{
					return _cache?.Get<String>($"{CacheKey.NickName}:{Id}")?.Value ?? "";
				}
				catch
				{
					return "";
				}
			}
		}

	}
}
