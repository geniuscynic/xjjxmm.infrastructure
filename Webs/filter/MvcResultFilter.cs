using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Serilog;
using XjjXmm.Infrastructure.Common;

namespace xjjxmm.infrastructure.webs.filter;

public class MvcResultFilter : IAsyncResultFilter
{
	//  private readonly IWebHostEnvironment _env;
	public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
	{
		var a = "a";
		if (context.Result is ObjectResult objectResult)
		{
			var statusCode = objectResult.StatusCode ?? context.HttpContext.Response.StatusCode;

			//if (!_options.NoWrapStatusCode.Any(s => s == statusCode))
			//{
			//var wrappContext = new DataWrapperContext(context.Result,
			//                                          context.HttpContext,
			//                                          //_options,
			//                                          context.ActionDescriptor);

			// var wrappedData = _wrapperExecutor.WrapSuccesfullysResult(objectResult.Value, wrappContext);
			//if (objectResult?.Value is string statusCodeString)
			//{
			//    objectResult.Value = new ResponseModel<string>(statusCodeString);
			//}
			//else
			objectResult.Value = new Response<object>(objectResult?.Value);

			objectResult.DeclaredType = objectResult.Value.GetType();
			//}
		}
		else if (context.Result is EmptyResult emptyResult)
		{
			//var statusCode = context.HttpContext.Response.StatusCode;
			context.Result =  new JsonResult( new Response<string>(""));

			//objectResult.DeclaredType = objectResult.Value.GetType();
		}

		await next();

		var b = "a";
	}



}