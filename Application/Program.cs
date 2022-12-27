using CustomLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Storage;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Application;

class Program
{
    public async static void Main(string[] args)
    {
		using IHost host = Host.CreateDefaultBuilder(args)
			.ConfigureServices((_, services) =>
				services
				.AddSingleton<CustomLogger.ILogger, Logger>()
				.AddFileStorage())
			.Build();

		await host.RunAsync();
	}

	public static void TestRunLogger()
    {
		for (int i = 0; i < 15; i++)
		{
			try
			{
				logger.WriteLog("Number with Flush: " + i.ToString());
			}
			catch
			{
				continue;
			}
		}

		logger.StopWithFlush();

		//Without flush
		ILogger logger2 = new Logger(storage);
		logger2.Start();

		for (int i = 50; i > 0; i--)
		{
			try
			{
				if (i == 25)
					logger2.StopWithoutFlush();

				logger2.WriteLog("Number with No flush: " + i.ToString());
				Thread.Sleep(20);
			}
			catch
			{
				continue;
			}
		}

		Console.WriteLine("Done");
		Console.ReadLine();
	}
}
