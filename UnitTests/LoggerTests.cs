using System;
using Xunit;
using Moq;
using LogComponent;
using System.IO;
using AutoFixture.Xunit2;
using System.Threading;
using CustomLogger;

namespace UnitTests
{
	public class LoggerTests
	{
		[Theory, AutoData]
		public void LoggerWriteLog(
				Mock<ILogStorage> storage,
				Mock<IDateTimeProvider> dateTimeProvider
			)
		{
			var stream = new Mock<StreamWriter>(Console.OpenStandardOutput());
			storage.Setup(mock => mock.Stream).Returns(() => stream.Object);
			
			ILogger sut = new Logger(storage.Object);

			sut.WriteLog("test1");
			sut.WriteLog("test2");
			sut.WriteLog("test3");
			sut.WriteLog("test4");
			sut.WriteLog("test5");
			sut.WriteLog("test6");
			sut.ProcessLogs(dateTimeProvider.Object);

			storage.Verify(mock => mock.CreateLogFile(), Times.Once);
			storage.Verify(mock => mock.Stream.Write(It.IsAny<string>()), Times.Exactly(6));
		}

		[Theory, AutoData]
		public void LoggerStopWithFlush(
				Mock<ILogStorage> storage
			)
		{
			var stream = new Mock<StreamWriter>(Console.OpenStandardOutput());
			storage.Setup(mock => mock.Stream).Returns(() => stream.Object);

			ILogger sut = new Logger(storage.Object);

			sut.Start();

			int logCount = 14;
			for (int i = 0; i < logCount; i++)
			{
				if (i == logCount / 2)
					sut.StopWithFlush();

				sut.WriteLog("test" + i);
				Thread.Sleep(50);
			}

			sut.Join();

			storage.Verify(mock => mock.CreateLogFile(), Times.Once);
			storage.Verify(mock => mock.Stream.Write(It.IsAny<string>()), Times.AtLeast(logCount / 2));
		}

		[Theory, AutoData]
		public void LoggerStopWithoutFlush(
				Mock<ILogStorage> storage
			)
		{
			var stream = new Mock<StreamWriter>(Console.OpenStandardOutput());
			storage.Setup(mock => mock.Stream).Returns(() => stream.Object);

			ILogger sut = new Logger(storage.Object);

			sut.Start();

			int logCount = 50;
			for (int i = 0; i < logCount; i++)
			{
				if (i == logCount / 2)
					sut.StopWithoutFlush();

				sut.WriteLog("test" + i);
				Thread.Sleep(20);
			}

			sut.Join();
			
			storage.Verify(mock => mock.CreateLogFile(), Times.Once);
			storage.Verify(mock => mock.Stream.Write(It.IsAny<string>()), Times.Exactly(logCount / 2));
		}

		[Theory, AutoData]
		public void CreateNewFileAtMidnight(
				Mock<ILogStorage> storage,
				Mock<IDateTimeProvider> dateTimeProvider
			)
		{
			var stream = new Mock<StreamWriter>(Console.OpenStandardOutput());
			storage.Setup(mock => mock.Stream).Returns(() => stream.Object);

			ILogger sut = new Logger(storage.Object);

			sut.WriteLog("test");
			sut.ProcessLogs(dateTimeProvider.Object);

			dateTimeProvider.Setup(mock => mock.Now()).Returns(DateTime.Today.AddDays(1.0));

			sut.WriteLog("test2");
			sut.ProcessLogs(dateTimeProvider.Object);

			dateTimeProvider.Verify(mock => mock.Now(), Times.Exactly(2));
			storage.Verify(mock => mock.CreateLogFile(), Times.Exactly(2));
			storage.Verify(mock => mock.Stream.Write(It.IsAny<string>()), Times.Exactly(3));
		}
	}
}
