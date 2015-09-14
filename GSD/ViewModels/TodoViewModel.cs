using GalaSoft.MvvmLight.CommandWpf;
using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;
using GSD.Resources;
using GSD.ViewServices;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

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
			_Done = Model.Done;

			if( Model?.Project?.Tags != null )
			{
				AllTags = new ObservableCollection<TodoTagViewModel>( Model.Project.Tags.Select( t => new TodoTagViewModel( t )
				{
					IsSelected = Model.Tags.Contains( t )
				} ) );
			}
			else
			{
				AllTags = new ObservableCollection<TodoTagViewModel>();
			}

			foreach( var t in AllTags )
			{
				t.Selected += Tag_Selected;
				t.Deselected += Tag_Deselected;
			}

			Tags = new ObservableCollection<TodoTagViewModel>( AllTags.Where( t => t.IsSelected ) );
		}

		public event EventHandler DeleteRequested;

		public event EventHandler SaveRequested;

		public void RaiseUpdates()
		{
			// ReSharper disable once ExplicitCallerInfoArgument
			RaisePropertyChanged( nameof( Model ) );
		}

		private async void ExecuteDeleteEntryCommand()
		{
			ConfirmationServiceArgs args = new ConfirmationServiceArgs( Strings.Confirm, Strings.DoYouReallyWantToDeleteThisEntry, Strings.Yes, Strings.No );
			if( !await ViewServices.Execute<IConfirmationService, bool>( args ) )
			{
				return;
			}

			DeleteRequested?.Invoke( this, EventArgs.Empty );
		}

		private void OnTagAdded( TagAddedMessage msg )
		{
			AllTags.Add( new TodoTagViewModel( msg.Tag ) );
		}

		private void OnTagRemoved( TagRemovedMessage msg )
		{
			var tag = AllTags.FirstOrDefault( t => t.Model == msg.Tag );

			AllTags.Remove( tag );
			Tags.Remove( tag );
		}

		private void SaveDone( bool done )
		{
			Model.Done = done;
			SaveRequested?.Invoke( this, EventArgs.Empty );
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

		public bool Done
		{
			[DebuggerStepThrough] get { return _Done; }
			set
			{
				if( _Done == value )
				{
					return;
				}

				_Done = value;
				RaisePropertyChanged();

				SaveDone( _Done );
			}
		}

		public Todo Model { get; }

		public ObservableCollection<TodoTagViewModel> Tags { get; }

		private readonly ITodoRepository TodoRepo;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private RelayCommand _DeleteEntryCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private bool _Done;
	}
}