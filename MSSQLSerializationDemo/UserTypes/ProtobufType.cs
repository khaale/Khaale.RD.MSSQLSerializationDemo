using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using ProtoBuf;

namespace MsSqlSerializationDemo.UserTypes
{
	public class ProtobufUserType<T> : IUserType where T : class
	{
		public new bool Equals(object x, object y)
		{
			return x == y;
		}

		public int GetHashCode(object x)
		{
			return x.GetHashCode();
		}

		public object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			if (names.Length != 1)
				throw new InvalidOperationException("names array has more than one element. can't handle this!");

			var val = rs[names[0]] as byte[];

			if (val != null)
			{
				return Deserialize(val);
			}

			return null;
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			var parameter = (DbParameter)cmd.Parameters[index];
			T toSave = value as T;

			if (toSave != null)
			{
				parameter.Value = Serialize(toSave);
			}
			else
			{
				parameter.Value = DBNull.Value;
			}
		}

		public object DeepCopy(object value)
		{
			T toCopy = value as T;

			if (toCopy == null)
				return null;

			byte[] serialized = Serialize(toCopy);

			return Deserialize(serialized);
		}

		private T Deserialize(byte[] serialized)
		{
			using (var r = new MemoryStream(serialized))
			{
				return Serializer.Deserialize<T>(r);
			}
		}

		private byte[] Serialize(T instance)
		{
			using (var memoryStream = new MemoryStream())
			{
				Serializer.Serialize(memoryStream, instance);
				return memoryStream.GetBuffer();
			}
		}

		public object Replace(object original, object target, object owner)
		{
			throw new NotImplementedException();
		}

		public object Assemble(object cached, object owner)
		{
			var bytes = cached as byte[];
			if (bytes == null)
			{
				return null;
			}

			return Deserialize(bytes);
		}

		public object Disassemble(object value)
		{
			var toCache = value as T;

			if (toCache != null)
			{
				return Serialize(toCache);
			}

			return null;
		}

		public SqlType[] SqlTypes
		{
			get
			{
				return new SqlType[] { new SqlProtobufType()  };
			}
		}

		public Type ReturnedType
		{
			get { return typeof(XmlDocument); }
		}

		public bool IsMutable
		{
			get { return false; }
		}
	}

	public class SqlProtobufType : SqlType
	{
		public SqlProtobufType()
			: base(DbType.Binary)
		{
		}
	}

}
