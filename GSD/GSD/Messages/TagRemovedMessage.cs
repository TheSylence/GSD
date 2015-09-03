using GSD.Models;

namespace GSD.Messages
{
	internal class TagRemovedMessage
	{
		public TagRemovedMessage( Tag tag )
		{
			Tag = tag;
		}

		public Tag Tag { get; }
	}
}