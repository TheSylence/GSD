using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GSD.Messages;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace GSD.ViewModels
{
	internal class NotificationViewModel : ViewModelBaseEx
	{
		public NotificationViewModel()
			: this( null )
		{
		}

		public NotificationViewModel( IMessenger messenger )
			: base( null, null, messenger )
		{
			MessengerInstance.Register<NotificationMessage>( this, OnNotification );

			HideTimer = new DispatcherTimer();
			HideTimer.Interval = TimeSpan.FromSeconds( 4 );
			HideTimer.Tick += HideTimer_Tick;
		}

		private void ExecuteDismissCommand()
		{
			Hide();
		}

		private void Hide()
		{
			HideTimer.Stop();
			MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.NotificationFlyoutName ) );
		}

		private void HideTimer_Tick( object sender, EventArgs e )
		{
			Hide();
		}

		private void OnNotification( NotificationMessage msg )
		{
			HideTimer.Stop();

			CurrentNotification = msg.Notification;
			MessengerInstance.Send( new FlyoutMessage( FlyoutMessage.NotificationFlyoutName ) );

			HideTimer.Start();
		}

		public string CurrentNotification
		{
			[System.Diagnostics.DebuggerStepThrough] get { return _CurrentNotification; }
			set
			{
				if( _CurrentNotification == value )
				{
					return;
				}

				_CurrentNotification = value;
				RaisePropertyChanged();
			}
		}

		public ICommand DismissCommand => _DismissCommand ?? ( _DismissCommand = new RelayCommand( ExecuteDismissCommand ) );
		private readonly DispatcherTimer HideTimer;

		[System.Diagnostics.DebuggerBrowsable( System.Diagnostics.DebuggerBrowsableState.Never )]
		private string _CurrentNotification;

		private RelayCommand _DismissCommand;
	}
}