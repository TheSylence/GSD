using System.Diagnostics;

namespace GSD.ViewModels.Utilities
{
	internal interface IProcessStarter
	{
		void Start( string process );
	}

	internal class ProcessStarter : IProcessStarter
	{
		public void Start( string process )
		{
			Process.Start( process );
		}
	}
}