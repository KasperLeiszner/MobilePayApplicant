using System;

namespace Common
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime Now() => DateTime.Now;
	}
}
