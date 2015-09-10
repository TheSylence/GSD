using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GSD.Models.Repositories;
using GSD.ViewServices;
using NHibernate;

namespace GSD.ViewModels
{
	internal abstract class ViewModelBaseEx : ViewModelBase, IDisposable
	{
		protected ViewModelBaseEx( IViewServiceRepository viewServices = null, ISettingsRepository settingsRepo = null )
		{
			ViewServices = viewServices ?? App.ViewServices;
			Session = App.Session;
			Settings = settingsRepo ?? new SettingsRepository();
		}

		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose( true );
		}

		protected void Dispose( bool disposing )
		{
			if( disposing )
			{
				Session?.Dispose();
			}
		}

		public IMessenger TestMessenger => MessengerInstance;

		protected IViewServiceRepository ViewServices { get; }
		protected readonly ISession Session;
		protected readonly ISettingsRepository Settings;
	}
}