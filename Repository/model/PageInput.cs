namespace XjjXmm.Infrastructure.Repository.Model;

/// <summary>
/// 分页信息输入
/// </summary>
public class PageInput
{
    /// <summary>
    /// 当前页标
    /// </summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// 每页大小
    /// </summary>
    public int PageSize { set; get; } = 10;


}

/// <summary>
/// 分页信息输入
/// </summary>
public class QueryPageInput : PageInput
{
	/// <summary>
    /// 查询条件
    /// </summary>
    public string? Name { get; set; }
	
	/// <summary>
	/// 0 所有 1 正常 2 删除
	/// </summary>
	public int Status { get; set; }
}

public class PageInput<T> : PageInput
{
		public T? Filter { get; set; }
}
