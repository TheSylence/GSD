using System.Threading.Tasks;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace GSD.ViewServices
{
	internal interface IBrowseFileService : IViewService
	{
	}

	internal class BrowseFileService : IBrowseFileService
	{
		public Task<object> Execute( MetroWindow window, object args )
		{
			var directory = args as string;

			SaveFileDialog dlg = new SaveFileDialog
			{
				InitialDirectory = directory
			};

			if( dlg.ShowDialog( window ) == true )
			{
				return Task.FromResult<object>( dlg.FileName );
			}

			return Task.FromResult<object>( null );
		}
	}
}