using MongoDB.Driver;
using SqlSugar;
using System.Linq.Expressions;
using xjjxmm.infrastructure.repository.interfaces;
using XjjXmm.Infrastructure.Repository.Model;

namespace XjjXmm.Infrastructure.Mongo
{
	public class MongoRepositoryQuery<T> where T : class, new()
	{
		protected readonly IMongoDatabase _context;
		private readonly IMongoCollection<T> _collection;

		private FilterDefinition<T>? _filterDefinition;
		private SortDefinition<T>? _sortDefinition;

		public MongoRepositoryQuery(IMongoDatabase context,
			IMongoCollection<T> collection)
		{
			_context = context;
			_collection = collection;

		}

		private void And(FilterDefinition<T> filterDefinition)
		{
			if (_filterDefinition == null)
			{
				_filterDefinition = filterDefinition;
			}
			else
			{
				_filterDefinition = Builders<T>.Filter.And(filterDefinition, _filterDefinition);
			}
		}

		private void And(Expression<Func<T, bool>> expression)
		{
			FilterDefinition<T> filterDefinition = Builders<T>.Filter.Where(expression);
			And(filterDefinition);
		}

		
		
		public MongoRepositoryQuery<T> OrderByAsc(Expression<Func<T, object>> expression)
		{
			_sortDefinition = Builders<T>.Sort.Ascending(expression);
			return this;
		}

		
		
		public MongoRepositoryQuery<T> OrderByAsc(bool ifOrderBy, Expression<Func<T, object>> expression)
		{
			if (ifOrderBy)
			{
				_sortDefinition = Builders<T>.Sort.Ascending(expression);
			}
			return this;
		}

		
		
		public MongoRepositoryQuery<T> OrderByDesc(Expression<Func<T, object>> expression)
		{
			_sortDefinition = Builders<T>.Sort.Descending(expression);
			return this;
		}

		public MongoRepositoryQuery<T> OrderByDesc(bool ifOrderBy, Expression<Func<T, object>> expression)
		{
			if (ifOrderBy)
			{
				_sortDefinition = Builders<T>.Sort.Descending(expression);
			}

			return this;
		}

		private IFindFluent<T, T> BuildCursor()
		{
			if (_filterDefinition == null)
			{
				_filterDefinition = FilterDefinition<T>.Empty;
			}
			var cursor = _collection.Find(_filterDefinition);
			if (_sortDefinition != null)
			{
				cursor = cursor.Sort(_sortDefinition);
			}

			return cursor;
		}
		public async Task<T> ToFirst()
		{
			var entity = await _collection.FindAsync(_filterDefinition);
			return await entity.FirstAsync();
		}

		public async Task<List<T>> ToList()
		{

			var cursor = BuildCursor();

			return await cursor.ToListAsync();
		}

		public async Task<PageOutput<T>> ToPage(int currentPage, int pageSize)
		{
			var cursor = BuildCursor();

			var total = await cursor.CountDocumentsAsync();

			cursor = cursor.Skip((currentPage - 1) * pageSize).Limit(pageSize);

			var data = await cursor.ToListAsync();

			return new PageOutput<T>
			{
				CurrentPage = currentPage,
				Total = (int)total,
				PageSize = pageSize,
				Data = data
			};
		}
		
		public MongoRepositoryQuery<T> Where(Expression<Func<T, bool>> expression)
		{

			And(expression);

			return this;
		}

		public MongoRepositoryQuery<T> Where(bool ifWhere, Expression<Func<T, bool>> expression)
		{
			if (ifWhere)
			{
				And(expression);
			}

			return this;
		}
	}



}
