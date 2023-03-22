using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using xjjxmm.infrastructure.repository.impl.sugar;
using XjjXmm.Infrastructure.ToolKit;

namespace XjjXmm.Infrastructure.Mongo
{
	public class MongoRepository<T> where T : class, new()
	{
		protected readonly IMongoDatabase _context;
		protected readonly string _tableName;
		public MongoRepository(IMongoDatabase context)
		{
			_context = context;
			_tableName = SqlHelper.GetTableName<T>();
		}
		private IMongoCollection<T> GetCollection()
		{
			return _context.GetCollection<T>(_tableName);
		}

		public async Task<bool> Add(T entity)
		{
			await GetCollection().InsertOneAsync(entity);
			return true;
		}

		public async Task<bool> Add(List<T> entity)
		{
			await GetCollection().InsertManyAsync(entity);
			return true;
		}

		public async Task<bool> Update(T entity)
		{
			var _id = ReflectKit.GetFieldValue(entity, "Id");
			var filter = Builders<T>.Filter.Eq("_id", _id);
			await GetCollection().FindOneAndReplaceAsync(filter, entity);
			return true;
		}
		
		public async Task<bool> Update(List<T> entities)
		{
			foreach (T entity in entities)
			{
				await Update(entity);
			}

			return true;
		}

		public async Task<bool> Delete(dynamic id)
		{
			FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
			var res = await GetCollection().FindOneAndDeleteAsync(filter);

			return res != null;
		}

		public async Task<bool> Delete(Expression<Func<T, bool>> whereExpression)
		{
			var res = await GetCollection().FindOneAndDeleteAsync(whereExpression);

			return res != null;
		}

		public async Task<bool> SoftDelete(dynamic id)
		{
			//var _id = (ObjectId)id;
			FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
			var update = Builders<T>.Update.Set("IsDelete", true);

			await GetCollection().FindOneAndUpdateAsync(filter, update);

			return true;
	}

		public async Task<bool> SoftDelete(dynamic[] id)
		{
			//var _id = (ObjectId)id;
			var filter = Builders<T>.Filter.In("_id", id);
			var update = Builders<T>.Update.Set("IsDelete", true);

			await GetCollection().FindOneAndUpdateAsync(filter, update);

			return true;
		}

		public async Task<T> Find(dynamic id)
		{
			FilterDefinition<T> filter = Builders<T>.Filter.Eq("_id", id);
			var res = await GetCollection().FindAsync(filter);

			return await res.FirstOrDefaultAsync();
		}

		public async Task<T?> First(Expression<Func<T, bool>> whereExpression)
		{
			var res = await GetCollection().FindAsync(whereExpression);

			return await res.FirstOrDefaultAsync();
		}

		public async Task<List<T>> GetAll()
		{
			var res = await GetCollection().FindAsync(_ => true);

			return await res.ToListAsync();
		}

		public async Task<List<T>> Get(Expression<Func<T, bool>> whereExpression)
		{
			var res = await GetCollection().FindAsync(whereExpression);

			return await res.ToListAsync();
		}

		public MongoRepositoryQuery<T> Queryable() 
		{
			return new MongoRepositoryQuery<T>(_context, GetCollection());
		}

		public MongoRepositoryUpdatable<T> Updatable()
		{
			return new MongoRepositoryUpdatable<T>(_context, GetCollection());
		}

	}
}
