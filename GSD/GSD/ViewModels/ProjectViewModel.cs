using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GSD.Models;

namespace GSD.ViewModels
{
	internal class ProjectViewModel : ViewModelBase
	{
		public ProjectViewModel( Project project )
		{
			Model = project;
		}

		public Project Model { get; }
		public ObservableCollection<TagViewModel> Tags { get; }
		public ObservableCollection<TodoViewModel> Todos { get; }
	}
}