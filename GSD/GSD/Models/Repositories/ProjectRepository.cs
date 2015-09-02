using NHibernate;

namespace GSD.Models.Repositories
{
	internal interface IProjectRepository : IRepository<Project>
	{

	}

	internal class ProjectRepository : Repository<Project>, IProjectRepository
	{
		public ProjectRepository( ISession session ) : base( session )
		{
		}
	}
}