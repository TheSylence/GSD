using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace GSD.ViewModels.Utilities
{
	interface IAppController
	{
		void ShowWindow();

		void Shutdown();
	}

	[ExcludeFromCodeCoverage]
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