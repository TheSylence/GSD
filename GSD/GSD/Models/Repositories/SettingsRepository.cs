using System.Collections.Generic;
using NHibernate;

namespace GSD.Models.Repositories
{
	internal interface ISettingsRepository : IRepository<Config>
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
				{LastProject, "-1"}
			};
		}

		public static readonly IReadOnlyDictionary<string, string> DefaultValues;
		internal const string Accent = "style.accent";
		internal const string LastProject = "state.project";
		internal const string Theme = "style.theme";
	}

	internal class SettingsRepository : Repository<Config>, ISettingsRepository
	{
		public SettingsRepository( ISession session ) : base( session )
		{
		}

		public void Set( string key, string value )
		{
			var cfg = GetById( key );
			cfg.Value = value ?? SettingKeys.DefaultValues[key];
			Update( cfg );
		}
	}
}