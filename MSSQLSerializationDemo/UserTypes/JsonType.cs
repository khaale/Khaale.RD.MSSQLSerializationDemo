using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Newtonsoft.Json;

namespace MsSqlSerializationDemo.UserTypes
{
	public class JsonUserType<T> : IUserType where T : class
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

			var val = rs[names[0]] as string;

			if (string.IsNullOrWhiteSpace(val) == false)
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

			string serialized = Serialize(toCopy);

			return Deserialize(serialized);
		}

		private T Deserialize(string serialized)
		{
			JsonSerializer s = new JsonSerializer();
			using (var r = new StringReader(serialized))
			{
				return (T) s.Deserialize(r, typeof(T));
			}
		}

		private string Serialize(T instance)
		{
			JsonSerializer s = new JsonSerializer();
			using (var w = new StringWriter())
			{
				s.Serialize(w, instance);
				return w.ToString();
			}
		}

		public object Replace(object original, object target, object owner)
		{
			throw new NotImplementedException();
		}

		public object Assemble(object cached, object owner)
		{
			var str = cached as string;
			if (string.IsNullOrWhiteSpace(str) == false)
			{
				return null;
			}

			return Deserialize(str);
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
				return new SqlType[] { new SqlJsonType() };
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

	public class SqlJsonType : SqlType
	{
		public SqlJsonType()
			: base(DbType.String)
		{
		}
	}

}
