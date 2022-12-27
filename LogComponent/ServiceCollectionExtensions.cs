using Microsoft.Extensions.DependencyInjection;

namespace CustomLogger
{
	public static class ServiceCollectionExtensions
	{
		public static void AddCustomLogger(this ServiceCollection serviceCollection)
		{
			serviceCollection
				.AddSingleton<ILogger, Logger>()
				.AddHostedService<WriteLogCommandHandler>();
		}
	}
}
