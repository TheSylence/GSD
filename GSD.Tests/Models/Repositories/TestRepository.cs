using GSD.Models.Repositories;
using NHibernate;

namespace GSD.Tests.Models.Repositories
{
	internal class TestRepository : Repository<TestEntity>
	{
		public TestRepository( ISession session ) : base( session )
		{
		}
	}
}