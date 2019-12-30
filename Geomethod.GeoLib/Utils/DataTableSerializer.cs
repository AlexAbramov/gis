using System;
using System.IO;
using System.Data;

namespace Geomethod.GeoLib
{
	/// <summary>
	/// Summary description for DataTableSerializer.
	/// </summary>
	public class DataTableSerializer
	{
		const string dataTableToken="DataTable";
		const int serVer=0;
		enum SerType{Boolean,Byte,Char,DateTime,Decimal,Double,Int16,Int32,Int64,SByte,Single,String,TimeSpan,UInt16,UInt32,UInt64,Bytes,Chars,Unknown}

		#region SerType
		static SerType GetSerType(Type type)
		{
			if(type==typeof(bool)) return SerType.Boolean;
			if(type==typeof(byte)) return SerType.Byte;
			if(type==typeof(byte[])) return SerType.Bytes;
			if(type==typeof(char)) return SerType.Char;
			if(type==typeof(char[])) return SerType.Chars;
			if(type==typeof(DateTime)) return SerType.DateTime;
			if(type==typeof(decimal)) return SerType.Decimal;
			if(type==typeof(double)) return SerType.Double;
			if(type==typeof(short)) return SerType.Int16;
			if(type==typeof(int)) return SerType.Int32;
			if(type==typeof(long)) return SerType.Int64;
			if(type==typeof(sbyte)) return SerType.SByte;
			if(type==typeof(float)) return SerType.Single;
			if(type==typeof(string)) return SerType.String;
			if(type==typeof(TimeSpan)) return SerType.TimeSpan;
			if(type==typeof(ushort)) return SerType.UInt16;
			if(type==typeof(uint)) return SerType.UInt32;
			if(type==typeof(ulong)) return SerType.UInt64;
			return SerType.Unknown;
		}
		static Type GetDataType(SerType serType)
		{
			switch(serType)
			{
				case SerType.Boolean: return typeof(bool);
				case SerType.Byte: return typeof(byte);
				case SerType.Bytes: return typeof(byte[]);
				case SerType.Char: return typeof(char);
				case SerType.Chars: return typeof(char[]);
				case SerType.DateTime: return typeof(DateTime);
				case SerType.Decimal: return typeof(decimal);
				case SerType.Double: return typeof(double);
				case SerType.Int16: return typeof(short);
				case SerType.Int32: return typeof(int);
				case SerType.Int64: return typeof(long);
				case SerType.SByte: return typeof(sbyte);
				case SerType.Single: return typeof(float);
				case SerType.String: return typeof(string);
				case SerType.TimeSpan: return typeof(TimeSpan);
				case SerType.UInt16: return typeof(ushort);
				case SerType.UInt32: return typeof(uint);
				case SerType.UInt64: return typeof(ulong);
				default: throw new Exception("Unsupported serialization type in GetDataType: "+serType.ToString());
			}
		}
		static object ReadObject(BinaryReader br,SerType serType)
		{
			switch(serType)
			{
				case SerType.Boolean: return br.ReadBoolean();
				case SerType.Byte: return br.ReadByte();
				case SerType.Bytes: {int count=br.ReadInt32(); return br.ReadBytes(count);}
				case SerType.Char: return br.ReadChar();
				case SerType.Chars: {int count=br.ReadInt32(); return br.ReadChars(count);}
				case SerType.DateTime: return new DateTime(br.ReadInt64());
				case SerType.Decimal: return br.ReadDecimal();
				case SerType.Double: return br.ReadDouble();
				case SerType.Int16: return br.ReadInt16();
				case SerType.Int32: return br.ReadInt32();
				case SerType.Int64: return br.ReadInt64();
				case SerType.SByte: return br.ReadSByte();
				case SerType.Single: return br.ReadSingle();
				case SerType.String: return br.ReadString();
				case SerType.TimeSpan: return new TimeSpan(br.ReadInt64());
				case SerType.UInt16: return br.ReadUInt16();
				case SerType.UInt32: return br.ReadUInt32();
				case SerType.UInt64: return br.ReadUInt64();
				default: throw new Exception("Unsupported serialization type in ReadObject: "+serType.ToString());
			}
		}
		static void WriteObject(BinaryWriter bw,SerType serType,object obj)
		{
			switch(serType)
			{
				case SerType.Boolean: bw.Write((bool)obj); break;
				case SerType.Byte: bw.Write((byte)obj); break;
				case SerType.Bytes: 
					byte[] bytes=(byte[])obj;
					bw.Write(bytes.Length);
					bw.Write(bytes);
					break;
				case SerType.Char: bw.Write((char)obj); break;
				case SerType.Chars:
					char[] chars=(char[])obj;
					bw.Write(chars.Length);
					bw.Write(chars);
					break;
				case SerType.DateTime: bw.Write(((DateTime)obj).Ticks); break;
				case SerType.Decimal: bw.Write((decimal)obj); break;
				case SerType.Double: bw.Write((double)obj); break;
				case SerType.Int16: bw.Write((short)obj); break;
				case SerType.Int32: bw.Write((int)obj); break;
				case SerType.Int64: bw.Write((long)obj); break;
				case SerType.SByte: bw.Write((sbyte)obj); break;
				case SerType.Single: bw.Write((float)obj); break;
				case SerType.String: bw.Write((string)obj); break;
				case SerType.TimeSpan: bw.Write(((TimeSpan)obj).Ticks); break;
				case SerType.UInt16: bw.Write((ushort)obj); break;
				case SerType.UInt32: bw.Write((uint)obj); break;
				case SerType.UInt64: bw.Write((ulong)obj); break;
				default: throw new Exception("Unsupported serialization type in WriteObject:  "+serType.ToString());
			}
		}
		#endregion

