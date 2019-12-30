using System;
using System.Drawing;
using System.Collections.Generic;
using Geomethod;

namespace Geomethod.GeoLib.Windows.Forms.Edit
{
	public class EditObject: IShapedObject
	{
        IGeoApp app;
		GLib lib;
		GType type;
		GObject origObject=null;
		int selIndex=-1;
		int highlightIndex=-1;
		List<Point> points = new List<Point>();
		public GType Type{get{return type;}}
		public GLib Lib{get{return lib;}}
		public GObject OrigObject{get{return origObject;}}
		public bool addPointsMode=true;

//        private CoordDataTable cdt;
        
        #region Construction
        public EditObject(IGeoApp app, GType type)
		{
            this.app = app;
            lib = type.Lib;
			this.type=type;

//            cdt = new CoordDataTable( );        //      ???
		}
		public EditObject(IGeoApp app, GObject obj)
		{
            this.app = app;
			lib = obj.Lib;
			this.type=obj.Type;
			origObject=obj;
			switch(obj.GeomType)
			{
				case GeomType.Point:
					points.Add(((GPoint)obj).Point);
					break;
				case GeomType.Caption:
					points.Add(((GCaption)obj).Point);
					break;
				case GeomType.Polyline:
					points.AddRange(((GPolyline)obj).Points);
					break;
				case GeomType.Polygon:
					points.AddRange(((GPolygon)obj).Points);
					break;
			}
			Last();

//            cdt = (CoordDataTable)obj;

        }
        #endregion
        public void First()
		{
			selIndex=points.Count>0 ? 0 : -1;
		}
		public void Last()
		{
			selIndex=points.Count-1;
		}
		public void Prev()
		{
			selIndex--;
			if(selIndex<0) Last();
		}
		public void Next()
		{
			selIndex++;
			if(selIndex>=points.Count) First();
		}
		public void CheckRepaint()
		{
			app.CheckRepaint(this);
		}
		public void MovePoint(Direction dir,Map map)
		{
			if(selIndex>=0&&selIndex<points.Count)
			{
				Size size=Map.GetVector(dir);
				map.GToW(ref size);
				Point p=(Point)points[selIndex];
				p+=size;
				points[selIndex]=p;
				UpdateBounds();
				CheckRepaint();
			}
		}
		public void DeletePoint()
		{
			if(selIndex>=0&&selIndex<points.Count)
			{
			  points.RemoveAt(selIndex);
				if(selIndex>=points.Count) Last();
				CheckRepaint();
			}
		}
		void UpdateBounds()
		{
			app.Lib.Selection.UpdateBounds(this);
		}
		public void AddPoint(Point wp)
		{
			points.Add(wp);
			Next();
			UpdateBounds();
			CheckRepaint();
			switch(type.GeomType)
			{
				case GeomType.Point:
				case GeomType.Caption:
					app.EndEditing();
					break;
			}
		}
		public void HighlightPoint(Point wp,Map map)
		{
			int index=GetNearPoint(wp,map);
			if(highlightIndex!=index)
			{
				highlightIndex=index;
				this.CheckRepaint();
			}
		}

		int GetNearPoint(Point wp,Map map)
		{
			double d =Constants.cmInMeter*lib.UnitMeasure * map.Scale * lib.Config.geometry.searchRadiusCm;
			long distSq=(long)(d*d);
			for(int i=0;i<points.Count;i++)
			{
				Point p=(Point)points[i];
				if(GeomUtils.DistanceSq(p,wp)<distSq) return i;
			}
			return -1;
		}

		public void Select(Map map)
		{
			switch(type.GeomType)
			{
				case GeomType.Polygon:
					map.DrawPolygon(lib.Config.styles.selStyle,Points);
					break;
				default:
                    map.DrawPolyline(lib.Config.styles.selStyle, Points);
					break;
			}
			if(selIndex>=0 && selIndex<points.Count)
			{
				Point selPoint=(Point)points[selIndex];
				map.DrawCircle(lib.Config.styles.editPointStyle,selPoint,lib.Config.geometry.pointRadius);
				int prevIndex=selIndex==0 ? points.Count-1 : selIndex-1;
				if(prevIndex!=selIndex)
				{
					Point prevPoint=(Point)points[prevIndex];
					Point[] line={prevPoint,selPoint};
					map.DrawPolyline(lib.Config.styles.editLineStyle,line);
				}
			}
		}
		public Point[] Points{get{return points.ToArray();}}
		public GObject Create()
		{
			GObject obj=null;
			switch(type.GeomType)
			{
				case GeomType.Point:
					obj=new GPoint(type,Points[0]);
					break;
				case GeomType.Caption:
					obj=new GCaption(type,Points[0]);
					break;
				case GeomType.Polyline:
					obj=new GPolyline(type,Points);
					break;
				case GeomType.Polygon:
					obj=new GPolygon(type,Points);
					break;
			}
			if(obj!=null) app.ShowProperties(obj);
			app.Lib.Selection.Clear();
			app.CheckRepaint(this);
			app.UpdateControls();
			return obj;
		}
		public GObject UpdateOrigObject()
		{
			switch(type.GeomType)
			{
				case GeomType.Point:
					((GPoint)origObject).Point=Points[0];
					break;
				case GeomType.Caption:
					((GCaption)origObject).Point=Points[0];
					break;
				case GeomType.Polyline:
					((GPolyline)origObject).Points=Points;
					break;
				case GeomType.Polygon:
					((GPolygon)origObject).Points=Points;
					break;
			}
			app.ShowProperties(origObject);
			app.Lib.Selection.Clear();
			app.CheckRepaint(this);
			app.UpdateControls();
			return origObject;
		}
		#region IShapedObject Members
		public Rect Bounds
		{
			get
			{
				return new Rect(Points);
			}
		}
		public bool IsVisibleOnMap(Map map)
		{
			int count=points.Count;
			if(count>0 && map.Intersects(Bounds)) 
			{
				switch(type.GeomType)
				{
					case GeomType.Polyline:
					{
						if(count==1) return true;
						return map.IsVisible(Bounds.MaxSize);
					}
					case GeomType.Polygon:
					{
						if(count<3) return true;
						return map.IsVisible(Bounds.MinSize);
					}
					default: return true;
				}
			}
			return false;
		}

		public void Draw(Map map)
		{
		}

		public void DrawSelected(Map map)
		{
			Rect bounds=Bounds;
			if(!map.Intersects(bounds)) return;
			Select(map);
		}

		public GeoLib.ClassId ClassId
		{
			get
			{
				return ClassId.None;
			}
		}

		public IShapedObject Parent
		{
			get
			{
				return null;
			}
		}

		public int Id{get{return 0;}}
		public bool GetCommonAttr(CommonAttr a) { return false; }
		#endregion
	}
}
