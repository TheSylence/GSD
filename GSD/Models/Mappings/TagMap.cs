using FluentNHibernate.Mapping;

namespace GSD.Models.Mappings
{
	// ReSharper disable once UnusedMember.Global
	internal class TagMap : ClassMap<Tag>
	{
		public TagMap()
		{
			Table( "tags" );

			Id( x => x.Id ).GeneratedBy.Native();

			Map( x => x.Name ).Not.Nullable().Length( 100 );
			Map( x => x.Color ).Length( 6 );

			References( x => x.Project ).Column( nameof( Tag.Project ) );
			HasManyToMany( x => x.Todos ).Table( "TodoTags" ).AsBag().ChildKeyColumn( "Todo" ).ParentKeyColumn( "Tag" );
		}
	}
}