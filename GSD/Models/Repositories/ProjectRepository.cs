using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace GSD.Models.Repositories
{
	internal interface IProjectRepository : IRepository<Project>
	{
		IEnumerable<string> GetAllNames();
	}

	internal class ProjectRepository : Repository<Project>, IProjectRepository
	{
		public ProjectRepository( ISession session ) : base( session )
		{
		}

		public IEnumerable<string> GetAllNames()
		{
			return Session.CreateCriteria<Project>().SetProjection( Projections.ProjectionList().Add( Projections.Property( nameof( Project.Name ) ) ) ).List<string>();
		}
	}
}