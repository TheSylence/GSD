using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;
using GSD.Resources;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace GSD.ViewModels
{
	internal class AddEntryViewModel : ValidationViewModel, IResettable
	{
		public AddEntryViewModel( IEnumerable<TagViewModel> tagList, ProjectViewModel currentProject, ITodoRepository todoRepo = null, IMessenger messenger = null )
			: base( null, null, messenger )
		{
			CurrentProject = currentProject;
			TodoRepo = todoRepo ?? new TodoRepository( App.Session );
			Tags = tagList.Select( t => new TagEntry( t ) ).ToList();

			Validate( nameof( Summary ) ).Check( () => !string.IsNullOrWhiteSpace( Summary ) ).Message( Strings.EntryNeedsSummary );
			Reset();
		}

		public void Reset()
		{
			Summary = string.Empty;
			Details = string.Empty;

			foreach( var t in Tags )
			{
				t.IsSelected = false;
			}

			ClearValidationErrors();
		}

		private bool CanExecuteAddCommand()
		{
			return !string.IsNullOrWhiteSpace( Summary );
		}

		private void ExecuteAddCommand()
		{
			var todo = new Todo
			{
				Summary = Summary,
				Details = Details,
				Project = CurrentProject.Model
			};

			TodoRepo.Add( todo );

			foreach( var tag in Tags.Where( t => t.IsSelected ) )
			{
				todo.Tags.Add( tag.Tag.Model );
			}

			TodoRepo.Update( todo );

			CurrentProject.Todos.Add( new TodoViewModel( todo ) );
			Reset();

			if( !StayOpen )
			{
				MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.AddEntryFlyoutName ) );
			}

			MessengerInstance.Send( new EntryAddedMessage( todo ) );
		}

		public ICommand AddCommand => _AddCommand ?? ( _AddCommand = new RelayCommand( ExecuteAddCommand, CanExecuteAddCommand ) );

		public string Details
		{
			[DebuggerStepThrough] get { return _Details; }
			set
			{
				if( _Details == value )
				{
					return;
				}

				_Details = value;
				RaisePropertyChanged();
			}
		}

		public bool StayOpen
		{
			[DebuggerStepThrough] get { return _StayOpen; }
			set
			{
				if( _StayOpen == value )
				{
					return;
				}

				_StayOpen = value;
				RaisePropertyChanged();
			}
		}

		public string Summary
		{
			[DebuggerStepThrough] get { return _Summary; }
			set
			{
				if( _Summary == value )
				{
					return;
				}

				_Summary = value;
				RaisePropertyChanged();
			}
		}

		public List<TagEntry> Tags { get; }

		private readonly ProjectViewModel CurrentProject;

		private readonly ITodoRepository TodoRepo;

		private RelayCommand _AddCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _Details;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _StayOpen;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _Summary;
	}
}