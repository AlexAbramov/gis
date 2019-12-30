using System;
using System.Drawing;
using System.IO;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Converters;

namespace Geomethod.GeoLib.Converters
{
	public class ShapeLoader
	{
		string[] fileNames;
		GLib lib=null;
		GType curType=null;
		double minx,maxx,miny,maxy;
		bool boundsUpdated=false;
		double sx=0;
		double sy=0;
		double scaleTransform=1;
		int logScaleTransform=0;
		bool hasScaleTransform=false;
		bool hasXTransform=false;
		bool hasYTransform=false;
		public ShapeLoader(string[] fileNames)
		{
			this.fileNames=fileNames;
		}
		void UpdateBounds(Boundary b)
	  {
			if(boundsUpdated)
			{
				if(minx>b.minx) minx=b.minx;
				if(maxx<b.maxx) maxx=b.maxx;
				if(miny>b.miny) miny=b.miny;
				if(maxy<b.maxy) maxy=b.maxy;
			}
			else
			{
				minx=b.minx;
				maxx=b.maxx;
				miny=b.miny;
				maxy=b.maxy;
				boundsUpdated=true;
			}
		}
		public GLib Load()
		{
			foreach(string filePath in fileNames)
			{
				using(ShapeFileReader	sr = new ShapeFileReader(filePath))
				{
					UpdateBounds(sr.bound);
				}
			}
			if(!boundsUpdated) return null;
			
			SetTransform();
			CreateLib();

			foreach(string filePath in fileNames)
			{
				using(ShapeFileReader	sr = new ShapeFileReader(filePath))
				{
					//					object[] attrs;
					if(!CreateType(sr.GetUnitType())) continue;
					curType.Name=Path.GetFileNameWithoutExtension(filePath);
					while(sr.Read())
					{
						switch(sr.GetUnitType())
						{
							case ShapeUnit.Arc:
								//								Read((ShapeArc)sr.Get());
								break;
							case ShapeUnit.Point:
								Read((ShapePoint)sr.Get());
								break;
							case ShapeUnit.Polygon:
								Read((ShapePolygon)sr.Get());
								break;
//							case ShapeUnit.PointGroup:
								//								Read((ShapePointGroup)sr.Get());
//								break;
							case ShapeUnit.PolyLine:
								Read((ShapePolyline)sr.Get());
								break;
						}
					}
				}
			}
			lib.SMin=10;
			if(lib.Bounds.IsNull) lib.SetBounds(new Rect(0,0,1000000,1000000));
			lib.SMax=Geomethod.GeoLib.GeoLibUtils.RoundScale(lib.Bounds.MaxSize/ 10);
			lib.Scales.InitScales();
			return lib;
		}

		bool CreateType(ShapeUnit su)
		{
			curType=null;
			switch(su)
			{
				case ShapeUnit.Arc: 
					break;
				case ShapeUnit.Point:
					curType=CreateType(GeomType.Point);
					break;
//				case ShapeUnit.PointGroup:
//					break;
				case ShapeUnit.Polygon:
					curType=CreateType(GeomType.Polygon);
					break;
                case ShapeUnit.PolyLine:
                    curType=CreateType(GeomType.Polyline);
					break;
			}
			return curType!=null;
		}
		
		GType CreateType(GeomType gt)
		{
			GType par=GetType(gt);
			return new GType(par);
		}
		
		void SetTransform()
		{
			double w=Math.Abs(maxx-minx);
			double h=Math.Abs(maxy-miny);
			double maxSize=w>h?w:h;
			int logSize=(int)Math.Ceiling(Math.Log10(maxSize));
			if(logSize<6 || logSize>9)
			{
				logScaleTransform=8-logSize;
				scaleTransform=Math.Pow(10,logScaleTransform);
				hasScaleTransform=true;
			}
			if(minx<0 || minx>maxSize)
			{
				sx=-minx;
				hasXTransform=true;
			}
			if(miny<0 || miny>maxSize)
			{
				sy=-miny;
				hasYTransform=true;
			}
		}

		int XTransform(double d)
		{
			if(hasScaleTransform) d*=scaleTransform;
			if(hasXTransform) d+=sx;
			return (int) d;
		}

		int YTransform(double d)
		{
			if(hasScaleTransform) d*=scaleTransform;
			if(hasYTransform) d+=sy;
			return (int) d;
		}

		void CreateLib()
		{
			Rect bounds=new Rect(XTransform(minx),YTransform(miny),XTransform(maxx),YTransform(maxy));
			lib=new GLib(null,bounds,new Indexer());
			lib.Name=Locale.Get("_glibdefaultname");
			lib.StyleStr="pc=red";
			GType type;
			GeomType[] geomTypes={GeomType.Polygon,GeomType.Polyline,GeomType.Point,GeomType.Caption};
			foreach(GeomType gt in geomTypes)
			{
				type=new GType(lib,gt);
				type.Name=Locale.Get(gt.ToString());
			}
		}

		void Read(ShapePoint sp)
		{
			GPoint gobj = new GPoint(curType, new Point(XTransform(sp.point.X),YTransform(sp.point.Y)));	
		}

		void Read(ShapeArc shapeArc)
		{

		}

		void Read(ShapePolygon sp)
		{
			for( int p = 0; p < sp.numParts; p++ )
			{
				uint	max = sp.numPoints;
				if( p < sp.numParts-1 )
					max = sp.parts[ p+1 ];

				uint	len = sp.numPoints;
				if( p < sp.numParts-1  )
					len = sp.parts[ p + 1 ] - sp.parts[ p ];
				else
					len = sp.numPoints - sp.parts[ p ]; 

				Point[]	pnt = new Point[ len ];
				int j = 0;
				for( uint i = sp.parts[ p ]; i < max; i++ )
				{
					pnt[j].X = XTransform(sp.points[i].X);
					pnt[j].Y = YTransform(sp.points[i].Y);
					j++;
				}
				GPolygon gobj = new GPolygon( curType, pnt );
			}
		}

		void Read(ShapePolyline sp)
		{
			for( int p = 0; p < sp.numParts; p++ )
			{
				uint	max = sp.numPoints;
				if( p < sp.numParts-1 )
					max = sp.parts[ p+1 ];

				uint	len = sp.numPoints;
				if( p < sp.numParts-1  )
					len = sp.parts[ p + 1 ] - sp.parts[ p ];
				else
					len = sp.numPoints - sp.parts[ p ]; 

				Point[]	pnt = new Point[ len ];
				int j = 0;
				for( uint i = sp.parts[ p ]; i < max; i++ )
				{
					pnt[j].X = XTransform(sp.points[i].X);
					pnt[j].Y = YTransform(sp.points[i].Y);
					j++;
				}
				GPolyline gobj = new GPolyline( curType, pnt );
			}
		}

		GType GetType(GeomType gt)
		{
			foreach(GType type in lib.Types)
			{
				if(type.GeomType==gt) return type;
			}
			return null;
		}
	}
}
