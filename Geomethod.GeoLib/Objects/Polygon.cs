using System;
using System.Data;
using System.Drawing;
using System.IO;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class GPolygon: GObject
	{
		Point[] points;
		Rect bounds=Rect.Null;
		
		public override GeomType GeomType{get{return GeomType.Polygon;}}
		public override Rect Bounds{get{return bounds;}}
		public override Point Center{get{return bounds.Center;}}
		public override bool Intersects(Rect rect){return bounds.Intersects(rect);}
		public GPolygon(GType type,Point[] points): base(type)
		{
			if(points.Length<3) throw new Exception("Polygon should have 3 points at least.");
			this.points=points;
			bounds.Init(points);
			range=type.GetRangeStrong(this);
			range.Add(this);
		}
		internal GPolygon(){}
	    protected override void GetCode(Context context,IDataReader dr)
		{
			points=context.Buf.GetPoints(dr,(int)ObjectField.Code);
			bounds.Init(points);
		}
		protected override void SetCode(Context context,IDbDataParameter par)
		{
			context.Buf.SetPoints(par,points);
		}
		protected override void ReadCode(Context context,BinaryReader br)
		{
			points=context.Buf.ReadPoints(br);
			bounds.Init(points);
		}
		protected override void WriteCode(Context context,BinaryWriter bw)
		{
			context.Buf.WritePoints(bw,points);
		}
		public override void Draw(Map map)
		{
			if(IsVisibleOnMap(map))
			{
				map.DrawPolygon(TargetStyle,points);
				if(caption.Length>0) map.DrawText(TargetStyle,bounds.Center,caption);
			}
		}
		public bool Contains(Point p)
		{
			if(!bounds.Contains(p)) return false;
			return GeomUtils.Contains(points,p);
		}
		public override void DrawSelected(Map map)
		{
			if(!map.Intersects(bounds)) return;
			Draw(map);
            map.DrawPolygon(Lib.Config.styles.selStyle, points);
		}
		public override Point[] Points{get{return points;}set{points=value;bounds.Init(points);CoordsChanged();}}


	}
}
