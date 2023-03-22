using System.ComponentModel;

namespace XjjXmm.Infrastructure.Exceptions;

/// <summary>
/// 状态码枚举
/// </summary>
public class StatusCodes
{
	/// <summary>
	/// 操作成功
	/// </summary>
	[Description("操作成功")] public const string Status0Ok = "0";
	
	[Description("验证失败")] public const string Status998ValidationFalid = "998";

	/// <summary>
	/// 操作失败
	/// </summary>
	[Description("操作失败")] public const string Status999Falid = "999";


	/// <summary>
	/// 操作成功
	/// </summary>
	[Description("操作成功")] public const string Status200Ok = "200";

	/// <summary>
	/// 未登录（需要重新登录）
	/// </summary>
	[Description("未登录")] public const string Status401Unauthorized = "401";

	/// <summary>
	/// 权限不足
	/// </summary>
	[Description("权限不足")] public const string Status403Forbidden = "403";

	/// <summary>
	/// 资源不存在
	/// </summary>
	[Description("资源不存在")]
	public const string Status404NotFound = "404";

	/// <summary>
	/// 系统内部错误（非业务代码里显式抛出的异常，例如由于数据不正确导致空指针异常、数据库异常等等）
	/// </summary>
	[Description("系统内部错误")] public const string Status500InternalServerError = "500";
}
