using System;
using System.Data;
using System.Drawing;
using System.IO;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class GPolyline: GObject
	{
		Point[] points;
		Rect bounds=Rect.Null;
		
		public override GeomType GeomType{get{return GeomType.Polyline;}}
		public override Rect Bounds{get{return bounds;}}
		public override Point Center{get{return bounds.Center;}}
		public override bool Intersects(Rect rect){return bounds.Intersects(rect);}
		public GPolyline(GType type,Point[] points): base(type)
		{
			if(points.Length<2) throw new GmException("Polyline should have 2 points at least.");
			this.points=points;
			bounds.Init(points);
			range=type.GetRangeStrong(this);
			range.Add(this);
		}
		internal GPolyline(){}
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
				map.DrawPolyline(TargetStyle,points);
			}
		}
		public long DistanceSq(Point p)
		{
			return GeomUtils.DistanceSq(p,points);
		}
		public override void DrawSelected(Map map)
		{
			if(!map.Intersects(bounds)) return;
			map.DrawPolyline(Lib.Config.styles.selStyle,points);
		}
		public override Point[] Points{get{return points;}set{points=value;bounds.Init(points);CoordsChanged();}}

	}
}
