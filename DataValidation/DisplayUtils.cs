using System.Reflection;
using XjjXmm.Infrastructure.DataValidation.Attributes;
using XjjXmm.Infrastructure.DataValidation.Context;
using XjjXmm.Infrastructure.DataValidation.Validators;
using FieldInfo = XjjXmm.Infrastructure.DataValidation.Context.FieldInfo;

namespace XjjXmm.Infrastructure.DataValidation
{
	public class DisplayUtils
	{
		private readonly object obj;
		BeanInfo beanInfo = new BeanInfo();
		
		public DisplayUtils(object obj)
		{
			this.obj = obj;
			beanInfo.GetBeanInfo(obj);
		}
		
		public void Process()
		{
			var validateContext = new ValidationContext() {  Root = obj, BeanInfo = beanInfo};

			Process(obj, beanInfo.FieldInfos, validateContext);
			
		}

		private void Process(object? obj, List<FieldInfo> fieldInfos, ValidationContext validationContext)
		{
			foreach (var fieldInfo in fieldInfos)
			{
				validationContext.FiedInfo = fieldInfo;
				
				if (IsShow(validationContext))
				{
					fieldInfo.PropertyInfo.SetValue(obj, null);
				}
				else
				{
					Process(obj, validationContext);	
				}
			}
		}

		private void Process(object? obj, ValidationContext validationContext)
		{
			var val = BeanInfo.GetFieldValue(validationContext.FiedInfo, obj);
			Process(val, validationContext.FiedInfo.Childrens, validationContext);
		}
		
		public bool IsShow(ValidationContext validationContext)
		{
			if (validationContext.FiedInfo.DisplayAttributes.Count == 0)
			{
				return true;
			}

			foreach (var display in validationContext.FiedInfo.DisplayAttributes)
			{
				FieldInfo fieldInfo = beanInfo.FindFieldInfo(display.Field, validationContext.FiedInfo);
				ValidationContext validationContext2 = new ValidationContext()
				{
					BeanInfo = validationContext.BeanInfo, FiedInfo = fieldInfo, Root = validationContext.Root
				};

				var isShow = IsShow(validationContext2);
				if (!isShow)
				{
					continue;
				}
					
				var val = BeanInfo.GetFieldValue(validationContext);
				isShow = beanInfo.isInclude(val, display.Value);
				if (isShow)
				{
					return true;
				}
			}

			return false;
		}

		
		/*private bool ValidateModel(bool needFullCheck = false, ValidateType validateType = ValidateType.CustomValdate)
		{
		    var type = _model.GetType();

		    var validators = type.GetCustomAttributes<AbstractValidator>();

		  

		    if (!validators.Any())
		    {
		        return true;
		    }



		    var val = _model;

		    foreach (var validator in validators)
		    {
		        if (validator.ValdateType == ValidateType.CustomValdate && validateType == ValidateType.AutoValdate)
		        {
		            continue;

		        }

		        var result = validator.IsValid(val, _model);

		        if (!result)
		        {
		            ValidationResults.Add(new ValidatonResult()
		            {
		                CustomMessage = validator.CustomMessage,
		                Description = validator.DisplayName,
		                Field = "",
		                FieldValue = val,
		                DefaultMessage = validator.DefaultMessage
		            });

		            if (!needFullCheck)
		            {
		                return false;
		            }
		        }
		    }

		    return true;

		}

		public bool Validate(bool needFullCheck = false, ValidateType validateType = ValidateType.CustomValdate)
		{
		    if (_model == null)
		    {
		        return false;
		    }

		    if(!ValidateModel(needFullCheck, validateType) && needFullCheck == false)
		    {
		        return false;
		    }

		    var type = _model.GetType();

		   

		    foreach (var propertyInfo in type.GetProperties())
		    {
		        var validators = propertyInfo.GetCustomAttributes<AbstractValidator>();

		        if (!validators.Any())
		        {
		            return true;
		        }



		        var val = propertyInfo.GetValue(_model);

		        foreach (var validator in validators)
		        {
		            if (validator.ValdateType == ValidateType.CustomValdate && validateType == ValidateType.AutoValdate)
		            {
		                continue;

		            }

		            var result = validator.IsValid(val, _model);

		            if (!result)
		            {
		                ValidationResults.Add(new ValidatonResult()
		                {
		                    CustomMessage = validator.CustomMessage,
		                    Description = validator.DisplayName,
		                    Field = propertyInfo.Name,
		                    FieldValue = val,
		                    DefaultMessage = validator.DefaultMessage
		                });

		                if (!needFullCheck)
		                {
		                    return false;
		                }
		            }
		        }


		    }

		    return ValidationResults.Count == 0;
		}

		public void AddCustomMessage(string message)
		{
		    ValidationResults.Add(new ValidatonResult()
		    {
		        CustomMessage = message
		    });
		}
	}*/
	}
}
