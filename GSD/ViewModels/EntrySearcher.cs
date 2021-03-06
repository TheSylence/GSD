using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight.Messaging;
using GSD.Messages;
using GSD.Resources;

namespace GSD.ViewModels
{
	internal class EntrySearcher : ViewModelBaseEx
	{
		public EntrySearcher( IProjectListViewModel projectList, IMessenger messenger = null )
			: base( null, null, messenger )
		{
			ProjectList = projectList;
			_CurrentProject = ProjectList.CurrentProject;
			Matches = new ObservableCollection<TodoViewModel>( AllEntries );

			MessengerInstance.Register<CurrentProjectChangedMessage>( this, OnCurrentProjectChanged );
			MessengerInstance.Register<EntryAddedMessage>( this, OnEntryAdded );
		}

		private void OnEntryAdded( EntryAddedMessage msg )
		{
			Search();

			if( Matches.Any( m => m.Model.Id == msg.Entry.Id ) )
			{
				return;
			}

			MessengerInstance.Send( new NotificationMessage( Strings.NewEntryNotMatched ) );
		}

		private void OnCurrentProjectChanged( CurrentProjectChangedMessage obj )
		{
			CurrentProject = ProjectList.CurrentProject;
		}

		private void Search()
		{
			Matches.Clear();
			IEnumerable<TodoViewModel> results = AllEntries;

			string expression = Text.ToLower();
			foreach( var match in StatusPattern.Matches( expression ).OfType<Match>().OrderByDescending( x => x.Index ) )
			{
				string statusName = match.Groups[1].Value;
				bool status = new[] {"done", "close", "closed"}.Contains( statusName );

				results = results.Where( r => r.Model.Done == status );

				expression = expression.Remove( match.Index, match.Length );
			}

			foreach( var match in TagPattern.Matches( expression ).OfType<Match>().OrderByDescending( x => x.Index ) )
			{
				string tagName = match.Groups[1].Value;
				if( string.IsNullOrWhiteSpace( tagName ) )
				{
					tagName = match.Groups[2].Value;
				}

				if( string.IsNullOrWhiteSpace( tagName ) )
				{
					continue;
				}

				results = results.Where( r => r.Tags.Any( t => t.Model.Name.Equals( tagName, StringComparison.OrdinalIgnoreCase ) ) );
				expression = expression.Remove( match.Index, match.Length );
			}

			foreach( var match in DetailsPattern.Matches( expression ).OfType<Match>().OrderByDescending( x => x.Index ) )
			{
				string searchText = match.Groups[1].Value;
				if( string.IsNullOrWhiteSpace( searchText ) )
				{
					searchText = match.Groups[2].Value;
				}

				if( string.IsNullOrWhiteSpace( searchText ) )
				{
					continue;
				}

				results = results.Where( r => ( r.Model.Details ?? string.Empty ).ToLower().Contains( searchText ) );
				expression = expression.Remove( match.Index, match.Length );
			}

			expression = expression.Trim();
			if( !string.IsNullOrWhiteSpace( expression ) )
			{
				results = results.Where( t => t.Model.Summary.ToLower().Contains( expression ) );
			}

			foreach( var entry in results )
			{
				Matches.Add( entry );
			}
		}

		public ProjectViewModel CurrentProject
		{
			[DebuggerStepThrough] get { return _CurrentProject; }
			set
			{
				if( _CurrentProject == value )
				{
					return;
				}

				_CurrentProject = value;
				RaisePropertyChanged();
				Search();
			}
		}

		public bool IsSearching
		{
			[DebuggerStepThrough] get { return _IsSearching; }
			set
			{
				if( _IsSearching == value )
				{
					return;
				}

				_IsSearching = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<TodoViewModel> Matches { get; }

		public string Text
		{
			[DebuggerStepThrough] get { return _Text ?? string.Empty; }
			set
			{
				if( _Text == value )
				{
					return;
				}

				_Text = value;
				RaisePropertyChanged();
				Search();

				IsSearching = !string.IsNullOrWhiteSpace( Text );
			}
		}

		private IEnumerable<TodoViewModel> AllEntries => CurrentProject?.Todos ?? Enumerable.Empty<TodoViewModel>();
		private readonly Regex DetailsPattern = new Regex( "(?:details:(?:(\\w+)|(?:\"(.*)\")))" );

		private readonly IProjectListViewModel ProjectList;

		private readonly Regex StatusPattern = new Regex( "status:(done|notdone|open|close(?:d?))" );

		private readonly Regex TagPattern = new Regex( "(?:tag|label):(?:(\\w+)|(?:\"(.*)\"))" );

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private ProjectViewModel _CurrentProject;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private bool _IsSearching;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private string _Text;
	}
}