using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
	public static class ServiceCollectionExtension
	{
		public static void AddFileStorage(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddOptions<FileStorageOptions>("FileStorage:FilePath");
			serviceCollection.AddSingleton<ILogStorage, FileStorage>();
		}
	}
}
