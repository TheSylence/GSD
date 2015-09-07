using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GSD.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Tool.hbm2ddl;

namespace GSD.Tests.Models
{
	[TestClass]
	public class SchemaTests
	{
		[TestMethod, TestCategory( "Models" )]
		public void CanGenerateSchema()
		{
			// Arrange
			var dbConfig = new SQLiteConfiguration().InMemory().ShowSql();

			var cfg = Fluently.Configure().Database( dbConfig )
				.Mappings( map => map.FluentMappings.AddFromAssembly( typeof( Todo ).Assembly ) )
				.BuildConfiguration();

			// Act
			new SchemaExport( cfg ).Execute( false, true, false );

			// Assert
		}
	}
}