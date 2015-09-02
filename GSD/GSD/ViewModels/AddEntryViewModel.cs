using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GSD.Models;
using GSD.Models.Repositories;

namespace GSD.ViewModels
{
	internal class AddEntryViewModel : ViewModelBaseEx
	{
		public AddEntryViewModel( TagListViewModel tagList, ProjectViewModel currentProject )
		{
			CurrentProject = currentProject;
			TodoRepo = new TodoRepository( App.Session );
			Tags = tagList.Tags.Select( t => new TagEntry( t ) ).ToList();
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
		}

		public ICommand AddCommand => _AddCommand ?? ( _AddCommand = new RelayCommand( ExecuteAddCommand, CanExecuteAddCommand ) );

		public string Details
		{
			[System.Diagnostics.DebuggerStepThrough] get { return _Details; }
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

		public string Summary
		{
			[System.Diagnostics.DebuggerStepThrough] get { return _Summary; }
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

		[System.Diagnostics.DebuggerBrowsable( System.Diagnostics.DebuggerBrowsableState.Never )] private string _Details;

		[System.Diagnostics.DebuggerBrowsable( System.Diagnostics.DebuggerBrowsableState.Never )] private string _Summary;
	}

	internal class TagEntry : ObservableObject
	{
		public TagEntry( TagViewModel t )
		{
			Tag = t;
		}

		public bool IsSelected
		{
			[System.Diagnostics.DebuggerStepThrough] get { return _IsSelected; }
			set
			{
				if( _IsSelected == value )
				{
					return;
				}

				_IsSelected = value;
				RaisePropertyChanged();
			}
		}

		public TagViewModel Tag { get; }

		[System.Diagnostics.DebuggerBrowsable( System.Diagnostics.DebuggerBrowsableState.Never )] private bool _IsSelected;
	}
}