using SqlSugar;
using System.Linq.Expressions;
using xjjxmm.infrastructure.repository.interfaces;

namespace xjjxmm.infrastructure.repository.impl.sugar;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class, new()
{
    protected readonly ISqlSugarClient _context;
    //private readonly IRepositoryQuery<T> _repositoryQuery;

    // SimpleClient
    //public RepositoryBase(ISqlSugarClient context, IRepositoryQuery<T> repositoryQuery)
    //{
    //    _context = context;
    //    _repositoryQuery = repositoryQuery;
    //}

    public RepositoryBase(ISqlSugarClient context)
    {
        _context = context;
    }

    public async Task<bool> Add(T entity)
    {
        return await _context.Insertable(entity).ExecuteCommandAsync() > 0;
    }

    public async Task<bool> Add(List<T> entity)
    {
        return await _context.Insertable(entity).ExecuteCommandAsync() > 0;
    }

    public async Task<long> AddReturnIdentity(T entity)
    {
        return await _context.Insertable(entity).ExecuteReturnBigIdentityAsync();
    }

    public async Task<long> AddReturnSnowflakeId(T entity)
    {
        return await _context.Insertable(entity).ExecuteReturnSnowflakeIdAsync();
    }

    public async Task<List<long>> AddReturnSnowflakeId(List<T> entity)
    {
        return await _context.Insertable(entity).ExecuteReturnSnowflakeIdListAsync();
    }



    public async Task<bool> Update(T entity)
    {
        return await _context.Updateable(entity).ExecuteCommandAsync() > 0;
    }



	public async Task<bool> Delete(string id)
    {
        return await _context.Deleteable<T>().In(id).ExecuteCommandAsync() > 0;
    }

    public async Task<bool> Delete(Expression<Func<T, bool>> whereExpression)
    {
        return await _context.Deleteable<T>(whereExpression).ExecuteCommandAsync() > 0;
    }

    public async Task<T> Find(string id)
    {
        return await _context.Queryable<T>().InSingleAsync(id);
    }

    public async Task<T?> First(Expression<Func<T, bool>> whereExpression)
    {
        return await _context.Queryable<T>().Where(whereExpression).FirstAsync();
    }

    public async Task<List<T>> GetAll()
    {
        return await _context.Queryable<T>().ToListAsync();
    }

    public async Task<List<T>> Get(Expression<Func<T, bool>> whereExpression)
    {
        return await _context.Queryable<T>().Where(whereExpression).ToListAsync();
    }

    public async Task<bool> SoftDelete(string id)
    {
	   return await _context.Deleteable<T>().In(id).IsLogic().ExecuteCommandAsync() > 0; //假删除 软删除
	   /*return await _context.Updateable<T>()
		   .SetColumns("IS_DELETED", 1)
		   .Where("ID='@id'", new { id }).ExecuteCommandAsync() > 0; //假删除 软删除*/
    }

    public async Task<bool> SoftDelete(string[] id)
    {
        return await _context.Deleteable<T>().In(id).IsLogic().ExecuteCommandAsync() > 0; //假删除 软删除
    }
    
    public async Task<bool> Restore(string id)
    {
	    return await _context.Updateable<T>()
		    .SetColumns("IS_DELETED", false)
		    .Where("id=@id", new { id }).ExecuteCommandAsync() > 0; //假删除 软删除
    }
    
    public IRepositoryQuery<T> Queryable()
    {
	    return new RepositoryQuery<T>(_context);
    }
    
    public IRepositoryUpdatable<T> Updatable()
    {
	    return new RepositoryUpdatable<T>(_context);
    }

    public IRepositoryUpdatable<T> Updatable(T entity)
    {
	    return new RepositoryUpdatable<T>(_context, entity);
    }

    public IRepositoryDeletable<T> Deletable()
    {
	    return new RepositoryDeletable<T>(_context);
    }

    public string GetTableName(object fieldValue)
    {
        return _context.SplitHelper<T>().GetTableName(fieldValue);
    }

    public void BeginTran()
    {
	    _context.AsTenant().BeginTran();
    }

    public void CommitTran()
    {
	    _context.AsTenant().CommitTran();
    }

    public void RollbackTran()
    {
	    _context.AsTenant().RollbackTran();
    }
}
