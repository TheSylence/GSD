﻿using GalaSoft.MvvmLight.CommandWpf;
using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;
using GSD.ViewServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace GSD.ViewModels
{
	internal class ProjectListViewModel : ValidationViewModel, IResettable
	{
		public ProjectListViewModel( IViewServiceRepository viewServices = null, ISettingsRepository settingsRepo = null, IProjectRepository projectRepo = null )
			: base( viewServices, settingsRepo )
		{
			ProjectRepo = projectRepo ?? new ProjectRepository( Session );

			Projects = new ObservableCollection<ProjectViewModel>( ProjectRepo.GetAll().OrderBy( p => p.Name ).Select( p => new ProjectViewModel( p ) ) );

			var last = Settings.GetById( SettingKeys.LastProject );
			CurrentProject = Projects.FirstOrDefault( p => p.Model.Id == last.Get<int>() ) ?? Projects.FirstOrDefault();

			if( CurrentProject != null )
			{
				CurrentProject.IsCurrent = true;
			}

			Validate( nameof( NewProjectName ) ).Check( () => !string.IsNullOrWhiteSpace( NewProjectName ) ).Message( "Project must have a name" );
			Validate( nameof( NewProjectName ) ).Check( () => !ProjectNames.Contains( NewProjectName ) ).Message( "This name is already used" );
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

			MessengerInstance.Unregister( this );
			MessengerInstance.Register<CurrentProjectChangedMessage>( this, OnCurrentProjectChanged );

			ProjectNames = ProjectRepo.GetAll().Select( p => p.Name ).ToList();

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

		private async void ExecuteDeleteProjectCommand( ProjectViewModel arg )
		{
			ConfirmationServiceArgs args = new ConfirmationServiceArgs( "Confirm", $"Do you really want to delete the project '{arg.Model.Name}'?" );

			if( !await ViewServices.Execute<IConfirmationService, bool>( args ) )
			{
				return;
			}

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
		}

		private void ExecuteNewProjectCommand()
		{
			var project = new Project
			{
				Name = NewProjectName
			};

			ProjectRepo.Add( project );
			var projectVm = new ProjectViewModel( project );
			Projects.Add( projectVm );

			if( CurrentProject != null )
			{
				CurrentProject.IsCurrent = false;
			}
			CurrentProject = projectVm;
			CurrentProject.IsCurrent = true;

			Reset();
		}

		private void OnCurrentProjectChanged( CurrentProjectChangedMessage msg )
		{
			CurrentProject = Projects.FirstOrDefault( p => p.IsCurrent );
			Settings.Set( SettingKeys.LastProject, CurrentProject?.Model?.Id.ToString() );
		}

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

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private ProjectViewModel _CurrentProject;

		private RelayCommand<ProjectViewModel> _DeleteProjectCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private RelayCommand _NewProjectCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private string _NewProjectName;

		private List<string> ProjectNames;
	}
}