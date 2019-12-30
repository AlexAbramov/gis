using System;
using System.Drawing;
using System.Collections;

namespace Geomethod.GeoLib
{
	public interface IVisitor
	{
		bool Visit(IShapedObject obj);
	}


	public class VisibleObjectsVisitor: IVisitor
	{
		Map map;
		IList list;
		Hashtable filter;

		public IList List{get{return list;}}

		public VisibleObjectsVisitor(Map map,IList list,Hashtable filter)
		{
			this.map=map;
			this.list=list;
			this.filter=filter;
		}

		#region IVisitor Members

		public bool Visit(IShapedObject obj)
		{
			bool visible=obj.IsVisibleOnMap(map);
			if(visible)
			{				
				if(filter==null || filter.ContainsKey(obj.GetType().Name))
				  list.Add(obj);
			}
			return visible;
		}

		#endregion
	}

	public class PointSearchVisitor: IVisitor
	{
		Point point;
		int dist;
		long distSq;
		int scale;
		ArrayList objects=new ArrayList();
		IFilter filter=null;

		public ArrayList Objects{get{return objects;}}
		public int Scale{get{return scale;}}
		public bool Intersects(Rect rect){return rect.Contains(point);}
		public bool Includes(int id){return filter==null?true:filter.Includes(id);}
		public GObject LastObject{get{return Count==0 ? null : objects[Count-1] as GObject;}}
		public int Count{get{return objects.Count;}}

		public PointSearchVisitor()
		{
		}

		public void Search(GLib lib,Point point,int scale,IFilter filter)
		{
			objects.Clear();
			this.point=point;
			this.scale=scale;
			double d=Constants.cmInMeter*lib.UnitMeasure*scale*lib.Config.geometry.searchRadiusCm;
			dist=(int)d;
			distSq=(long)(d*d);
			objects.Clear();
			lib.Visit(this);
		}

		#region IVisitor Members

		public bool Visit(IShapedObject obj)
		{
			if(obj is GLib) return true;

			if(obj is GComposite)
			{
				if(!Includes(obj.Id)) return false;
				if(!((GComposite)obj).Contains(scale)) return false;
			}

			Rect r=obj.Bounds;
			r.Inflate(dist);
			if(!Intersects(r)) return false;

			if(!(obj is GObject)) return true;

			switch(((GObject)obj).GeomType)
			{
				case GeomType.Polygon:
					if(((GPolygon)obj).Contains(point))
					{
						objects.Add(obj);
					}
					break;
				case GeomType.Polyline:
					if(((GPolyline)obj).DistanceSq(point)<distSq)
					{
						objects.Add(obj);
					}
					break;
				case GeomType.Point:
					if(((GPoint)obj).DistanceSq(point)<distSq)
					{
						objects.Add(obj);
					}
					break;
				case GeomType.Caption:
					if(((GCaption)obj).DistanceSq(point)<distSq)
					{
						objects.Add(obj);
					}
					break;
			}
			return false;
		}

		#endregion
	}


	public class StatVisitor: IVisitor
	{
		Stat stat;

		public Stat Stat{get{return stat;}}

		#region Construction
		public StatVisitor(Stat stat)
		{
			this.stat=stat;
		}

		public StatVisitor()
		{
			this.stat=new Stat();
		}
		#endregion

		#region IVisitor Members

		public bool Visit(IShapedObject obj)
		{
			int batchLevel=(int)stat.batchLevel;
			switch(obj.ClassId)
			{
				case ClassId.Lib:
					GLib lib=(GLib)obj;
					stat.nBgImages+=lib.BgImages.Count;
					stat.nLayers+=lib.Layers.Count;
					stat.nViews+=lib.Views.Count;
					return batchLevel>=(int)BatchLevel.Type;
				case ClassId.Type:
					stat.nTypes++;
					return true;
				case ClassId.Range:
					if(batchLevel<(int)BatchLevel.Range) return false;
					stat.nRanges++;
					return (int)stat.batchLevel>=(int)BatchLevel.Object;
				case ClassId.Object:
					stat.nObjects++;
					break;
			}
			return false;
		}

		#endregion
	}


}
