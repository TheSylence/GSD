using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;
using GSD.ViewServices;

namespace GSD.ViewModels
{
	internal class TodoViewModel : ViewModelBaseEx
	{
		public TodoViewModel( Todo todo )
		{
			MessengerInstance.Register<TagAddedMessage>( this, OnTagAdded );
			MessengerInstance.Register<TagRemovedMessage>( this, OnTagRemoved );

			TodoRepo = new TodoRepository( App.Session );
			Model = todo;

			AllTags = new ObservableCollection<TodoTagViewModel>( Model.Project.Tags.Select( t => new TodoTagViewModel( Model, t )
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

		public event EventHandler DeleteRequested;

		private async void ExecuteDeleteEntryCommand()
		{
			ConfirmationServiceArgs args = new ConfirmationServiceArgs( "Confirm", "Do you really want to delete this entry?" );
			if( !await ViewServices.Execute<IConfirmationService, bool>( args ) )
			{
				return;
			}

			DeleteRequested?.Invoke( this, EventArgs.Empty );
		}

		private void OnTagAdded( TagAddedMessage msg )
		{
			AllTags.Add( new TodoTagViewModel( Model, msg.Tag ) );
		}

		private void OnTagRemoved( TagRemovedMessage msg )
		{
			var tag = AllTags.FirstOrDefault( t => t.Model == msg.Tag );

			AllTags.Remove( tag );
			Tags.Remove( tag );
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

		public RelayCommand DeleteEntryCommand => _DeleteEntryCommand ?? ( _DeleteEntryCommand = new RelayCommand( ExecuteDeleteEntryCommand ) );

		public Todo Model { get; }

		public ObservableCollection<TodoTagViewModel> Tags { get; }

		private readonly ITodoRepository TodoRepo;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _DeleteEntryCommand;
	}
}