using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace GSD.ViewServices
{
	internal interface IBrowseFileService : IViewService
	{
	}

	[ExcludeFromCodeCoverage]
	internal class BrowseFileService : IBrowseFileService
	{
		public Task<object> Execute( MetroWindow window, object args )
		{
			var directory = args as string;

			SaveFileDialog dlg = new SaveFileDialog
			{
				InitialDirectory = directory
			};

			return Task.FromResult<object>( dlg.ShowDialog( window ) == true ? dlg.FileName : null );
		}
	}
}