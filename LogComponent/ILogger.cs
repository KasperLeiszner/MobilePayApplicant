﻿using System.Threading.Tasks;

namespace CustomLogger;

public interface ILogger
{
	public void StopWithoutFlush();
	public void StopWithFlush();
	public void WriteLog(string logText);
}