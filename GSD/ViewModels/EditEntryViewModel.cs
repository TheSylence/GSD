using GalaSoft.MvvmLight.CommandWpf;
using GSD.Messages;
using GSD.Models.Repositories;
using System.Diagnostics;
using GSD.Resources;

namespace GSD.ViewModels
{
	internal class EditEntryViewModel : ValidationViewModel
	{
		public EditEntryViewModel( TodoViewModel entry, ITodoRepository todoRepo = null )
		{
			Entry = entry;
			TodoRepo = todoRepo ?? new TodoRepository( App.Session );

			Summary = entry.Model.Summary;
			Details = entry.Model.Details;

			Validate( nameof( Summary ) ).Check( () => !string.IsNullOrWhiteSpace( Summary ) ).Message( Strings.EntryNeedsSummary );
		}

		private bool CanExecuteSaveCommand()
		{
			return !string.IsNullOrWhiteSpace( Summary );
		}

		private void ExecuteSaveCommand()
		{
			Entry.Model.Details = Details;
			Entry.Model.Summary = Summary;

			TodoRepo.Update( Entry.Model );
			Entry.RaiseUpdates();

			MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.EditEntryFlyoutName ) );
		}

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

		public RelayCommand SaveCommand => _SaveCommand ?? ( _SaveCommand = new RelayCommand( ExecuteSaveCommand, CanExecuteSaveCommand ) );

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

		private readonly TodoViewModel Entry;
		private readonly ITodoRepository TodoRepo;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private string _Details;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private RelayCommand _SaveCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )] private string _Summary;
	}
}