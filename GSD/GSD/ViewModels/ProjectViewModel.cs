using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight;
using GSD.Messages;
using GSD.Models;

namespace GSD.ViewModels
{
	internal class ProjectViewModel : ViewModelBase
	{
		public ProjectViewModel( Project project )
		{
			Model = project;

			Todos = new ObservableCollection<TodoViewModel>( Model.Todos.Select( t => new TodoViewModel( t ) ) );
		}

		public bool IsCurrent
		{
			[DebuggerStepThrough]
			get
			{
				return _IsCurrent;
			}
			set
			{
				if( _IsCurrent == value )
				{
					return;
				}

				_IsCurrent = value;
				RaisePropertyChanged();
				MessengerInstance.Send( new CurrentProjectChangedMessage() );
			}
		}

		public Project Model { get; }

		public ObservableCollection<TodoViewModel> Todos { get; }

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _IsCurrent;
	}
}