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

		internal const string AddEntryFlyoutName = "AddEntryFlyout";
		internal const string EditEntryFlyoutName = "EditEntryFlyout";
		internal const string ProjectFlyoutName = "ProjectsFlyout";
		internal const string SettingsFlyoutName = "SettingsFlyout";
		internal const string TagFlyoutName = "TagsFlyout";
		internal const string NotificationFlyoutName = "NotificationsFlyout";
	}
}