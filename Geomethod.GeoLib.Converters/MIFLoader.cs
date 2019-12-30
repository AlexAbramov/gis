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
  public class MIFLoader
  {
    
		string[] fileNames;
		GLib lib=null;
        int bottom = Int32.MaxValue;
        int top = Int32.MinValue;
        int left = Int32.MaxValue;
        int right = Int32.MinValue;

        string captionFilename;
        bool readCaption;
        string nameFilename;
        bool readName;
        string[] captionNames;
        string[] nameNames;

      MIFFileReader mr;

      static    int cnt = 0;


    public MIFLoader( string[] fileNames )
		{
			this.fileNames=fileNames;
		}

    public GLib Load() 
		{
            lib = new GLib( null, Rect.Max, new Indexer( ) );
            lib.Name = Locale.Get( "_glibdefaultname" );
            lib.StyleStr = "pc=blue";
            GType type;
            GeomType[] geomTypes ={ GeomType.Polygon, GeomType.Polyline, GeomType.Point, GeomType.Caption };
            foreach( GeomType gt in geomTypes )
            {
                type = new GType( lib, gt );
                type.Name = Locale.Get( gt.ToString( ) );
            }
		
            NumberFormatInfo nfi = new NumberFormatInfo( );
            nfi.NumberDecimalSeparator = ".";

            foreach( string filePath in fileNames )
                Load( filePath, nfi ) ;

            lib.SMin = 10;
            //			if(lib.Bounds.IsNull) 
            //                lib.SetBounds( rect );

            lib.SetBounds( new Rect( left, bottom, right, top ) );
            lib.SMax = Geomethod.GeoLib.GeoLibUtils.RoundScale( lib.Bounds.MaxSize / 50 );
            lib.Scales.InitScales( );

			
/*			int gcnt = 0;
			foreach( GType gtype in lib.AllTypes )
				if( gtype.Ranges != null )
				foreach( GRange range in gtype.Ranges )
					foreach( GObject gobj in range.Objects )
						gcnt++;

				gcnt++;		//		Вроде количество созданных объектов совпадает с количеством в файлах
 */ 
			return lib;
			
       }

        public void Load( string filename, NumberFormatInfo nfi )
        {


            using( /*MIFFileReader*/ mr = new MIFFileReader( filename, /*false*/ true, true, nfi ) )
            {
				mr.keepOriginalBounds = /*true*/ false;

                Dictionary<MIFUnit, GType> types = new Dictionary<MIFUnit, GType>( );

                captionFilename = Path.ChangeExtension( filename, ".caption" );
                readCaption = File.Exists( captionFilename );
                nameFilename = Path.ChangeExtension( filename, ".name" );
                readName = File.Exists( nameFilename );

                if( readCaption )
                    using( StreamReader sr = new StreamReader( captionFilename ) )
                    {
                        string str = sr.ReadLine( );
                        captionNames = str.Split( ' ' );
                    }
                if( readName )
                    using( StreamReader sr = new StreamReader( nameFilename ) )
                    {
                        string str = sr.ReadLine( );
                        nameNames = str.Split( ' ' );
                    }

                while( mr.Read( ) )
                {
                    GType gType = GetType( filename, mr.GetUnitType(), types );

//      Это пока закомментировано
//                        if( sr.mifType != type )
//                            throw new MIFReaderException( "Type \'" + sr.mifType + "\' not matches type \'" + type + "\'", sr.mifNLine );

                    switch( mr.GetUnitType() )
                    {
//                        case MIFUnit.Arc:
//                            break;
                        case MIFUnit.Point:
                            Read( (MIFPoint)mr.Get( ), gType  );
                            break;
                        case MIFUnit.Region:
                            Read( (MIFRegion)mr.Get(), gType );
                            break;
                        case MIFUnit.Line:
                            Read( (MIFLine)mr.Get( ), gType );
                            break;
						case MIFUnit.Polyline:
							Read( (MIFPolyline)mr.Get( ), gType );
							break;
						case MIFUnit.Text:
							Read( (MIFText)mr.Get( ), gType );
							break;
						default:
                            throw new MIFReaderException( "Type \'" + mr.GetUnitType().ToString( ) + "\' not implemented", mr.mifNLine );
                    }
                }
				if( mr.keepOriginalBounds )
				{
					left = (int)( mr.coordsys.p1.X * 100 );
					bottom = (int)(mr.coordsys.p1.Y * 100 );
					right = (int)(mr.coordsys.p2.X * 100 );
					top = (int)( mr.coordsys.p2.Y * 100 );
				}

                
            }

        }


        private GType GetType( string filename, MIFUnit mu, Dictionary<MIFUnit, GType> types )
        {
            GType gType = null;
            if( !types.ContainsKey( mu ) )
            {
                gType = CreateType( mu );
                gType.Name = Path.GetFileNameWithoutExtension( filename ) + mu.ToString( );
                types.Add( mu, gType );
            }
            else
                gType = types[ mu ];
            return gType;
        }

		GType CreateType( MIFUnit mu)
		{
			switch(mu)
			{
				case MIFUnit.Point:
					return  CreateType(GeomType.Point);
				case MIFUnit.Region:
					return  CreateType(GeomType.Polygon);
                case    MIFUnit.Line:
                    return  CreateType( GeomType.Polyline );
				case MIFUnit.Polyline:
					return CreateType( GeomType.Polyline );
				case MIFUnit.Text:
					return CreateType( GeomType.Caption );
				default:
                    throw new MIFReaderException( "Type \'" + mu.ToString( ) + "\' not implemented" );
			}
		}
		
		GType CreateType(GeomType gt)
		{
			GType par=GetType(gt);
			return new GType(par);
		}
		
	  void Read( MIFPoint mp, GType gType )
	  {
		  Point pnt = new Point( (int)( mp.point.X * 100.0 ), (int)( mp.point.Y * 100.0 ) );
		  GPoint gobj = new GPoint( gType, pnt );

          SetCaption( gobj );
          SetName( gobj );

          UpdateBounds( pnt );
	  }

	  void Read(  MIFText mp, GType gType )
	  {
		  Point pnt = new Point( (int)( mp.point1.X * 100.0 ), (int)( mp.point1.Y * 100.0 ) );
		  GCaption gobj = new GCaption( gType, pnt );
		  gobj.Caption = mp.text;
		  gobj.Angle = (float)mp.angle;

//          SetCaption( );
          SetName( gobj );

		  UpdateBounds( pnt );
	  }

        void Read( MIFRegion mp, GType gType )
		{
            for( int p = 0; p < mp.parts; p++ )
            {
                Point[] pnt = new Point[ mp.points[p].Length ];
                for( int i = 0; i < mp.points[p].Length; i++ )
                {
                    pnt[ i ].X = (int)( mp.points[ p ][ i ].X * 100.0);
                    pnt[ i ].Y = (int)( mp.points[ p ][ i ].Y * 100.0);

                    UpdateBounds( pnt[ i ] );
                }

				if( mp.points[ p ].Length > 2 )
				{
					GPolygon gobj = new GPolygon( gType, pnt );

                    SetCaption( gobj );
                    SetName( gobj );

//      02.06.09        Временное явление!!!
                    _setBuildToAddr( gobj );
                    cnt++;


				}             
            }
		}

      private void _setBuildToAddr( GPolygon gobj )
      {
          if( gobj.Type.Name == "BuildingsRegion" )
          foreach( GType gtype in lib.AllTypes )
              if( gtype.Name == "addrsText" )
                  if( gtype.Ranges != null )
                      foreach( GRange range in gtype.Ranges )
                          foreach( GObject gobj1 in range.Objects )
                              if( gobj.Contains( gobj1.Center ) )
                              {
                                  gobj.Caption = gobj1.Caption;
                                  gobj.Name = gobj1.Name;
                                  return;
                              }
      }

        void Read( MIFPolyline mp, GType gType )
        {
            for( int p = 0; p < mp.parts; p++ )
            {
                Point[] pnt = new Point[ mp.points[ p ].Length ];
                for( int i = 0; i < mp.points[ p ].Length; i++ )
                {
                    pnt[ i ].X = (int)( ( mp.points[ p ][ i ].X * 100.0 ) );
                    pnt[ i ].Y = (int)( ( mp.points[ p ][ i ].Y * 100.0 ) );

                    UpdateBounds( pnt[ i ] );
                }
                GPolyline gobj = new GPolyline( gType, pnt );
                SetCaption( gobj );
                SetName( gobj );

            }
        }

        void Read( MIFLine mp, GType gType )
        {

            Point[] pnt = new Point[ 2 ];
            pnt[ 0 ].X = (int)( ( mp.point1.X * 100.0 ) );
            pnt[ 0 ].Y = (int)( ( mp.point1.Y * 100.0 ) );
            pnt[ 1 ].X = (int)( ( mp.point2.X * 100.0 ) );
            pnt[ 1 ].Y = (int)( ( mp.point2.Y * 100.0 ) );

            UpdateBounds( pnt[ 0 ] );
            UpdateBounds( pnt[ 1 ] );
            GPolyline gobj = new GPolyline( gType, pnt );

            SetCaption( gobj );
            SetName( gobj );
        }

      private void SetCaption( GObject gobj )
      {
          if( !readCaption )
              return;

          string str = "";
          foreach( string name in captionNames )
              str += " " + ( (string)mr[ name ] ).Trim( '\"' );

          gobj.Caption = str;
      }
      private void SetName( GObject gobj )
      {
          if( !readName )
              return;

          string str = "";
          foreach( string name in nameNames )
              str += " " + ( (string)mr[ name ] ).Trim( '\"' );

          gobj.Name = str;
      }

        GType GetType( GeomType gt )
		{
			foreach(GType type in lib.Types)
				if(type.GeomType==gt) 
                    return type;

			return null;
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
