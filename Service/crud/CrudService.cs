using XjjXmm.Infrastructure.Exceptions;
using XjjXmm.Infrastructure.IdGenerator;
using XjjXmm.Infrastructure.Mapper;
using xjjxmm.infrastructure.repository.entity;
using xjjxmm.infrastructure.repository.interfaces;
using XjjXmm.Infrastructure.ToolKit;

namespace xjjxmm.infrastructure.service.crud;

public class CrudService<TModel, TEntity> : CrudService<TModel, TModel, TEntity>,
	ICrudService<TModel>
	where TEntity : class, new()
{
	public CrudService(IRepositoryBase<TEntity> repositoryBase) : base(repositoryBase)
	{
	}
}
	
public class CrudService<TSaveModel, TDetailModel, TEntity> 
	: CrudService<TSaveModel, TSaveModel, TDetailModel, TEntity>, ICrudService<TSaveModel, TDetailModel>
	where TEntity : class, new()
{
	public CrudService(IRepositoryBase<TEntity> repositoryBase) : base(repositoryBase)
	{
	}

	public virtual async Task<bool> Save(TSaveModel input)
	{
		var entity = input.MapTo<TSaveModel, TEntity>();
		if (entity is EntityFull full)
		{
			if (full.Id.IsNullOrEmpty())
			{
				var res =  await base.Add(input);
				return res;
			}
			else
			{
				return await base.Update(input);
			}
		}
		else if (entity is EntityAdd tmp)
		{
			if (tmp.Id.IsNullOrEmpty())
			{
				var res = await base.Add(input);
				
				return res;
			}
			else
			{
				return await base.Update(input);
			}
		}

		throw new BussinessException(StatusCodes.Status999Falid, "实体层没有继承entityful/entityAdd");
	}
}
	
public class CrudService<TAddModel, TUpdateModel, TDetailModel, TEntity> : BaseService, ICrudService<TAddModel, TUpdateModel, TDetailModel> 
	where TEntity : class, new()
{
	private readonly IRepositoryBase<TEntity> _repositoryBase;
       
	/// <summary>
	/// 映射
	/// </summary>
	// public IMapper _mapper;
	public CrudService(IRepositoryBase<TEntity> repositoryBase)
	{
		_repositoryBase = repositoryBase;
	}

	protected TEntity GetAddEntity(TAddModel model)
	{
		var entity = model.MapTo<TAddModel, TEntity>();
		FillAdd(entity);
		return entity;
	}
        
	protected List<TEntity> GetAddEntity(List<TAddModel> model)
	{
		var entity = model.MapTo<TAddModel, TEntity>().ToList();
		FillAdd(entity);

		return entity;
	}
        
	protected TEntity GetUpdateEntity(TUpdateModel model)
	{
		var entity = model.MapTo<TUpdateModel, TEntity>();
		FillUpdate(entity);
		return entity;
	}
        
	protected List<TEntity> GetUpdateEntity(List<TUpdateModel> model)
	{
		var entity = model.MapTo<TUpdateModel, TEntity>().ToList();
		FillUpdate(entity);

		return entity;
	}

	public virtual async Task<bool> Add(TAddModel model)
	{
		var entity = GetAddEntity(model);
		model.SetFieldValue("Id", entity.GetFieldValue("Id"));
		return await _repositoryBase.Add(entity);
	}

	public virtual async Task<bool> Add(List<TAddModel> model)
	{
		var entity = GetAddEntity(model);
		return await _repositoryBase.Add(entity);
	}

	public virtual async Task<bool> Update(TUpdateModel model)
	{
		var entity = GetUpdateEntity(model);
		return await _repositoryBase.Update(entity);
	}
        
	public virtual async Task<bool> Delete(string id)
	{
		return await _repositoryBase.Delete(id);
	}

	public virtual async Task<bool> SoftDelete(string id)
	{
		return await _repositoryBase.SoftDelete(id);
	}

	public virtual async Task<bool> SoftDelete(string[] id)
	{
		return await _repositoryBase.SoftDelete(id);
	}

	public virtual async Task<bool> Restore(string id)
	{
		return await _repositoryBase.Restore(id);
	}
	
	public virtual async Task<TDetailModel> Find(string id)
	{
		TEntity entity = await _repositoryBase.Find(id);
		return entity.MapTo<TEntity, TDetailModel>();
	}

	public virtual async Task<IEnumerable<TDetailModel>> GetAll()
	{
		var entities = await _repositoryBase.GetAll();

		var res = entities.MapTo<TEntity, TDetailModel>();
		return res;
	}
	
	protected void FillAdd(TEntity entity)
	{
		if (entity is EntityFull)
		{
			var tmp = entity as EntityFull;
			tmp.CreatedTime = DateTime.Now;
			tmp.CreatorName = _userContext.NickName;
			tmp.CreatorId = _userContext.Id;

			tmp.UpdateTime = DateTime.Now;
			tmp.UpdaterId = _userContext.Id;
			tmp.UpdaterName = _userContext.NickName;

			tmp.TimeStamp = DateTime.Now.ToTotalMilliseconds();
			tmp.Id = SnowFlakeKit.NextId().ToString();
		}
		else if (entity is EntityAdd)
		{
			var tmp = entity as EntityAdd;
			tmp.CreatedTime = DateTime.Now;
			tmp.CreatorName = _userContext.NickName;
			tmp.CreatorId = _userContext.Id;
			
			tmp.Id = SnowFlakeKit.NextId().ToString();
		}
	}

	protected void FillAdd(List<TEntity> entities)
	{
		entities.ForEach(e => FillAdd(e));
	}
	protected void FillUpdate(TEntity entity)
	{
		if (entity is EntityFull)
		{
			var tmp = entity as EntityFull;

			tmp.UpdateTime = DateTime.Now;
			tmp.UpdaterId = _userContext.Id;
			tmp.UpdaterName = _userContext.NickName;
			tmp.TimeStamp = DateTime.Now.ToTotalMilliseconds();
		}
	}

	protected void FillUpdate(List<TEntity> entities)
	{
		entities.ForEach(e => FillUpdate(e));
	}
}
