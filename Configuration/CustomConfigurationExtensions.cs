using Microsoft.Extensions.Configuration;

namespace XjjXmm.Infrastructure.Configuration
{
	public static class CustomConfigurationExtensions
	{
		public static IConfigurationBuilder AddCustom(this IConfigurationBuilder builder)
		{
			return builder.Add(new CustomConfigurationSource());
		}
	}
}
