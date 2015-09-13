using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GSD.Resources;
using GSD.ViewModels;
using GSD.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace GSD.ViewServices
{
	internal interface IMoveDatabaseService : IViewService
	{ }

	internal class MoveDatabaseService : IMoveDatabaseService
	{
		public async Task<object> Execute( MetroWindow window, object args )
		{
			var settings = new MetroDialogSettings
			{
				AffirmativeButtonText = Strings.Yes,
				NegativeButtonText = Strings.No
			};

			var result = await window.ShowMessageAsync(Strings.MoveDatabase, Strings.MoveDatabaseMessage, MessageDialogStyle.AffirmativeAndNegative, settings );
			if( result != MessageDialogResult.Affirmative )
			{
				return null;
			}

			SemaphoreSlim evt = new SemaphoreSlim( 1 );

			var pathDlg = new DatabasePathDialog();
			var vm = pathDlg.DataContext as DatabasePathViewModel;
			Debug.Assert( vm != null );

			vm.CloseRequested += async ( s, e ) =>
			{
				await window.HideMetroDialogAsync( pathDlg, settings );
				evt.Release();
			};

			await window.ShowMetroDialogAsync( pathDlg, settings );
			await evt.WaitAsync();

			return vm.Path;
		}
	}
}