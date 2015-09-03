using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight;
using GSD.Models;
using GSD.Models.Repositories;

namespace GSD.ViewModels
{
	internal class TodoViewModel : ViewModelBase
	{
		public TodoViewModel( Todo todo )
		{
			TodoRepo = new TodoRepository( App.Session );
			Model = todo;

			AllTags = new ObservableCollection<TodoTagViewModel>( Model.Project.Tags.Select( t => new TodoTagViewModel( todo, t )
			{
				IsSelected = Model.Tags.Contains( t )
			} ) );

			foreach( var t in AllTags )
			{
				t.Selected += Tag_Selected;
				t.Deselected += Tag_Deselected;
			}

			Tags = new ObservableCollection<TodoTagViewModel>( AllTags.Where( t => t.IsSelected ) );
		}

		private void Tag_Deselected( object sender, EventArgs e )
		{
			var tag = sender as TodoTagViewModel;
			Debug.Assert( tag != null );
			Tags.Remove( tag );

			Model.Tags.Remove( tag.Model );
			TodoRepo.Update( Model );
		}

		private void Tag_Selected( object sender, EventArgs e )
		{
			var tag = sender as TodoTagViewModel;
			Debug.Assert( tag != null );
			Tags.Add( tag );

			Model.Tags.Add( tag.Model );
			TodoRepo.Update( Model );
		}

		public ObservableCollection<TodoTagViewModel> AllTags { get; }
		public Todo Model { get; }
		public ObservableCollection<TodoTagViewModel> Tags { get; }
		private readonly ITodoRepository TodoRepo;
	}
}