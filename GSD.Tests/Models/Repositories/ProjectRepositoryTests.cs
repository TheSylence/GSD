using GSD.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GSD.Tests.Models.Repositories
{
	[TestClass]
	public class ProjectRepositoryTests : RepositoryTestBase
	{
		[ClassInitialize]
		public static void _ClassInit( TestContext context )
		{
			MapAction = map => map.FluentMappings.AddFromAssemblyOf<Project>();

			ClassInitStatic( context );
		}

		[TestCleanup]
		public void _AfterTest()
		{
			AfterTest();
		}

		[TestInitialize]
		public void _BeforeTest()
		{
			BeforeTest();

			using( var tx = Session.BeginTransaction() )
			{
				foreach( var ent in TestData )
				{
					Session.Save( ent );
				}

				tx.Commit();
			}
		}

		private readonly Project[] TestData =
		{
			new Project {Id = 1, Name = "one"},
			new Project {Id = 2, Name = "two"},
			new Project {Id = 3, Name = "three"}
		};
	}
}