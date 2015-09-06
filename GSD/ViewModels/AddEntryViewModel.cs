using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;

namespace GSD.ViewModels
{
	internal class AddEntryViewModel : ViewModelBaseEx, IResettable
	{
		public AddEntryViewModel( TagListViewModel tagList, ProjectViewModel currentProject )
		{
			CurrentProject = currentProject;
			TodoRepo = new TodoRepository( App.Session );
			Tags = tagList.Tags.Select( t => new TagEntry( t ) ).ToList();
		}

		public void Reset()
		{
			Summary = string.Empty;
			Details = string.Empty;

			foreach( var t in Tags )
			{
				t.IsSelected = false;
			}
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
			[DebuggerStepThrough]
			get
			{
				return _StayOpen;
			}
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