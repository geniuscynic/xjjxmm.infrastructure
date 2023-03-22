using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XjjXmm.Infrastructure.LogKit
{
    public class LogConfig
    {
        const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] ({ThreadId}) {Message:lj}{NewLine}{Exception}";

        public static Serilog.ILogger Create(string logName = "log")
        {

            // const string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] ({ThreadId}) {Message:lj}{NewLine}{Exception}";

            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
               .MinimumLevel.Information()

               //.Enrich.WithProperty("SourceContext", null) //加入属性SourceContext，也就运行时是调用Logger的具体类
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
               .Enrich.FromLogContext()
               .Enrich.WithThreadId()
               .Enrich.WithThreadName()
               //.Enrich.WithProperty("logName", logName)
               .WriteTo.File($"log/info_{logName}_.txt", rollingInterval: RollingInterval.Day)
               .WriteTo
                   .Logger(lc => lc.Filter.ByIncludingOnly(t => t.Level == LogEventLevel.Error)
                    .WriteTo.File($"log/error_{logName}_.txt", rollingInterval: RollingInterval.Day)
               )
                .WriteTo.Console(outputTemplate: outputTemplate);
            //.WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day);

            Log.Logger = loggerConfiguration.CreateLogger();

            return Log.Logger;
        }
    }
}
