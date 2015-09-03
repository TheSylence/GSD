using GSD.Models;

namespace GSD.Messages
{
	internal class TagAddedMessage
	{
		public TagAddedMessage( Tag tag )
		{
			Tag = tag;
		}

		public Tag Tag { get; }
	}
}