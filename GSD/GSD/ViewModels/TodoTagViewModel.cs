using GSD.Models;

namespace GSD.ViewModels
{
	internal class TodoTagViewModel : ViewModelBaseEx
	{
		public TodoTagViewModel( Tag tag )
		{
			Model = tag;
		}

		public Tag Model { get; }
	}
}