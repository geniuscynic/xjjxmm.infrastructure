
using XjjXmm.Infrastructure.DataValidation.Context;

namespace XjjXmm.Infrastructure.DataValidation
{
	public abstract  class ValidationAttribute : Attribute
	{
		public virtual string Message { get; set; } = "{}不符合规定";

		public string Group { get; set; } = "";

		public int Order { get; set; } = 1;

		public abstract bool IsValid(object? o, ValidationContext context);
	}
}
