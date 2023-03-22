using MongoDB.Driver;
using System.Linq.Expressions;

namespace XjjXmm.Infrastructure.Mongo
{
	public class MongoRepositoryUpdatable<T>  where T : class, new()
	{
		private readonly IMongoDatabase _context;
		private readonly IMongoCollection<T> _collection;

		private FilterDefinition<T> _filterDefinition;
		private UpdateDefinition<T> _updateDefinition;

		public MongoRepositoryUpdatable(IMongoDatabase context,
			IMongoCollection<T> collection)
		{
			this._context = context;
			this._collection = collection;
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


		public MongoRepositoryUpdatable<T> Where(Expression<Func<T, bool>> expression)
		{
			And(expression);

			return this;
		}

		public MongoRepositoryUpdatable<T> SetColumns<TField>(Expression<Func<T, TField>> columns, TField field)
		{
			_updateDefinition = Builders<T>.Update.Set(columns, field);
			
			return this;
		}


		public async Task<bool> Execute()
		{
			var res = await _collection.UpdateManyAsync(_filterDefinition, _updateDefinition);
			return res.ModifiedCount > 0;
		}

		public async Task<int> ExecuteNums()
		{
			var res = await _collection.UpdateManyAsync(_filterDefinition, _updateDefinition);
			return (int)res.ModifiedCount;
		}
	}
}
