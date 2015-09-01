using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GSD.Models;

namespace GSD.ViewModels
{
	internal class TodoViewModel : ViewModelBase
	{
		public TodoViewModel( Todo todo )
		{
			Model = todo;
			Tags = new ObservableCollection<TodoTagViewModel>( Model.Tags.Select( t => new TodoTagViewModel( t ) ) );
		}

		public Todo Model { get; }

		public ObservableCollection<TodoTagViewModel> Tags { get; }
	}
}