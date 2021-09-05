using LogComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LogComponent
{
    public class Logger : ILogger
    {
        private readonly List<LogLine> _lines = new List<LogLine>();
        private readonly DateTimeProvider _dateTimeProvider;
        private readonly ILogStorage _storage;
        
        private Thread _runThread;

        private bool _exit;
        private bool _quitWithFlush;

        public List<LogLine> Lines { get => _lines.ToList(); }

        public Logger(ILogStorage storage)
        {
            _storage = storage;
            _dateTimeProvider = new DateTimeProvider();

            _storage.CreateLogFile();
        }

        public void Start()
		{
            _runThread = new Thread(MainLoop);
            _runThread.Start();
        }

        public void Join()
		{
            _runThread.Join();
		}

        public void MainLoop()
		{
            try
			{
                while (!_exit)
                {
                    if (_lines.Count > 0)
                    {
                        ProcessLogs(_dateTimeProvider);
                    }
                }
            }
            catch
			{
                _storage.Stream.Write("Error occured, recovering...");
            }
        }

        public void ProcessLogs(IDateTimeProvider dateTimeProvider)
        {
            var handled = new List<LogLine>();

            foreach (LogLine logLine in _lines.ToList())
            {
                if (!_exit || _quitWithFlush)
                {
                    handled.Add(logLine);

                    var stringBuilder = new StringBuilder();

                    if ((dateTimeProvider.CurrentDate.AddDays(1.0) - dateTimeProvider.Now()).TotalSeconds <= 0)
                    {
                        dateTimeProvider.CurrentDate = DateTime.Now;

                        _storage.CreateLogFile();

                        stringBuilder.Append(Environment.NewLine);
                        _storage.Stream.Write(stringBuilder.ToString());
                    }

                    stringBuilder.Append(logLine.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                    stringBuilder.Append("\t");
                    stringBuilder.Append(logLine.LineText());
                    stringBuilder.Append("\t");
                    stringBuilder.Append(Environment.NewLine);

                    _storage.Stream.Write(stringBuilder.ToString());
                }
            }

            for (int y = 0; y < handled.Count; y++)
            {
                _lines.Remove(handled[y]);
            }

            if (_quitWithFlush && _lines.Count == 0)
                _exit = true;
        }

        public void StopWithoutFlush()
        {
            _exit = true;
        }

        public void StopWithFlush()
        {
            _quitWithFlush = true;
        }

        public void WriteLog(string s)
        {
            _lines.Add(new LogLine() { Text = s, Timestamp = DateTime.Now });
        }
    }
}