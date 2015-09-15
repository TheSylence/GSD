using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace GSD.ViewModels.Utilities
{
	internal interface IAppController
	{
		void ShowWindow();

		void Shutdown();
	}

	[ExcludeFromCodeCoverage]
	internal class AppController : IAppController
	{
		public void ShowWindow()
		{
			var window = Application.Current.MainWindow;

			if( !window.IsVisible )
			{
				window.Show();
			}

			if( window.WindowState == WindowState.Minimized )
			{
				window.WindowState = WindowState.Normal;
			}

			window.Activate();
			window.Topmost = true;
			window.Topmost = false;
			window.Focus();
		}

		public void Shutdown()
		{
			Application.Current.Shutdown();
		}
	}
}