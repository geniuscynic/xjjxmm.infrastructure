using SqlSugar;
using System.Linq.Expressions;
using xjjxmm.infrastructure.repository.interfaces;

namespace xjjxmm.infrastructure.repository.impl.sugar;

public class RepositoryDeletable<T> : IRepositoryDeletable<T> where T : class, new()
{
	private readonly ISqlSugarClient _context;
	private IDeleteable<T> _deleteable;
	public RepositoryDeletable(ISqlSugarClient context)
	{
		_context = context;
		_deleteable = _context.Deleteable<T>();
	}
	
	public IRepositoryDeletable<T> Where(Expression<Func<T, bool>> expression)
	{
		_deleteable = _deleteable.Where(expression);

		return this;
	}
	
	public async Task<bool> Execute()
	{
		return await _deleteable.ExecuteCommandHasChangeAsync();
	}

	public async Task<int> ExecuteNums()
	{
		return await _deleteable.ExecuteCommandAsync();
	}
}