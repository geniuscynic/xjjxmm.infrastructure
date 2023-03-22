using Microsoft.AspNetCore.Mvc;
using xjjxmm.infrastructure.service.crud;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace xjjxmm.infrastructure.Webs.controller;

/// <summary>
/// 管理
/// </summary>
[Route("[controller]")]
[ApiController]
public class BaseController<T, TModel> : BaseController<T, TModel, TModel> 
	where T: ICrudService<TModel>
{
	public BaseController(T service) : base(service)
	{
		
	}
}

/// <summary>
/// 管理
/// </summary>
[Route("[controller]")]
[ApiController]
public class BaseController<T, TSaveModel, TDetailModel> : BaseController<T, TSaveModel, TSaveModel, TDetailModel> 
	where T: ICrudService<TSaveModel, TDetailModel>
{
	public BaseController(T service) : base(service)
	{
		
	}
	
	/// <summary>
	/// 保存
	/// </summary>
	/// <returns></returns>
	[HttpPost("save")]
	public async Task<bool> Save(TSaveModel dto)
	{
		return await _service.Save(dto);
	}
}

/// <summary>
/// 管理
/// </summary>
[Route("[controller]")]
[ApiController]
public class BaseController<T, TAddModel, TUpdateModel, TDetailModel> : ControllerBase where 
	T: ICrudService<TAddModel, TUpdateModel, TDetailModel>
{
	protected readonly T _service;
	
	public BaseController(T service)
	{
		_service = service;
	}
		
	/// <summary>
	/// 获取所有
	/// </summary>
	/// <returns></returns>
	[HttpGet("all")]
	public virtual async Task<IEnumerable<TDetailModel>> Gets()
	{
		return await _service.GetAll();
	}
	
	/// <summary>
	/// 获取详情
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet("detail/{id}")]
	public virtual async Task<TDetailModel> Get(string id)
	{
		return await _service.Find(id);
	}
	
	/// <summary>
	/// 新增
	/// </summary>
	/// <returns></returns>
	[HttpPost("add")]
	public async Task<bool> Add(TAddModel dto)
	{
		return await _service.Add(dto);
	}
	
	/// <summary>
	/// 更新
	/// </summary>
	/// <returns></returns>
	[HttpPost("update")]
	public async Task<bool> Update(TUpdateModel dto)
	{
		return await _service.Update(dto);
	}
		
	/// <summary>
	/// 删除租户
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet("delete/{id}")]
	public async Task<bool> Delete(string id)
	{
		return await _service.SoftDelete(id);
	}
		
	/// <summary>
	/// 找回
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet("restore/{id}")]
	public async Task<bool> Restore(string id)
	{
		return await _service.Restore(id);
	}
		
}
