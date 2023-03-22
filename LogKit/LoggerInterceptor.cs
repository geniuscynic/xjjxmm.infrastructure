using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System.Diagnostics;
using System.Reflection;

namespace XjjXmm.Infrastructure.LogKit
{
	public class LoggerInterceptor : IInterceptor
	{
		private readonly LoggerAsyncInterceptor _interceptor;

		public LoggerInterceptor(LoggerAsyncInterceptor interceptor)
		{
			_interceptor = interceptor;
		}
		
		public void Intercept(IInvocation invocation)
		{
			_interceptor.ToInterceptor().Intercept(invocation);
		}

	}

	public class LoggerAsyncInterceptor : IAsyncInterceptor
	{
		public void InterceptSynchronous(IInvocation invocation)
		{
			var watch = Stopwatch.StartNew();
			
			var logger = Log.ForContext(invocation.TargetType);
			
			invocation.Proceed();
			
			//获取执行信息
			var methodName = invocation.Method.Name;
			//记录日志
			logger.Information("开始执行: {MethodName}，", methodName);

			
			var task = invocation.ReturnValue;
		

			watch.Stop();
			var ms = watch.Elapsed;
			
			//记录日志
			logger.Information("结束执行: {MethodName}，返回结果：{@Task}, 用时: {Ms}", methodName, task, ms);

			invocation.ReturnValue = task;
		}

		public void InterceptAsynchronous(IInvocation invocation)
		{
			invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
		}

		private async Task InternalInterceptAsynchronous(IInvocation invocation)
		{
			var watch = Stopwatch.StartNew();
			
			var logger = Log.ForContext(invocation.TargetType);
			invocation.Proceed();
			
			//获取执行信息
			var methodName = invocation.Method.Name;
			//记录日志
			logger.Information("开始执行: {MethodName}，", methodName);

			
			var task = (Task)invocation.ReturnValue;
			await task;

			watch.Stop();
			var ms = watch.Elapsed;
			
			//记录日志
			logger.Information("结束执行: {MethodName}，返回结果：void, 用时: {Ms}", methodName, ms);
		}
		
		public void InterceptAsynchronous<TResult>(IInvocation invocation)
		{
			//调用业务方法
			invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
		}
		
		private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
		{
			var watch = Stopwatch.StartNew();
			
			var logger = Log.ForContext(invocation.TargetType);
			invocation.Proceed();
			
			//获取执行信息
			var methodName = invocation.Method.Name;
			//记录日志
			logger.Information("开始执行: {MethodName}，", methodName);

			
			var task = (Task<TResult>)invocation.ReturnValue;
			TResult result = await task;

			watch.Stop();
			var ms = watch.Elapsed;
			
			//记录日志
			logger.Information("结束执行: {MethodName}，返回结果：{@Result}, 用时: {Ms}", methodName, result, ms);

			return result;
		}
	}
}
