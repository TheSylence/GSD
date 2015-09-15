using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight;
using GSD.Messages;
using GSD.Models;
using GSD.Models.Repositories;

namespace GSD.ViewModels
{
	internal class ProjectViewModel : ViewModelBase
	{
		public ProjectViewModel( Project project )
		{
			Model = project;

			TodoRepo = new TodoRepository( App.Session );

			Todos = new ObservableCollection<TodoViewModel>( Model.Todos.Select( t => new TodoViewModel( t ) ) );
			Todos.CollectionChanged += Todos_CollectionChanged;

			foreach( var todo in Todos )
			{
				todo.DeleteRequested += Todo_DeleteRequested;
				todo.SaveRequested += Todo_SaveRequested;
			}
		}

		private void Todo_DeleteRequested( object sender, EventArgs e )
		{
			var todo = sender as TodoViewModel;
			Debug.Assert( todo != null );

			TodoRepo.Delete( todo.Model );
			Todos.Remove( todo );
		}

		private void Todo_SaveRequested( object sender, EventArgs e )
		{
			TodoViewModel todo = sender as TodoViewModel;
			Debug.Assert( todo != null );

			TodoRepo.Update( todo.Model );
		}

		private void Todos_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			// ReSharper disable ExplicitCallerInfoArgument
			RaisePropertyChanged( nameof( OpenTodoCount ) );
			RaisePropertyChanged( nameof( Progress ) );
			// ReSharper restore ExplicitCallerInfoArgument

			if( e.Action != NotifyCollectionChangedAction.Add )
			{
				return;
			}

			foreach( TodoViewModel newItem in e.NewItems )
			{
				newItem.DeleteRequested += Todo_DeleteRequested;
				newItem.SaveRequested += Todo_SaveRequested;
			}
		}

		public bool IsCurrent
		{
			[DebuggerStepThrough]
			get
			{
				return _IsCurrent;
			}
			set
			{
				if( _IsCurrent == value )
				{
					return;
				}

				_IsCurrent = value;
				RaisePropertyChanged();
				CurrentChanged?.Invoke( this, EventArgs.Empty );
			}
		}

		public event EventHandler CurrentChanged;

		public Project Model { get; }
		public int OpenTodoCount => Todos.Count( t => !t.Model.Done );

		public int Progress
		{
			get
			{
				if( ClosedTodoCount == 0 )
				{
					return 0;
				}

				return (int)Math.Round( Todos.Count / (double)ClosedTodoCount * 100 );
			}
		}

		public ObservableCollection<TodoViewModel> Todos { get; }
		private int ClosedTodoCount => Todos.Count( t => t.Model.Done );
		private readonly ITodoRepository TodoRepo;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _IsCurrent;
	}
}