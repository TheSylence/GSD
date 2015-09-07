﻿using NHibernate;

namespace GSD.Models.Repositories
{
	internal interface ITagRepository : IRepository<Tag>
	{
	}

	internal class TagRepository : Repository<Tag>, ITagRepository
	{
		public TagRepository( ISession session ) : base( session )
		{
		}
	}
}