using System;
using System.Drawing;
using System.Collections;

using System.Data;
using System.Data.Common;

using Geomethod.Converters;


namespace Test
{
	public class TestShape
	{
		public TestShape( string filename )
		{
            int cnt     = 0;
            int pcnt    = 0;    
			try
			{
//              string	drv = "Driver={Microsoft dBase Driver (*.dbf)};DBQ=";
//              OdbcConnection con = new OdbcConnection(drv + Path.GetDirectoryName( filename ));
//              con.Open();

            	using( ShapeFileReader	sr = new ShapeFileReader( filename ) )
				{
					while( sr.Read() )
					{
                        cnt++;
						if( sr.shapeType == ShapeUnit.Polygon )
						{
							ShapePolygon sp = (ShapePolygon)sr.Get( );
						
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
									pnt[j].X = (int)sp.points[i].X;
									pnt[j].Y = (int)sp.points[i].Y;
									j++;
								}
                                pcnt += (int)len;
							}
						}
                        if (sr.shapeType == ShapeUnit.PolyLine)
                        {
                            ShapePolyline sp = (ShapePolyline)sr.Get();

                            for (int p = 0; p < sp.numParts; p++)
                            {
                                uint max = sp.numPoints;
                                if (p < sp.numParts - 1)
                                    max = sp.parts[p + 1];

                                uint len = sp.numPoints;
                                if (p < sp.numParts - 1)
                                    len = sp.parts[p + 1] - sp.parts[p];
                                else
                                    len = sp.numPoints - sp.parts[p];

                                Point[] pnt = new Point[len];
                                int j = 0;
                                for (uint i = sp.parts[p]; i < max; i++)
                                {
                                    pnt[j].X = (int)sp.points[i].X;
                                    pnt[j].Y = (int)sp.points[i].Y;
                                    j++;
                                }
                                pcnt += (int)len;
                            }
                        }
/*                        string str = null;
                        object[] an = sr.GetAttrNames();
                        object[] tp = sr.GetAttrTypes();
                        for (int i = 0; i < an.Length; i++)
                            str += an[i] + " " + tp[i] + "\n";
                        Console.WriteLine(str, "Имена полей и типов");
*/
					}
				}
//                con.Close();
			}
			catch( Exception e )
			{
				Console.WriteLine( e.ToString() );
			}
            Console.WriteLine("Read " + cnt.ToString() + " objects, " + pcnt.ToString() + " points", 
                    this.GetType().ToString()  );
		}

    public TestShape()
		{

            /*			string	drv = "Driver={Microsoft dBase Driver (*.dbf)};DBQ=";

                        //		Здесь конструктор с connection

            //			string	filename1 = @"f:\dima\shape\bk_st.shp";
                        string	filename1 = @"f:\dima\shape\detroit\hist.shp";
            //			string	filename1 = @"f:\dima\shape\detroit\roads.shp";

                        OdbcConnection	con   =  new OdbcConnection( drv + Path.GetDirectoryName( filename1 ) );
                        con.Open();

                        using( ShapeFileReader	sr = new ShapeFileReader( filename1, con ) )
                        {
                            string	str =  null;
                            object[]	an = sr.GetAttrNames();
                            object[]	tp = sr.GetAttrTypes();	
                            for( int i = 0; i < an.Length; i++ )
                                str += an[i] + " " + tp[i] + "\n";
                            Console.WriteLine( str, "Имена полей и типов" );
				
                            int	cnt = 0;
                            while( sr.Read() )
                            {
                                if( sr.shapeType == ShapeUnit.Point )
                                {
                                    object[]	attrs;
                                    ShapePoint	sp = (ShapePoint)sr.Get( out attrs );
                                    if( cnt < 2 )
                                    {
                                        Console.WriteLine( sp.point.X.ToString() + " " + sp.point.Y.ToString(), "Координаты" );
                                        string	at = null;
                                        for( int i = 0; i < attrs.Length; i++ )
                                            at += attrs[i].ToString() + "\n";
                                        Console.WriteLine( at, "Атрибуты" );
                                    }
                                }
                                cnt++;
                            }
                            string	Bound = sr.shapeType.ToString()  + "\n " +
                                sr.bound.maxx.ToString() + "\n " +
                                sr.bound.maxy.ToString() + "\n " +
                                sr.bound.minx.ToString() + "\n " +
                                sr.bound.miny.ToString() + "\n " +
                                cnt.ToString();
                            Console.WriteLine( Bound, "Тип, границы и количество объектов" );

                        }
                        con.Close();
            */
            //			Здесь конструктор без connection

			string	filename2 = @"f:\dima\shape\bk_tb.shp";
			using( ShapeFileReader	sr = new ShapeFileReader( filename2 ) )
			{
				while( sr.Read() )
				{
					if( sr.shapeType == ShapeUnit.Polygon )
					{
						ShapePolygon sp = (ShapePolygon)sr.Get( );
						
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
								pnt[j].X = (int)sp.points[i].X;
								pnt[j].Y = (int)sp.points[i].Y;
								j++;
							}
						}
					}
				}

			}

			//		Здесь конструктор с connection и таблицей

			/*			try
						{
//				string	filename3 = @"f:\dima\shape\bk_st.shp";
//				string	filename3 = @"f:\dima\shape\detroit\roads.shp";
				string	filename3 = @"f:\dima\shape\detroit\hist.shp";
				string	drvJet	= @"Provider=Microsoft.Jet.OLEDB.4.0;";
				string	ext		= @";Extended Properties=dBASE IV;User ID=Admin;Password=";
				string	str2	= drvJet + "Data Source=" + Path.GetDirectoryName( filename3 ) + ext;
				OleDbConnection	con3 = new OleDbConnection(  str2 );
				con3.Open();

				DataTable	dt = new DataTable();
	
				ShapeFileReader	sr1 = new ShapeFileReader( filename3, con3, dt );
				sr1.Dispose();
				con3.Close();
	
				for( int i = 0; i < dt.Rows.Count; i++ )
				{
					string	str = null;
					for( int j = 0;  j < dt.Columns.Count; j++ )
						str += dt.Columns[j].ColumnName + "\t = " + dt.Rows[i][j].ToString() + "\n";
					Console.WriteLine( str, dt.TableName + "[" + i + "]" );

					if( i > 2 )
						break;
				}		

			}
			catch( Exception e )
			{
				Console.WriteLine( e.ToString() );
			}
*/			
		}

		public class MIFTestClass
		{

			public	MIFTestClass( string file )
			{

				using( MIFFileReader mfr = new MIFFileReader( file ) )
				{
					while( mfr.Read() )
					{

//                        Console.WriteLine(mfr.mifType.ToString());
						switch( mfr.mifType )
						{
							case	MIFUnit.Point:
							{
                                MIFPoint obj = (MIFPoint)mfr.Get( );
								break;
							}
                            case MIFUnit.Region:
                            {
                                MIFRegion obj = (MIFRegion)mfr.Get();
/*                                for ( int i = 0; i < obj.parts; i++ )
                                    for ( int j = 0; j < obj.points[ i ].Length; j++ )
                                        Console.WriteLine( "[" + i.ToString( ) + "][" + j.ToString( ) + "] = " +
                                               obj.points[ i ][ j ].X.ToString( ) + " " +
                                               obj.points[ i ][ j ].Y.ToString( ) );

*/                                break;
                            }
                            case MIFUnit.Polyline:
                            {
                                MIFPolyline obj = (MIFPolyline)mfr.Get( );
/*                                for ( int i = 0; i < obj.parts; i++ )
                                    for ( int j = 0; j < obj.points[ i ].Length; j++ )
                                        Console.WriteLine( "[" + i.ToString( ) + "][" + j.ToString( ) + "] = " +
                                               obj.points[ i ][ j ].X.ToString( ) + " " +
                                               obj.points[ i ][ j ].Y.ToString( ) );

*/                                break;

                            }
                            case MIFUnit.Line:
                            {
                                MIFLine obj = (MIFLine)mfr.Get( );
/*                                Console.WriteLine(
                                       obj.point1.X.ToString( ) + " " + obj.point1.Y.ToString( ) + " " +
                                       obj.point2.X.ToString( ) + " " + obj.point2.Y.ToString( ),
                                       mfr.mifType.ToString( ) );
 */
                                break;
                            }
				
						}
//						Console.WriteLine( mfr.mifType.ToString() );
//                        Console.WriteLine( mfr.RowLine().ToString() );

                        string attrs = null;
                        for (int i = 0; i < mfr.attrs.Length; i++)
                            attrs += mfr.GetAttrNames()[i].ToString() + "(" + mfr.GetAttrTypes()[i].ToString() + ")" + 
                                    " = " + mfr.attrs[i].ToString() + "\n";

//                        Console.WriteLine(attrs);
					}

					Console.WriteLine( MIFPolyline.cnt.ToString(),	"Polyline" );
					Console.WriteLine( MIFLine.cnt.ToString(),		"Line" );
					Console.WriteLine( MIFPoint.cnt.ToString(),		"Point" );
					Console.WriteLine( MIFRegion.cnt.ToString(),	"Region" );
					Console.WriteLine( MIFArc.cnt.ToString(),		"Arc" );
					Console.WriteLine( MIFText.cnt.ToString(),		"Text" );
					Console.WriteLine( MIFRectangle.cnt.ToString(),	"Rect" );
					Console.WriteLine( MIFRectangle.cnt.ToString(),	"RoundRect" );
					Console.WriteLine( MIFRectangle.cnt.ToString(),	"Ellipse" );

/*					Console.WriteLine( mfr.version,		"version" );
					Console.WriteLine( mfr.charset ,	"charset" );
					Console.WriteLine( mfr.delimiter ,	"delimiter" );
					Console.WriteLine( mfr.unique ,		"unique" );
					Console.WriteLine( mfr.index ,		"index" );
					Console.WriteLine( mfr.coordsys ,	"coordsys" );
					Console.WriteLine( mfr.transform ,	"transform" );

 */
/*					for( int i = 0; i < mfr.fieldcount; i++ )
						Console.WriteLine( mfr.fieldname[i] + " " + mfr.fieldtype[i] );
*/
				}

			}

		}

	}
	
}


