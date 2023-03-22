
using XjjXmm.Infrastructure.DataValidation.Context;

namespace XjjXmm.Infrastructure.DataValidation.Validators
{
	public interface IValidatableObject
	{
		IEnumerable<ValidationResult> Validate(object o, ValidationContext validationContext);
	}
	
	public interface IValidatableObject2
	{
		IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(object o, 
			System.ComponentModel.DataAnnotations.ValidationContext
			validationContext);
	}
}
