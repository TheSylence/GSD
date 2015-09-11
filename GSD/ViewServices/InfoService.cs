using System.Threading.Tasks;
using GSD.ViewModels;
using GSD.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace GSD.ViewServices
{
	internal interface IInfoService : IViewService
	{ }

	internal class InfoService : IInfoService
	{
		public async Task<object> Execute( MetroWindow window, object args )
		{
			var settings = new MetroDialogSettings();

			var dlg = new InfoDialog();

			var ctrl = dlg.DataContext as IViewController;
			if( ctrl != null )
			{
				ctrl.CloseRequested += async ( s, e ) =>
				{
					await window.HideMetroDialogAsync( dlg, settings );
				};
			}

			await window.ShowMetroDialogAsync( dlg, settings );

			return null;
		}
	}
}