using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight;

namespace GSD.ViewModels
{
	internal class MainViewModel : ViewModelBase
	{
		public ProjectViewModel CurrentProject
		{
			[DebuggerStepThrough]
			get
			{
				return _CurrentProject;
			}
			set
			{
				if( _CurrentProject == value )
				{
					return;
				}

				_CurrentProject = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<ProjectViewModel> Projects { get; }

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private ProjectViewModel _CurrentProject;
	}
}