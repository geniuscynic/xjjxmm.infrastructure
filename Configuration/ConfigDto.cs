using Newtonsoft.Json;
using XjjXmm.Infrastructure.ToolKit;


namespace XjjXmm.Infrastructure.Configuration
{

	public class ConfigDto
	{
		public string JsonKey
		{
			get
			{
				if (Group.IsNullOrEmpty())
				{
					return Key;
				}

				return $"{Group}:{Key}";
			}
		}
		
		public string AppId { get; set; } = "";

		public string Group { get; set; } = "";
		public string Key { get; set; } = "";

		public string Value { get; set; } = "";

		public int Seq { get; set; } = 1;

		public DateTime UpdateTime { get; set; } = DateTime.Now;
	}
}
