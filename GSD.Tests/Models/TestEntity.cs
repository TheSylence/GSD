using FluentNHibernate.Mapping;
using GSD.Models;

namespace GSD.Tests.Models
{
	internal class TestEntity : Entity
	{
		public virtual string PropOne { get; set; }
		public virtual int PropTwo { get; set; }
	}

	// ReSharper disable once ClassNeverInstantiated.Global
	internal class TestEntityMap : ClassMap<TestEntity>
	{
		public TestEntityMap()
		{
			Id( x => x.Id ).GeneratedBy.Native();

			Map( x => x.PropOne );
			Map( x => x.PropTwo );
		}
	}
}