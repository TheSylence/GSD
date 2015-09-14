using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Messaging;
using GSD.Messages;
using GSD.ViewModels;
using MahApps.Metro.Controls;

namespace GSD.Behaviors
{
	[ExcludeFromCodeCoverage]
	internal class FlyoutOpener : Behavior<Flyout>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			Messenger.Default.Register<FlyoutMessage>( this, OnFlyoutMessage );
		}

		private void OnFlyoutMessage( FlyoutMessage msg )
		{
			if( msg.FlyoutName.Equals( Name, StringComparison.Ordinal ) )
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

		public string Name
		{
			get { return (string)GetValue( NameProperty ); }
			set { SetValue( NameProperty, value ); }
		}

		public static readonly DependencyProperty NameProperty = DependencyProperty.Register( "Name", typeof( string ), typeof( FlyoutOpener ), new PropertyMetadata( null ) );
	}
}