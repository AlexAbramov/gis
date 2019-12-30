using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Data;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class NamedColor: ISerializableObject
	{
		GLib lib;
		int id;
		string name;
		Color color;
		BitArray32 updateAttr=0;

		#region Access
		public int Id{get{return id;}}
		public GeoLib.ClassId ClassId{get{return ClassId.Color;}}
		public string Name{get{return name;}set{if(value==null)value=""; if(name==value)return; name=value; UpdateAttr(ColorField.Name);}}
		public Color Color{get{return color;}set{if(color==value)return; color=value; UpdateAttr(ColorField.Val);}}
		void UpdateAttr(ColorField f){updateAttr[(int)f]=true;lib.SetChanged();}
		public bool GetCommonAttr(CommonAttr a) { return false; }
		//		public bool IsUpdated(ColorField f) { return updateAttr[(int)f]; }
		#endregion

		#region Construction
		public NamedColor(GLib lib,string name,Color color)
		{
			this.lib=lib;
			id=lib.GenerateId(this,ref updateAttr);
			this.name=name;
			this.color=color;
		}
		internal NamedColor(IDataReader dr)
		{
			int i=0;
			id=dr.GetInt32(i++);
			name=dr.GetString(i++);
			color=Color.FromArgb(dr.GetInt32(i++));
		}
		internal NamedColor(BinaryReader br)
		{
			id=br.ReadInt32();
//			lib.UpdateGen(this);
			name=br.ReadString();
			color=Color.FromArgb(br.ReadInt32());
		}
		#endregion

		#region Serialization
		public void Save(Context context)
		{
			if(context.ExportMode || updateAttr[Constants.updateAttrCreated])
			{
				GmCommand cmd=context.TargetConn.CreateCommandById("insertIntoGisColors");
				cmd.AddInt("Id",id);
				cmd.AddString("Name",name,MaxLength.Name);
				cmd.AddInt("Val",color.ToArgb());
				cmd.ExecuteNonQuery();
			}
			else
			{
				if(updateAttr.IsEmpty) return;
				GmCommand cmd=context.Conn.CreateCommand();
				string cmdText="";
				cmd.AddInt("Id",id);
				if(updateAttr[(int)ColorField.Name])
				{
					cmdText+="Name=@Name,";
					cmd.AddString(Name,name,MaxLength.Name);
				}
				if(updateAttr[(int)ColorField.Val])
				{
					cmdText+="Val=@Val,";
					cmd.AddInt("Val",color.ToArgb());
				}
				Geomethod.StringUtils.RemoveLastChar(ref cmdText);
				cmd.CommandText="update gisColors set "+cmdText+" where Id=@Id";
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
			bw.Write(color.ToArgb());
		}
        public void Remove(Context context)
        {
            GmCommand cmd = context.Conn.CreateCommandById("deleteFromGisColorsWhereId");
            cmd.AddInt("Id", id);
            cmd.ExecuteNonQuery();
        }
		#endregion

    }
}
