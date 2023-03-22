namespace xjjxmm.infrastructure.service.crud;

public interface ICrudService<TDetailModel> :ICrudService<TDetailModel, TDetailModel>
{

}

public interface ICrudService<TSaveModel, TDetailModel> :ICrudService<TSaveModel, TSaveModel, TDetailModel>
{
	Task<bool> Save(TSaveModel input);
}
public interface ICrudService<TAddModel, TUpdateModel, TDetailModel> 
{
	/// <summary>
	/// 返回主键
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	Task<bool> Add(TAddModel entity);

	/// <summary>
	/// 批量返回主键
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	Task<bool> Add(List<TAddModel> entity);

	/// <summary>
	/// 更新实体
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	Task<bool> Update(TUpdateModel entity);

	/// <summary>
	/// 根据主键删除
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	Task<bool> Delete(string id);

	/// <summary>
	/// 软删除
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	Task<bool> SoftDelete(string id);

	/// <summary>
	/// 批量软删除
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	Task<bool> SoftDelete(string[] id);

	public Task<bool> Restore(string id);
	
	Task<TDetailModel> Find(string id);

	Task<IEnumerable<TDetailModel>> GetAll();

}