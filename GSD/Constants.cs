using System;
using System.IO;

namespace GSD
{
	internal static class Constants
	{
		public static string DefaultDatabasePath => Path.Combine( DataPath, "data.db3" );

		public static string SettingsFileName => Path.Combine( DataPath, "settings.cfg" );

		private static string DataPath
		{
			get
			{
				string path = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ), "btbsoft", "GSD" );

				if( !Directory.Exists( path ) )
				{
					Directory.CreateDirectory( path );
				}

				return path;
			}
		}

		public const string AutostartArgument = "/autostart";
	}
}