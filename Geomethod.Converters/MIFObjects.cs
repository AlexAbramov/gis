using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Globalization;


namespace Geomethod.Converters
{
	public	enum	MIFUnit: uint
	{
		Point,
		Line,
		Polyline,
		Region,
		Arc,
        Text,
		Rectangle,
		RoundedRectangle,
		Ellipse,
        Null
	}

	public	class	MPoint
	{
		public  double	X;
		public  double	Y;
//        NumberFormatInfo nfi;

		public	MPoint( double X, double Y )
		{
			this.X = X;
			this.Y = Y;
		}
        public MPoint( MIFFileReader mr, NumberFormatInfo nfi )
        {
            mr.ReadLine( );
            string[] strs = mr.curStr.Split( ' ' );
            X = double.Parse( strs[ 0 ], nfi );
            Y = double.Parse( strs[ 1 ], nfi );
        }
    }

	public	class	MIFSymbol
	{
        public  string symbol;
		
		public	MIFSymbol( string str )
		{
            symbol = str;
		}
        public MIFSymbol(MIFFileReader mr)
        {
            symbol = mr.curStr;
            mr.ReadLine();
        }
	}

	public	class	MIFPen
	{
        public  int width, pattern, color;

        public MIFPen(string str)
        {
            string[] strs = str.Split(' ');
            string[] strs1 = strs[1].Replace('(', ' ').Replace(')', ' ').Split(',');
            width = Int32.Parse(strs1[0]);
            pattern = Int32.Parse(strs1[1]);
            color = Int32.Parse(strs1[2]);
        }

        public MIFPen(MIFFileReader mr)
        {
            string[] strs = mr.curStr.Split(' ');
            string[] strs1 = strs[1].Replace('(', ' ').Replace(')', ' ').Split(',');
            width   = Int32.Parse(strs1[0]);
            pattern = Int32.Parse(strs1[1]);
            color   = Int32.Parse(strs1[2]);
            mr.ReadLine();
        }
    }
	public	class	MIFBrush
	{
        public  int pattern, forecolor, backcolor;

        public MIFBrush(string str)
        {
            string[] strs = str.Split(' ');
            string[] strs1 = strs[1].Replace('(', ' ').Replace(')', ' ').Split(',');
            pattern = Int32.Parse(strs1[0]);
            forecolor = Int32.Parse(strs1[1]);
            if (strs1.Length == 3)
                backcolor = Int32.Parse(strs1[2]);
        }
        public MIFBrush(MIFFileReader mr)
        {
            string[] strs = mr.curStr.Split(' ');
            string[] strs1 = strs[1].Replace('(', ' ').Replace(')', ' ').Split(',');
            pattern     = Int32.Parse(strs1[0]);
            forecolor   = Int32.Parse(strs1[1]);
            if (strs1.Length == 3)
                backcolor = Int32.Parse(strs1[2]);
            mr.ReadLine();
        }
    }
	public	class	MIFFont
	{
        public  string fontname;
        public  string style;
        public  int size;
        public  int forecolor;
        public  int backcolor;

        public MIFFont(string str)
        {
            string[] strs = str.Replace(',', ' ').Replace('(', ' ').Replace(')', ' ').Split(' ');
            fontname = strs[1];
            style = strs[2];
            size = Int32.Parse(strs[3]);
            forecolor = Int32.Parse(strs[4]);
            if (strs.Length == 5)
                backcolor = Int32.Parse(strs[5]);

        }
        public MIFFont(MIFFileReader mr)
        {
            string[] strs = mr.curStr.Replace(',', ' ').Replace('(', ' ').Replace(')', ' ').Split(' ');
            fontname = strs[1];
            style = strs[2];
            size = Int32.Parse(strs[3]);
            forecolor = Int32.Parse(strs[4]);
            if (strs.Length == 5)
                backcolor = Int32.Parse(strs[5]);
            mr.ReadLine();
        }

	}
	public	class	MIFObject
	{
		int line;
		protected	MIFObject( int line )
		{
			this.line = line;
		}
	}

