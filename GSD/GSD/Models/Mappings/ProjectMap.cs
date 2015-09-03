using FluentNHibernate.Mapping;

namespace GSD.Models.Mappings
{
	internal class ProjectMap : ClassMap<Project>
	{
		public ProjectMap()
		{
			Table( "projects" );

			Id( x => x.Id ).GeneratedBy.Native();

			Map( x => x.Name ).Not.Nullable().Length( 100 );

			HasMany<Tag>( x => x.Tags ).KeyColumn( nameof( Tag.Project ) );
			HasMany<Todo>( x => x.Todos ).KeyColumn( nameof( Todo.Project ) );
		}
	}
}