using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class Layer : IComparable<Layer>, INamed, ISerializableObject
	{
		GLib lib;
		int id;
		BitArray32 attr=0;
		BitArray32 updateAttr=0;
		string name="";
		Dictionary<int, object> types = new Dictionary<int, object>();

		#region Properties
		public string Name
		{
			get{return name;}
			set { if (name != value) { name = value; UpdateAttr(LayerField.Name); } }
		}
		public int Id{get{return id;}}
		public ClassId ClassId{get{return ClassId.Layer;}}
		public bool IsAllInclusive { get { return GetAttr(LayerAttr.AllInclusive); } }
        public bool IsReadOnly { get { return GetCommonAttr(CommonAttr.ReadOnly); } }
        public bool IsVirtual { get { return GetCommonAttr(CommonAttr.Virtual); } }
        void UpdateAttr(LayerField f) { updateAttr[(int)f] = true; if(!IsVirtual) lib.SetChanged(); }
		void SetAttr(LayerAttr a) { attr[(int)a] = true; }
		bool GetAttr(LayerAttr a) { return attr[(int)a]; }
		public bool GetCommonAttr(CommonAttr a) { return attr[(int)a]; }
		int[] TypesArray
		{
			get
			{
				int count=types.Count;
				int[] ar=new int[count];
				if(count>0) types.Keys.CopyTo(ar,0);
				return ar;
			}
		}
		#endregion

		#region Construction
		public Layer(GLib lib)
		{
			this.lib=lib;
			id = 0;
            attr[(int)CommonAttr.Virtual] = true;
			InitFromLib();
		}

		public Layer(GLib lib, string name): this(lib,name,false)
		{
		}

		public Layer(GLib lib, string name, bool isAllInclusive)
		{
			this.lib = lib;
			id = lib.GenerateId(this, ref updateAttr);
			this.name = name;
			if (isAllInclusive) SetAttr(LayerAttr.AllInclusive);
		}

		public Layer(string name, Layer layer)
		{
			lib=layer.lib;
			id=lib.GenerateId(this,ref updateAttr);
			this.name=name;
			Clear();
			Merge(layer);
			updateAttr=0;
		}
		
		public Layer(Context context, IDataReader dr)
		{
			lib=context.Lib;
			id=dr.GetInt32((int)LayerField.Id);
			attr=dr.GetInt32((int)LayerField.Attr);
			name=dr.GetString((int)LayerField.Name);
			int[] typeIds=context.Buf.GetIntArray(dr,(int)LayerField.Code);
			Add(typeIds);
			updateAttr=0;
		}

		public Layer(Context context, BinaryReader br)
		{
			lib=context.Lib;
			id=br.ReadInt32();
			attr=br.ReadInt32();
			name=br.ReadString();
			int[] typeIds=context.Buf.ReadIntArray(br);
			Add(typeIds);
			updateAttr=0;
		}
		#endregion

		#region Methods
		public void InitFromLib()
		{
			Clear();
			Add(lib.Id);
			foreach(GType type in lib.AllTypes) Add(type.Id);
		}
		public void Init(Layer layer)
		{
			Clear();
			Merge(layer);
		}

		public void Add(int typeId)
		{
			if(!Includes(typeId))
			{
				types.Add(typeId,null);
				UpdateAttr(LayerField.Code);
			}
		}
		public void Add(int[] typeIds)
		{
			foreach(int typeId in typeIds) Add(typeId);			
		}
		public bool Includes(int typeId) { return IsAllInclusive ? true : types.ContainsKey(typeId); }
		public void Remove(int typeId)
		{
			if(types.ContainsKey(typeId))
			{
				types.Remove(typeId);
				UpdateAttr(LayerField.Code);
			}
		}
		public void Clear()
		{
			if(types.Count>0)
			{
				types.Clear(); 
				UpdateAttr(LayerField.Code);
			}
		}
		public void Update(int typeId, bool check)
		{
			if(check) Add(typeId);
			else Remove(typeId);
		}
		public void Merge(Layer layer)
		{
			foreach(int typeId in layer.types.Keys) Add(typeId);
		}
		public void Remove(Context context)
		{
			GmCommand cmd = context.Conn.CreateCommandById("deleteFromGisLayersWhereId");
			cmd.AddInt("Id", id);
			cmd.ExecuteNonQuery();
		}
		#endregion

		#region Serialization
		public void Save(Context context)
		{
			if(context.ExportMode || updateAttr[Constants.updateAttrCreated])
			{
				GmCommand cmd = context.TargetConn.CreateCommandById("insertIntoGisLayers");
//				GmCommand cmd = context.TargetConn.CreateCommand();
//				cmd.CommandText = SqlCommands.Get("insertIntoGisLayers");
				cmd.AddInt("Id", id);
				cmd.AddInt("Attr",attr);
				cmd.AddString("Name",name,MaxLength.Name);
				context.Buf.SetIntArray(cmd.AddBinary("Code"),this.TypesArray);
				cmd.ExecuteNonQuery();
			}
			else if(updateAttr.NonEmpty)
			{
				GmCommand cmd=context.Conn.CreateCommand();
				string cmdText="";
				if(updateAttr[(int)ViewField.Attr])
				{
					cmdText+="Attr= @Attr,";
					cmd.AddInt("Attr",attr);
				}
				if(updateAttr[(int)ViewField.Name])
				{
					cmdText+="Name= @Name,";
					cmd.AddString("Name",name,MaxLength.Name);
				}
				if(updateAttr[(int)ViewField.Code])
				{
					cmdText+="Code= @Code,";
					context.Buf.SetIntArray(cmd.AddBinary("Code"),this.TypesArray);
				}
				Geomethod.StringUtils.RemoveLastChar(ref cmdText);
				cmd.CommandText="update gisLayers set "+cmdText+" where Id= @Id";
				cmd.AddInt("Id",id);
				cmd.ExecuteNonQuery();
			}
			if(!context.ExportMode) updateAttr=0;
		}
		public void Write(Context context, BinaryWriter bw)
		{
			bw.Write((int)ClassId);
			bw.Write(id);
			bw.Write(attr);
			bw.Write(name);
			context.Buf.WriteIntArray(bw,this.TypesArray);
		}
		#endregion

		#region IComparable<Layer> Members

		int IComparable<Layer>.CompareTo(Layer layer)
		{
			if (layer == null) return -1;
			if (IsAllInclusive) return -1;
			return name.CompareTo(layer.Name);
		}

		#endregion
	}
}
