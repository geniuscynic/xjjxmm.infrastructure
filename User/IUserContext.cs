using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XjjXmm.Infrastructure.User
{
    public interface IUserContext
    {
        string Id { get;  }

        string LoginName { get;  }

        string NickName { get;  }
        
        bool IsAccess { get; }

		string ClientId { get; }

		string Token { get; }
	}


}
