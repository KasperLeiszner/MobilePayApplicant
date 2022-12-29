using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Storage
{
	public static class ServiceCollectionExtension
	{
		public static void AddFileStorage(this IServiceCollection serviceCollection, IConfiguration config)
		{
			serviceCollection.Configure<FileStorageOptions>(config.GetSection("FileStorageOptions"));
			serviceCollection.AddSingleton<ILogStorage, FileStorage>();
		}
	}
}
