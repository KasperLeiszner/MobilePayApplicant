using Microsoft.Extensions.Hosting;
using System.Threading;
using System;
using System.Threading.Tasks;
using CustomLogger;

namespace Application
{
	public class LoggerTestRunner : BackgroundService
	{
		private readonly ILogger _logger;
		private readonly ILogger _logger2;

		public LoggerTestRunner(ILogger logger, ILogger logger2)
		{
			_logger = logger;
			_logger2 = logger2;
		}

		public void TestRunLogger()
		{
			for (int i = 1000; i > 0; i--)
			{
				_logger2.WriteLog("Number with No flush: " + i.ToString());
			}

			Thread.Sleep(1); // For showcase purposes  
			_logger2.StopWithoutFlush();
		}

		public void TestRunLoggerWithFlush()
		{
			for (int i = 0; i < 1000; i++)
			{
				_logger.WriteLog("Number with Flush: " + i.ToString());
			}

			Thread.Sleep(1); // For showcase purposes  
			_logger.StopWithFlush();
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			TestRunLogger();
			TestRunLoggerWithFlush();

			Console.WriteLine("Done");
			Console.ReadLine();

			return Task.CompletedTask;
		}
	}
}
