using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MsSqlSerializationDemo.Entities
{
	public class XmlAsStringEntity : EntityBase
	{
		private List<SubEntity> _subEntities;

		public virtual string SubEntityBody { get; set; }

		public override List<SubEntity> SubEntities
		{
			get
			{
				if (_subEntities == null && !string.IsNullOrEmpty(SubEntityBody))
				{
					XmlSerializer s = new XmlSerializer(typeof(List<SubEntity>));
					using (var r = new StringReader(SubEntityBody))
					{
						_subEntities = (List<SubEntity>)s.Deserialize(r);
					}
				}
				return new ReadOnlyCollection<SubEntity>(_subEntities ?? new List<SubEntity>()).ToList();
			}
			set
			{
				_subEntities = value != null
					             ? value.ToList()
					             : null;

				SubEntityBody = null;

				if (value != null)
				{
					XmlSerializer s = new XmlSerializer(typeof(List<SubEntity>));
					using (var w = new StringWriter())
					{
						s.Serialize(w, _subEntities);
						var val = w.ToString();
						SubEntityBody = val;
					}
				}
			}
		}
	}

	public class XmlAsStringEntityMap : ClassMapping<XmlAsStringEntity>
	{
		public XmlAsStringEntityMap()
		{
			Id(x => x.ID, m=> m.Generator(Generators.Identity));
			Property(x => x.SubEntityBody, m =>
				{
					m.Type(NHibernateUtil.StringClob);
					m.Column(cm => cm.SqlType("nvarchar(max)"));
				});
		}
	}
}