using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GSD.Models;
using GSD.Models.Repositories;

namespace GSD.ViewModels
{
	internal class TagListViewModel : ViewModelBaseEx, IResettable
	{
		public TagListViewModel( ProjectListViewModel projectList )
		{
			TagRepo = new TagRepository( Session );

			ProjectList = projectList;
			ProjectList.CurrentProjectChanged += ProjectList_CurrentProjectChanged;

			var tags = Enumerable.Empty<TagViewModel>();
			if( ProjectList.CurrentProject != null )
			{
				tags = ProjectList.CurrentProject.Model.Tags.Select( t => new TagViewModel( t ) );
			}

			Tags = new ObservableCollection<TagViewModel>( tags );
		}

		public void Reset()
		{
			NewTagName = string.Empty;
		}

		private bool CanExecuteNewTagCommand()
		{
			return !string.IsNullOrWhiteSpace( NewTagName );
		}

		private void ExecuteNewTagCommand()
		{
			var tag = new Tag
			{
				Name = NewTagName,
				Project = ProjectList.CurrentProject.Model
			};

			TagRepo.Add( tag );
			ProjectList.AddTag( tag );
			Tags.Add( new TagViewModel( tag ) );
			Reset();
		}

		private void ProjectList_CurrentProjectChanged( object sender, EventArgs e )
		{
			Tags.Clear();

			foreach( var tag in ProjectList.CurrentProject.Model.Tags )
			{
				Tags.Add( new TagViewModel( tag ) );
			}
		}

		public ICommand NewTagCommand => _NewTagCommand ?? ( _NewTagCommand = new RelayCommand( ExecuteNewTagCommand, CanExecuteNewTagCommand ) );

		public string NewTagName
		{
			[DebuggerStepThrough]
			get
			{
				return _NewTagName;
			}
			set
			{
				if( _NewTagName == value )
				{
					return;
				}

				_NewTagName = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<TagViewModel> Tags { get; }
		private readonly ProjectListViewModel ProjectList;
		private readonly ITagRepository TagRepo;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _NewTagCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _NewTagName;
	}
}