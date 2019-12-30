using System;
using System.Data;
using System.Drawing;
using System.IO;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class GCaption: GObject
	{
		// {BE1B17F3-6760-4fb2-86B1-61B64181D7A0}
		Point point;
		float angle;
		public override GeomType GeomType { get { return GeomType.Caption; } }
		public override Rect Bounds{get{return new Rect(point);}}
		public override Point Center{get{return point;}}
		public override bool Intersects(Rect rect){return rect.Contains(point);}
		public Point Point{get{return point;}set{point=value;CoordsChanged();}}
		public float Angle{get{return angle;}set{angle=value;UpdateAttr(ObjectField.Code);}}
		public GCaption(GType type,Point point): base(type)
		{			
			this.point=point;
			angle=0.0f;
			range=type.GetRangeStrong(this);
			range.Add(this);
		}
		internal GCaption(){}
		int[] Data
		{
			get
			{
				return new int[]{point.X,point.Y,Geomethod.BufferUtils.FloatToInt(angle)};
			}
			set
			{
				int[] intArray=value;
				point.X=intArray[0];
				point.Y=intArray[1];
				angle=Geomethod.BufferUtils.IntToFloat(intArray[2]);
			}
		}
		protected override void GetCode(Context context,IDataReader dr)
		{
			Data=context.Buf.GetIntArray(dr,(int)ObjectField.Code);
		}
		protected override void SetCode(Context context,IDbDataParameter par)
		{
			context.Buf.SetIntArray(par,Data);
		}
		protected override void ReadCode(Context context,BinaryReader br)
		{
			Data=context.Buf.ReadIntArray(br);
		}
		protected override void WriteCode(Context context,BinaryWriter bw)
		{
			context.Buf.WriteIntArray(bw,Data);
		}
		public override void Draw(Map map)
		{
            if (map.Intersects(point))
            {
                if (caption.Length > 0)
                    map.DrawText(TargetStyle, point, caption, angle);
                else map.DrawCircle(TargetStyle, point, Lib.Config.geometry.pointRadius);
            }
		}
		public long DistanceSq(Point p){return GeomUtils.DistanceSq(point,p);}
		public override void DrawSelected(Map map)
		{
            if (map.Intersects(point))
            {
                Draw(map);
                map.DrawCircle(Lib.Config.styles.selStyle, point, Lib.Config.geometry.selRadius);
            }
		}
        public override Point[] Points { get { Point[] points ={ point }; return points; } set { if (value.Length > 0) { point = value[0]; CoordsChanged(); } } }


	}

}
