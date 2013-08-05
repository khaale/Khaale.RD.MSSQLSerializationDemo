using System;
using ProtoBuf;

namespace MsSqlSerializationDemo.Entities
{
	[Serializable]
	[ProtoContract]
	public class SubEntity
	{
		#region Properties

		[ProtoMember(1)]
		public string Id { get; set; }

		[ProtoMember(2)]
		public SubEntityState State { get; set; }

		[ProtoMember(3)]
		public SubEntityType Type { get; set; }
		
		public bool IsStateWrong
		{
			get { return State == SubEntityState.Unknown; }
		}

		[ProtoMember(4)]
		public string OriginalStatus { get; set; }

		[ProtoMember(5)]
		public string OriginalType { get; set; }

		[ProtoMember(6)]
		public bool Flag1 { get; set; }

		[ProtoMember(7)]
		public bool Flag2 { get; set; }

		[ProtoMember(8)]
		public bool Flag3 { get; set; }

		[ProtoMember(9)]
		public Enum2 Enum2 { get; set; }

		[ProtoMember(10)]
		public DateTime Date1 { get; set; }

		[ProtoMember(11)]
		public decimal Value1 { get; set; }

		[ProtoMember(12)]
		public string String1 { get; set; }

		[ProtoMember(13)]
		public decimal Value2 { get; set; }

		[ProtoMember(14)]
		public DateTime Date2 { get; set; }

		[ProtoMember(15)]
		public DateTime Date3 { get; set; }

		[ProtoMember(16)]
		public DateTime? TheLastDate { get; set; }

		[ProtoMember(17)]
		public string String2 { get; set; }

		[ProtoMember(18)]
		public DictionaryKeyComponent Key1 { get; set; }

		[ProtoMember(19)]
		public string String3 { get; set; }

		[ProtoMember(20)]
		public string String4 { get; set; }

		[ProtoMember(21)]
		public string String5 { get; set; }

		[ProtoMember(22)]
		public string String6 { get; set; }

		[ProtoMember(23)]
		public string String7 { get; set; }

		[ProtoMember(24)]
		public Enum1? Enum1 { get; set; }

		#endregion
	}
}