using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GSD.Resources;
using GSD.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace GSD.ViewServices
{
	internal interface IProgressService : IViewService
	{
	}

	[ExcludeFromCodeCoverage]
	internal class ProgressService : IProgressService
	{
		public async Task<object> Execute( MetroWindow window, object args )
		{
			ProgressServiceArgs psa = args as ProgressServiceArgs;
			Debug.Assert( psa != null );

			var controller = await window.ShowProgressAsync( Strings.PleaseWait, Strings.MovingDatabase );
			var report = new ProgressReport( controller );

			await Task.Run( () =>
			{
				psa.Action( report );
			} );

			await controller.CloseAsync();

			return null;
		}
	}

	internal class ProgressServiceArgs
	{
		public ProgressServiceArgs( Action<IProgressReport> action )
		{
			Action = action;
		}

		public Action<IProgressReport> Action { get; }
	}
}