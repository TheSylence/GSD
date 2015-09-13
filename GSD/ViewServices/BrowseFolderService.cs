using System.Threading.Tasks;
using MahApps.Metro.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace GSD.ViewServices
{
	internal interface IBrowseFolderService : IViewService
	{
	}

	internal class BrowseFolderService : IBrowseFolderService
	{
		public Task<object> Execute( MetroWindow window, object args )
		{
			var directory = args as string;

			var dlg = new CommonOpenFileDialog();
			dlg.Title = "My Title";
			dlg.IsFolderPicker = true;
			dlg.InitialDirectory = directory;

			dlg.AddToMostRecentlyUsedList = false;
			dlg.AllowNonFileSystemItems = false;
			dlg.DefaultDirectory = directory;
			dlg.EnsureFileExists = true;
			dlg.EnsurePathExists = true;
			dlg.EnsureReadOnly = false;
			dlg.EnsureValidNames = true;
			dlg.Multiselect = false;
			dlg.ShowPlacesList = true;

			if( dlg.ShowDialog() == CommonFileDialogResult.Ok )
			{
				return Task.FromResult<object>( dlg.FileName );
			}

			return Task.FromResult<object>( null );
		}
	}
}