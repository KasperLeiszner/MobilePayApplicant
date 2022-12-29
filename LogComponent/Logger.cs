using Common;
using Polly;
using Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomLogger
{
	public class Logger : ILogger
	{
		private readonly Queue<Log> _logs;

		private readonly ILogStorage _storage;
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly IAsyncPolicy _retryPolicy;

		private bool _exit;
		private bool _exitWithFlush;

		public Logger(
			ILogStorage storage, 
			IDateTimeProvider dateTimeProvider,
			IAsyncPolicy retryPolicy)
		{
			_storage = storage;
			_dateTimeProvider = dateTimeProvider;
			_retryPolicy = retryPolicy;
			_logs = new();

			new TaskFactory().StartNew(() => ProcessIncomingLogs());
		}

		public void StopWithoutFlush()
		{
			_exit = true;
		}

		public void StopWithFlush()
		{
			_exitWithFlush = true;
		}

		public void WriteLog(string logText)
		{
			_logs.Enqueue(new Log(logText, _dateTimeProvider.Now()));
		}

		private bool IsQueueEmpty() => _logs.Count <= 0;

		private Task ProcessIncomingLogs()
		{
			while (_exit is false)
			{
				if (_exitWithFlush && IsQueueEmpty() is true)
					break;

				if (IsQueueEmpty() is false)
					_storage.Save(_logs.Dequeue());
					//_retryPolicy.ExecuteAsync(() => _storage.Save(_logs.Dequeue()));
			}

			return Task.CompletedTask;
		}
	}
}
