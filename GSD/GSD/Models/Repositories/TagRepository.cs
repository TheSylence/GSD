using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace GSD.Models.Repositories
{
	internal interface ITagRepository : IRepository<Tag>
	{
		IEnumerable<Tag> GetForProject( Project project );
	}

	internal class TagRepository : Repository<Tag>, ITagRepository
	{
		public TagRepository( ISession session ) : base( session )
		{
		}

		public IEnumerable<Tag> GetForProject( Project project )
		{
			return Session.CreateCriteria<Tag>().Add( Restrictions.Eq( nameof( Tag.Project ), project ) ).List<Tag>();
		}
	}
}