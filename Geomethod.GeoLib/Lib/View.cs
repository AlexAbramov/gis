using System;
using System.IO;
using System.Drawing;
using System.Data;
using System.Collections;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	/// <summary>
	/// Summary description for View.
	/// </summary>
	public class View : IComparable<View>, INamed, ISerializableObject
	{
		GLib lib;
		const int intCodeCount=5;

		int id=0;
		BitArray32 attr=0;
		string name="";
		Point pos=Point.Empty;
		int scale=0;
		float angle=0;
		int layerId=0;
		BitArray32 updateAttr=0;

		#region Properties
		public int Id{get{return id;}}
		public bool IsOverall{get{return GetAttr(ViewAttr.Overall);}}
		public string Name{get{return name;}set{name=value; UpdateAttr(ViewField.Name);}}
		public Point Pos{get{return pos;}}
		public int Scale{get{return scale;}}
		public float Angle{get{return angle;}}
		public int LayerId{get{return layerId;}}
		protected void UpdateAttr(ViewField f){updateAttr[(int)f]=true;lib.SetChanged();}
		void SetAttr(ViewAttr a) { attr[(int)a] = true; }
		bool GetAttr(ViewAttr a) { return attr[(int)a]; }
		#endregion

		#region Construction
		public static View CreateOverall(GLib lib, string name){return new View(lib,name);}
		View(GLib lib, string name)
		{
			id = 0;
			this.name = name;
			SetAttr(ViewAttr.Overall);
			InitFromLib();
//			name=Constants.DefaultViewName;
		}

		public void InitFromLib()
		{
			pos=lib.Bounds.Center;
			scale = lib.SMax;
			angle = 0;
			layerId = 0;
		}

		public View(string name, Map map)
		{
			lib=map.Lib;
			id=lib.GenerateId(this, ref updateAttr);
			this.name=name;
			Init(map);
		}
	
		internal View(Context context, IDataReader dr)
		{
			lib=context.Lib;
			id=dr.GetInt32((int)ViewField.Id);
			attr=dr.GetInt32((int)ViewField.Attr);
			name=dr.GetString((int)ViewField.Name);
			Init(context.Buf.GetIntArray(dr,(int)ViewField.Code));
		}

		internal View(Context context, BinaryReader br)
		{
			lib=context.Lib;
			id=br.ReadInt32();
//			context.Lib.UpdateGen(this);
	        attr=br.ReadInt32();
			name=br.ReadString();
			Init(context.Buf.ReadIntArray(br));
		}
		#endregion

		void Init(int[] intArray)
		{
			int i=0;
			pos.X=intArray[i++];
			pos.Y=intArray[i++];
			scale=intArray[i++];
			angle=Geomethod.BufferUtils.IntToFloat(intArray[i++]);
			layerId=intArray[i++];
		}

		int[] GetIntArray()
		{
			int[] intArray=new int[intCodeCount];
			int i=0;
			intArray[i++]=pos.X;
			intArray[i++]=pos.Y;
			intArray[i++]=scale;
			intArray[i++] = Geomethod.BufferUtils.FloatToInt(angle);
			intArray[i++]=layerId;
			return intArray;
		}

		public void Init(Map map)
		{
			pos=map.Pos;
			scale=map.Scale;
			angle=map.Angle;
			updateAttr[(int)ViewField.Code]=true;
			lib.SetChanged();
		}
		
		public void Remove(Context context)
		{
			if(this.IsOverall) return;
			GmCommand cmd=context.Conn.CreateCommandById("deleteFromGisViewsWhereId");
			cmd.AddInt("Id",id);
			cmd.ExecuteNonQuery();
		}

		#region Serialization
		public void Save(Context context)
		{
			if(this.IsOverall) return;
			if(context.ExportMode || updateAttr[Constants.updateAttrCreated])
			{
				GmCommand cmd=context.TargetConn.CreateCommandById("insertIntoGisViews");
				cmd.AddInt("Id",id);
				cmd.AddInt("Attr",attr);
				cmd.AddString("Name",name,MaxLength.Name);
				context.Buf.SetIntArray(cmd.AddBinary("Code"),GetIntArray());
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
					context.Buf.SetIntArray(cmd.AddBinary("Code"),this.GetIntArray());
				}
				Geomethod.StringUtils.RemoveLastChar(ref cmdText);
				cmd.CommandText="update gisViews set "+cmdText+" where Id= @Id";
				cmd.AddInt("Id",id);
				cmd.ExecuteNonQuery();
			}
			if(!context.ExportMode) updateAttr=0;
		}

		public void Write(Context context, BinaryWriter bw)
		{
			if(this.IsOverall) return;
			bw.Write((int)ClassId);
			bw.Write(id);
			bw.Write(attr);
			bw.Write(name);
			context.Buf.WriteIntArray(bw,GetIntArray());
		}
		#endregion

		#region ISerializable Members

		public ClassId ClassId{get{return ClassId.View;}}
		public bool GetCommonAttr(CommonAttr a) { return attr[(int)a]; }

		#endregion

		#region IComparable<View> Members

		public int CompareTo(object obj)
		{
			return CompareTo(obj as View);
		}

		public int CompareTo(View view)
		{
			if (view == null) return -1;
			if (IsOverall) return -1;
			return name.CompareTo(view.Name);
		}

		#endregion
	}
}
