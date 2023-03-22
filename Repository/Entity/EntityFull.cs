using SqlSugar;

namespace xjjxmm.infrastructure.repository.entity;

public class PK<T>
{
	/// <summary>
	/// 版本
	/// </summary>
	[SugarColumn(ColumnName = "ID", IsPrimaryKey = true)]
	public T Id { get; set; }
}

public class EntityAdd : PK<string>
{
	/// <summary>
	/// 创建者Id
	/// </summary>
	[SugarColumn(ColumnName = "CREATOR_ID", IsOnlyIgnoreUpdate = true)]
	public string? CreatorId { get; set; }

	/// <summary>
	/// 创建者
	/// </summary>
	[SugarColumn(ColumnName = "CREATOR_NAME", IsOnlyIgnoreUpdate = true)]
	public string? CreatorName { get; set; }

	/// <summary>
	/// 创建时间
	/// </summary>
	[SugarColumn(ColumnName = "CREATE_TIME", IsOnlyIgnoreUpdate = true)]
	public DateTime CreatedTime { get; set; } = DateTime.Now;
}

public class EntityFull : EntityAdd
{
	/// <summary>
	/// 版本
	/// </summary>
	[SugarColumn(ColumnName = "LAST_TIMESTAMP")]
	public long TimeStamp { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    [SugarColumn(ColumnName = "IS_DELETED")]
    public bool IsDeleted { get; set; } = false;
    
    /// <summary>
    /// 修改者Id
    /// </summary>
     [SugarColumn(ColumnName = "UPDATER_ID")]
    public string? UpdaterId { get; set; }

    /// <summary>
    /// 修改者
    /// </summary>
    [SugarColumn(ColumnName = "UPDATER_NAME")]
    public string? UpdaterName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [SugarColumn(ColumnName = "UPDATE_TIME")]
    public DateTime UpdateTime { get; set; } = DateTime.Now;
}
