using System.Diagnostics;
using System.Threading.Tasks;
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
			var pathDlg = new DatabasePathDialog();
			var vm = pathDlg.DataContext as DatabasePathViewModel;
			Debug.Assert( vm != null );

			TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

			vm.CloseRequested += async ( s, e ) =>
			{
				await window.HideMetroDialogAsync( pathDlg );
				tcs.TrySetResult( vm.Path );
			};

			await window.ShowMetroDialogAsync( pathDlg );

			return await tcs.Task;
		}
	}
}