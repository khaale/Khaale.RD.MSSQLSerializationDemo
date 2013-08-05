using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FizzWare.NBuilder;
using MsSqlSerializationDemo.Entities;
using NUnit.Framework;

namespace MsSqlSerializationDemo.Tests
{
	[TestFixture(typeof(XmlAsStringEntity))]
	[TestFixture(typeof(XmlEntity))]
	[TestFixture(typeof(BinaryEntity))]
	[TestFixture(typeof(ProtobufEntity))]
	[TestFixture(typeof(JsonEntity))]
	public class TestSerialization
	{
		private readonly Type _type;
		protected virtual string TableName { get { return _type.Name; } }

		public TestSerialization(Type type)
		{
			_type = type;
		}


		[SetUp]
		public void SetUp()
		{
			//warming up
			var entity = CreateEntities(1).First();

			using (var session = DbSetup.SessionFactory.OpenStatelessSession())
			{
				session.Insert(entity);
			}

			DbSetup.ClearTable(TableName);
		}

		private ICollection<EntityBase> CreateEntities(int count)
		{
			var entities = new List<EntityBase>();

			for (var i = 0; i < count; i++)
			{
				var entity = (EntityBase)Activator.CreateInstance(_type);
				entity.SubEntities = Builder<SubEntity>.CreateListOfSize(5).Build().ToList();
				entities.Add(entity);
			}
			return entities;
		}

		[Test]
		public void TestDataSavedAndLoaded()
		{
			//warming up
			var entity = CreateEntities(1).First();

			using (var session = DbSetup.SessionFactory.OpenStatelessSession())
			{
				session.Insert(entity);
			}
			using (var session = DbSetup.SessionFactory.OpenStatelessSession())
			{
				var result = session.Get(_type.FullName, entity.ID);
				Assert.AreEqual(entity.SubEntities.Count, ((EntityBase)result).SubEntities.Count);
			}
		}

		[Test]
		public void TestInsert_SingleItem()
		{
			var count = 10000;

			var stw = Stopwatch.StartNew();
			var entities = CreateEntities(count);			
			foreach (var entity in entities)
			{
				using (var session = DbSetup.SessionFactory.OpenStatelessSession())
				{
					session.Insert(entity);
				}
			}

			var spaceUsedData = DbSetup.GetSpaceUsedData(TableName);
			stw.Stop();
			Debug.WriteLine("Elapsed time: {0}, count: {1}, ms/item: {2}", stw.Elapsed, count, (double)stw.ElapsedMilliseconds / count);
			Debug.WriteLine("Table size: {0}, count: {1}, kb/item: {2:0.00}", spaceUsedData.DataKb, spaceUsedData.Rows, (double)spaceUsedData.DataKb/spaceUsedData.Rows);
		}

		[Test]
		public void TestSelect_SingleItem()
		{
			var entityCount = 100;
			var count = 10000;
			var rnd = new Random();

			var entities = CreateEntities(entityCount);
			foreach (var entity in entities)
			{
				using (var session = DbSetup.SessionFactory.OpenStatelessSession())
				{
					session.Insert(entity);
				}
			}

			var stw = Stopwatch.StartNew();

			foreach (var i in Enumerable.Range(0, count))
			{
				using (var session = DbSetup.SessionFactory.OpenStatelessSession())
				{
					var id = rnd.Next(1,entityCount - 1);
					var entity = session.Get(_type.FullName, id);
					var t = ((EntityBase)entity).SubEntities;
				}
			}

			stw.Stop();
			Debug.WriteLine("Elapsed time: {0}, count: {1}, ms/item: {2:0.00}", stw.Elapsed, count, (double)stw.ElapsedMilliseconds / count);
		}
	}
}
