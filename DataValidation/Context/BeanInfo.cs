
using System.Reflection;
using XjjXmm.Infrastructure.DataValidation.Attributes;
using XjjXmm.Infrastructure.DataValidation.Display;
using XjjXmm.Infrastructure.DataValidation.Validators;

namespace XjjXmm.Infrastructure.DataValidation.Context
{
	public class BeanInfo
	{
		public  List<FieldInfo> FieldInfos { get;  } = new List<FieldInfo>();
		
		public void GetBeanInfo(object obj)
		{
			var type = obj.GetType();

			foreach (var propertyInfo in type.GetProperties())
			{
				FieldInfo fieldInfo = new FieldInfo();
				fieldInfo.PropertyInfo = propertyInfo;

				FieldInfos.Add(fieldInfo);

				GetFieldInfo(fieldInfo);
			}
		}

		private void GetFieldInfo(FieldInfo fieldInfo)
		{
			var validatorConstraint = fieldInfo.PropertyInfo.GetCustomAttributes<ValidationAttribute>();
			fieldInfo.ValidationAttributes.AddRange(validatorConstraint);

			var displayAttributes = fieldInfo.PropertyInfo.GetCustomAttributes<DisplayAttribute>();
			fieldInfo.DisplayAttributes.AddRange(displayAttributes);
			
			if (fieldInfo.PropertyInfo.GetCustomAttribute<ValidateAttribute>() != null)
			{
				var type = fieldInfo.PropertyInfo.PropertyType;

				foreach (var subPropertyInfo in type.GetProperties())
				{
					FieldInfo childInfo = new FieldInfo();
					childInfo.PropertyInfo = subPropertyInfo;
					childInfo.PathInfos.Add(fieldInfo);
					
					fieldInfo.Childrens.Add(childInfo);


					GetFieldInfo(childInfo);
				}
			}
			
		}


		public static object? GetFieldValue(FieldInfo fieldInfo, object? obj)
		{
			/*foreach(var item in  fieldInfo.PathInfos)
			{
				if (obj != null)
				{
					obj = item.PropertyInfo.GetValue(obj);
				}
			}*/

			if (obj != null)
			{
				obj = fieldInfo.PropertyInfo.GetValue(obj);
			}

			return obj;
		}
		
		public static object? GetFieldValue(ValidationContext validationContext)
		{
			var obj = validationContext.Root;
			foreach(var item in  validationContext.FiedInfo.PathInfos)
			{
				if (obj != null)
				{
					obj = item.PropertyInfo.GetValue(obj);
				}
			}

			if (obj != null)
			{
				obj = validationContext.FiedInfo.PropertyInfo.GetValue(obj);
			}

			return obj;
		}

		public  FieldInfo FindFieldInfo(string field, FieldInfo currentField)
		{
			if (field.Contains("."))
			{
				FieldInfo? fieldInfo = null;
				var currentFieldList = FieldInfos;

				foreach (string s in field.Split("."))
				{
					fieldInfo = currentFieldList.FirstOrDefault(t => t.Name == s);
					if (fieldInfo == null)
					{
						throw new Exception($"找不到字段:{field}");
					}

					currentFieldList = fieldInfo.Childrens;
				}

				return fieldInfo;
			}
			else
			{
				field = currentField.Path + "." + field;

				return FindFieldInfo(field, currentField);
			}
		}

		
		private bool IsShow(ValidationContext validationContext)
		{
			if (validationContext.FiedInfo.DisplayAttributes.Count == 0)
			{
				return true;
			}

			foreach (var display in validationContext.FiedInfo.DisplayAttributes)
			{
				FieldInfo fieldInfo = FindFieldInfo(display.Field, validationContext.FiedInfo);
					ValidationContext validationContext2 = new ValidationContext()
					{
						BeanInfo = validationContext.BeanInfo, FiedInfo = fieldInfo, Root = validationContext.Root
					};

					var isShow = IsShow(validationContext2);
					if (!isShow)
					{
						continue;
					}
					
					var val = GetFieldValue(validationContext);
					isShow = isInclude(val, display.Value);
					if (isShow)
					{
						return true;
					}
			}

			return false;
		}

		public bool isInclude(object? obj, List<string> val)
		{
			if (obj == null)
			{
				return false;
			}

			if (obj is string str)
			{
				return val.Contains(str);
			}
			else if (obj is IEnumerable<string> list)
			{
				return list.Any(val.Contains);
			}
			else if(obj is bool bl)
			{
				return Convert.ToBoolean(bl);
			}

			return false;
		}
	}
}
