using FluentNHibernate.Mapping;

namespace GSD.Models.Mappings
{
	internal class ConfigMap : ClassMap<Config>
	{
		public ConfigMap()
		{
			Table( "configs" );

			Id( x => x.Id );
			Map( x => x.Value ).Not.Nullable();
		}
	}
}