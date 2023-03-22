using XjjXmm.Infrastructure.Exceptions;
using XjjXmm.Infrastructure.ToolKit;

namespace XjjXmm.Infrastructure.Common
{

	/// <summary>
	/// 通用返回信息类
	/// </summary>
	public class Response<T>
    {
	    public Response()
	    {

	    }
	    
		public Response(T result)
		{
			Result = result;
		}

        /// <summary>
        /// 状态码
        /// </summary>
        public string Code { get; set; } = StatusCodes.Status0Ok.ToString();
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T Result { get; set; }


		
    }

	public static class ResponseExtension
	{
		public static async Task<T> GetResponseData<T>(this Func<Task<Response<T>>> response)
		{
			return (await response()).Result;
		}
	}
}
