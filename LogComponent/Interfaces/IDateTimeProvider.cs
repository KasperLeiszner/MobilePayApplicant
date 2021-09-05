using System;

namespace LogComponent.Interfaces
{
	public interface IDateTimeProvider
	{
		DateTime CurrentDate { get; set; }
		DateTime Now();
	}
}
