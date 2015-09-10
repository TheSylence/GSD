using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;

namespace GSD.Tests.Models.Repositories
{
	public class RepositoryTestBase
	{
		protected static void ClassInitStatic()
		{
			var dbConfig = SQLiteConfiguration.Standard.InMemory().ShowSql();

			Config = Fluently.Configure().Database( dbConfig )
				.Mappings( SetMappings )
				.BuildConfiguration();
		}

		protected void AfterTest()
		{
			Session.Dispose();
		}

		protected void BeforeTest()
		{
			var sessionFactory = Config.BuildSessionFactory();
			Session = sessionFactory.OpenSession();
			new SchemaExport( Config ).Execute( true, true, false, Session.Connection, null );
		}

		private static void SetMappings( MappingConfiguration map )
		{
			Action<MappingConfiguration> defaultAction = m => m.FluentMappings.Add<TestEntityMap>();

			( MapAction ?? defaultAction )( map );
		}

		protected static ISession Session;
		private static Configuration Config;
		protected static Action<MappingConfiguration> MapAction;
	}
}