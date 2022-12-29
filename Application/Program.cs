using Common;
using CustomLogger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Storage;
using System;
using System.IO;
using System.Threading;

namespace Application;

class Program
{
    public static void Main(string[] args)
    {
		var config = (IConfiguration) new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();

		using IHost host = Host
			.CreateDefaultBuilder(args)
			.ConfigureServices((_, services) =>
			{
				services.AddFileStorage(config);
				services.AddCustomLogger();
				services.AddTransient<IDateTimeProvider, DateTimeProvider>();
				services.AddHostedService<LoggerTestRunner>();
			})
			.Build();

		host.Run();
	}
}
