using SqlSugar;
using System.Linq.Expressions;
using xjjxmm.infrastructure.repository.interfaces;
using XjjXmm.Infrastructure.Repository.Model;

namespace xjjxmm.infrastructure.repository.impl.sugar;

public class RepositoryQuery<T> : IRepositoryQuery<T> where T : class, new()
{
    protected readonly ISqlSugarClient _context;
    private ISugarQueryable<T> _query;
    // SimpleClient
    public RepositoryQuery(ISqlSugarClient context)                            
    {
        _context = context;
        _query = _context.Queryable<T>();
    }

    public IRepositoryQuery<T> OrderByAsc(Expression<Func<T, object>> expression)
    {
        _query = _query = _query.OrderBy(expression);
        return this;
    }

    public IRepositoryQuery<T> OrderByAsc(bool ifOrderBy, Expression<Func<T, object>> expression)
    {
        _query = _query = _query.OrderByIF(ifOrderBy, expression);
        return this;
    }

    public IRepositoryQuery<T> OrderByDesc(Expression<Func<T, object>> expression)
    {
        _query = _query = _query.OrderBy(expression, OrderByType.Desc);
        return this;
    }

    public IRepositoryQuery<T> OrderByDesc(bool ifOrderBy, Expression<Func<T, object>> expression)
    {
        _query = _query = _query.OrderByIF(ifOrderBy,expression, OrderByType.Desc);
        return this;
    }

    public async Task<T> ToFirst()
    {
        return await _query.FirstAsync();
    }

    public async Task<List<T>> ToList()
    {
        return await _query.ToListAsync();
    }

    public async Task<PageOutput<T>> ToPage(int currentPage, int pageSize)
    {
        RefAsync<int> total = 0;
        var result = await _query.ToPageListAsync(currentPage, pageSize, total);

        return new PageOutput<T>
        {
            CurrentPage = currentPage,
            Total = total.Value,
            PageSize = pageSize,
            Data = result
        };
    }

    public async Task<List<T>> ToTree(Expression<Func<T, IEnumerable<object>>> childListExpression, Expression<Func<T, object>> parentIdExpression, dynamic rootValue)
    {
        return await _query.ToTreeAsync(childListExpression, parentIdExpression,  rootValue);
    }

    public IRepositoryQuery<T> Where(Expression<Func<T, bool>> expression)
    {
        _query = _query.Where(expression);
        return this;
    }

    public IRepositoryQuery<T> Where(bool ifWhere, Expression<Func<T, bool>> expression)
    {
        _query = _query.WhereIF(ifWhere, expression);
        return this;
    }

    public IRepositoryQuery<T, T2> LeftJoin<T2>(Expression<Func<T, T2, bool>> expression)
    {
        var query = _query.LeftJoin<T2>(expression);
        return new RepositoryQuery<T,T2>(_context, query);
    }

    public IRepositoryQuery<T,T2> Join<T2>(Expression<Func<T, T2, bool>> expression)
    {
        var query = _query.InnerJoin<T2>(expression);
        return new RepositoryQuery<T, T2>(_context, query);
    }

    public IRepositoryQuery<T> SplitTable(Func<List<SplitTableInfo>, IEnumerable<SplitTableInfo>> getTableNamesFunc)
    {
        _query = _query.SplitTable(getTableNamesFunc);

        return this;
    }


	public IRepositoryQuery<T> Include<T2>(Expression<Func<T, List<T2>>> include)
	{
		_query = _query.Includes(include);

		return this;
	}

	public IRepositoryQuery<T> Include<T2>(Expression<Func<T, T2>> include)
	{
		_query = _query.Includes(include);

		return this;
	}
}


public class RepositoryQuery<T1, T2> : RepositoryQuery<T1>, IRepositoryQuery<T1, T2> where T1 : class, new()
{
    private ISugarQueryable<T1,T2> _query;
    public RepositoryQuery(ISqlSugarClient context, ISugarQueryable<T1, T2> query) : base(context)
    {
        _query = query;
    }

    public IRepositoryQuery<T1, T2> OrderByAsc(Expression<Func<T1, T2, object>> expression)
    {
        _query =_query.OrderBy(expression);
        return this;
    }

    public IRepositoryQuery<T1, T2> OrderByAsc(bool ifOrderBy, Expression<Func<T1, T2, object>> expression)
    {
        _query = _query.OrderByIF(ifOrderBy, expression);
        return this;
    }

    public IRepositoryQuery<T1, T2> OrderByDesc(Expression<Func<T1, T2, object>> expression)
    {
        _query = _query.OrderBy(expression, OrderByType.Desc);
        return this;
    }

    public IRepositoryQuery<T1, T2> OrderByDesc(bool ifOrderBy, Expression<Func<T1, T2, object>> expression)
    {
        _query = _query.OrderByIF(ifOrderBy, expression, OrderByType.Desc);
        return this;
    }

    public IRepositoryQuery<T1,T2> Where(Expression<Func<T1, T2, bool>> expression)
    {
        _query = _query.Where(expression);
        return this;
    }

    public new async Task<T1> ToFirst()
    {
        return await _query.FirstAsync();
    }

    public new async Task<List<T1>> ToList()
    {
        return await _query.ToListAsync();
    }

    public new async Task<PageOutput<T1>> ToPage(int currentPage, int pageSize)
    {
        RefAsync<int> total = 0;
        var result = await _query.ToPageListAsync(currentPage, pageSize, total);

        return new PageOutput<T1>
        {
            CurrentPage = currentPage,
            Total = total.Value,
            PageSize = pageSize,
            Data = result
        };
    }
}
