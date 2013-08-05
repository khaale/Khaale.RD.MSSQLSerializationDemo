using System.Collections.Generic;
using MsSqlSerializationDemo.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Transform;
using NUnit.Framework;

namespace MsSqlSerializationDemo.Tests
{
	[SetUpFixture]
	class DbSetup
	{
		private static ISessionFactory _sessionFactory;

		public static ISessionFactory SessionFactory
		{
			get { return _sessionFactory; }
		}

		[SetUp]
		public void SetUp()
		{
			CreateMapper();

			Configuration cfg = new Configuration();
			cfg.Configure();
			foreach (var m in CreateMapper())
			{
				cfg.AddMapping(m);
			}

			var schemaExport = new SchemaExport(cfg);
			schemaExport.Execute(true, true, false);

			_sessionFactory = cfg.BuildSessionFactory();
		}

		private IEnumerable<HbmMapping> CreateMapper()
		{
			var mapper = new ModelMapper();

			mapper.AddMapping<XmlAsStringEntityMap>();
			mapper.AddMapping<XmlEntityMap>();
			mapper.AddMapping<BinaryEntityMap>();
			mapper.AddMapping<ProtobufEntityMap>();
			mapper.AddMapping<JsonEntityMap>();
			return mapper.CompileMappingForEachExplicitlyAddedEntity();
		}

		public static void ClearTable(string tableName)
		{
			using (var session = _sessionFactory.OpenStatelessSession())
			{
				var query = string.Format("truncate table [{0}]", tableName);
				session.CreateSQLQuery(query).UniqueResult();
			}
		}

		public static SpaceUsedData GetSpaceUsedData(string tableName)
		{
			using (var session = _sessionFactory.OpenStatelessSession())
			{
				var query = string.Format("sp_spaceused [{0}]", tableName);
				var result = session.CreateSQLQuery(query)
					.AddScalar("rows", NHibernateUtil.Int32)
					.AddScalar("data", NHibernateUtil.String)
					.SetResultTransformer(Transformers.AliasToBeanConstructor(typeof(SpaceUsedData).GetConstructors()[0]))
					.UniqueResult();
				return (SpaceUsedData) result;
			}
		}
	}

	public struct SpaceUsedData
	{
		public SpaceUsedData(int rows, string dataKb) : this()
		{
			Rows = rows;
			DataKb = int.Parse(dataKb.Replace("KB","").Trim());
		}

		public int Rows { get; private set; }
		public int DataKb { get; private set; }

	}
}
