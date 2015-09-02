using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight;
using GSD.Messages;
using GSD.Models;

namespace GSD.ViewModels
{
	internal class ProjectViewModel : ViewModelBase
	{
		public ProjectViewModel( Project project )
		{
			Model = project;

			Todos = new ObservableCollection<TodoViewModel>( Model.Todos.Select( t => new TodoViewModel( t ) ) );
			Todos.CollectionChanged += Todos_CollectionChanged;
		}

		private void Todos_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			RaisePropertyChanged( nameof( OpenTodoCount ) );
			RaisePropertyChanged( nameof( Progress ) );
		}

		public int ClosedTodoCount => Todos.Count( t => t.Model.Done );

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
				MessengerInstance.Send( new CurrentProjectChangedMessage() );
			}
		}

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

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private bool _IsCurrent;
	}
}