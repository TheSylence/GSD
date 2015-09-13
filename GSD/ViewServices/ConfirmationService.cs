using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace GSD.ViewServices
{
	internal interface IConfirmationService : IViewService
	{
	}

	[ExcludeFromCodeCoverage]
	internal class ConfirmationService : IConfirmationService
	{
		public async Task<object> Execute( MetroWindow window, object args )
		{
			ConfirmationServiceArgs csa = args as ConfirmationServiceArgs;
			Debug.Assert( csa != null );

			MetroDialogSettings settings = new MetroDialogSettings
			{
				AffirmativeButtonText = csa.OkText,
				NegativeButtonText = csa.CancelText
			};

			var result = await window.ShowMessageAsync( csa.Title, csa.Message, MessageDialogStyle.AffirmativeAndNegative, settings );

			return result == MessageDialogResult.Affirmative;
		}
	}

	internal class ConfirmationServiceArgs
	{
		public ConfirmationServiceArgs( string title, string message, string yes, string no )
		{
			Title = title;
			Message = message;
			CancelText = no;
			OkText = yes;
		}

		public string CancelText { get; }
		public string Message { get; }
		public string OkText { get; }
		public string Title { get; }
	}
}