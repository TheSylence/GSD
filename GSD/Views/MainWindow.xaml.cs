using GSD.Models.Repositories;

namespace GSD.Views
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			Closing += MainWindow_Closing;
		}

		private void MainWindow_Closing( object sender, System.ComponentModel.CancelEventArgs e )
		{
			if( App.Settings.GetById( SettingKeys.CloseToTray ).Get<bool>() )
			{
				e.Cancel = true;
				Hide();
			}
		}
	}
}