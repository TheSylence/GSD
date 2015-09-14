using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GSD.ViewModels;
using GSD.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace GSD.ViewServices
{
	internal interface IMoveDatabaseService : IViewService
	{ }

	[ExcludeFromCodeCoverage]
	internal class MoveDatabaseService : IMoveDatabaseService
	{
		public async Task<object> Execute( MetroWindow window, object args )
		{
			var pathDlg = new DatabasePathDialog();
			var vm = pathDlg.DataContext as DatabasePathViewModel;
			Debug.Assert( vm != null );

			TaskCompletionSource<MoveDatabaseResult> tcs = new TaskCompletionSource<MoveDatabaseResult>();

			vm.CloseRequested += async ( s, e ) =>
			{
				await window.HideMetroDialogAsync( pathDlg );
				tcs.TrySetResult( new MoveDatabaseResult( vm.Path, vm.OverwriteExisting ) );
			};

			await window.ShowMetroDialogAsync( pathDlg );

			return await tcs.Task;
		}
	}

	class MoveDatabaseResult
	{
		public MoveDatabaseResult( string path, bool overwrite )
		{
			Path = path;
			OverwriteExisting = overwrite;
		}

		public bool OverwriteExisting { get; }
		public string Path { get; }
	}
}