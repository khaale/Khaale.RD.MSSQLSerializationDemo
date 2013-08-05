using System;
using System.Collections.Generic;
using ProtoBuf;

namespace MsSqlSerializationDemo.Entities
{
	public abstract class EntityBase
	{
		public virtual long ID { get; set; }
		public virtual List<SubEntity> SubEntities { get; set; }
	}

	public enum Enum1
	{
		Unspecified = 0,
	}

	[Serializable]
	[ProtoContract]
	public struct DictionaryKeyComponent
	{
		[ProtoMember(1)]
		public int? Id { get; set; }
		[ProtoMember(2)]
		public string Code { get; set; }
	}

	public enum Enum2
	{
		Unspecified = 0,
	}

	public enum SubEntityType
	{
		Unknown
	}

	public enum SubEntityState
	{
		Unknown
	}
}
