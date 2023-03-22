using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XjjXmm.Infrastructure.Constant
{
    public class CacheKey
    {
	    public const string AuthToken = "user:code";
	    public const string AccessToken = "user:accessCode";
	    
        public const string User = "user";
        public const string UserName = "user:login";
        public const string NickName = "user:name";
        public const string TentantId = "user:tentantId";
    }

    public class CommonConstant
    {
	    public const string AppId = "xjjxmm:appId";
	    public const string ConfigHost = "xjjxmm:configHost";

	    public const string Jwt = "jwt";
    }
}
