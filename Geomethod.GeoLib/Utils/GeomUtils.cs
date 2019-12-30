using System;
using System.IO;
using System.Drawing;

namespace Geomethod.GeoLib
{
	public struct Rect
	{
		public static readonly Rect Max=new Rect(int.MinValue,int.MinValue,int.MaxValue,int.MaxValue);
		public static readonly Rect Null=new Rect(int.MinValue,int.MinValue,int.MinValue,int.MinValue);
		public int left;
		public int bottom;
		public int right;
		public int top;

		#region Construction
		public Rect(int left,int bottom,int right,int top)
		{
			this.left=left;
			this.bottom=bottom;
			this.right=right;
			this.top=top;
			Normalize();
		}
		public Rect(Rectangle r)
		{
			left=r.Left;
			bottom=r.Top;
			right=r.Right;
			top=r.Bottom;
			Normalize();
		}
		public Rect(Point p)
		{
			left=right=p.X;;
			bottom=top=p.Y;
		}
		public Rect(Size size)
		{
			left=bottom=0;
			right=size.Width;
			top=size.Height;
			Normalize();
		}
		public Rect(Point[] points)
		{
			this=Rect.Null;
			Init(points);
		}
		public void Init(int left,int bottom,int right,int top)
		{
			this.left=left;
			this.bottom=bottom;
			this.right=right;
			this.top=top;
			Normalize();
		}
		public void Init(Point p)
		{
			left=right=p.X;;
			bottom=top=p.Y;
		}
		public void Init(int x, int y)
		{
			left=right=x;
			bottom=top=y;
		}
		public void Init(Size size)
		{
			Init(0,0,size.Height,size.Width);
		}
		public void Init(Point[] points)
		{
			if(points.Length==0)
			{
				this=Rect.Null;
				return;
			}
			for(int i=0;i<points.Length;i++)
			{
				int x=points[i].X;
				int y=points[i].Y;
				if(i==0)
				{
					Init(x,y);
				}
				else
				{
					if(x<left) left=x;
					else if(right<x) right=x; 
					if(y<bottom) bottom=y;
					else if(top<y) top=y; 
				}
			}
		}
#endregion

		#region Utils
        public Size Size { get { return new Size(right-left, top-bottom); } }
		public override string ToString(){return string.Format("{{{0} {1} {2} {3}}}",left,bottom,right,top);}
		public void LeftBitShift(byte b)
		{
			left<<=b;
			bottom<<=b;
			right<<=b;
			top<<=b;
		}
		public void RightBitShift(byte b)
		{
			left>>=b;
			bottom>>=b;
			right>>=b;
			top>>=b;
		}
		public long Width{get{return (long)right-left;}}
		public long Height{get{return (long)top-bottom;}}
		public long MinSize{get{return Width<Height?Width:Height;}}
		public long MaxSize{get{return Width>Height?Width:Height;}}
		public bool IsNull{get{return this==Null;}}
		public bool IsNormalized{get{return left<=right && bottom<=top;}}
		public void Normalize()
		{
			if(left>right){int i=left;left=right;right=i;}
			if(bottom>top){int i=bottom;bottom=top;top=i;}
		}
		public bool Contains(Rect r){return left<=r.left && bottom<=r.bottom && r.right<=right && r.top<=top;}
		public bool Contains(Point p){return left<=p.X && bottom<=p.Y && p.X<=right && p.Y<=top;}
		public bool Intersects(Rect r){return !(r.right<left||r.top<bottom||right<r.left||top<r.bottom);}
		public override int GetHashCode(){return left^bottom^right^top;}
		public Point Center{get{return new Point((int)((right+(long)left)/2),(int)((bottom+(long)top)/2));}}
		public Point LeftTop{get{return new Point(left,top);}}
		public Point RightTop{get{return new Point(right,top);}}
		public Point LeftBottom{get{return new Point(left,bottom);}}
		public Point RightBottom{get{return new Point(right,bottom);}}
		public Point[] Points{get{return new Point[]{LeftBottom,LeftTop,RightTop,RightBottom};}}
		public bool Update(Rect r)
		{
			if(r.IsNull) return false;
			if(IsNull){this=r;this.Normalize();return true;}
			bool updated=false;
			if(r.left<left){left=r.left;updated=true;}
			if(right<r.right){right=r.right;updated=true;}
			if(r.bottom<bottom){bottom=r.bottom;updated=true;}
			if(top<r.top){top=r.top;updated=true;}
			return updated;
		}
		public static bool operator ==(Rect r1,Rect r2)
		{
			return r1.left==r2.left && r1.top==r2.top && r1.right==r2.right && r1.bottom==r2.bottom;
		}
		public static bool operator !=(Rect r1,Rect r2){return !(r1==r2);}
		public void Move(int dx,int dy)
		{
			if(dx!=0){left+=dx;right+=dx;}
			if(dy!=0){bottom+=dy;top+=dy;}
		}
		public override bool Equals(object o){return (o is Rect) && ((Rect)o)==this;}
		public static implicit operator Rectangle(Rect r) 
		{
			return new Rectangle(r.left,r.bottom,(int)r.Width,(int)r.Height);
		}
		public static implicit operator Rect(Rectangle r) 
		{
			return new Rect(r);
		}
		public void Inflate(int s)
		{
			Inflate(s,s);
		}
		public void Inflate(int sx,int sy)
		{
			left-=sx;
			right+=sx;
			bottom-=sy;
			top+=sy;
			Normalize();
		}
#endregion
	}

