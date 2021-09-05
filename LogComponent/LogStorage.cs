using System;
using System.IO;
using LogComponent.Interfaces;

namespace LogComponent
{
	public class LogStorage : ILogStorage
	{
        private readonly string _filePath;
        private StreamWriter _Stream;

        public StreamWriter Stream { get => _Stream; }

        public string FilePath { get => _filePath; }

        public LogStorage(string filePath)
		{
            _filePath = filePath;
		}

        public void CreateLogFile()
        {
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);

            _Stream = File.AppendText(_filePath + @"\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");
            _Stream.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);
            _Stream.AutoFlush = true;
        }
    }
}
