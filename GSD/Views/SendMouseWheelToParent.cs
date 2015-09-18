using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GSD.Views
{
	/// <summary>
	/// Found at http://stackoverflow.com/a/12080370/868361
	/// </summary>
	public static class SendMouseWheelToParent
	{
		/// <summary>
		/// Gets the IsSendingMouseWheelEventToParent for a given <see cref="TextBox"/>.
		/// </summary>
		/// <param name="control">
		/// The <see cref="TextBox"/> whose IsSendingMouseWheelEventToParent is to be retrieved.
		/// </param>
		/// <returns>
		/// The IsSendingMouseWheelEventToParent, or <see langword="null"/>
		/// if no IsSendingMouseWheelEventToParent has been set.
		/// </returns>
		public static bool? GetIsSendingMouseWheelEventToParent( Control control )
		{
			if( control == null )
				throw new ArgumentNullException( "" );

			return control.GetValue( ScrollProperty ) as bool?;
		}

		/// <summary>
		/// Sets the IsSendingMouseWheelEventToParent for a given <see cref="TextBox"/>.
		/// </summary>
		/// <param name="control">
		/// The <see cref="TextBox"/> whose IsSendingMouseWheelEventToParent is to be set.
		/// </param>
		/// <param name="sendToParent">
		/// The IsSendingMouseWheelEventToParent to set, or <see langword="null"/>
		/// to remove any existing IsSendingMouseWheelEventToParent from <paramref name="control"/>.
		/// </param>
		public static void SetIsSendingMouseWheelEventToParent( Control control, bool? sendToParent )
		{
			if( control == null )
				throw new ArgumentNullException( "" );

			control.SetValue( ScrollProperty, sendToParent );
		}

		private static void OnValueChanged( DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e )
		{
			var scrollViewer = dependencyObject as Control;
			if( scrollViewer == null )
			{
				return;
			}

			bool? isSendingMouseWheelEventToParent = e.NewValue as bool?;
			scrollViewer.PreviewMouseWheel -= scrollViewer_PreviewMouseWheel;

			if( isSendingMouseWheelEventToParent != null && isSendingMouseWheelEventToParent != false )
			{
				scrollViewer.SetValue( ScrollProperty, isSendingMouseWheelEventToParent );
				scrollViewer.PreviewMouseWheel += scrollViewer_PreviewMouseWheel;
			}
		}

		private static void scrollViewer_PreviewMouseWheel( object sender, MouseWheelEventArgs e )
		{
			var scrollview = sender as Control;
			if( scrollview == null )
			{
				return;
			}

			var eventArg = new MouseWheelEventArgs( e.MouseDevice, e.Timestamp, e.Delta )
			{
				RoutedEvent = UIElement.MouseWheelEvent,
				Source = sender
			};

			var parent = scrollview.Parent as UIElement;
			parent?.RaiseEvent( eventArg );
		}

		public static readonly DependencyProperty ScrollProperty = DependencyProperty.RegisterAttached( "IsSendingMouseWheelEventToParent",
			 typeof( bool ), typeof( SendMouseWheelToParent ), new FrameworkPropertyMetadata( OnValueChanged ) );
	}
}