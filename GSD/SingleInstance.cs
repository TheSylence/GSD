using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using GSD.ViewModels.Utilities;

namespace GSD
{
	internal class SingleInstance : IDisposable
	{
		public SingleInstance( Application app, IAppController appController = null )
		{
			App = app;
			AppController = appController ?? new AppController();

			string name = $"Local\\GSD:{Assembly.GetExecutingAssembly().GetName().Name}";

			AppMutex = new Mutex( true, name, out MutexCreated );

			MessageId = RegisterWindowMessage( name );
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		public void RegisterWindow( Window window )
		{
			WindowHandle = ( new WindowInteropHelper( window ) ).Handle;
			var source = HwndSource.FromHwnd( WindowHandle );
			source?.AddHook( HandleMessages );
		}

		public void Shutdown()
		{
			App.Shutdown();
		}

		[DllImport( "user32.dll", SetLastError = true, CharSet = CharSet.Auto )]
		private static extern uint RegisterWindowMessage( string lpString );

		private void Dispose( bool disposing )
		{
			AppMutex.Dispose();
		}

		private IntPtr HandleMessages( IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled )
		{
			if( msg == MessageId )
			{
				AppController.ShowWindow();
			}

			return IntPtr.Zero;
		}

		public bool IsFirstInstance => MutexCreated;
		private readonly Application App;
		private readonly IAppController AppController;
		private readonly Mutex AppMutex;
		private readonly uint MessageId;
		private readonly bool MutexCreated;
		private IntPtr WindowHandle;
	}
}