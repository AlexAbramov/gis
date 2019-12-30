using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Globalization;
using System.Text;

namespace Geomethod.Converters
{

	public interface IGisDataReader<Type, Unit>
	{
		bool	Read( );
		Type	Get();
		Unit	GetUnitType( );

	}

	public	class	MIFReaderException: Exception
	{
		string	mess;
        public MIFReaderException( string mess )
        {
            this.mess = mess;
        }
        public MIFReaderException( string mess, int line )
        {
            this.mess = mess  + "(line: " + line + ")";
        }
        public override string Message
        {
            get
			{
/*				return	"Ошибка при загрузке MIF-файла:\n" + mess + 
					" по причине: \n" + base.Message;
 */
                return mess;
			}
		}
     }

	public class MIFCoordSys
	{
		public	string	row;
		public MPoint p1, p2;

		public MIFCoordSys( string row, NumberFormatInfo nfi )
		{
			this.row = row;

			string str = row.Substring( row.IndexOf( "bounds", StringComparison.OrdinalIgnoreCase ) );
			string[] strs = str.Split( new char[] {' ', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries );
			p1 = new MPoint( Double.Parse( strs[ 1 ], nfi ), Double.Parse( strs[ 2 ], nfi ) );
			p2 = new MPoint( Double.Parse( strs[ 3 ], nfi ), Double.Parse( strs[ 4 ], nfi ) );
		}
	}

	public class MIFFileReader : IGisDataReader<MIFObject, MIFUnit>, IDisposable
	{


//		Заголовок файла
		public	string		version;
		public	string		charset;
        public  char        delimiter = '\t';
		public	string		unique;
		public	string		index;
		public	MIFCoordSys	coordsys;
		public	string		transform;
		public	int			fieldcount = 0;


		private string	    fileName;
		private string	    dirName;
		private string	    file;
		private StreamReader	sr;
		private StreamReader	mid;

		public	string	    eMessage;
		public  string	    curStr;
		private	MIFObject	current = null;
		private	MIFUnit		mifType;

        public  object[]    attrs;
        private string      midLine;
		private	ArrayList	attrNames	= null;
		private	ArrayList	attrTypes	= null;

        public  int         mifNLine    = 0;
        public  int         midNLine    = 0;
        public  int         cnt         = 0;

        public bool readMID = false;
		public bool keepOriginalBounds;
		public bool textOnNextLine = false;
		NumberFormatInfo nfi;
//		public Encoding encoding = Encoding.ASCII;
//		public Encoding encoding = Encoding.Unicode;

		public Encoding encoding = Encoding.GetEncoding( 1251 );


        public string RowLine()
        {
            if( readMID )
                return midLine;
            else
                return null;
        }

		public MIFUnit GetUnitType( )
		{
			return mifType;
		}

		public MIFFileReader( string file, bool readMID, bool textOnNextLine )
        {
            fileName = Path.GetFileNameWithoutExtension( file ).ToString( );
            dirName = Path.GetDirectoryName( file ).ToString( );
            this.file = file;
            this.readMID = readMID;
			this.nfi = new NumberFormatInfo( );
			this.textOnNextLine = textOnNextLine;
            Open( );
        }
        public MIFFileReader( string file, bool readMID, bool textOnNextLine, NumberFormatInfo nfi )
        {
            fileName = Path.GetFileNameWithoutExtension( file ).ToString( );
            dirName = Path.GetDirectoryName( file ).ToString( );
            this.file = file;
            this.readMID = readMID;
			this.nfi = nfi;
			this.textOnNextLine = textOnNextLine;
            Open( );
        }


		public	void	ReadLine( bool req )
		{
			curStr = sr.ReadLine( );
			mifNLine++;
			if( req )
			{
				if( curStr == null )
					throw new MIFReaderException( "Ожидается строка " );
			}
			curStr = curStr.Trim();
		}

		public	void	ReadLine( )
		{
			curStr = sr.ReadLine();
			mifNLine++;
            if ( curStr != null )
                curStr = curStr.Trim( );

		}

		private	void	Open()
		{
			try
			{
				sr = new StreamReader( file, encoding );
				this.ReadHeader();
				
                if( readMID )
				    mid = new StreamReader( Path.GetFileNameWithoutExtension( file ) + ".mid", encoding ); 
		
			}
			catch( Exception e )
			{
				eMessage = e.ToString();
				throw new MIFReaderException( "Ошибка при открытии :" + e.Message );
			}
		}

		public	void Dispose( )
		{
			sr.Close();
            if( readMID )
			    mid.Close();
		}
		
