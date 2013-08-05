using System.Collections.Generic;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MsSqlSerializationDemo.Entities
{
	public class BinaryEntity : EntityBase
	{
	}

	public class BinaryEntityMap : ClassMapping<BinaryEntity>
	{
		public BinaryEntityMap()
		{
			Id(x => x.ID, m => m.Generator(Generators.Identity));
			Property(x => x.SubEntities, m =>
				{
					m.Column(cm => cm.SqlType("varbinary(max)"));
					m.Type(NHibernateUtil.GetSerializable(typeof (List<SubEntity>)));
				});
		}
	}
}