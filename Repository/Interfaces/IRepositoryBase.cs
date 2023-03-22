using System.Linq.Expressions;

namespace xjjxmm.infrastructure.repository.interfaces;

public interface IRepositoryBase<T> where T : class, new()
{

    /// <summary>
    /// 返回是否成功
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> Add(T entity);

    /// <summary>
    /// 返回是否成功
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> Add(List<T> entity);

    /// <summary>
    /// 返回主键
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
   // Task<long> AddReturnIdentity(T entity);

    /// <summary>
    /// 返回主键
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
   // Task<long> AddReturnSnowflakeId(T entity);

    /// <summary>
    /// 批量返回主键
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    //Task<List<long>> AddReturnSnowflakeId(List<T> entity);

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> Update(T entity);

	//Task<bool> Update(Expression<Func<T, bool>> columns, Expression<Func<T, bool>> whereExpression);
	//Task<bool> Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereExpression);

	/// <summary>
	/// 根据主键删除
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	Task<bool> Delete(string id);

    /// <summary>
    /// 条件删除
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>

    Task<bool> Delete(Expression<Func<T, bool>> whereExpression);

    /// <summary>
    /// 软删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> SoftDelete(string id);

    /// <summary>
    /// 批量软删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> SoftDelete(string[] id);

    /// <summary>
    /// 恢复删除字段
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<bool> Restore(string id);
    
    /// <summary>
    /// 条件软删除
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    //Task<bool> SoftDelete(Expression<Func<T, bool>> whereExpression);

    Task<T> Find(string id);

    Task<T?> First(Expression<Func<T, bool>> whereExpression);

    Task<List<T>> GetAll();

    Task<List<T>> Get(Expression<Func<T, bool>> whereExpression);

    IRepositoryQuery<T> Queryable();

    IRepositoryUpdatable<T> Updatable();
    
    IRepositoryUpdatable<T> Updatable(T entity);
    
    IRepositoryDeletable<T> Deletable();
    
    string GetTableName(object fieldValue);
    
    
    void BeginTran();//去掉了.ado
    void CommitTran();//去掉了.ado
    void RollbackTran();//去掉了.ado
	
}
