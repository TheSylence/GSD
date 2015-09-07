﻿using System.Diagnostics;
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

		private bool CanExecuteEditEntryCommand( TodoViewModel arg )
		{
			return arg != null;
		}

		private bool CanExecuteOpenTagManagementCommand()
		{
			return ProjectList.CurrentProject != null;
		}

		private void ExecuteAddEntryCommand()
		{
			var vm = new AddEntryViewModel( TagList, ProjectList.CurrentProject );
			MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.AddEntryFlyoutName, vm ) );
		}

		private void ExecuteEditEntryCommand( TodoViewModel arg )
		{
			var vm = new EditEntryViewModel( arg );
			MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.EditEntryFlyoutName, vm ) );
		}

		private void ExecuteOpenProjectManagementCommand()
		{
			MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.ProjectFlyoutName ) );
		}

		private void ExecuteOpenSettingsCommand()
		{
			MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.SettingsFlyoutName ) );
		}

		private void ExecuteOpenTagManagementCommand()
		{
			MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.TagFlyoutName ) );
		}

		public ICommand AddEntryCommand => _AddEntryCommand ?? ( _AddEntryCommand = new RelayCommand( ExecuteAddEntryCommand, CanExecuteAddEntryCommand ) );
		public ICommand EditEntryCommand => _EditEntryCommand ?? ( _EditEntryCommand = new RelayCommand<TodoViewModel>( ExecuteEditEntryCommand, CanExecuteEditEntryCommand ) );
		public ICommand OpenProjectManagementCommand => _OpenProjectManagementCommand ?? ( _OpenProjectManagementCommand = new RelayCommand( ExecuteOpenProjectManagementCommand ) );
		public RelayCommand OpenSettingsCommand => _OpenSettingsCommand ?? ( _OpenSettingsCommand = new RelayCommand( ExecuteOpenSettingsCommand ) );
		public ICommand OpenTagManagementCommand => _OpenTagManagementCommand ?? ( _OpenTagManagementCommand = new RelayCommand( ExecuteOpenTagManagementCommand, CanExecuteOpenTagManagementCommand ) );
		public ProjectListViewModel ProjectList { get; }
		public TagListViewModel TagList { get; }

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _AddEntryCommand;

		private RelayCommand<TodoViewModel> _EditEntryCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _OpenProjectManagementCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _OpenSettingsCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _OpenTagManagementCommand;
	}
}