		public	bool	Read()
		{
//			ReadLine();
			for( ;; )
			{
				if( curStr == null )
					return	false;

				if( curStr.Length == 0 )
				{
					ReadLine();
					continue;
				}
                cnt++;
				if( curStr.StartsWith( "point", StringComparison.OrdinalIgnoreCase ) )
				{
                    mifType = MIFUnit.Point;
                    current = new MIFPoint( this, nfi );
					NextAttr();
					return	true;
				}
				if( curStr.StartsWith( "line", StringComparison.OrdinalIgnoreCase ) )
				{
                    mifType = MIFUnit.Line;
                    current = new MIFLine( this, nfi );
					NextAttr();
					return	true;
				}
				if( curStr.StartsWith( "pline", StringComparison.OrdinalIgnoreCase ) )
				{
                    mifType = MIFUnit.Polyline;
					current	= new MIFPolyline( this, nfi );
					NextAttr();
					return	true;
				}
				if( curStr.StartsWith( "region", StringComparison.OrdinalIgnoreCase ) )
				{
                    mifType = MIFUnit.Region;
					current	= new MIFRegion( this, nfi  );
					NextAttr();
					return	true;
				}
				if( curStr.StartsWith( "arc", StringComparison.OrdinalIgnoreCase ) )
				{
                    mifType = MIFUnit.Arc;
                    current = new MIFArc( this, nfi );
					NextAttr();
					return	true;
				}
				if( curStr.StartsWith( "text", StringComparison.OrdinalIgnoreCase ) )
				{
                    mifType = MIFUnit.Text;
                    current = new MIFText( this, nfi );
					NextAttr();
					return	true;
				}
				if( curStr.StartsWith( "rect", StringComparison.OrdinalIgnoreCase ) )
				{
                    mifType = MIFUnit.Rectangle;
                    current = new MIFRectangle( this, nfi );
					NextAttr();
					return	true;
				}
				if( curStr.StartsWith( "roundrect", StringComparison.OrdinalIgnoreCase ) )
				{
                    mifType = MIFUnit.RoundedRectangle;
                    current = new MIFRoundedRectangle( this, nfi );
					NextAttr();
					return	true;
				}
				if( curStr.StartsWith( "ellipse", StringComparison.OrdinalIgnoreCase ) )
				{
                    mifType = MIFUnit.Ellipse;
                    current = new MIFEllipse( this, nfi );
					NextAttr();
					return	true;
				}	
				ReadLine();
			}			
		}

		private	void	ReadHeader()
		{
			for( ;; )
			{
/*                if ((curStr = sr.ReadLine()) == null)
                {
                    throw new MIFReaderException("Ошибка чтения заголовка");
                }
*/
				ReadLine( );
				if( curStr == null )
					throw new MIFReaderException( "Ошибка чтения заголовка" );

				if( curStr.StartsWith( "version", StringComparison.OrdinalIgnoreCase ) )
					version = curStr;

				if( curStr.StartsWith( "charset", StringComparison.OrdinalIgnoreCase ) )
					charset = curStr;

				if( curStr.StartsWith( "delimiter", StringComparison.OrdinalIgnoreCase ) )
                {
                    string[] strs = curStr.Split(' ');
                    if (strs[1].ToCharArray()[0] == '"')
                        delimiter = strs[1].ToCharArray()[1];
                    else
                        delimiter = strs[1].ToCharArray()[0];
                }
				if( curStr.StartsWith( "unique", StringComparison.OrdinalIgnoreCase ) )
					unique = curStr;

				if( curStr.StartsWith( "coordsys", StringComparison.OrdinalIgnoreCase ) )
					coordsys = new MIFCoordSys( curStr, nfi );

//				if( curStr.ToLower().StartsWith( "transform" ) )
//					coordsys = curStr;

				if( curStr.StartsWith( "columns", StringComparison.OrdinalIgnoreCase ) )
					ReadFields();

				if( curStr.StartsWith( "Data", StringComparison.OrdinalIgnoreCase ) )
					break;

			}

/*            if ((curStr = sr.ReadLine()) == null)
            {
                throw new MIFReaderException("Ошибка чтения заголовка");
            }
*/
			ReadLine( );
			if( curStr == null )
				throw new MIFReaderException( "Ошибка чтения заголовка" );

		}
		private	void	ReadFields()
		{
			string[]	strs1 = curStr.Split( ' ' );
			fieldcount = Int32.Parse( strs1[ 1 ] );

			attrTypes	= new ArrayList( fieldcount );
			attrNames	= new ArrayList( fieldcount );

			for( int i = 0; i < fieldcount; i++ )
			{
				ReadLine( true );
				string[]	strs = curStr.Split( ' ' );					
				attrNames.Add( strs[ 0 ] );
				attrTypes.Add( strs[ 1 ] );
			}
		}

        public	MIFObject   Get(out object[] attrs)
        {
            attrs = this.attrs;
            return current;
        }
        public	MIFObject	Get()
        {
            return current;
        }

		private	void	NextAttr()
		{
            if( !readMID )
                return;

            midLine = mid.ReadLine();
            char[] del = new char[] { delimiter };
            string[] RowAttrs = RowLine( ).Split( del );

            attrs = new object[ RowAttrs.Length ];
            RowAttrs.CopyTo( attrs, 0 );
            midNLine++;
		}


		public	object[]	GetAttrNames()
		{
            if( readMID )
                return attrNames.ToArray( );
            else
                return  null;
        }
		public	object[]	GetAttrTypes()
		{
            if( readMID )
                return attrTypes.ToArray( );
            else
                return null;
		}

        public object this[ string name ]
        {
            get
            {
                return  GetAttr( name ); 
            }
        }

        public object GetAttr( int i )
        {
            return attrs[ i ];
        }
        public object GetAttr( string name )
        {
            int i = Array.IndexOf( attrNames.ToArray( ), name );  
            return attrs[ i ];
        }
    }
}
