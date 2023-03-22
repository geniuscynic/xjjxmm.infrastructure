using Autofac;
using Autofac.Core;

namespace XjjXmm.Infrastructure.Ioc
{
	public class IocManager
	{
		public static IocManager Instance = new IocManager();

		public ILifetimeScope Container { get; set; }

		public T GetService<T>()
		{
			return ResolutionExtensions.Resolve<T>((IComponentContext)(object)this.Container);
		}

		public T GetService<T>(string serviceKey)
		{
			return ResolutionExtensions.ResolveKeyed<T>((IComponentContext)(object)this.Container, (object)serviceKey);
		}

		public T GetService<T>(string serviceKey, params Parameter[] parameters)
		{
			return ResolutionExtensions.ResolveKeyed<T>((IComponentContext)(object)this.Container, (object)serviceKey, parameters);
		}

		public object GetService(Type serviceType)
		{
			return ResolutionExtensions.Resolve((IComponentContext)(object)this.Container, serviceType);
		}

		public object GetService(string serviceKey, Type serviceType)
		{
			return ResolutionExtensions.ResolveKeyed((IComponentContext)(object)this.Container, (object)serviceKey, serviceType);
		}

		public bool IsRegistered<T>()
		{
			return ResolutionExtensions.IsRegistered<T>((IComponentContext)(object)this.Container);
		}

		public bool IsRegistered<T>(string serviceKey)
		{
			return ResolutionExtensions.IsRegisteredWithKey<T>((IComponentContext)(object)this.Container, (object)serviceKey);
		}

		public bool IsRegistered(Type serviceType)
		{
			return ResolutionExtensions.IsRegistered((IComponentContext)(object)this.Container, serviceType);
		}

		public bool IsRegisteredWithKey(string serviceKey, Type serviceType)
		{
			return ResolutionExtensions.IsRegisteredWithKey((IComponentContext)(object)this.Container, (object)serviceKey, serviceType);
		}
	}
}
