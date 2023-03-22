using System.Reflection;

namespace XjjXmm.Infrastructure.DataValidation.Context
{
	public class ValidationContext
	{
		public object Root { get; set; }
		
		public FieldInfo FiedInfo { get; set; }
		
		public BeanInfo BeanInfo { get; set; }
	}
}
