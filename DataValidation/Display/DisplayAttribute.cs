namespace XjjXmm.Infrastructure.DataValidation.Display
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DisplayAttribute : Attribute
	{
		public string Field { get;  }
		
		public List<string> Value { get; }

		public DisplayAttribute(string field, List<string> value)
		{
			Field = field;
			Value = value;
		}
	}
}
