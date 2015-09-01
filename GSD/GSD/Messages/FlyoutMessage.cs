using GalaSoft.MvvmLight.Messaging;

namespace GSD.Messages
{
	internal class FlyoutMessage : MessageBase
	{
		public FlyoutMessage( string flyoutName )
		{
			FlyoutName = flyoutName;
		}

		public string FlyoutName { get; }
	}
}