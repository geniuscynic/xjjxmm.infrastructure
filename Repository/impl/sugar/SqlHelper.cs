using SqlSugar;
using System.Reflection;

namespace xjjxmm.infrastructure.repository.impl.sugar;

internal class SqlHelper

{
	public static string GetTableName<T>()
	{
		return typeof(T).GetCustomAttribute<SugarTable>()?.TableName ?? typeof(T).Name;
	}

}