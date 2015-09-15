using GSD.Models;

namespace GSD.Messages
{
	internal class EntryAddedMessage
	{
		public EntryAddedMessage( Todo entry )
		{
			Entry = entry;
		}

		public Todo Entry { get; }
	}
}