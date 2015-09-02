using System;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Messaging;
using GSD.Messages;
using GSD.ViewModels;
using MahApps.Metro.Controls;

namespace GSD.Behaviors
{
	internal class FlyoutOpener : Behavior<Flyout>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			FlyoutName = AssociatedObject.Name;

			Messenger.Default.Register<FlyoutMessage>( this, OnFlyoutMessage );
		}

		private void OnFlyoutMessage( FlyoutMessage msg )
		{
			if( msg.FlyoutName.Equals( FlyoutName, StringComparison.Ordinal ) )
			{
				AssociatedObject.IsOpen = !AssociatedObject.IsOpen;
				if( AssociatedObject.IsOpen )
				{
					if( msg.DataContext != null )
					{
						AssociatedObject.DataContext = msg.DataContext;
					}

					var resettable = AssociatedObject.DataContext as IResettable;
					resettable?.Reset();
				}
			}
		}

		private string FlyoutName;
	}
}