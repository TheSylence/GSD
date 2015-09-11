using System;
using System.Diagnostics;
using System.Reflection;
using GalaSoft.MvvmLight.CommandWpf;

namespace GSD.ViewModels
{
	internal class InfoViewModel : IViewController
	{
		public InfoViewModel()
		{
			Version = Assembly.GetExecutingAssembly().GetName().Version;
		}

		public event EventHandler CloseRequested;

		private void ExecuteCloseCommand()
		{
			CloseRequested?.Invoke( this, EventArgs.Empty );
		}

		public RelayCommand CloseCommand => _CloseCommand ?? ( _CloseCommand = new RelayCommand( ExecuteCloseCommand ) );

		public Version Version { get; }

		[DebuggerBrowsable( DebuggerBrowsableState.Never )]
		private RelayCommand _CloseCommand;
	}
}