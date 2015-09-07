using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GSD.Models.Repositories;
using GSD.ViewServices;
using NHibernate;
using System;

namespace GSD.ViewModels
{
	internal class ViewModelBaseEx : ViewModelBase, IDisposable
	{
		protected ViewModelBaseEx( ISettingsRepository settingsRepo = null )
		{
			Session = App.Session;
			Settings = settingsRepo ?? new SettingsRepository( Session );
		}

		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose( true );
		}

		protected virtual void Dispose( bool disposing )
		{
			if( disposing )
			{
				Session?.Dispose();
			}
		}

		public IMessenger TestMessenger => MessengerInstance;

		protected IViewServiceRepository ViewServices => App.ViewServices;
		protected readonly ISession Session;
		protected readonly ISettingsRepository Settings;
	}
}