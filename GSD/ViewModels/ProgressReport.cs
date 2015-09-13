using MahApps.Metro.Controls.Dialogs;

namespace GSD.ViewModels
{
	internal interface IProgressReport
	{
		void SetProgress( int current, int total );
	}

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