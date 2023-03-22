using SqlSugar;
using System.Linq.Expressions;
using xjjxmm.infrastructure.repository.interfaces;

namespace xjjxmm.infrastructure.repository.impl.sugar;

public class RepositoryUpdatable<T> : IRepositoryUpdatable<T> where T : class, new()
{
	private readonly ISqlSugarClient _context;
	private IUpdateable<T> _updateable;
	public RepositoryUpdatable(ISqlSugarClient context)
	{
		_context = context;
		_updateable = _context.Updateable<T>();
	}
		
	public RepositoryUpdatable(ISqlSugarClient context, T entity)
	{
		_context = context;
		_updateable = _context.Updateable<T>(entity);
	}

	public IRepositoryUpdatable<T> Where(Expression<Func<T, bool>> expression)
	{
		_updateable = _updateable.Where(expression);

		return this;
	}

	public IRepositoryUpdatable<T> SetColumns(Expression<Func<T, bool>> columns)
	{
		_updateable = _updateable.SetColumns(columns);

		return this;
	}

	public IRepositoryUpdatable<T> SetColumns(Expression<Func<T, T>> columns)
	{
		_updateable = _updateable.SetColumns(columns);

		return this;
	}

	public IRepositoryUpdatable<T> IgnoreColumns(Expression<Func<T, object>> columns)
	{
		_updateable = _updateable.IgnoreColumns(columns);

		return this;
	}

	public async Task<bool> Execute()
	{
		return await _updateable.ExecuteCommandHasChangeAsync();
	}

	public async Task<int> ExecuteNums()
	{
		return await _updateable.ExecuteCommandAsync();
	}
}
