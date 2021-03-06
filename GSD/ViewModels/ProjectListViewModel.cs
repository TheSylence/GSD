﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;
using GSD.Resources;
using GSD.ViewServices;

namespace GSD.ViewModels
{
	internal interface IProjectListViewModel
	{
		event EventHandler<EventArgs> CurrentProjectChanged;

		void AddTag( Tag tag );

		ProjectViewModel CurrentProject { get; set; }
	}

	internal class ProjectListViewModel : ValidationViewModel, IResettable, IProjectListViewModel
	{
		public ProjectListViewModel( IViewServiceRepository viewServices = null, ISettingsRepository settingsRepo = null, IProjectRepository projectRepo = null, IMessenger messenger = null )
			: base( viewServices, settingsRepo, messenger )
		{
			ProjectRepo = projectRepo ?? new ProjectRepository( Session );

			Projects = new ObservableCollection<ProjectViewModel>( ProjectRepo.GetAll().OrderBy( p => p.Name ).Select( p => new ProjectViewModel( p ) ) );

			var last = Settings.GetById( SettingKeys.LastProject );
			CurrentProject = Projects.FirstOrDefault( p => p.Model.Id == last.Get<int>() ) ?? Projects.FirstOrDefault();

			foreach( var proj in Projects )
			{
				proj.CurrentChanged += Proj_CurrentChanged;
			}

			if( CurrentProject != null )
			{
				CurrentProject.IsCurrent = true;
			}

			ProjectNames = new List<string>();

			Validate( nameof( NewProjectName ) ).Check( () => !string.IsNullOrWhiteSpace( NewProjectName ) ).Message( Strings.ProjectMustHaveName );
			Validate( nameof( NewProjectName ) ).Check( () => !ProjectNames.Contains( NewProjectName ) ).Message( Strings.ThisNameIsAlreadyUsed );
			Reset();
		}

		public event EventHandler<EventArgs> CurrentProjectChanged;

		public void AddTag( Tag tag )
		{
			CurrentProject.Model.Tags.Add( tag );
			ProjectRepo.Update( CurrentProject.Model );
		}

		public void Reset()
		{
			NewProjectName = string.Empty;

			ProjectNames = ProjectRepo.GetAllNames().ToList();

			ClearValidationErrors();
		}

		private bool CanExecuteDeleteProjectCommand( ProjectViewModel arg )
		{
			return arg != null;
		}

		private bool CanExecuteNewProjectCommand()
		{
			return !string.IsNullOrWhiteSpace( NewProjectName ) && !ProjectNames.Contains( NewProjectName );
		}

		private void ExecuteCloseFlyoutCommand()
		{
			MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.ProjectFlyoutName ) );
		}

		private async void ExecuteDeleteProjectCommand( ProjectViewModel arg )
		{
			ConfirmationServiceArgs args = new ConfirmationServiceArgs( Strings.Confirm, string.Format( Strings.DoYouReallyWantToDeleteProjectXXX, arg.Model.Name ), Strings.Yes, Strings.No );

			if( !await ViewServices.Execute<IConfirmationService, bool>( args ) )
			{
				return;
			}

			arg.CurrentChanged -= Proj_CurrentChanged;

			Projects.Remove( arg );
			ProjectRepo.Delete( arg.Model );

			if( !Projects.Contains( CurrentProject ) )
			{
				CurrentProject = Projects.FirstOrDefault();
				if( CurrentProject != null )
				{
					CurrentProject.IsCurrent = true;
				}
			}

			MessengerInstance.Send( new NotificationMessage( Strings.ProjectDeleted ) );
		}

		private void ExecuteNewProjectCommand()
		{
			var project = new Project
			{
				Name = NewProjectName
			};

			ProjectRepo.Add( project );
			var projectVm = new ProjectViewModel( project );
			projectVm.CurrentChanged += Proj_CurrentChanged;

			Projects.Add( projectVm );

			if( CurrentProject != null )
			{
				CurrentProject.IsCurrent = false;
			}
			CurrentProject = projectVm;
			CurrentProject.IsCurrent = true;

			Reset();
		}

		private void Proj_CurrentChanged( object sender, EventArgs e )
		{
			if( !ListenForCurrentChange )
			{
				return;
			}

			ProjectViewModel vm = sender as ProjectViewModel;
			if( vm == null )
			{
				return;
			}

			if( vm.IsCurrent )
			{
				ListenForCurrentChange = false;
				try
				{
					foreach( var proj in Projects.Where( p => p != vm ) )
					{
						proj.IsCurrent = false;
					}

					CurrentProject = vm;
					Settings.Set( SettingKeys.LastProject, CurrentProject?.Model?.Id.ToString() );
					MessengerInstance.Send( new CurrentProjectChangedMessage( vm ) );
				}
				finally
				{
					ListenForCurrentChange = true;
				}
			}
		}

		public ICommand CloseFlyoutCommand => _CloseFlyoutCommand ?? ( _CloseFlyoutCommand = new RelayCommand( ExecuteCloseFlyoutCommand ) );

		public ProjectViewModel CurrentProject
		{
			[DebuggerStepThrough] get { return _CurrentProject; }
			set
			{
				if( Equals( _CurrentProject, value ) )
				{
					return;
				}

				_CurrentProject = value;
				RaisePropertyChanged();
				CurrentProjectChanged?.Invoke( this, EventArgs.Empty );
			}
		}

		public ICommand DeleteProjectCommand => _DeleteProjectCommand ??
												( _DeleteProjectCommand =
													new RelayCommand<ProjectViewModel>( ExecuteDeleteProjectCommand, CanExecuteDeleteProjectCommand ) );

		public ICommand NewProjectCommand => _NewProjectCommand ?? ( _NewProjectCommand = new RelayCommand( ExecuteNewProjectCommand, CanExecuteNewProjectCommand ) );

		public string NewProjectName
		{
			[DebuggerStepThrough] get { return _NewProjectName; }
			set
			{
				if( _NewProjectName == value )
				{
					return;
				}

				_NewProjectName = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<ProjectViewModel> Projects { get; }
		private readonly IProjectRepository ProjectRepo;
		private RelayCommand _CloseFlyoutCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private ProjectViewModel _CurrentProject;

		private RelayCommand<ProjectViewModel> _DeleteProjectCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _NewProjectCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _NewProjectName;

		private bool ListenForCurrentChange = true;
		private List<string> ProjectNames;
	}
}