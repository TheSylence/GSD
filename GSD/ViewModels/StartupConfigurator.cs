using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace GSD.ViewModels
{
	internal interface IStartupConfigurator
	{
		void SetStartup( bool enable, string args = null );
	}

	internal class StartupConfigurator : IStartupConfigurator
	{
		public void SetStartup( bool enable, string args = null )
		{
			const string keyName = "GSD";

			RegistryKey runKey = Registry.CurrentUser.OpenSubKey( "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true );
			if( runKey == null )
			{
				return;
			}

			if( enable )
			{
				string path = Assembly.GetExecutingAssembly().Location;
				if( path.Contains( " " ) )
				{
					path = $"\"{path}\"";
				}

				if( !string.IsNullOrWhiteSpace( args ) )
				{
					path = $"{path} {args}";
				}

				runKey.SetValue( keyName, path );
			}
			else
			{
				runKey.DeleteValue( keyName, false );
			}
		}
	}
}