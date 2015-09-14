using System.Diagnostics.CodeAnalysis;
using MahApps.Metro.Controls.Dialogs;

namespace GSD.ViewModels.Utilities
{
	internal interface IProgressReport
	{
		void SetProgress( int current, int total );
	}

	[ExcludeFromCodeCoverage]
	internal class ProgressReport : IProgressReport
	{
		public ProgressReport( ProgressDialogController controller )
		{
			Controller = controller;
		}

		public void SetProgress( int current, int total )
		{
			Controller.SetProgress( total / (double)current );
		}

		private readonly ProgressDialogController Controller;
	}
}