	public	class	MIFPoint: MIFObject
	{
		static	public	int	cnt;
		public	MPoint		point;
		public	MIFSymbol	symbol;

        public MIFPoint( MIFFileReader mr, NumberFormatInfo nfi ) : base( mr.mifNLine )
		{
//			line = mr.RowLine;

			cnt++;
            string[] strs = mr.curStr./*Replace( '.', ',' ).*/Split( ' ' );
            point = new MPoint( double.Parse( strs[ 1 ], nfi ), double.Parse( strs[ 2 ], nfi ) );

			if( mr.curStr != null && mr.curStr.StartsWith( "symbol", StringComparison.OrdinalIgnoreCase ) )
				symbol = new MIFSymbol( mr );
            mr.ReadLine( );
		}
	}

	public class MIFLine: MIFObject
	{
		static	public	int	cnt;
		public	MPoint	point1,	point2;
		public	MIFPen	pen;

        public MIFLine( MIFFileReader mr, NumberFormatInfo nfi ): base( mr.mifNLine )
		{
			cnt++;
			string[]	strs = mr.curStr.Split( ' ' );
			point1 = new MPoint( double.Parse( strs[1], nfi ), double.Parse( strs[2], nfi ) );	
			point2 = new MPoint( double.Parse( strs[3], nfi ), double.Parse( strs[4], nfi ) );	

			mr.ReadLine();
			if( mr.curStr != null && mr.curStr.StartsWith( "pen", StringComparison.OrdinalIgnoreCase ) )
				pen = new MIFPen( mr );
		}
	}

	public class MIFPolyline: MIFObject
	{
		static	public	int	cnt;
        public  MPoint[][] points;
        public  int parts = 1;
		public	MIFPen	pen;
		public	bool	smooth = false;

		public MIFPolyline( MIFFileReader mr, NumberFormatInfo nfi ): base( mr.mifNLine )
		{
			cnt++;

			string[]	strs = mr.curStr.Split( ' ' );
			if( strs.Length == 2 )
			{
				parts = 1;
				points = new MPoint[ parts ][];
				int	n = Int32.Parse( strs[1] );
				points[0] = new MPoint[n]; 
				for( int j = 0; j < n; j++ )
				{
					mr.ReadLine( true );					
					string[]	strsn = mr.curStr.Split( ' ' );
					points[0][j] = new MPoint( double.Parse( strsn[0], nfi ), double.Parse( strsn[1], nfi ) );	
				}
			}
			if( strs.Length == 3 )
			{
				parts = Int32.Parse( strs[2] );
				points = new MPoint[ parts ][];
				for( int i = 0; i < parts; i++ )
				{
					mr.ReadLine( true );
					int	n = Int32.Parse( mr.curStr );
					points[i] = new MPoint[n]; 
					for( int j = 0; j < n; j++ )
						points[i][j] = new MPoint( mr, nfi );	

				}
			}			
			mr.ReadLine();
			if( mr.curStr != null && mr.curStr.StartsWith( "pen", StringComparison.OrdinalIgnoreCase ) )
				pen = new MIFPen( mr );

			if( mr.curStr != null && mr.curStr.StartsWith( "smooth", StringComparison.OrdinalIgnoreCase ) )
			{
				smooth = true;
				mr.ReadLine();
			}
		}
	}
	public class MIFRegion: MIFObject
	{
		static	public	int	cnt;

		public  MPoint[][]	points;
		public  int		parts;

		public	MIFPen		pen;
		public	MIFBrush	brush;
		public	MPoint		center;

