using System;
using System.Data;
using System.Drawing;
using System.IO;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class GPoint: GObject
	{
		Point point;
		public override GeomType GeomType{get{return GeomType.Point;}}
		public override Rect Bounds{get{return new Rect(point);}}
		public override Point Center{get{return point;}}
		public override bool Intersects(Rect rect){return rect.Contains(point);}
		public Point Point{get{return point;}set{point=value;CoordsChanged();}}
		public GPoint(GType type,Point point): base(type)
		{
			this.point=point;
			range=type.GetRangeStrong(this);
			range.Add(this);
		}
		internal GPoint(){}
		protected override void GetCode(Context context,IDataReader dr)
		{
			point=context.Buf.GetPoint(dr,(int)ObjectField.Code);
		}
		protected override void SetCode(Context context,IDbDataParameter par)
		{
			context.Buf.SetPoint(par,point);
		}
		protected override void ReadCode(Context context,BinaryReader br)
		{
			point=context.Buf.ReadPoint(br);
		}
		protected override void WriteCode(Context context,BinaryWriter bw)
		{
			context.Buf.WritePoint(bw,point);
		}
		public override void Draw(Map map)
		{
			if(!map.Intersects(point)) return;
			map.DrawPoint(TargetStyle,point);
			if(caption.Length>0) map.DrawText(TargetStyle,point,caption);
		}
		public long DistanceSq(Point p){return GeomUtils.DistanceSq(point,p);}
		public override void DrawSelected(Map map)
		{
			if(!map.Intersects(point)) return;
			Draw(map);
            map.DrawCircle(Lib.Config.styles.selStyle, point, Lib.Config.geometry.selRadius);
		}

        public override Point[] Points { get { Point[] points ={ point }; return points; } set { if (value.Length > 0) { point = value[0]; CoordsChanged(); } } }

	}

}
