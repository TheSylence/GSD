using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interactivity;

namespace GSD.Behaviors
{
	[ExcludeFromCodeCoverage]
	internal class DataContextDisposer : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			Action dispose = () =>
			{
				var dataContext = AssociatedObject.DataContext as IDisposable;
				dataContext?.Dispose();
			};

			AssociatedObject.Unloaded += ( s, e ) => dispose();
			AssociatedObject.Dispatcher.ShutdownStarted += ( s, e ) => dispose();
		}
	}
}