using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace XjjXmm.FrameWork.LogExtension
{
	public interface ILog
	{
		void Debug(string messageTemplate);
		void Debug(string messageTemplate, params object?[]? propertyValues);

		void Information(string messageTemplate);
		void Information(string messageTemplate, params object?[]? propertyValues);

		void Trace(string messageTemplate);
		void Trace(string messageTemplate, params object?[]? propertyValues);

		void Critical(Exception exception, string messageTemplate);

		void Error(Exception exception, string messageTemplate);
	}
	
	public interface ILog<T> : ILog
    {
      
	}
}
