using LogComponent.Interfaces;
using System;

namespace LogComponent
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime CurrentDate { get; set; }

		public DateTimeProvider()
		{
			CurrentDate = Now();
		}

		public DateTime Now() => DateTime.Now;
	}
}
