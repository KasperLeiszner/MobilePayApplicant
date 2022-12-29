using System.Threading.Tasks;

namespace Storage
{
	public interface ILogStorage
	{
		public Task Save(Log log);
	}
}
