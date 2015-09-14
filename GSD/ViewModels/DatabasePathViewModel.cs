﻿using GalaSoft.MvvmLight.CommandWpf;
using GSD.Resources;
using GSD.ViewServices;
using System;
using System.Diagnostics;

namespace GSD.ViewModels
{
	internal class DatabasePathViewModel : ValidationViewModel, IViewController
	{
		public DatabasePathViewModel()
			: this( null )
		{
		}

		public DatabasePathViewModel( IViewServiceRepository viewServiceRepo )
			: base( viewServiceRepo )
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
			var path = await ViewServices.Execute<IBrowseFileService, string>( System.IO.Path.GetDirectoryName( Path ) );
			if( !string.IsNullOrWhiteSpace( path ) )
			{
				Path = path;
			}
		}

		private void ExecuteCancelCommand()
		{
			_Path = null;
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
			[DebuggerStepThrough] get { return _Path; }
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