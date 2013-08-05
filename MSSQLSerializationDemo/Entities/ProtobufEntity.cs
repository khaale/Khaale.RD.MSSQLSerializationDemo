using System.Collections.Generic;
using MsSqlSerializationDemo.UserTypes;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MsSqlSerializationDemo.Entities
{
	public class ProtobufEntity : EntityBase
	{
	}

	public class ProtobufEntityMap : ClassMapping<ProtobufEntity>
	{
		public ProtobufEntityMap()
		{
			Id(x => x.ID, m => m.Generator(Generators.Identity));
			Property(x => x.SubEntities, m =>
				{
					m.Column(cm => cm.SqlType("varbinary(max)"));
					m.Type<ProtobufUserType<List<SubEntity>>>();
				});
		}
	}
}