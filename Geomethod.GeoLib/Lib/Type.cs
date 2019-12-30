using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class GType: GComposite, IComparable
	{
		int parentId=0;
		int priority=0;
		GeomType geomType;

		GLib lib;
		GType parent;
		List<GRange> ranges;
        object getRangeStrongLock = new object();

		#region Access
		public int ParentId{get{return parentId;}}
		public int Priority{get{return priority;}set{priority=value;UpdateAttr((int)TypeField.Priority);}}
		public GeomType GeomType{get{return geomType;}}
		public bool ReadOnly { get { return GetCommonAttr(CommonAttr.ReadOnly); } set{SetCommonAttr(CommonAttr.ReadOnly,value); UpdateAttr((int)TypeField.Attr); } }
		protected override int NameFieldId{get{return (int)TypeField.Name;}}
		protected override int StyleFieldId{get{return (int)TypeField.Style;}}
		protected override int SMinFieldId{get{return (int)TypeField.SMin;}}
		protected override int SMaxFieldId{get{return (int)TypeField.SMax;}}
		public bool IsUpdated(TypeField f) { return updateAttr[(int)f]; }
		public override GComposite ParentComposite { get { return parent == null ? (GComposite)lib : (GComposite)parent; } }
		public override GLib Lib{get{return lib;}}	
		#endregion

		#region Construction
		public GType(GLib lib,GeomType geomType)
		{
			this.lib=lib;
			id=lib.GenerateId(this,ref updateAttr);
			priority=lib.LastPriority;
			this.geomType=geomType;	
			lib.Add(this);
			lib.Register(this);
		}
		public GType(GType parent)
		{
			lib=parent.Lib;
			id=lib.GenerateId(this,ref updateAttr);
			parentId=parent.Id;
			priority=parent.LastPriority;
			this.geomType=parent.geomType;
			this.parent=parent;
			parent.Add(this);
			lib.Register(this);
		}
		internal GType(Context context,IDataReader dr)
		{
			this.lib=context.Lib;
			id=dr.GetInt32((int)TypeField.Id);
			parentId=dr.GetInt32((int)TypeField.ParentId);
			priority=dr.GetInt32((int)TypeField.Priority);
			attr=dr.GetInt32((int)TypeField.Attr);
			name=dr.GetString((int)TypeField.Name);
//      DZ      16.01.09
//			context.SetStyle(dr.GetString(((int)TypeField.Style)),ref styleStr,ref style);
            context.SetStyle( 
                dr.IsDBNull( (int)TypeField.Style ) ? "" : dr.GetString( (int)TypeField.Style ), ref styleStr, ref style );
			geomType = (GeomType)dr.GetInt32((int)TypeField.GeomType);
			smin=dr.GetInt32((int)TypeField.SMin);
			smax=dr.GetInt32((int)TypeField.SMax);
			lib.Register(this);
		}
		internal GType(Context context,BinaryReader br)
		{
			this.lib=context.Lib;
			id=br.ReadInt32();
//			lib.UpdateGen(this);
			parentId=br.ReadInt32();
			priority=br.ReadInt32();
			attr=br.ReadInt32();
			name=br.ReadString();
			context.SetStyle(br.ReadString(),ref styleStr,ref style);
			geomType=(GeomType)br.ReadInt32();
			smin=br.ReadInt32();
			smax=br.ReadInt32();
			if(parentId!=0)	parent=lib.GetType(parentId);
			this.ParentComposite.Add(this);     
			lib.Register(this);
		}
		#endregion

		#region Serialization
		public void Save(Context context)
		{
			if(context.ExportMode || updateAttr[Constants.updateAttrCreated])
			{
				GmCommand cmd=context.TargetConn.CreateCommandById("insertIntoGisTypes");
				cmd.AddInt("Id",id);
				cmd.AddInt("ParentId",parentId);
				cmd.AddInt("Priority",priority);
				cmd.AddInt("Attr",attr);
				cmd.AddString("Name",name,MaxLength.Name);
//      14.01.09
//                if( styleStr == "" )
//                    cmd.AddString( "Style", /*styleStr*/ " ", MaxLength.Style );
//                else
				    cmd.AddString("Style",styleStr,MaxLength.Style);
				cmd.AddInt("GeomType",(int)geomType);
				cmd.AddInt("SMin",smin);
				cmd.AddInt("SMax",smax);
				cmd.ExecuteNonQuery();
			}
			else if(updateAttr.NonEmpty)
			{
				GmCommand cmd=context.Conn.CreateCommand();
				string cmdText="";
				if(updateAttr[(int)TypeField.ParentId])
				{
					cmdText+="ParentId= @ParentId,";
					cmd.AddInt("ParentId",parentId);
				}
				if(updateAttr[(int)TypeField.Priority])
				{
					cmdText+="Priority= @Priority,";
					cmd.AddInt("Priority",priority);
				}
				if(updateAttr[(int)TypeField.Attr])
				{
					cmdText+="Attr= @Attr,";
					cmd.AddInt("Attr",attr);
				}
				if(updateAttr[(int)TypeField.Name])
				{
					cmdText+="Name= @Name,";
					cmd.AddString("Name",name,MaxLength.Name);
				}
				if(updateAttr[(int)TypeField.Style])
				{
					cmdText+="Style= @Style,";
					cmd.AddString("Style",styleStr,MaxLength.Style);
				}
				if(updateAttr[(int)TypeField.GeomType])
				{
					cmdText+="GeomType= @GeomType,";
					cmd.AddInt("GeomType",(int)geomType);
				}
				if(updateAttr[(int)TypeField.SMin])
				{
					cmdText+="SMin=@SMin,";
					cmd.AddInt("SMin",smin);
				}
				if(updateAttr[(int)TypeField.SMax])
				{
					cmdText+="SMax= @SMax,";
					cmd.AddInt("SMax",smax);
				}
				Geomethod.StringUtils.RemoveLastChar(ref cmdText);
				cmd.CommandText="update gisTypes set "+cmdText+" where Id= @Id";
				cmd.AddInt("Id",id);
				cmd.ExecuteNonQuery();
			}
			if(!context.ExportMode) updateAttr=0;
			if(context.Filter==null) return;
			if(types!=null) foreach(GType type in types) type.Save(context);
			if(!context.Filter.Includes(BatchLevel.Range)) return;
			if(ranges!=null) foreach(GRange range in ranges) range.Save(context);
		}
		public void Write(Context context,BinaryWriter bw)
		{
			bw.Write((int)ClassId);
			bw.Write(id);
			if(parentId==0 && parent!=null) parentId=parent.Id;
			bw.Write(parentId);
			bw.Write(priority);
			bw.Write(attr);
			bw.Write(name);
			bw.Write(styleStr);
			bw.Write((int)geomType);
			bw.Write(smin);
			bw.Write(smax);

			if(context.Filter!=null)
			{
				if(types!=null) foreach(GType type in types) type.Write(context,bw);
				if(!context.Filter.Includes(BatchLevel.Range)) return;
				if(ranges!=null) foreach(GRange range in ranges) range.Write(context,bw);
			}
		}
		#endregion

		#region Load
		public void Load(Context context)
		{
			if(ranges!=null) throw new Exception("Ranges already loaded.");
			GmCommand cmd=context.Conn.CreateCommandById("selectCountFromGisRangesWhereTypeId");
			cmd.AddInt("TypeId",id);
			int count=(int)cmd.ExecuteScalar();
			ranges=new List<GRange>(count);
			if(count>0)
			{
				cmd=context.Conn.CreateCommandById("selectAllFromGisRangesWhereTypeId");
				cmd.AddInt("TypeId",id);
				using(IDataReader dr=cmd.ExecuteReader())
				{
					while(dr.Read())
					{
						GRange range=new GRange(context,this,dr);
					}
				}
			}
		}
		public void Unload()
		{
			if(types!=null)
			{
				foreach(GType type in types) type.Unload();
			}
			if(ranges!=null)
			{
				foreach(GRange range in ranges) range.Unload();
			}
		}
		#endregion

		#region Remove
		internal void Remove(GRange range){if(ranges!=null){ranges.Remove(range); Lib.Unregister(range);}}
		internal void Clear()
		{
			if(types!=null)
			{
				foreach(GType type in types) type.Clear();
				types.Clear();
				types=null;
			}
			if(ranges!=null)
			{
				foreach(GRange range in ranges) range.Unload();
				ranges.Clear();
				ranges=null;
			}
		}
		internal void Remove(Context context,bool updateParentType)
		{
			if(types!=null)
			{
				foreach(GType type in types) type.Remove(context,false);
				types.Clear();
				types=null;
			}
			if(ranges!=null)
			{
				foreach(GRange range in ranges) range.Remove(context,false);
				ranges.Clear();
				ranges=null;
			}
			if(context!=null && id!=0)
			{
				GmCommand cmd=context.Conn.CreateCommandById("deleteFromGisTypesWhereId");
				cmd.AddInt("Id",id);
				cmd.ExecuteNonQuery();
			}
			if(updateParentType) ParentComposite.Remove(this);
			else lib.Unregister(this);
		}
		public void Remove(){Remove(null,true);}
		public void Remove(Context context){Remove(context,true);}
		#endregion

		#region IShapedObject
		public override ClassId ClassId{get{return ClassId.Type;}}
		public override void Draw(Map map)
		{
			if(!Contains(map.Scale)) return;
			if(types!=null) foreach(GType type in types) type.Draw(map);
			if(!map.Includes(Id)) return;
//			if(ranges==null && map.Context!=null) Load(map.Context);
			if(!map.Intersects(bounds)) return;
			if(!map.Includes(BatchLevel.Range)) return;
			if(ranges!=null) foreach(GRange range in ranges) range.Draw(map);
		}
		#endregion

		#region Utils
		internal void SetParent(GType parent)
		{
            this.parent=parent;
			ParentComposite.Add(this);
		}
		public void Move(GComposite comp)
		{
			if(comp is GType && ((GType)comp).GeomType!=this.geomType) throw new Exception("GeomType mismatch.");
			int parentId0=parentId;
			int priority0=priority;
			ParentComposite.Remove(this);
			parent=comp is GLib ? null : (GType)comp;
			parentId=parent!=null ? parent.Id : 0;
			priority=ParentComposite.LastPriority;
			ParentComposite.Add(this);
			if(parentId!=parentId0) UpdateAttr((int)TypeField.ParentId);
			if(priority!=priority0) UpdateAttr((int)TypeField.Priority);
			lib.SetChanged();
		}
		internal void Add(GRange range)
		{
			if(ranges==null) ranges=new List<GRange>();
			ranges.Add(range);
			UpdateBounds(range.Bounds);
		}
		internal GRange GetRangeStrong(GObject obj)
		{
            lock (getRangeStrongLock)
            {
			    Rect rangeBounds=lib.Indexer.GetIndex(obj.Bounds);
                if (ranges == null) ranges = new List<GRange>();
                else
                {
                    foreach (GRange range in ranges)
                        if (range.Equals(rangeBounds)) return range;
                }
                GRange rng = new GRange(this, rangeBounds);
                return rng;
            }
		}
		public Style TargetStyle
		{
			get
			{
				if(style!=null) return style;
				if(parent!=null) return parent.TargetStyle;
                return null;
//				return Lib.DefaultStyle;
			}
		}
		public bool BelongToBranch(int typeId)
		{
			if(id==typeId) return true;
			return parent==null ? false : parent.BelongToBranch(typeId);
		}
		public ICollection<GRange> Ranges{get{return ranges;}}
		internal void UpdateBounds(Rect rect)
		{
			if(!bounds.Update(rect)) return;
			if(parent!=null) parent.UpdateBounds(rect);
		}

		#endregion

		#region Iterations
		public override void Visit(IVisitor vis)
		{
			if(types!=null) foreach(GType type in types) type.Visit(vis);
			if(vis.Visit(this))
			{
				if(ranges!=null) foreach(GRange range in ranges) range.Visit(vis);
			}
		}
		#endregion

		#region IComparable
		public int CompareTo(object obj){return obj is GType ? priority-((GType)obj).Priority : -1;}
		#endregion
	}
}