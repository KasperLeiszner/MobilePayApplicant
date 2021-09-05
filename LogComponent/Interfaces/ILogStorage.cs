using System.IO;

namespace LogComponent.Interfaces
{
	public interface ILogStorage
	{
		void CreateLogFile();
		StreamWriter Stream { get; }
		string FilePath { get; }
	}
}
