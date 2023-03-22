using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace XjjXmm.FrameWork.LogExtension
{
    public class XjjXmmLoggerHelper
    {
        private Serilog.ILogger _logger;

        private XjjXmmLoggerHelper(Serilog.ILogger logger)
        {
			this._logger = logger;
			//_logger = Serilog.Log.ForContext<T>();
        }

        public static XjjXmmLoggerHelper GetLogger<T>()
        {
			return new XjjXmmLoggerHelper(Serilog.Log.ForContext<T>());
        }

		public static XjjXmmLoggerHelper GetLogger()
		{
			return new XjjXmmLoggerHelper(Serilog.Log.Logger);
		}

		public void Debug(string messageTemplate)
        {
            _logger.Debug(messageTemplate);
        }

		public void Debug(string messageTemplate, params object?[]? propertyValues)
		{
			_logger.Debug(messageTemplate, propertyValues);
		}
		
		public void Information(string messageTemplate)
        {
            _logger.Information(messageTemplate);
        }

		public void Information(string messageTemplate, params object?[]? propertyValues)
		{
			_logger.Information(messageTemplate, propertyValues);
		}

		public void Trace(string messageTemplate)
        {
            _logger.Verbose(messageTemplate);
        }

		public void Trace(string messageTemplate, params object?[]? propertyValues)
		{
			_logger.Verbose(messageTemplate, propertyValues);
		}

		public void Error(Exception exception, string messageTemplate)
        {
            _logger.Error(exception, messageTemplate);
        }

        public void Critical(Exception exception, string messageTemplate)
        {
            _logger.Fatal(exception, messageTemplate);
        }
    }

	public class XjjXmmSeriLogger<T> : ILog<T>
	{

		public void Debug(string messageTemplate)
		{
			XjjXmmLoggerHelper.GetLogger<T>	 ().Debug(messageTemplate);
		}

		public void Debug(string messageTemplate, params object?[]? propertyValues)
		{
			XjjXmmLoggerHelper.GetLogger<T>().Debug(messageTemplate, propertyValues);
		}

		public void Information(string messageTemplate)
		{
			XjjXmmLoggerHelper.GetLogger<T>().Information(messageTemplate);
		}

		public void Information(string messageTemplate, params object?[]? propertyValues)
		{
			XjjXmmLoggerHelper.GetLogger<T>().Information(messageTemplate, propertyValues);
		}

		public void Trace(string messageTemplate)
		{
			XjjXmmLoggerHelper.GetLogger<T>().Trace(messageTemplate);
		}

		public void Trace(string messageTemplate, params object?[]? propertyValues)
		{
			XjjXmmLoggerHelper.GetLogger<T>().Trace(messageTemplate, propertyValues);
		}

		public void Error(Exception exception, string messageTemplate)
		{
			XjjXmmLoggerHelper.GetLogger<T>().Error(exception, messageTemplate);
		}

		public void Critical(Exception exception, string messageTemplate)
		{
			XjjXmmLoggerHelper.GetLogger<T>().Critical(exception, messageTemplate);
		}
	}
}
