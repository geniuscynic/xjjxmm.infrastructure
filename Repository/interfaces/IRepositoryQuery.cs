using SqlSugar;
using System.Linq.Expressions;
using XjjXmm.Infrastructure.Repository.Model;

namespace xjjxmm.infrastructure.repository.interfaces;

public interface IRepositoryQuery<T> where T : class, new()
{
    IRepositoryQuery<T> Where(Expression<Func<T, bool>> expression);

    IRepositoryQuery<T> Where(bool ifWhere, Expression<Func<T, bool>> expression);

    IRepositoryQuery<T> OrderByAsc(Expression<Func<T, object>> expression);

    IRepositoryQuery<T> OrderByAsc(bool ifOrderBy, Expression<Func<T, object>> expression);

    IRepositoryQuery<T> OrderByDesc(Expression<Func<T, object>> expression);

    IRepositoryQuery<T> OrderByDesc(bool ifOrderBy, Expression<Func<T, object>> expression);




    Task<T> ToFirst();

    Task<List<T>> ToList();

    Task<PageOutput<T>> ToPage(int currentPage, int pageSize);

    Task<List<T>> ToTree(Expression<Func<T, IEnumerable<object>>> childListExpression, Expression<Func<T, object>> parentIdExpression, dynamic rootValue);
   
    IRepositoryQuery<T> SplitTable(Func<List<SplitTableInfo>, IEnumerable<SplitTableInfo>> getTableNamesFunc);
    IRepositoryQuery<T, T2> LeftJoin<T2>(Expression<Func<T, T2, bool>> expression);
    IRepositoryQuery<T, T2> Join<T2>(Expression<Func<T, T2, bool>> expression);
	IRepositoryQuery<T> Include<T2>(Expression<Func<T, List<T2>>> include);
	IRepositoryQuery<T> Include<T2>(Expression<Func<T, T2>> include);
}

public interface IRepositoryQuery<T1, T2> : IRepositoryQuery<T1>
    where T1 : class, new()
{
    IRepositoryQuery<T1, T2> Where(Expression<Func<T1, T2, bool>> expression);

    IRepositoryQuery<T1, T2> OrderByAsc(Expression<Func<T1, T2, object>> expression);

    IRepositoryQuery<T1, T2> OrderByAsc(bool ifOrderBy, Expression<Func<T1, T2, object>> expression);

    IRepositoryQuery<T1, T2> OrderByDesc(Expression<Func<T1, T2, object>> expression);

    IRepositoryQuery<T1, T2> OrderByDesc(bool ifOrderBy, Expression<Func<T1, T2, object>> expression);





}
