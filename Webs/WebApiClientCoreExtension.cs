using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ubiety.Dns.Core;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using WebApiClientCore.Extensions.NewtonsoftJson;
using WebApiClientCore.Serialization.JsonConverters;
using XjjXmm.Infrastructure.Common;
using XjjXmm.Infrastructure.Configuration;

namespace XjjXmm.Infrastructure.Webs;

public static class WebApiClientCoreExtension
{
	public static IServiceCollection ConfigHttpApi<T>(this IServiceCollection services, string configKey = "") where T : class
	{
		if (string.IsNullOrEmpty(configKey))
		{
			configKey = typeof(T).Name;
		}

		services
			.AddHttpApi<T>()
			.ConfigureHttpApi(ConfigHelper.Configuration.GetSection(configKey))
			//.ConfigureHttpApi(o =>
			//{
				//o.GlobalFilters.Add(new ResponseAttribute());
				// 符合国情的不标准时间格式，有些接口就是这么要求必须不标准
				//o.JsonSerializeOptions.Converters.Add(new JsonDateTimeConverter("yyyy-MM-dd HH:mm:ss"));
			//})
			.ConfigureNewtonsoftJson(o =>
			{
				o.JsonSerializeOptions.NullValueHandling = NullValueHandling.Ignore;
				o.JsonSerializeOptions.Converters.Add(new LongJsonConverter());
				o.JsonDeserializeOptions.Converters.Add(new LongJsonConverter());

			});;

		return services;
	}
}

public class ResponseAttribute : JsonNetReturnAttribute
{
	public override async Task SetResultAsync(ApiResponseContext context)
	{
		Type TFirst = context.ActionDescriptor.Return.DataType.Type;
		Type listOf = typeof(Response<>);
			
		Type listOfTFirst = listOf.MakeGenericType(TFirst);
		
		//ApiResponseContext apiResponseContext = context;
		
		HttpContent content = context.HttpContext.ResponseMessage?.Content;
		if (content == null)
			return;
		string str = await content.ReadAsStringAsync().ConfigureAwait(false);
		//Type type = context.ActionDescriptor.Return.DataType.Type;
		string optionsName = context.HttpContext.OptionsName;
		JsonNetSerializerOptions serializerOptions = ServiceProviderServiceExtensions.GetService<IOptionsMonitor<JsonNetSerializerOptions>>(context.HttpContext.ServiceProvider).Get(optionsName);
		dynamic res = JsonConvert.DeserializeObject(str, listOfTFirst, serializerOptions.JsonDeserializeOptions);
		context.Result = res.Result;
		//JsonNetSerializerOptions serializerOptions = ServiceProviderServiceExtensions.GetService<IOptionsMonitor<JsonNetSerializerOptions>>(context.HttpContext.ServiceProvider).Get(optionsName);
		//dynamic res = await context.JsonDeserializeAsync(listOfTFirst).ConfigureAwait(false);
		//context.Result = JsonConvert.DeserializeObject(str, type, serializerOptions.JsonDeserializeOptions);


		//apiResponseContext.Result = res.Data;
		//apiResponseContext =  null;
	}

	
}

public class RawResponseAttribute : SpecialReturnAttribute
{
	public override async Task SetResultAsync(ApiResponseContext context)
	{
		Type type = context.ActionDescriptor.Return.DataType.Type;
		Type typeFromHandle = typeof(Response<>);
		Type listOfTFirst = typeFromHandle.MakeGenericType(type);
		HttpContent httpContent = context.HttpContext.ResponseMessage?.Content;
		if (httpContent != null)
		{
			string value = await httpContent.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			string optionsName = context.HttpContext.OptionsName;
			JsonNetSerializerOptions jsonNetSerializerOptions = context.HttpContext.ServiceProvider.GetService<IOptionsMonitor<JsonNetSerializerOptions>>()!.Get(optionsName);
			dynamic val = JsonConvert.DeserializeObject(value, listOfTFirst, jsonNetSerializerOptions.JsonDeserializeOptions);
			context.Result = (object?)val.Result;
		}


	}
}
