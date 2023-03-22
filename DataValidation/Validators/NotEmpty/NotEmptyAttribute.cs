
using System.Collections;
using XjjXmm.Infrastructure.DataValidation.Context;

namespace XjjXmm.Infrastructure.DataValidation.Validators.NotEmpty
{
	[AttributeUsage(AttributeTargets.Property)]
	public class NotEmptyAttribute : ValidationAttribute
	{
		public override string Message { get; set; }= "请填写{}";
		
		public override bool IsValid(object? o, ValidationContext context)
		{
			if (o == null)
			{
				return false;
			}

			if (o is ICollection array && array.Count == 0)
			{
				return false;
			}

			if (string.IsNullOrEmpty(o.ToString()))
			{
				return false;
			}

			return true;
		}
	}
}
