using NHibernate;

namespace GSD.Models.Repositories
{
	internal class TodoRepository : Repository<Todo>, ITodoRepository
	{
		public TodoRepository( ISession session ) : base( session )
		{
		}
	}
}