using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Geomethod.GeoLib
{
	public class CoordTransform
	{
		#region Fields
		protected Point pos;
		protected Point pixelPos;
		double pixelScale;// pixel to world coord
		float angle;//grad
		protected bool mirror;

		double m1,m2,m3,m4;// aux
		#endregion

		#region Properties
		public double PixelScale { get { return pixelScale; } set { pixelScale = value; UpdateMatrix(); } }
		public float Angle { get { return angle; } set { angle = value; UpdateMatrix(); } }
		#endregion

        #region Construction
		protected void Init(Point pos, Point pixelPos, double pixelScale, float angle, bool mirror)
		{
			this.pos = pos;
			this.pixelPos=pixelPos;
			this.pixelScale = pixelScale;
			this.angle = angle;
			this.mirror=mirror;
			UpdateMatrix();
		}
		#endregion

		#region Transforms
		public void WToG(Point[] points){for(int i=0;i<points.Length;i++) WToG(ref points[i]);}
		public void GToW(Point[] points){for(int i=0;i<points.Length;i++) GToW(ref points[i]);}
		public void WToG(ref Point p)
		{
			double x=p.X-pos.X;
			double y=p.Y-pos.Y;
			p.X=pixelPos.X+(int)(m1*x-m2*y);
			p.Y=(int)(m2*x+m1*y);
			if(!mirror) p.Y=-p.Y;
			p.Y+=pixelPos.Y;
		}
		public Point WToG(Point p)
		{
			int y=(int)(m2*p.X+m1*p.Y);
			if(!mirror) y=-y;
			return new Point(pixelPos.X+(int)(m1*p.X-m2*p.Y),pixelPos.Y+y);
		}
		public void GToW(ref Point p)
		{
			double x=p.X-pixelPos.X;
			double y=p.Y-pixelPos.Y;
			if(!mirror) y=-y;
			p.X=pos.X+(int)(m3*x+m4*y);
			p.Y=pos.Y+(int)(-m4*x+m3*y);
		}
		public Point GToW(Point p)
		{
			p.X-=pixelPos.X;
			p.Y-=pixelPos.Y;
			if(!mirror) p.Y=-p.Y;
			return new Point(pos.X+(int)(m3*p.X+m4*p.Y),pos.Y+(int)(-m4*p.X+m3*p.Y));
		}
		public void GToW(ref Size s)
		{
			double x=s.Width;
			double y=s.Height;
			if(!mirror) y=-y;
			s.Width=(int)(m3*x+m4*y);
			s.Height=(int)(-m4*x+m3*y);
		}
		public Rect GToW(Rect r)
		{
			Point[] points=r.Points;
			GToW(points);
			r.Init(points);
			return r;
		}
		#endregion

		#region Aux
		void UpdateMatrix()
		{
			double angleRad=angle*Math.PI/180;
			double cosa=(double)Math.Cos(angleRad);
			double sina=(double)Math.Sin(angleRad);
			m1=cosa/pixelScale;
			m2=sina/pixelScale;			
			m3=cosa*pixelScale;
			m4=sina*pixelScale;
		}
		#endregion
	}
}
