using System.Linq.Expressions;

namespace xjjxmm.infrastructure.repository.interfaces
{
	public interface IRepositoryUpdatable<T> where T : class, new()
	{
		IRepositoryUpdatable<T> Where(Expression<Func<T, bool>> expression);
		
		IRepositoryUpdatable<T> SetColumns(Expression<Func<T, bool>> columns);

		IRepositoryUpdatable<T> SetColumns(Expression<Func<T, T>> columns);
		
		IRepositoryUpdatable<T> IgnoreColumns(Expression<Func<T, object>> columns);
		
		Task<bool> Execute();

		Task<int> ExecuteNums();
	}
}
