using FluentNHibernate.Mapping;

namespace GSD.Models.Mappings
{
	// ReSharper disable once UnusedMember.Global
	internal class ProjectMap : ClassMap<Project>
	{
		public ProjectMap()
		{
			Table( "projects" );

			Id( x => x.Id ).GeneratedBy.Native();

			Map( x => x.Name ).Not.Nullable().Length( 100 );

			HasMany( x => x.Tags ).KeyColumn( nameof( Tag.Project ) ).Cascade.Delete();
			HasMany( x => x.Todos ).KeyColumn( nameof( Todo.Project ) ).Cascade.Delete();
		}
	}
}