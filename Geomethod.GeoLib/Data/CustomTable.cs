using System;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.Common;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	/// <summary>
	/// Summary description for CustomTable.
	/// </summary>
/*	public class CustomDataTable: ISerializableObject, IComparable
	{
		GLib lib;
		int id;
		string name;
		BitArray32 attr = 0;
		BitArray32 updateAttr = 0;
		DataTable dataTable=new DataTable();

		#region Access
		public int Id{get{return id;}}
		public DataTable DataTable{get{return dataTable;}}
		public GeoLib.ClassId ClassId{get{return ClassId.CustomTable;}}
		public string Name{get{return name;}set{if(value==null)value=""; if(name==value)return; name=value; UpdateAttr(CustomTableField.Name);}}
		void UpdateAttr(CustomTableField f){updateAttr[(int)f]=true;lib.SetChanged();}
		public bool GetCommonAttr(CommonAttr a) { return attr[(int)a]; }
		#endregion

		#region Construction
		public CustomDataTable(GLib lib,string name)
		{
			this.lib=lib;
			id=lib.GenerateId(this, ref updateAttr);
			this.name=name;
		}
		internal CustomDataTable(GLib lib, IDataReader dr)
		{
			this.lib=lib;
			int i=0;
			id=dr.GetInt32(i++);
			name=dr.GetString(i++);
		}
		internal CustomDataTable(GLib lib, BinaryReader br)
		{
			this.lib=lib;
			id=br.ReadInt32();
//			lib.UpdateGen(this);
			name=br.ReadString();
			DataTableSerializer.Read(br, dataTable);
//			dt.ExtendedProperties.Add(
//			dt.Columns[0].ExtendedProperties.
		}
		#endregion

		#region Serialization
		public void Save(Context context)
		{
			if(context.ExportMode || updateAttr[Constants.updateAttrCreated])
			{
				GmCommand cmd=context.TargetConn.CreateCommandById("insertIntoGisTables");
				cmd.AddInt("Id",id);
				cmd.AddString("Name",name,MaxLength.Name);
				cmd.ExecuteNonQuery();
			}
			else if(updateAttr.NonEmpty)
			{
				GmCommand cmd=context.Conn.CreateCommand();
				string cmdText="";
				cmd.AddInt("Id",id);
				if(updateAttr[(int)CustomTableField.Name])
				{
					cmdText+="Name=@Name,";
					cmd.AddString(Name,name,MaxLength.Name);
				}
				Geomethod.StringUtils.RemoveLastChar(ref cmdText);
				cmd.CommandText="update gisCustomTables set "+cmdText+" where Id=@Id";
				cmd.AddInt("Id",id);
				cmd.ExecuteNonQuery();
			}
			if(!context.ExportMode) updateAttr=0;
		}
		public void Write(Context context,BinaryWriter bw)
		{
			bw.Write((int)ClassId);
			bw.Write(id);
			bw.Write(name);
			if(context.Lib.HasDb)
			{
				dataTable.Clear();
			  string query=string.Format("select * from [{0}]",this.Name);
				DbDataAdapter dataAdapter=context.Conn.CreateDataAdapter(query);
				dataAdapter.Fill(dataTable);
			}
			DataTableSerializer.Write(bw,dataTable);
		}
		#endregion

		#region Misc
		public void Remove(Context context)
		{
			if(context!=null)
			{
				GmCommand cmd=context.Conn.CreateCommandById("deleteFromGisCustomTablesWhereId");
				cmd.AddInt("Id",id);
				cmd.ExecuteNonQuery();
			}
			Clear();
			lib.SetChanged();
		}
		public void Clear()
		{
			dataTable.Clear();
		}
		public ArrayList GetRecords(int gisObjectId)
		{
			ArrayList res=new ArrayList();
			DataColumn dc=dataTable.Columns["GisObjectId"];
			if(dc==null) return res;
			foreach(DataRow dr in dataTable.Rows)
			{
				int id=(int)dr[dc];
				if(id==gisObjectId) res.Add(dr);
			}
			return res;
		}
		public override string ToString()
		{
			return Name;
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			CustomDataTable ct= obj as CustomDataTable;
			if(ct==null) return -1;
			return Name.CompareTo(ct.Name);
		}

		#endregion
	}*/
}
