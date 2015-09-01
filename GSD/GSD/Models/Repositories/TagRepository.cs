using NHibernate;

namespace GSD.Models.Repositories
{
	internal class TagRepository : Repository<Tag>, ITagRepository
	{
		public TagRepository( ISession session ) : base( session )
		{
		}
	}
}