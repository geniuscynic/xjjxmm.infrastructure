using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace XjjXmm.Infrastructure.Swagger
{
    /// <summary> 
    /// 隐藏接口，不生成到swagger文档展示 
    /// 注意：如果不加[HiddenApi]标记的接口名称和加过标记的隐藏接口名称相同，则该普通接口也会被隐藏不显示，所以建议接口名称最好不要重复
    /// </summary> 
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public partial class HiddenApiAttribute : Attribute { }
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerIgnoreFilter : IDocumentFilter
    {
        /// <summary> 
        /// 重写Apply方法，移除隐藏接口的生成 
        /// </summary> 
        /// <param name="swaggerDoc">swagger文档文件</param> 
        /// <param name="schemaRegistry"></param> 
        /// <param name="apiExplorer">api接口集合</param> 
       /* public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            foreach (ApiDescription apiDescription in apiExplorer.ApiDescriptions)
            {
                if (Enumerable.OfType<HiddenApiAttribute>(apiDescription.GetControllerAndActionAttributes<HiddenApiAttribute>()).Any())
                {
                    string key = "/" + apiDescription.RelativePath;
                    if (key.Contains("?"))
                    {
                        int idx = key.IndexOf("?", StringComparison.Ordinal);
                        key = key.Substring(0, idx);
                    }
                    swaggerDoc.paths.Remove(key);
                }
            }
        }*/

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //context.ApiDescriptions
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {                          
               
                //if (apiDescription.ControllerAttributes().OfType<HiddenAttribute>().Count() == 0
                //    && apiDescription.ActionAttributes().OfType<HiddenAttribute>().Count() == 0)
                //{
                //    continue;
                //}

                /*var key = "/" + apiDescription.RelativePath.TrimEnd('/');
                if (!key.Contains("/test/") && swaggerDoc.Paths.ContainsKey(key))
                {
                    swaggerDoc.Paths.Remove(key);
                }*/
            }
        }

        
    }
}
