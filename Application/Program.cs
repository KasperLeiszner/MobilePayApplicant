using System;
using System.Threading;
using LogComponent.Interfaces;
using LogComponent;

namespace LogUsers
{
    class Program
    {
        static void Main(string[] args)
        {
			ILogStorage storage = new LogStorage(@"C:\LogTest");

			//With Flush
			ILogger logger = new Logger(storage);
			logger.Start();

			for (int i = 0; i < 15; i++)
			{
				try
				{
					logger.WriteLog("Number with Flush: " + i.ToString());
					Thread.Sleep(50);
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
}
