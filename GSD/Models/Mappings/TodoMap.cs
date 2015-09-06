using FluentNHibernate.Mapping;

namespace GSD.Models.Mappings
{
	internal class TodoMap : ClassMap<Todo>
	{
		public TodoMap()
		{
			Table( "todos" );

			Id( x => x.Id ).GeneratedBy.Native();

			Map( x => x.Summary ).Not.Nullable().Length( 100 );
			Map( x => x.Details ).Length( 10000 );
			Map( x => x.Done ).Not.Nullable();

			HasManyToMany<Tag>( x => x.Tags ).AsBag().Table( "TodoTags" ).ChildKeyColumn( "Tag" ).ParentKeyColumn( "Todo" );
			References<Project>( x => x.Project ).Column( nameof( Todo.Project ) );
		}
	}
}