	public class GeomUtils
	{
		public static double PerpSq(Point p,Point p0,Point p1)
		{
			if(p1==p0) return DistanceSq(p,p0);
			long dx=(long)p1.X-p0.X;
			long dy=(long)p1.Y-p0.Y;
			long x=(long)p.X-p0.X;
			long y=(long)p.Y-p0.Y;
			double d=dx*y-dy*x;
			return d*d/(dx*dx+dy*dy);
		}
		public static long DistanceSq(Point p0,Point p1)
		{
			long x=(long)p1.X-p0.X;
			long y=(long)p1.Y-p0.Y;
			return x*x+y*y;
		}
		public static long DistanceSq(Point p,Point p0,Point p1)
		{
			long ps=(long)PerpSq(p,p0,p1);
			long s=DistanceSq(p0,p1);
			long s0=DistanceSq(p,p0);
			long s1=DistanceSq(p,p1);
			if(Math.Abs(s0-s1)<s) return ps;
			return s0>s1? s0: s1;
		}
		public static long DistanceSq(Point p,Point[] points)
		{
			long res=DistanceSq(p,points[0]);
			for(int i=1;i<points.Length;i++)
			{
				long s=DistanceSq(p,points[i-1],points[i]);
				if(res>s) res=s;
			}
			return res;
		}
		public static bool Contains(Point[] points,Point p)
		{
			if (points==null || points.Length < 3) return true;
		
			int x = p.X;
			int y = p.Y;
			int hits = 0;
			Point lastp=points[points.Length-1];
			int lastx = lastp.X;
			int lasty = lastp.Y;
			int curx, cury;
		
			for (int i = 0; i < points.Length; lastx = curx, lasty = cury, i++)
			{
				curx = points[i].X;
				cury = points[i].Y;
				if (cury == lasty) continue;
				int leftx;
				if (curx < lastx)
				{
					if (x >= lastx)	continue;
					leftx = curx;
				}
				else
				{
					if (x >= curx) continue;
					leftx = lastx;
				}
			
				double test1, test2;
				if (cury < lasty)
				{
					if (y < cury || y >= lasty)	continue;
					if (x < leftx)
					{
						hits++; continue;
					}
					test1 = x - curx;
					test2 = y - cury;
				}
				else
				{
					if (y < lasty || y >= cury)	continue;
					if (x < leftx)
					{
						hits++; continue;
					}
					test1 = x - lastx;
					test2 = y - lasty;
				}
				if (test1 < (test2 / (lasty - cury) * (lastx - curx))) hits++;
				//		  if (test1 < test2 * (lastx - curx) / (lasty - cury)) hits++;
				//		  if (test1 * (lasty - cury) <= test2  * (lastx - curx)) hits++;
			}
			return (hits & 1) != 0;
		}
		public static void Normalize(ref Rectangle r)
		{
			if(r.Width<0){r.X+=r.Width;r.Width=-r.Width;}
			if(r.Height<0){r.Y+=r.Height;r.Height=-r.Height;}
		}
		public static void NormalizeAngle(ref float a)//grad
		{
			if(Math.Abs(a)>180) a=a%180;
			if(a>90) a-=180;
			else if(a<-90) a+=180;
		}
	}
}
