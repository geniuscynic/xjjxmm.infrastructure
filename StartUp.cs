using Autofac;
using Autofac.Extensions.DependencyInjection;
using EasyCaching.Core.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XjjXmm.FrameWork.LogExtension;
using XjjXmm.Infrastructure.Common;
using XjjXmm.Infrastructure.Configuration;
using XjjXmm.Infrastructure.LogKit;
using XjjXmm.Infrastructure.Mongo;
using XjjXmm.Infrastructure.Redis;
using XjjXmm.Infrastructure.Repository;
using xjjxmm.infrastructure.repository.impl.sugar;
using xjjxmm.infrastructure.repository.interfaces;
using XjjXmm.Infrastructure.ToolKit;
using XjjXmm.Infrastructure.User;
using xjjxmm.infrastructure.webs.filter;
using XjjXmm.Infrastructure.Webs.Route;
using SqlSugar;
using Newtonsoft.Json;
using XjjXmm.Infrastructure.Ioc;

namespace XjjXmm.Infrastructure
{
	public static class StartUp
	{
		public static void Regist(this WebApplicationBuilder builder, Action<ContainerBuilder> configureDelegate)
		{
			
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			
			//扫描本地log及从配置中心读log
			builder.WebHost.ConfigureAppConfiguration((context, configurationBuilder) =>
			{
				configurationBuilder.Scan(builder.Environment.EnvironmentName);
				configurationBuilder.AddCustom();
			});

			//serilog
			ConfigHelper.Configuration = builder.Configuration;


			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(ConfigHelper.Configuration)
				.CreateBootstrapLogger();

			//builder.Host.UseSerilog();
			//注册级配置serilog
			builder.Host.UseSerilog((context, services, configuration) =>
			{
				configuration.ReadFrom.Configuration(ConfigHelper.Configuration);
			});

			Log.Information($"EnvironmentName: {builder.Environment.EnvironmentName}");

			var frameConfig = ConfigHelper.GetSection<FrameworkConfig>("xjjxmm.framework");
			//用newtionsoftjson 代替默认方案， 
			builder.Services.AddControllers(opt =>
				{            // 路由参数在此处仍然是有效的，比如添加一个版本号
					if (!frameConfig?.prefix?.IsNullOrEmpty()??false)
					{
						opt.UseCentralRoutePrefix(new RouteAttribute(frameConfig.prefix));
					}
				})
				.AddMvcOptions(t =>
			{
				
				if (frameConfig?.usePrettyResult == "1")
				{
					t.Filters.Add<MvcResultFilter>();
				}
				
				t.Filters.Add<MvcExceptionFilter>();
			}).AddNewtonsoftJson(option =>
			{
				option.SerializerSettings.Converters.Add(new LongJsonConverter());
			}).AddControllersAsServices();

			


			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Swagger接口文档",
					Version = "v1",
					Description = $"XjjXmm.WebApi HTTP API V1",
				});

				// 获取xml注释文件的目录
				var xmlPath = Path.Combine(AppContext.BaseDirectory, "web.xml");
				c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改
			});

			builder.Services.AddHttpContextAccessor();

			//跨域
			builder.Services.AddCors(setupAction =>
			{
				setupAction.AddDefaultPolicy(policy =>
				{
					policy//.WithOrigins("http://localhost:8008")
					 .AllowAnyOrigin()
					 //.AllowCredentials()
					 .AllowAnyHeader()
					 //.WithMethods("OPTION", "POST", "GET");
					 .AllowAnyMethod()
					 ;
					//.AllowCredentials();
				});
			});

			//引入autoface
			builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

			builder.Host.ConfigureContainer<ContainerBuilder>((_, builder2) =>
			{
				builder2.RegisterGeneric(typeof(XjjXmmSeriLogger<>)).As(typeof(ILog<>)).SingleInstance();

				builder2.RegisterType<LoggerInterceptor>().AsSelf().SingleInstance();
				builder2.RegisterType<LoggerAsyncInterceptor>().AsSelf().SingleInstance();

				builder2.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope();
				builder2.RegisterGeneric(typeof(MongoRepository<>)).InstancePerLifetimeScope();

				if (!ConfigHelper.GetConfig("RedisHosts").IsNullOrEmpty())
				{
					builder2.Register(com => new RedisClient()).AsSelf().SingleInstance();
				}

				builder2.RegisterType<UserContext>().As<IUserContext>().InstancePerLifetimeScope();


				configureDelegate(builder2);
			});

			//引入easy cache
			builder.Services.AddEasyCaching(option =>
			{
				option.UseRedis(ConfigHelper.Configuration, "redis1").WithJson("redis1");
				/*option.UseRedis(config => 
					{
						config.DBConfig.Endpoints.Add(new ServerEndPoint("192.168.88.25", 6379));
					}, "redis1")// using MessagePack to serialize caching data.
					.WithJson("redis1");*/
				//	//with messagepack serialization
				//	;      
			});
		}

		public static void Use(this IApplicationBuilder app)
		{
			app.UseSerilogRequestLogging(); // <-- Add this line
			app.UseCors();

			
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "XjjXmm");

					c.RoutePrefix = "swagger";

				});
				
				
				IocManager.Instance.Container = app.ApplicationServices.GetAutofacRoot();
		}

		public static void RegistType(this WebApplicationBuilder builder, Type type)
		{
			builder.Host.ConfigureContainer<ContainerBuilder>((_, builder2) =>
			{
				builder2.RegisterAssemblyTypes(ReflectKit.GetAssembly(type))
					.Where(a => a.Name.EndsWith("Repository"))
					.AsImplementedInterfaces()
					.InstancePerLifetimeScope();

				builder2.RegisterAssemblyTypes(ReflectKit.GetAssembly(type))
					.Where(a => a.Name.EndsWith("Service"))
					.AsSelf()
					.InstancePerLifetimeScope();
			});
		}



		public static void RegistSql(this ContainerBuilder builder)
		{
			var dbtype = ConfigHelper.GetConfig("db:type");
			var connectionString = ConfigHelper.GetConfig("db:connectionString");

			
			if(dbtype.IsNullOrEmpty() || connectionString.IsNullOrEmpty())
			{
				return;
			}

			builder.Register(com =>
			{
				var sqlSugar = new SqlSugarScope(new ConnectionConfig()
				{
					DbType = EnumKit.Parse<DbType>(dbtype),
					ConnectionString = connectionString,
					IsAutoCloseConnection = true,
				},
				db =>
				{
					//单例参数配置，所有上下文生效
					db.Aop.OnLogExecuting = (sql, pars) =>
					{
						var par = pars?.Select(t => t.Value);

						Log.Information($"Sql:{sql}\r\n paramters: {JsonConvert.SerializeObject(par)}");
						//Log.Information($"Sql:{sql}\r\n paramters: {JsonConvert.SerializeObject(pars)}");
						//Console.WriteLine(sql);//输出sql
						//Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数
					};
				}
			 );

				return sqlSugar;

			}).AsImplementedInterfaces().SingleInstance();
		}
	}
}
