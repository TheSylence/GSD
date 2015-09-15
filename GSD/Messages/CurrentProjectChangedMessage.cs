using GSD.ViewModels;

namespace GSD.Messages
{
	internal class CurrentProjectChangedMessage
	{
		public CurrentProjectChangedMessage( ProjectViewModel project )
		{
			CurrentProject = project;
		}

		public ProjectViewModel CurrentProject { get; }
	}
}