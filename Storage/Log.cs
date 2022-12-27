using System;

namespace Storage
{
	public record Log(string Text, DateTime TimeStamp)
	{
		public override string ToString()
		{
			return Text.Length > 0 ? Text : "";
		}
	}
}