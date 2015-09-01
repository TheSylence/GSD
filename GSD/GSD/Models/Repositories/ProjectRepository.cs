using NHibernate;

namespace GSD.Models.Repositories
{
	internal class ProjectRepository : Repository<Project>, IProjectRepository
	{
		public ProjectRepository( ISession session ) : base( session )
		{
		}
	}
}