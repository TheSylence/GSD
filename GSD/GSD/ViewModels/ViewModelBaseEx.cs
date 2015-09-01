using System;
using GalaSoft.MvvmLight;
using NHibernate;

namespace GSD.ViewModels
{
	internal class ViewModelBaseEx : ViewModelBase, IDisposable
	{
		protected ViewModelBaseEx()
		{
			Session = App.SessionFactory.OpenSession();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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

		protected readonly ISession Session;
	}
}