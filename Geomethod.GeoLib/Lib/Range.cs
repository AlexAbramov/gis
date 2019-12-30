using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class GRange: IShapedObject
	{
		int id;
		GType type;
		Rect bounds=Rect.Null;
		List<GObject> objects;
		BitArray32 updateAttr=0;

		#region Access
		public GType Type{get{return type;}}
		public GLib Lib{get{return type.Lib;}}
		public bool Loaded{get{return objects!=null;}}
		public ICollection<GObject> Objects{get{return objects;}}
        internal void Add(GObject obj) { if (objects == null) objects = new List<GObject>(); objects.Add(obj);}
		internal bool NotSaved{get{return updateAttr[Constants.updateAttrCreated];}}
		#endregion

		#region Construction		
		internal GRange(GType type,Rect bounds)
		{
			id=type.Lib.GenerateId(this,ref updateAttr);
			this.type=type;
			this.bounds=bounds;
			type.Add(this);
			Lib.Register(this);
		}
		internal GRange(Context context,GType type,IDataReader dr)
		{
			id=dr.GetInt32((int)RangeField.Id);
			bounds=context.Buf.GetRect(dr,(int)RangeField.Code);
			this.type=type;
			type.Add(this);
			Lib.Register(this);
		}
		internal GRange(Context context,IDataReader dr)
		{
			id=dr.GetInt32((int)RangeField.Id);
			int typeId=dr.GetInt32((int)RangeField.TypeId);
			bounds=context.Buf.GetRect(dr,(int)RangeField.Code);
			type=context.Lib.GetType(typeId);
			type.Add(this);
			Lib.Register(this);
		}
		internal GRange(Context context,BinaryReader br)
		{
			id=br.ReadInt32();
//			context.Lib.UpdateGen(this);
			int typeId=br.ReadInt32();
			bounds=context.Buf.ReadRect(br);
			type=context.Lib.GetType(typeId);
			type.Add(this);
			Lib.Register(this);
		}
		#endregion

		#region Serialization
		public void Save(Context context)
		{
			if(context.ExportMode || updateAttr[Constants.updateAttrCreated])
			{
				GmCommand cmd=context.TargetConn.CreateCommandById("insertIntoGisRanges");
				cmd.AddInt("Id",id);
				cmd.AddInt("TypeId",type.Id);
				context.Buf.SetRect(cmd.AddBinary("Code"),bounds);
				cmd.ExecuteNonQuery();
				if(!context.ExportMode) updateAttr=0;
			}
			if(context.Filter==null) return;
			if(!context.Filter.Includes(BatchLevel.Object)) return;
			if(objects!=null) foreach(GObject obj in objects) obj.Save(context);
		}
		public void Write(Context context,BinaryWriter bw)
		{
			bw.Write((int)ClassId);
			bw.Write(id);
			bw.Write(Type.Id);
			context.Buf.WriteRect(bw,bounds);
			if(context.Filter==null) return;
			if(!context.Filter.Includes(BatchLevel.Object)) return;
			if(objects!=null) foreach(GObject obj in objects) obj.Write(context,bw);
		}
		#endregion 

		#region Load
		public void Load(Context context)
		{        
			if(objects!=null) return;
            lock (this)
            {
                if (objects != null) return;
                GmCommand cmd = context.Conn.CreateCommandById("selectCountFromGisObjectsWhereRangeId");
                cmd.AddInt("RangeId", id);
                int count = (int)cmd.ExecuteScalar();
                objects = new List<GObject>(count);
                if (count > 0)
                {
                    cmd = context.Conn.CreateCommandById("selectAllFromGisObjectsWhereRangeId");
                    cmd.AddInt("RangeId", id);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            GObject.Create(context, this, dr, true);
                        }
                    }
                }
            }
		}
		public void Unload()
		{
			if(objects!=null)
			{
                lock (this)
                {
                    if (objects != null)
                    {
                        objects.Clear();
                        objects = null;
                        Lib.SetStateAttr(LibStateAttr.AllObjectsLoaded, false);
                    }
                }
			}
		}
		#endregion

		#region Remove
		internal void Remove(GObject obj){if(objects!=null) objects.Remove(obj);}
		internal void Remove(Context context, bool updateType)
		{
			if(context!=null && objects==null)
			{
				Load(context);
			}
			foreach(GObject obj in objects) obj.Remove(context,false);
			if(context!=null && id!=0)
			{
				GmCommand cmd=context.Conn.CreateCommandById("deleteFromGisRangesWhereId");
				cmd.AddInt("Id",id);
				cmd.ExecuteNonQuery();
			}
			if(updateType) type.Remove(this);
			else Lib.Unregister(this);
			Unload();
		}
		public void Remove(){Remove(null,true);}
		public void Remove(Context context){Remove(context,true);}
		#endregion

		#region IShapedObject
		public bool GetCommonAttr(CommonAttr a) 
		{
			switch (a)
			{
				case CommonAttr.ReadOnly: return true;
				default: return false;
			}
		}
		public ClassId ClassId { get { return ClassId.Range; } }
		public int Id{get{return id;}}
		public Rect Bounds{get{return bounds;}}
		public IShapedObject Parent{get{return type;}}
		public bool IsVisibleOnMap(Map map)
		{
			return map.Intersects(bounds);
		}
		public void Draw(Map map)
		{
			if(!map.Intersects(bounds)) return;
			if(!map.Includes(BatchLevel.Object)) return;
			if(objects==null && map.Context!=null && Lib.HasDb)
			{
				Load(map.Context);
			}
			if(objects!=null) foreach(GObject obj in objects) obj.Draw(map);
		}
		#endregion

		#region Utils
		public bool Intersects(Rect rect){return rect.Intersects(bounds);}
		public bool Contains(Point p){return bounds.Contains(p);}
		internal void SetLoaded(){if(objects==null) objects=new List<GObject>();}
		public override bool Equals(object obj){return obj is Rect ? bounds==(Rect)obj : false;}
		public override int GetHashCode(){return bounds.GetHashCode();}
		public GObject GetObject(int objectId)
		{
			if(objects==null) return null;
			foreach(GObject obj in objects) if(obj.Id==objectId) return obj;
			return null;
		}
		public void DrawSelected(Map map)
		{
			Rect bounds=Bounds;
			if(!map.Intersects(bounds)) return;
			map.DrawPolygon(Lib.Config.styles.selStyle,bounds.Points);
		}
		public void Visit(IVisitor vis)
		{
			if(vis.Visit(this))
			{
				if(objects!=null) foreach(GObject obj in objects) vis.Visit(obj);
			}
		}
		#endregion

	}
}
