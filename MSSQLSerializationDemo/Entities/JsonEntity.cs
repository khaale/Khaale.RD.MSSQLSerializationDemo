using System.Collections.Generic;
using MsSqlSerializationDemo.UserTypes;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MsSqlSerializationDemo.Entities
{
	public class JsonEntity : EntityBase
	{
	}

	public class JsonEntityMap : ClassMapping<JsonEntity>
	{
		public JsonEntityMap()
		{
			Id(x => x.ID, m => m.Generator(Generators.Identity));
			Property(x => x.SubEntities, m =>
				{
					m.Column(cm => cm.SqlType("nvarchar(max)"));
					m.Type<JsonUserType<List<SubEntity>>>();
				});
		}
	}
}