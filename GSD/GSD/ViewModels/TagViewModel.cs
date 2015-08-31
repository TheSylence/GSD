using GSD.Models;

namespace GSD.ViewModels
{
	internal class TagViewModel
	{
		public TagViewModel( Tag tag )
		{
			Model = tag;
		}

		public Tag Model { get; }
	}
}