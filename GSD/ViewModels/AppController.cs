using System.Windows;

namespace GSD.ViewModels
{
	interface IAppController
	{
		void ShowWindow();

		void Shutdown();
	}

	internal class AppController : IAppController
	{
		public void ShowWindow()
		{
			Application.Current.MainWindow.Show();
		}

		public void Shutdown()
		{
			Application.Current.Shutdown();
		}
	}
}