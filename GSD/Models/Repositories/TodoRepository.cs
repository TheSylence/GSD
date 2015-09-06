using NHibernate;

namespace GSD.Models.Repositories
{
	internal interface ITodoRepository : IRepository<Todo>
	{
	}

	internal class TodoRepository : Repository<Todo>, ITodoRepository
	{
		public TodoRepository( ISession session ) : base( session )
		{
		}
	}
}