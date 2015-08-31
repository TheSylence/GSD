using GalaSoft.MvvmLight;
using GSD.Models;

namespace GSD.ViewModels
{
	internal class TodoViewModel : ViewModelBase
	{
		public TodoViewModel( Todo todo )
		{
			Model = todo;
		}

		public Todo Model { get; }
	}
}