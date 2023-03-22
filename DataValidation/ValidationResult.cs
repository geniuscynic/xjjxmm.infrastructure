namespace XjjXmm.Infrastructure.DataValidation
{
	public class ValidationResult
	{
		public string Message { get; set; }

		public ValidationResult(string message)
		{
			this.Message = message;
		}
		
		/// <summary>Returns a string representation of the current validation result.</summary>
		/// <returns>The current validation result.</returns>
		public override string ToString() => this.Message;
	}
}
