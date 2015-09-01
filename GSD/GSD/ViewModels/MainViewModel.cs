using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GSD.Messages;

namespace GSD.ViewModels
{
	internal class MainViewModel : ViewModelBaseEx
	{
		public MainViewModel()
		{
			ProjectList = new ProjectListViewModel();
			TagList = new TagListViewModel( ProjectList );
		}

		private bool CanExecuteAddEntryCommand()
		{
			return ProjectList.CurrentProject != null;
		}

		private bool CanExecuteOpenTagManagementCommand()
		{
			return ProjectList.CurrentProject != null;
		}

		private void ExecuteAddEntryCommand()
		{
		}

		private void ExecuteOpenProjectManagementCommand()
		{
			MessengerInstance.Send( new FlyoutMessage( ProjectFlyoutName ) );
		}

		private void ExecuteOpenTagManagementCommand()
		{
			MessengerInstance.Send( new FlyoutMessage( TagFlyoutName ) );
		}

		public ICommand AddEntryCommand => _AddEntryCommand ?? ( _AddEntryCommand = new RelayCommand( ExecuteAddEntryCommand, CanExecuteAddEntryCommand ) );

		public ICommand OpenProjectManagementCommand => _OpenProjectManagementCommand ?? ( _OpenProjectManagementCommand = new RelayCommand( ExecuteOpenProjectManagementCommand ) );

		public ICommand OpenTagManagementCommand => _OpenTagManagementCommand ?? ( _OpenTagManagementCommand = new RelayCommand( ExecuteOpenTagManagementCommand, CanExecuteOpenTagManagementCommand ) );

		public ProjectListViewModel ProjectList { get; }

		public TagListViewModel TagList { get; }

		private const string ProjectFlyoutName = "ProjectsFlyout";

		private const string TagFlyoutName = "TagsFlyout";

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _AddEntryCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _OpenProjectManagementCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _OpenTagManagementCommand;
	}
}