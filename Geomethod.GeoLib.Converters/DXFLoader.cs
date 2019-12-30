using System;
using System.Drawing;
using System.IO;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Converters;
using System.Globalization;

using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Geomethod.GeoLib.Converters
{
    public class DXFLoader
    {

		public static int unitcount = 0;
		public static int pointcount = 0;

		public int cnterr = 0;
		public	int cnt = 0;

        private string[]    fileNames;
  
		GLib lib;

		int bottom = Int32.MaxValue;
		int top = Int32.MinValue;
		int left = Int32.MaxValue;
		int right = Int32.MinValue;


        public DXFLoader( string[] fileNames )
		{
			this.fileNames=fileNames;
		}

        public GLib Load( )
        {

			lib = new GLib( null, Rect.Max, new Indexer( ) );
            lib.Name = Locale.Get( "_glibdefaultname" );
            lib.StyleStr = "pc=green";

            GType type;
            GeomType[] geomTypes ={ GeomType.Polygon, GeomType.Polyline, GeomType.Point, GeomType.Caption };
            foreach( GeomType gt in geomTypes )
            {
                type = new GType( lib, gt );
                type.Name = Locale.Get( gt.ToString( ) );
            }


            foreach( string filePath in fileNames )
                Load( filePath );

            lib.SMin = 10;
            //			if(lib.Bounds.IsNull) 
            //                lib.SetBounds( rect );

            lib.SetBounds( new Rect( left, bottom, right, top ) );
            lib.SMax = Geomethod.GeoLib.GeoLibUtils.RoundScale( lib.Bounds.MaxSize / 50 );
            lib.Scales.InitScales( );
 
            return lib;
        }

		public void Load( string filename )
		{
			using( DXFFileReader dxf = new DXFFileReader( filename ) )
			{
//				dxf.ScanAll();
//				return;

				Dictionary<DXFUnit, GType> types = new Dictionary<DXFUnit, GType>( );
				int pc = 0, lc = 0, plc = 0, rc = 0;
				int err = 0;

				while( dxf.Read( ) )
                {
					if( dxf.GetUnitType( ) == DXFUnit.Null )
						continue;

					if( dxf.GetUnitType() == DXFUnit.Polyline )
						if( dxf.Get( ).points[ 0 ] == dxf.Get( ).points[ dxf.Get( ).points.Count - 1 ] )
							dxf.Get().type = DXFUnit.Polygon;								

                    GType gType = GetType( filename, dxf.GetUnitType(), types );

                    switch( dxf.GetUnitType() )
                    {
						case DXFUnit.Point:
						{
							ReadPoint( dxf.Get(), gType );
							pc++;
							break;
						}
						case DXFUnit.Polyline:
						{
							ReadPolyline( dxf.Get(), gType );
							lc++;
							break;
						}
						case DXFUnit.Polygon:
						{
							ReadPolygon( dxf.Get(), gType );
							plc++;
							break;
						}
                        case DXFUnit.Text:
							ReadText( dxf.Get( ), gType );
							rc++;
                            break;
						default:
						{
//							throw new DXFReaderException( "Type \'" + dxf.GetUnitType().ToString( ) + "\' not implemented" /*, mr.mifNLine*/ );
							err++;
							break;
						}

                    }
					cnt++;
                }
				cnt++;
			}
		}

		private GType GetType( string filename, DXFUnit dxfu, Dictionary<DXFUnit, GType> types )
        {
            GType gType = null;
			if( !types.ContainsKey( dxfu ) )
            {
				gType = CreateType( dxfu );
				gType.Name = Path.GetFileNameWithoutExtension( filename ) + dxfu.ToString( );
				types.Add( dxfu, gType );
            }
            else
				gType = types[ dxfu ];
            return gType;
        }

		GType CreateType( DXFUnit dxf )
		{
			switch( dxf )
			{
				case DXFUnit.Point:
					return CreateType( GeomType.Point );
				case DXFUnit.Text:
					return CreateType( GeomType.Caption );
				case DXFUnit.Polygon:
					return CreateType( GeomType.Polygon );
				case DXFUnit.Polyline:
					return CreateType( GeomType.Polyline );
				default:
					throw new DXFReaderException( "Type \'" + dxf.ToString( ) + "\' not implemented" );
//					cnterr++;
//					return CreateType( GeomType.Polyline );
//					break;
			}
		}
		GType CreateType( GeomType gt )
		{
			GType par = GetType( gt );
			return new GType( par );
		}
		GType GetType( GeomType gt )
		{
			foreach( GType type in lib.Types )
				if( type.GeomType == gt )
					return type;

			return null;
		}

		private void ReadPolygon( DXFObject dxf, GType gType )
		{
			Point[] pnt = new Point[ dxf.points.Count ];
			for( int i = 0; i < dxf.points.Count; i++ )
			{
				pnt[ i ].X = (int)( dxf.points[ i ].X * 100.0 );
				pnt[ i ].Y = (int)( dxf.points[ i ].Y * 100.0 );

				pointcount++;
				UpdateBounds( pnt[ i ] );
			}
			unitcount++;

			if( dxf.points.Count > 2 )
			{
				GPolygon gobj = new GPolygon( gType, pnt );
			}
		}

		private void ReadPolyline( DXFObject dxf, GType gType )
		{
			Point[] pnt = new Point[ dxf.points.Count ];
			for( int i = 0; i < dxf.points.Count; i++ )
			{
				pnt[ i ].X = (int)( dxf.points[ i ].X * 100.0 );
				pnt[ i ].Y = (int)( dxf.points[ i ].Y * 100.0 );

				pointcount++;
				UpdateBounds( pnt[ i ] );
			}
			unitcount++;
			GPolyline gobj = new GPolyline( gType, pnt );
		}

		private void ReadPoint( DXFObject dxf, GType gType )
		{
			Point pnt = new Point( (int)( dxf.points[ 0 ].X * 100.0 ), (int)( dxf.points[ 0 ].Y * 100.0 ) );
			UpdateBounds( pnt );
			pointcount++;
			unitcount++;
			GPoint gobj = new GPoint( gType, pnt );
		}


		private void ReadText( DXFObject dxf, GType gType )
		{
			Point pnt = new Point( (int)( dxf.points[0].X * 100.0 ), (int)( dxf.points[0].Y * 100.0 ) );
			GCaption gobj = new GCaption( gType, pnt );
			gobj.Caption = dxf.text;
			gobj.Angle = (float)dxf.angle;

			UpdateBounds( pnt );
		}
		private void UpdateBounds( Point pnt )
		{
			left = Math.Min( left, pnt.X );
			right = Math.Max( right, pnt.X );
			top = Math.Max( top, pnt.Y );
			bottom = Math.Min( bottom, pnt.Y );
		}
	}
}
