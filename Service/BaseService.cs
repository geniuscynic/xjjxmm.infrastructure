using EasyCaching.Core;
using XjjXmm.Infrastructure.Ioc;
using XjjXmm.Infrastructure.User;

namespace xjjxmm.infrastructure.service
{
    public class BaseService { 

        protected static readonly IUserContext _userContext = IocManager.Instance.GetService<IUserContext>();

		protected static readonly IEasyCachingProvider _cache = IocManager.Instance.GetService<IEasyCachingProvider>();
	}
}
