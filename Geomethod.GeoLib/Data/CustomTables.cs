using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	/// <summary>
	/// Summary description for CustomTables.
	/// </summary>
	public class CustomTables: ISerializableObject
	{
		GLib lib;
		GmDataSet dataSet;

		Dictionary<int, List<int>> typeBinding = new Dictionary<int, List<int>>();//typeId,array of tableIds

		#region Properties
		public GmDataSet DataSet { get { return dataSet; } }
		public IEnumerable<GmDataTable> GetDataTables { get { return dataSet.GetDataTables(); } }
		#endregion

		#region Construction
		public CustomTables(GLib lib)
		{
			this.lib=lib;
			dataSet = new GmDataSet();
		}
		#endregion

		#region Serialization
		public void Load(Context context)
		{
/*!!!			using(IDataReader dr=context.Conn.ExecuteReaderById("selectAllFromGisCustomTables"))
			{
				while(dr.Read()) Add(new CustomDataTable(lib,dr));
			}
			using(IDataReader dr=context.Conn.ExecuteReaderById("selectAllFromGisTypeBinding"))
			{
				while(dr.Read())
				{
					int i=0;
					int typeId=dr.GetInt32(i++);
					int tableId=dr.GetInt32(i++);
					Bind(typeId,tableId);
				}
			}*/
		}
		public void Save(Context context)
		{
//!!!			if(tables.Count>0) foreach(CustomDataTable ct in Tables) ct.Save(context);
		}
		public void Write(Context context, BinaryWriter bw)
		{
/*!!!			bw.Write((int)ClassId.CustomTables);
			dataSet.WriteXmlSchema(bw.BaseStream);
			ArrayList pairs=new ArrayList(typeBinding.Count*10);
			lock(typeBinding)
			{
				foreach(int typeId in typeBinding.Keys)
				{
					List<int> tableIds=typeBinding[typeId];
					foreach(int tableId in tableIds)
					{
						pairs.Add(typeId);
						pairs.Add(tableId);
					}
				}
			}			
			bw.Write(pairs.Count/2);
			foreach(int i in pairs) bw.Write(i);
			if(tables.Count>0) foreach(CustomDataTable ct in Tables) ct.Write(context,bw);*/
		}
		public void Read(Context context,BinaryReader br)
		{
/*!!!			dataSet.ReadXmlSchema(br.BaseStream);
			int count = br.ReadInt32();
			for(int i=0;i<count;i++)
			{
				int typeId=br.ReadInt32();
				int tableId=br.ReadInt32();
				Bind(typeId,tableId);
			}*/
		}
		#endregion

		#region Tables
/*		public void Add(CustomDataTable ct)
		{
			tables.Add(ct.Id,ct);
		}
		public void Remove(Context context,CustomDataTable ct)
		{
			tables.Remove(ct.Id);
			ct.Remove(context);
		}*/
		public void Clear()
		{
			dataSet.Clear();
		}
		#endregion

		public List<int> GetBinding(int typeId)
		{
			return typeBinding.ContainsKey(typeId)?typeBinding[typeId]:new List<int>();
		}

		#region Binding
		public void Bind(int typeId, int tableId)
		{
			List<int> ar = GetBinding(typeId);
			if (!ar.Contains(tableId)) ar.Add(tableId);
//			if(obj==null) typeBinding[typeId]=ar;
		}
/*!!!		public ArrayList GetBoundTables(int typeId)
		{
			List<CustomDataTable> res=new List<CustomDataTable>();
			ArrayList ar=typeBinding[typeId] as ArrayList;
			if(ar==null) ar=new ArrayList();
			foreach(int tableId in ar)
			{
				CustomDataTable ct=tables[tableId] as CustomDataTable;
				if(ct!=null)
				{
					res.Add(ct);
				}
			}
			return res;
		}*/
		#endregion

		#region ISerializable Members

		public bool GetCommonAttr(CommonAttr a) { return false; }
		public GeoLib.ClassId ClassId
		{
			get
			{
				// TODO:  Add CustomTables.Command getter implementation
				return ClassId.CustomTables;
			}
		}

		public int Id
		{
			get
			{
				return 0;
			}
		}

		#endregion
	}
}
