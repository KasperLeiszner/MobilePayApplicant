using Common;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomLogger;

public class Logger : ILogger
{
	private readonly Queue<Log> _logs = new();
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogStorage _storage;

	private bool _exit;
	private bool _quitWithFlush;

	public Logger(
		ILogStorage storage,
		IDateTimeProvider dateTimeProvider)
	{
		_storage = storage;
		_dateTimeProvider = dateTimeProvider;
	}

	public void StopWithoutFlush()
	{
		_exit = true;
	}

	public void StopWithFlush()
	{
		_quitWithFlush = true;
	}

	public Task WriteLog(string s)
	{
		_logs.Enqueue(new Log(s, _dateTimeProvider.Now()));

		return Task.CompletedTask;
	}
}
