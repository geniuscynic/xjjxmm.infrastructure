using System.Linq.Expressions;

namespace xjjxmm.infrastructure.repository.interfaces;

public interface IRepositoryDeletable<T> where T : class, new()
{
	IRepositoryDeletable<T> Where(Expression<Func<T, bool>> expression);
		
	Task<bool> Execute();

	Task<int> ExecuteNums();
}