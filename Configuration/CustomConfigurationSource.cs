using Microsoft.Extensions.Configuration;

namespace XjjXmm.Infrastructure.Configuration
{
	public class CustomConfigurationSource : IConfigurationSource
	{
		public IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return new CustomConfigurationProvider();
		}
	}
}
