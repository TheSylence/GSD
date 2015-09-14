using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GSD.Models.Repositories
{
	public interface ISettingsRepository : IRepository<Config>
	{
		void Set( string key, string value );
	}

	internal static class SettingKeys
	{
		static SettingKeys()
		{
			DefaultValues = new Dictionary<string, string>
			{
				{Accent, "Blue"},
				{Theme, "BaseLight"},
				{LastProject, "-1"},
				{DatabasePath, Constants.DefaultDatabasePath},
				{ExpandEntries, "False"},
				{Language, "en-US"},
				{StartMinimized, "False"},
				{StartWithWindows, "False"},
				{CloseToTray, "True"}
			};
		}

		public static readonly IReadOnlyDictionary<string, string> DefaultValues;
		internal const string Accent = "style.accent";
		internal const string CloseToTray = "ui.closetotray";
		internal const string DatabasePath = "io.dbpath";
		internal const string ExpandEntries = "state.expand";
		internal const string Language = "ui.language";
		internal const string LastProject = "state.project";
		internal const string StartMinimized = "ui.startminimized";
		internal const string StartWithWindows = "app.startwithwindows";
		internal const string Theme = "style.theme";
	}

	internal class SettingsRepository : ISettingsRepository
	{
		public SettingsRepository( string fileName = null )
		{
			FileName = fileName ?? Constants.SettingsFileName;

			if( File.Exists( FileName ) )
			{
				Entries = new Dictionary<string, string>();
				foreach( var parts in File.ReadAllLines( FileName ).Select( line => line.Split( new[] {'='}, 2 ) ) )
				{
					Entries.Add( parts[0], parts[1] );
				}

				foreach( var kvp in SettingKeys.DefaultValues )
				{
					var cfg = GetById( kvp.Key );
					if( cfg != null )
					{
						continue;
					}

					Entries.Add( kvp.Key, kvp.Value );
				}
			}
			else
			{
				Entries = SettingKeys.DefaultValues.ToDictionary( kvp => kvp.Key, kvp => kvp.Value );
			}
		}

		public void Add( Config entity )
		{
			throw new NotSupportedException();
		}

		public void Delete( Config entity )
		{
			throw new NotSupportedException();
		}

		public IEnumerable<Config> GetAll()
		{
			return Entries.Select( kvp => new Config {Id = kvp.Key, Value = kvp.Value} );
		}

		public Config GetById( object id )
		{
			string key = id.ToString();

			string value;
			return Entries.TryGetValue( key, out value )
				? new Config
				{
					Id = key,
					Value = value
				}
				: null;
		}

		public void Set( string key, string value )
		{
			var cfg = GetById( key );
			cfg.Value = value ?? SettingKeys.DefaultValues[key];
			Update( cfg );
		}

		public void Update( Config entity )
		{
			Entries[entity.Id] = entity.Value;
			Save();
		}

		private void Save()
		{
			File.WriteAllLines( FileName, Entries.Select( kvp => $"{kvp.Key}={kvp.Value}" ) );
		}

		private readonly Dictionary<string, string> Entries;
		private readonly string FileName;
	}
}