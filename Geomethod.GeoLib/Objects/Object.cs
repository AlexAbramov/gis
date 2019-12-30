using System;
using System.Data;
using System.Drawing;
using System.IO;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public abstract class GObject: IShapedObject
	{
		int id;
		protected GRange range=null;
		BitArray32 attr=0;
		protected BitArray32 updateAttr=0;
		string name="";
		protected string caption="";
		string styleStr="";
		string textAttr="";
		Style style=null;

		#region Access
		public bool ReadOnly{get{return GetCommonAttr(CommonAttr.ReadOnly);}set{SetCommonAttr(CommonAttr.ReadOnly,value);UpdateAttr(ObjectField.Attr);}}
		public Style Style{get{return style;}}
		public Style TargetStyle{get{return style==null ? range.Type.TargetStyle : style;}}
		public bool Updated{get{return updateAttr!=0;}}
		public int RangeId{get{return range.Id;}}
		public GRange Range{get{return range;}}
		public int TypeId{get{return range.Type.Id;}}
		public string TextAttr{get{return textAttr;}}
		public GType Type{get{return range.Type;}}
		public GLib Lib{get{return range.Lib;}}
		public string Name
		{
			get{return name;}
			set{if(value==null) value="";	if(name==value) return;	name=value;	UpdateAttr(ObjectField.Name);}
		}
		public string Path{get{return this.Type.Path+'/'+name;}}
		public string Caption
		{
			get{return caption;}
			set{if(value==null) value="";	if(caption==value) return; caption=value;	UpdateAttr(ObjectField.Caption);}
		}
		public string StyleStr
		{
			get{return styleStr;}
			set
			{
				if(Lib.GetContext().SetStyle(value,ref styleStr,ref style))
				{
					UpdateAttr(ObjectField.Style);
				}
			}
		}
		protected void UpdateAttr(ObjectField f){updateAttr[(int)f]=true;Lib.SetChanged();}
		#endregion
	
		#region Construction		
		protected GObject(GType type)
		{
			id=type.Lib.GenerateId(this,ref updateAttr);
			if(type.GeomType!=GeomType) throw new Exception("GeomType mismatch.");
		}
		protected GObject(){}
		internal static GObject Create(Context context,BinaryReader br)
		{
			GRange range=context.Lib.GetRange(br.ReadInt32());
			GObject gobj=Create(range);
			gobj.Read(context,br);
			return gobj;
		}
		internal static GObject Create(Context context,GRange range,IDataReader dr,bool updateRange)
		{
			if(range==null)
			{
				int rangeId=dr.GetInt32((int)ObjectField.RangeId);
				range=context.Lib.GetRange(rangeId);
			}
			GObject gobj=Create(range);
			gobj.Init(context,dr);
			if(updateRange) range.Add(gobj);
			return gobj;
		}
		static GObject Create(GRange range)
		{
			GObject gobj;
			switch(range.Type.GeomType)
			{
				case GeomType.Point: gobj=new GPoint(); break;
				case GeomType.Polyline: gobj=new GPolyline(); break;
				case GeomType.Polygon: gobj=new GPolygon(); break;
				case GeomType.Caption: gobj=new GCaption(); break;
				default: throw new Exception("Unknown GeomType: "+range.Type.GeomType);
			}
			gobj.range=range;
//			gobj.AssignId();
			return gobj;
		}
		#endregion

		#region Serialization
		void Init(Context context,IDataReader dr)
		{
			id=dr.GetInt32((int)ObjectField.Id);
			attr=dr.GetInt32((int)ObjectField.Attr);
//      DZ      16.01.09

//			name= dr.GetString((int)ObjectField.Name);
            name = dr.IsDBNull( (int)ObjectField.Name ) ? "" : dr.GetString( (int)ObjectField.Name );
//			caption=dr.GetString((int)ObjectField.Caption);
            caption = dr.IsDBNull( (int)ObjectField.Caption ) ? "" : dr.GetString( (int)ObjectField.Caption );
//			context.SetStyle(dr.GetString((int)ObjectField.Style),ref styleStr,ref style);
            context.SetStyle( 
                dr.IsDBNull( (int)ObjectField.Style ) ? "" : dr.GetString( (int)ObjectField.Style ), ref styleStr, ref style );
//			textAttr=dr.GetString((int)ObjectField.TextAttr);
            textAttr = dr.IsDBNull( (int)ObjectField.TextAttr ) ? "" : dr.GetString( (int)ObjectField.TextAttr );

//			this.range=range;
			GetCode(context,dr);
		}
		public void Save(Context context)
		{
			if(!context.ExportMode && range.NotSaved) range.Save(context);
			if(context.ExportMode || updateAttr[Constants.updateAttrCreated])
			{
				GmCommand cmd=context.TargetConn.CreateCommandById("insertIntoGisObjects");
				cmd.AddInt("Id",id);
				cmd.AddInt("TypeId",range.Type.Id);
				cmd.AddInt("RangeId",range.Id);
				cmd.AddInt("Attr",attr);
				cmd.AddString("Name",name,MaxLength.Name);
				cmd.AddString("Caption",caption,MaxLength.Caption);
				cmd.AddString("Style",styleStr,MaxLength.Style);
				cmd.AddString("TextAttr",textAttr,MaxLength.TextAttr);
				SetCode(context,cmd.AddBinary("Code"));
				cmd.ExecuteNonQuery();
			}
			else if(updateAttr.NonEmpty)
			{
				GmCommand cmd=context.Conn.CreateCommand();
				string cmdText="";
				if(updateAttr[(int)ObjectField.TypeId])
				{
					cmdText+="TypeId= @TypeId,";
					cmd.AddInt("TypeId",range.Type.Id);
				}
				if(updateAttr[(int)ObjectField.RangeId])
				{
					cmdText+="RangeId= @RangeId,";
					cmd.AddInt("RangeId",range.Id);
				}
				if(updateAttr[(int)ObjectField.Attr])
				{
					cmdText+="Attr= @Attr,";
					cmd.AddInt("Attr",attr);
				}
				if(updateAttr[(int)ObjectField.Name])
				{
					cmdText+="Name= @Name,";
					cmd.AddString("Name",name,MaxLength.Name);
				}
				if(updateAttr[(int)ObjectField.Caption])
				{
					cmdText+="Caption= @Caption,";
					cmd.AddString("Caption",caption,MaxLength.Caption);
				}
				if(updateAttr[(int)ObjectField.Style])
				{
					cmdText+="Style= @Style,";
					cmd.AddString("Style",styleStr,MaxLength.Style);
				}
				if(updateAttr[(int)ObjectField.TextAttr])
				{
					cmdText+="TextAttr= @TextAttr,";
					cmd.AddString("TextAttr",textAttr,MaxLength.TextAttr);
				}
				if(updateAttr[(int)ObjectField.Code])
				{
					cmdText+="Code= @Code,";
					SetCode(context,cmd.AddBinary("Code"));
				}
				Geomethod.StringUtils.RemoveLastChar(ref cmdText);
				cmd.CommandText="update gisObjects set "+cmdText+" where Id= @Id";
				cmd.AddInt("Id",id);
				cmd.ExecuteScalar();
			}
			if(!context.ExportMode) updateAttr=0;
		}
		public void Write(Context context,BinaryWriter bw)
		{
			bw.Write((int)ClassId);
			bw.Write(range.Id);
			bw.Write(id);
			bw.Write(attr);
			bw.Write(name);
			bw.Write(caption);
			bw.Write(styleStr);
			bw.Write(textAttr);
			WriteCode(context,bw);
		}
		void Read(Context context,BinaryReader br)
		{
			id=br.ReadInt32();
//			context.Lib.UpdateGen(this);
			attr=br.ReadInt32();
			name=br.ReadString();
			caption=br.ReadString();
			context.SetStyle(br.ReadString(),ref styleStr,ref style);
			textAttr=br.ReadString();
			ReadCode(context,br);
			range.Add(this);
		}
		#endregion	

		#region Aux
/*		void AssignId()
		{
			id=Lib.GenerateId(this,ref updateAttr);
		}*/
		void CheckRange()
		{
			GRange range=Type.GetRangeStrong(this);
			if(this.range!=range)
			{
				this.range.Remove(this);
				this.range=range;
				range.Add(this);
			}
		}
		protected void CoordsChanged()
		{
			UpdateAttr(ObjectField.Code);
			CheckRange();
		}
		#endregion

		#region IShapedObject
		public ClassId ClassId{get{return ClassId.Object;}}
		public int Id{get{return id;}}
		public IShapedObject Parent{get{return range;}}
		public abstract Rect Bounds{get;}
		public abstract void Draw(Map map);
		public bool IsVisibleOnMap(Map map)
		{
			if(map.Intersects(Bounds)) 
			{
				switch(this.GeomType)
				{
					case GeomType.Polyline: return map.IsVisible(Bounds.MaxSize);
					case GeomType.Polygon: return map.IsVisible(Bounds.MaxSize);
					default: return true;
				}
			}
			return false;
		}
		public bool GetCommonAttr(CommonAttr a) { return attr[(int)a]; }
		protected void SetCommonAttr(CommonAttr a, bool val) { attr[(int)a] = val; UpdateAttr(ObjectField.Attr); }
		#endregion

		#region Override
		public abstract GeomType GeomType{get;}
		public abstract Point Center{get;}
		public abstract bool Intersects(Rect rect);
		protected abstract void GetCode(Context context,IDataReader dr);
		protected abstract void SetCode(Context context,IDbDataParameter par);
		protected abstract void ReadCode(Context context,BinaryReader br);
		protected abstract void WriteCode(Context context,BinaryWriter br);
		public abstract void DrawSelected(Map map);
        public abstract Point[] Points { get;set;}
		#endregion

		#region Remove
		internal void Remove(Context context,bool updateRange)
		{
			if(context!=null)
			{
				GmCommand cmd=context.Conn.CreateCommandById("deleteFromGisObjectsWhereId");
				cmd.AddInt("Id",id);
				cmd.ExecuteNonQuery();
			}
			if(updateRange)
			{
				range.Remove(this);
				Lib.SetChanged();
			}
		}
		public void Remove(){Remove(null,true);}
		public void Remove(Context context){Remove(context,true);}
		#endregion

    }

}