		public MIFRegion( MIFFileReader mr, NumberFormatInfo nfi ): base( mr.mifNLine )
		{
			cnt++;

			string[]	strs = mr.curStr.Split( ' ' );
			if( strs.Length == 2 )
			{
				parts = Int32.Parse( strs[1] );
				points = new MPoint[ parts ][];
				for( int i = 0; i < parts; i++ )
				{
                    mr.ReadLine(true);
                    int n = Int32.Parse(mr.curStr);
                    points[i] = new MPoint[n];
                    for (int j = 0; j < n; j++)
						points[i][j] = new MPoint( mr, nfi );	
				}
				mr.ReadLine();
				if( mr.curStr != null && mr.curStr.StartsWith( "pen", StringComparison.OrdinalIgnoreCase ) )
					pen = new MIFPen( mr);

				if( mr.curStr != null && mr.curStr.StartsWith( "brush", StringComparison.OrdinalIgnoreCase ) )
					brush = new MIFBrush( mr);
    
//                if( mr.curStr != null && mr.curStr.ToLower().StartsWith( "center" ) )
//					center = new MPoint( mr );	
			}
		}
	}
	public class MIFArc: MIFObject
	{
		static	public	int	cnt;
        public  MPoint point1, point2;
        public  double a, b;
		public	MIFPen	pen;

		public MIFArc( MIFFileReader mr, NumberFormatInfo nfi )	: base( mr.mifNLine )
		{
			cnt++;

			string[]	strs1 = mr.curStr.Split( ' ' );
			mr.ReadLine( true );
			string[]	strs2 = mr.curStr.Split( ' ' );
			if( strs1.Length == 5 && strs2.Length == 2 )
			{
				point1 = new MPoint( double.Parse( strs1[1], nfi ), double.Parse( strs1[2], nfi ) );	
				point2 = new MPoint( double.Parse( strs1[3], nfi ), double.Parse( strs1[4], nfi ) );	
				a = double.Parse( strs2[0], nfi );
				b = double.Parse( strs2[1], nfi );

				mr.ReadLine();
				if( mr.curStr != null && mr.curStr.StartsWith( "pen", StringComparison.OrdinalIgnoreCase ) )
					pen = new MIFPen( mr );
			}
		}
	}
	public class MIFText: MIFObject
	{
		static	public	int	cnt;
		public	string	text;
        public  MPoint point1, point2;

		public	MIFFont	font;
		public	double	spacing;
		public	string	justify;				// {Left | Center | Right } 
		public	double	angle;
		public	string	labelLineStyle;
		public	MPoint	lableLinePoint;			//		{simple | arrow} x y 

		public MIFText( MIFFileReader mr, NumberFormatInfo nfi ): base( mr.mifNLine )
		{
			cnt++;

			if( mr.textOnNextLine )
			{
				mr.ReadLine( true );
				text = mr.curStr;
				mr.ReadLine( true );
			}
			else
			{
				string[] strs1 = mr.curStr.Split( ' ' );
				text = strs1[ 1 ];
				mr.ReadLine( true );
			}
			string[]	strs2 = mr.curStr.Split( ' ' );
            point1 = new MPoint( double.Parse( strs2[ 0 ], nfi ), double.Parse( strs2[ 1 ], nfi ) );
            point2 = new MPoint( double.Parse( strs2[ 2 ], nfi ), double.Parse( strs2[ 3 ], nfi ) );	
			
			mr.ReadLine( true );
			if( mr.curStr != null && mr.curStr.StartsWith( "font", StringComparison.OrdinalIgnoreCase ) )
				font = new MIFFont( mr);

			if( mr.curStr != null && mr.curStr.StartsWith( "spacing", StringComparison.OrdinalIgnoreCase ) )
			{
				string[]	strs7 = mr.curStr.Split( ' ' );
                spacing = double.Parse( strs7[ 1 ], nfi );
				mr.ReadLine();
			}
			if( mr.curStr != null && mr.curStr.StartsWith( "justify", StringComparison.OrdinalIgnoreCase ) )
			{
				string[]	strs3 = mr.curStr.Split( ' ' );
				justify = strs3[1];
				mr.ReadLine();
			}
			if( mr.curStr != null && mr.curStr.StartsWith( "angle", StringComparison.OrdinalIgnoreCase ) )
			{
				string[]	strs4 = mr.curStr.Split( ' ' );
                angle = double.Parse( strs4[ 1 ], nfi );
				mr.ReadLine();
			}
			if( mr.curStr != null && mr.curStr.StartsWith( "lable", StringComparison.OrdinalIgnoreCase ) )
			{
				string[]	strs5 = mr.curStr.Split( ' ' );
				labelLineStyle = strs5[1];
                lableLinePoint = new MPoint( double.Parse( strs5[ 2 ], nfi ), double.Parse( strs5[ 3 ], nfi ) );									
				mr.ReadLine();
			}
		}
	}
	public class MIFRectangle: MIFObject
	{
		static	public	int	cnt;
        public  MPoint point1, point2;
		public	MIFPen		pen;
		public	MIFBrush	brush;

