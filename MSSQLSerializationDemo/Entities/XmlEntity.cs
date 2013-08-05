using System.Collections.Generic;
using MsSqlSerializationDemo.UserTypes;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MsSqlSerializationDemo.Entities
{
	public class XmlEntity : EntityBase
	{
		public override List<SubEntity> SubEntities { get; set; }
	}

	public class XmlEntityMap : ClassMapping<XmlEntity>
	{
		public XmlEntityMap()
		{
			Id(x => x.ID, m => m.Generator(Generators.Identity));
			Property(x => x.SubEntities, m => m.Type<XmlUserType<List<SubEntity>>>());
		}
	}
}