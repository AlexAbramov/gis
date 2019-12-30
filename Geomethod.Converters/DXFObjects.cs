using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace Geomethod.Converters
{
	public enum DXFUnitNative : uint
	{
		_3DFACE, 
		_3DSOLID, 
		ACAD_PROXY_ENTITY, 
		ARC, 
		ATTDEF, 
		ATTRIB, 
		BODY, 
		CIRCLE, 
		DIMENSION, 
		ELLIPSE, 
		HATCH, 
		IMAGE, 
		INSERT, 
		LEADER, 
		LINE, 
		LWPOLYLINE, 
		MLINE, 
		MTEXT, 
		OLEFRAME, 
		OLE2FRAME, 
		POINT, 
		POLYLINE, 
		RAY, 
		REGION, 
		SEQEND, 
		SHAPE, 
		SOLID, 
		SPLINE, 
		TEXT, 
		TOLERANCE, 
		TRACE, 
		VERTEX, 
		VIEWPORT, 
		XLINE 
	}
	public enum DXFUnit
	{
		Point, 
		Text,
		Polyline,
		Polygon,
		NotSupported,
		Null

	}

	public	struct	_DXFPoint
	{
		public	double	X;
		public	double	Y;

		public _DXFPoint( double X, double Y )
		{
			this.X = X;
			this.Y = Y;
		}
		static public bool operator ==( _DXFPoint pl, _DXFPoint pr )
		{
			return pl.X == pr.X && pl.Y == pr.Y;
		}
		static public bool operator !=( _DXFPoint pl, _DXFPoint pr )
		{
			return pl.X != pr.X || pl.Y != pr.Y;
		}
        public override int GetHashCode()
        {
            return (int)X+(int)Y;
        }
        public override bool Equals(object obj)
        {
            if (obj is _DXFPoint)
            {
                _DXFPoint p = (_DXFPoint)obj;
                return p.X == X && p.Y == Y;
            }
            return false;
        }
	}

    public	class DXFObject/*: IDisposable*/
    {
		public List<_DXFPoint> points;
		public DXFUnit type;
		public DXFUnitNative nativetype;
		public string text;
		public float angle;

		public DXFObject( DXFUnitNative type )
		{
			this.nativetype = type;
			switch( type )
			{
				case DXFUnitNative.TEXT:
					this.type = DXFUnit.Text;
					break;
				case DXFUnitNative.LINE:
					this.type = DXFUnit.Polyline;
					break;
				case DXFUnitNative.POLYLINE:
					this.type = DXFUnit.Polyline;
					break;	
				case DXFUnitNative.POINT:
					this.type = DXFUnit.Point;
					break;
				default:
					this.type = DXFUnit.NotSupported;
					break;
			}
			points = new List<_DXFPoint>();
		}

		public	void Add( _DXFPoint point )
		{
			points.Add( point );
			DXFFileReader.pointcount++;
		}

/*		public void Dispose( )
		{
			points.Clear( );
//			points.Dispose( );		//			???
		}
*/
		public static implicit operator Point[]( DXFObject dxf )
		{
			Point[] pnt = new Point[ dxf.points.Count ];
			for( int i = 0; i < dxf.points.Count; i++ )
			{
				pnt[ i ].X = (int)( dxf.points[ i ].X * 100.0 );
				pnt[ i ].Y = (int)( dxf.points[ i ].Y * 100.0 );
			}
			return pnt;
		}
    }
}
