using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;

namespace CustomLogger
{
	public static class ServiceCollectionExtensions
	{
		public static void AddCustomLogger(this IServiceCollection serviceCollection)
		{
			serviceCollection
				.AddTransient(_ => 
					Policy
						.Handle<Exception>()
						.WaitAndRetryAsync(100, _ => TimeSpan.FromMilliseconds(new Random().Next(1, 5) * 100)))
				.AddTransient<ILogger, Logger>();
		}
	}
}
