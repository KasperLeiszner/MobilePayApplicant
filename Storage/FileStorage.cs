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

	public FileStorage(IOptions<FileStorageOptions> config, IDateTimeProvider dateTimeProvider)
	{
		_dateTimeProvider = dateTimeProvider;
		_config = config.Value;

		if (!Directory.Exists(_config.FilePath))
			Directory.CreateDirectory(_config.FilePath);

		_streamWriter = CreateStoragePartition();
	}

	public Task Save(Log log)
	{
		if (log.TimeStamp > _dateTimeProvider.Now())
			_streamWriter = CreateStoragePartition();

		_streamWriter.WriteLine($"{log.TimeStamp}\t{log}");

		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_streamWriter.Flush();
		_streamWriter.BaseStream.Flush();
		_streamWriter.Dispose();
	}

	protected StreamWriter CreateStoragePartition()
	{
		var streamWriter = File.AppendText(_config.FilePath + @"\Log " + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");
		streamWriter.WriteLine("Timestamp ".PadRight(25, ' ') + "Data".PadRight(15, ' '));

		return streamWriter;
	}
}

