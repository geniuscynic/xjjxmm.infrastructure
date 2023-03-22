using System.Reflection;
using XjjXmm.Infrastructure.DataValidation.Attributes;
using XjjXmm.Infrastructure.DataValidation.Context;
using XjjXmm.Infrastructure.DataValidation.Validators;
using FieldInfo = XjjXmm.Infrastructure.DataValidation.Context.FieldInfo;

namespace XjjXmm.Infrastructure.DataValidation
{
	public class DataValidator
	{
		public List<ValidationResult> ValidationResults { get; } = new List<ValidationResult>();

		public ValidationResult? FirstValidationResult => ValidationResults.FirstOrDefault();

		public bool Valid(Object obj)
		{
			BeanInfo beanInfo = new BeanInfo();
			beanInfo.GetBeanInfo(obj);
			
			var validateContext = new ValidationContext() {  Root = obj, BeanInfo = beanInfo};

			ValidateField(obj, beanInfo.FieldInfos, validateContext);
			
			if (obj is IValidatableObject validatableObject)
			{
				var result = validatableObject.Validate(obj, validateContext);

				ValidationResults.AddRange(result);
			} 

			return true;
		}

		private void ValidateField(object? obj, List<FieldInfo> fieldInfos, ValidationContext validationContext)
		{
			foreach (var fieldInfo in fieldInfos)
			{
				validationContext.FiedInfo = fieldInfo;
				ValidateField(obj, validationContext);
			}
		}

		private void ValidateField(object? obj, ValidationContext validationContext)
		{
			var validators = validationContext.FiedInfo.ValidationAttributes;

			var val = BeanInfo.GetFieldValue(validationContext.FiedInfo, obj);
			
			if (validators.Any())
			{
				// var validateContext = new ValidationContext() { FiedInfo = propertyInfo, Root = obj };
				foreach (var validator in validators)
				{
					var isValid = validator.IsValid(val, validationContext);
					if (!isValid)
					{
						var message = validator.Message.Replace("{}", validationContext.FiedInfo.Desc);

						ValidationResults.Add(new ValidationResult(message));
					}
				}
			}

			ValidateField(val, validationContext.FiedInfo.Childrens, validationContext);

			if (val is IValidatableObject validatableObject)
			{
				var result = validatableObject.Validate(val, validationContext);

				ValidationResults.AddRange(result);
			} 
			
			/*if (validationContext.FiedInfo.GetCustomAttribute<ValidateAttribute>() != null)
			{
				var val = validationContext.FiedInfo.GetValue(obj);
				valid(val, )
			}*/
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
