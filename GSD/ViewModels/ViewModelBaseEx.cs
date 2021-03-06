﻿using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GSD.Models.Repositories;
using GSD.ViewServices;
using NHibernate;

namespace GSD.ViewModels
{
	internal abstract class ViewModelBaseEx : ViewModelBase, IDisposable
	{
		protected ViewModelBaseEx( IViewServiceRepository viewServices = null, ISettingsRepository settingsRepo = null, IMessenger messenger = null )
		{
			MessengerInstance = messenger;
			ViewServices = viewServices ?? App.ViewServices;
			Session = App.Session;
			Settings = settingsRepo ?? App.Settings;
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
		

		protected IViewServiceRepository ViewServices { get; }
		protected readonly ISession Session;
		protected readonly ISettingsRepository Settings;
	}
}