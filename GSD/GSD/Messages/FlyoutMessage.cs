using GalaSoft.MvvmLight.Messaging;

namespace GSD.Messages
{
	internal class FlyoutMessage : MessageBase
	{
		public FlyoutMessage( string flyoutName, object dataContext = null )
		{
			FlyoutName = flyoutName;
			DataContext = dataContext;
		}

		public object DataContext { get; }
		public string FlyoutName { get; }
	}
}