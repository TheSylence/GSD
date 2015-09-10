using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace GSD.Models.Repositories
{
	internal interface ITagRepository : IRepository<Tag>
	{
		IEnumerable<string> GetAllNames( Project project );
	}

	internal class TagRepository : Repository<Tag>, ITagRepository
	{
		public TagRepository( ISession session ) : base( session )
		{
		}

		public IEnumerable<string> GetAllNames( Project project )
		{
			return Session.CreateCriteria<Tag>()
				.Add( Restrictions.Eq( nameof( Tag.Project ), project ) )
				.SetProjection( Projections.ProjectionList().Add( Projections.Property( nameof( Tag.Name ) ) ) )
				.List<string>();
		}
	}
}