		public MIFRectangle( MIFFileReader mr, NumberFormatInfo nfi ): base( mr.mifNLine )
		{
			cnt++;

			string[]	strs1 = mr.curStr.Split( ' ' );
			if( strs1.Length == 5 )
			{
                point1 = new MPoint( double.Parse( strs1[ 1 ], nfi ), double.Parse( strs1[ 2 ], nfi ) );
                point2 = new MPoint( double.Parse( strs1[ 3 ], nfi ), double.Parse( strs1[ 4 ], nfi ) );	
			}

			mr.ReadLine();
			if( mr.curStr != null && mr.curStr.StartsWith( "pen", StringComparison.OrdinalIgnoreCase ) )
				pen = new MIFPen( mr);

			if( mr.curStr != null && mr.curStr.StartsWith( "brush", StringComparison.OrdinalIgnoreCase ) )
				brush = new MIFBrush( mr );
		}
	}
	public class MIFRoundedRectangle: MIFObject
	{
		static	public	int	cnt;
        public  MPoint point1, point2;
        public  double a;
		public	MIFPen		pen;
		public	MIFBrush	brush;


		public MIFRoundedRectangle( MIFFileReader mr, NumberFormatInfo nfi ) : base( mr.mifNLine )
		{
			cnt++;
			string[]	strs1 = mr.curStr.Split( ' ' );
			if( strs1.Length == 5 )
			{
                point1 = new MPoint( double.Parse( strs1[ 1 ], nfi ), double.Parse( strs1[ 2 ], nfi ) );
                point2 = new MPoint( double.Parse( strs1[ 3 ], nfi ), double.Parse( strs1[ 4 ], nfi ) );	
			}
			mr.ReadLine( true );
            a = double.Parse( mr.curStr, nfi );

			if( mr.curStr != null && mr.curStr.StartsWith( "pen", StringComparison.OrdinalIgnoreCase ) )
				pen = new MIFPen( mr );

			if( mr.curStr != null && mr.curStr.StartsWith( "brush", StringComparison.OrdinalIgnoreCase ) )
				brush = new MIFBrush( mr );
		}
	}
	public class MIFEllipse: MIFObject
	{
		static	public	int	cnt;
        public  MPoint point1, point2;
		public	MIFPen		pen;
		public	MIFBrush	brush;


		public MIFEllipse( MIFFileReader mr, NumberFormatInfo nfi ) : base( mr.mifNLine )
		{
			cnt++;

            string[] strs1 = mr.curStr.Split( ' ' );
			if( strs1.Length == 5 )
			{
                point1 = new MPoint( double.Parse( strs1[ 1 ], nfi ), double.Parse( strs1[ 2 ], nfi ) );
                point2 = new MPoint( double.Parse( strs1[ 3 ], nfi ), double.Parse( strs1[ 4 ], nfi ) );	
			}

			mr.ReadLine();
			if( mr.curStr != null && mr.curStr.StartsWith( "pen", StringComparison.OrdinalIgnoreCase ) )
				pen = new MIFPen( mr );

			if( mr.curStr != null && mr.curStr.StartsWith( "brush", StringComparison.OrdinalIgnoreCase ) )
				brush = new MIFBrush( mr );
		}
	}

}
