using System;
using System.Text;

namespace XjjXmm.Infrastructure.Exceptions;

public class BussinessException : Exception
{

	/// <summary>
	/// 错误码
	/// </summary>
	public string Code { get; set; }

	public string Message => GetMessage();
	public BussinessException(Exception innerException) : this(null, null, innerException)
	{

	}

	public BussinessException(string message) : this(message, null,  null)
	{

	}
	
	public BussinessException(string message, string code) : this(message, code,  null)
	{

	}
	
	public BussinessException(string message, Exception innerException) : this(message,null, innerException)
	{

	}

	public BussinessException(string? message, string? code = null,  Exception? innerException = null) : base(message, innerException)
	{
		Code = code;
	}

	/// <summary>
	/// 获取错误消息
	/// </summary>
	private string GetMessage()
	{
		return GetMessage(this);
	}

	/// <summary>
	/// 获取错误消息
	/// </summary>
	private string GetMessage(Exception ex)
	{
		var result = new StringBuilder();
		var list = GetExceptions(ex);
		foreach (var exception in list)
			AppendMessage(result, exception);
		return result.ToString();
	}

	/// <summary>
	/// 添加异常消息
	/// </summary>
	private void AppendMessage(StringBuilder result, Exception exception)
	{
		if (exception == null)
			return;
		result.AppendLine(exception.Message);
	}

	/// <summary>
	/// 获取异常列表
	/// </summary>
	private IList<Exception> GetExceptions()
	{
		return GetExceptions(this);
	}

	/// <summary>
	/// 获取异常列表
	/// </summary>
	/// <param name="ex">异常</param>
	private IList<Exception> GetExceptions(Exception ex)
	{
		var result = new List<Exception>();
		AddException(result, ex);
		return result;
	}

	/// <summary>
	/// 添加内部异常
	/// </summary>
	private void AddException(List<Exception> result, Exception exception)
	{
		if (exception == null)
			return;
		result.Add(exception);
		AddException(result, exception.InnerException);
	}
}
