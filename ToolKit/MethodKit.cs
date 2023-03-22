using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XjjXmm.Infrastructure.ToolKit
{
    public class MethodKit
    {
        public static async Task Retry(int maxRetryCount, Func<Task> funcGetData)
        {
            Exception lastException = null;
            var fixdMaxRetryCount = maxRetryCount > 1 ? maxRetryCount : 1;
            for (var i = 0; i < fixdMaxRetryCount; i++)
            {
                try
                {
                    await funcGetData();

                }
                catch (Exception e)
                {
                    lastException = e;
                    //_spider.GetLogger().Error(e, _spider.Domain);
                    Serilog.Log.Error(e, "retry");
                    Serilog.Log.Information("重试");

                    await Task.Delay(TimeSpan.FromSeconds(60 * maxRetryCount));
                }

                if (lastException == null)
                {
                    break;
                }
            }
            if (lastException != null)
            {
                throw lastException;
            }
        }
    }
}
