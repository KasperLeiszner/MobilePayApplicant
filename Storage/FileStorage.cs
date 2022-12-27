using Common;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Storage;
public class FileStorage : ILogStorage, IDisposable
{
	private readonly FileStorageOptions _config;
	private readonly IDateTimeProvider _dateTimeProvider;
	private StreamWriter _streamWriter;

	public FileStorage(
		IOptions<FileStorageOptions> config, 
		IDateTimeProvider dateTimeProvider)
	{
		_dateTimeProvider = dateTimeProvider;
		_streamWriter.AutoFlush = true;
		_config = config.Value;

		if (!Directory.Exists(_config.FilePath))
			Directory.CreateDirectory(_config.FilePath);
	}

	public Task Save(Log log)
	{
		if (log.TimeStamp > _dateTimeProvider.Now())
			_streamWriter = CreateStoragePartition();

		_streamWriter.Write(
			"Timestamp ".PadRight(25, ' ') + log.TimeStamp.ToString().PadRight(25, ' ') + 
			"Data".PadRight(15, ' ') + log + Environment.NewLine);

		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_streamWriter.Dispose();
	}

	private StreamWriter CreateStoragePartition()
	{
		return File.AppendText(_config.FilePath + @"\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");
	}
}

