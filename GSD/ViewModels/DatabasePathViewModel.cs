using System;
using System.Diagnostics;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using GSD.Resources;
using GSD.ViewServices;

namespace GSD.ViewModels
{
	internal class DatabasePathViewModel : ValidationViewModel, IViewController
	{
		public DatabasePathViewModel()
		{
			Validate( nameof( Path ) ).Check( () => !string.IsNullOrWhiteSpace( Path ) ).Message( Strings.MustEnterValidPath );
			ClearValidationErrors();
		}

		public event EventHandler CloseRequested;

		private bool CanExecuteOkCommand()
		{
			return !string.IsNullOrWhiteSpace( Path );
		}

		private async void ExecuteBrowseFolderCommand()
		{
			var path = await ViewServices.Execute<IBrowseFolderService, string>( Path );
			if( !string.IsNullOrWhiteSpace( path ) )
			{
				await DispatcherHelper.RunAsync( () => Path = path );
			}
		}

		private void ExecuteCancelCommand()
		{
			Path = null;
			CloseRequested?.Invoke( this, EventArgs.Empty );
		}

		private void ExecuteOkCommand()
		{
			CloseRequested?.Invoke( this, EventArgs.Empty );
		}

		public RelayCommand BrowseFolderCommand => _BrowseFolderCommand ?? ( _BrowseFolderCommand = new RelayCommand( ExecuteBrowseFolderCommand ) );

		public RelayCommand CancelCommand => _CancelCommand ?? ( _CancelCommand = new RelayCommand( ExecuteCancelCommand ) );

		public RelayCommand OkCommand => _OkCommand ?? ( _OkCommand = new RelayCommand( ExecuteOkCommand, CanExecuteOkCommand ) );

		public string Path
		{
			[DebuggerStepThrough]
			get
			{
				return _Path;
			}
			set
			{
				if( _Path == value )
				{
					return;
				}

				_Path = value;
				RaisePropertyChanged();
			}
		}

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _BrowseFolderCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _CancelCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _OkCommand;

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private string _Path;
	}
}