		public static void Read(BinaryReader br,DataTable dataTable)
		{
			dataTable.BeginInit();
			dataTable.Clear();
			string token=br.ReadString();
			if(token!=dataTableToken) throw new Exception("Wrong token for DataTable: "+token);
			int curSerVer=br.ReadInt32();
			dataTable.TableName=br.ReadString();

			//columns
			int nCols=br.ReadInt32();
			for(int i=0;i<nCols;i++)
			{
				DataColumn dc=new DataColumn();
				dc.AllowDBNull=br.ReadBoolean();
				dc.Caption=br.ReadString();
				dc.ColumnName=br.ReadString();
				SerType st=(SerType)br.ReadInt32();
				dc.DataType=GetDataType(st);
				dc.MaxLength=br.ReadInt32();
				dataTable.Columns.Add(dc);
			}

			//rows
			int count=br.ReadInt32();
			for(int i=0;i<count;i++)
			{
				object[] values=new object[dataTable.Columns.Count];
				for(int col=0;col<values.Length;col++)
				{
					DataColumn dc=dataTable.Columns[col];
					SerType serType=GetSerType(dc.DataType);
					values[col]=(dc.AllowDBNull && br.ReadBoolean()==true) ? null : ReadObject(br,serType);
				}
				dataTable.Rows.Add(values);
			}
			dataTable.EndInit();
		}

		public static void Write(BinaryWriter bw,DataTable dataTable)
		{
			//header
			bw.Write(dataTableToken);
			DataColumnCollection cols=dataTable.Columns;
			bw.Write(serVer);
			bw.Write(dataTable.TableName);

			//columns
			bw.Write(cols.Count);
			foreach(DataColumn dc in cols)
			{
				bw.Write(dc.AllowDBNull);
				bw.Write(dc.Caption);
				bw.Write(dc.ColumnName);
				bw.Write((int)GetSerType(dc.DataType));
				bw.Write(dc.MaxLength);
			}

			//rows
			bw.Write(dataTable.Rows.Count);
			foreach(DataRow dr in dataTable.Rows)
			{
				foreach(DataColumn dc in cols)
				{
					object obj=dr[dc];
					if(dc.AllowDBNull)
          {
						bool isNull=obj==null;
						bw.Write(isNull);
						if(isNull) continue;
					}

					SerType serType=GetSerType(dc.DataType);
					WriteObject(bw,serType,obj);
				}
			}
		}
	}
}
