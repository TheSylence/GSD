using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace GSD.ViewModels
{
	internal class TrayIconViewModel
	{
		public TrayIconViewModel()
			: this( null )
		{
		}

		public TrayIconViewModel( IAppController controller )
		{
			Controller = controller ?? new AppController();
		}

		private void ExecuteQuitCommand()
		{
			Controller.Shutdown();
		}

		private void ExecuteShowWindowCommand()
		{
			Controller.ShowWindow();
		}

		public ICommand QuitCommand => _QuitCommand ?? ( _QuitCommand = new RelayCommand( ExecuteQuitCommand ) );
		public ICommand ShowWindowCommand => _ShowWindowCommand ?? ( _ShowWindowCommand = new RelayCommand( ExecuteShowWindowCommand ) );
		private RelayCommand _QuitCommand;
		private RelayCommand _ShowWindowCommand;
		private readonly IAppController Controller;
	}
}