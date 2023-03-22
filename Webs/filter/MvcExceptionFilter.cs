using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using XjjXmm.FrameWork.LogExtension;
using XjjXmm.Infrastructure.Common;
using XjjXmm.Infrastructure.Exceptions;

namespace xjjxmm.infrastructure.webs.filter;

public class MvcExceptionFilter : IAsyncExceptionFilter
{
	private readonly ILog<MvcExceptionFilter> _log;

	public MvcExceptionFilter(ILog<MvcExceptionFilter> log)
	{
		_log = log;
	}
		
	public Task OnExceptionAsync(ExceptionContext context)
	{
		_log.Error(context.Exception, "MvcExceptionFilter");
			
		if (context.Exception is BussinessException exception)
		{
			//var statusCode = exception.Code;
				
			//objectResult.Value = new Response<object>(objectResult?.Value);

			//objectResult.DeclaredType = objectResult.Value.GetType();

			context.Result = new JsonResult(new Response<string>()
			{
				Success = false,
				Code = exception.Code,
				Message = context.Exception.Message
			});
		}
		else
		{
			context.Result = new JsonResult(new Response<string>()
			{
				Success = false, Code = "500", Message = context.Exception.Message
			});
				
		}

		return Task.CompletedTask;
	}
}