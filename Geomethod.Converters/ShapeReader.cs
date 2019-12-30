using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.Common;

namespace Geomethod.Converters
{
	public	enum	ShapeAttrMode
	{
		ReadAttrByRecord,
		ReadAllTable,
		ReadNothing
	}

	public	class	ShapeReaderException: Exception
	{
		string	mess;
		public	ShapeReaderException( string mess )
		{
			this.mess = mess;
		}
		public	override	string	Message
		{
			get
			{
                return	"Ошибка при загрузке Shape-файла:"  + 
                		" по причине: \n" + mess;
			}
		}
	}

	public class ShapeFileReader : IGisDataReader<ShapeObject, ShapeUnit>, IDisposable
	{

		string				dirName;
		string				fileName;
		string				file;
		BinaryReader		br;
		FileStream			fs;
		public	bool		opened	= false;
		DBFConnection		dbf		= null;
		OdbcDataReader		table	= null;

		public	string		eMessage;

		private	ShapeObject	current = null;
		private	object[]	attrs	= null;
		
		private	ShapeUnit	shapeType;
		private	ArrayList	attrNames = null;
		private	ArrayList	attrTypes = null;
		public	Boundary	bound;

		IDbConnection		conn;
		ShapeAttrMode		attrMode;
		DataTable			dt;

		private	void	Open()
		{
			fs = new FileStream( file, System.IO.FileMode.Open );
			br = new BinaryReader( fs );
			if( br.BaseStream.CanRead )
				opened = true;

            this.ReadHeader();
			if( attrMode == ShapeAttrMode.ReadAttrByRecord )
				OpenDBF();

			if( attrMode == ShapeAttrMode.ReadAllTable )
				FillDataTable( dt );
		}

		public	ShapeFileReader( BinaryReader br )
		{
			this.br = br;
			if( br.BaseStream.CanRead )
				opened = true;

			attrMode = ShapeAttrMode.ReadNothing;
		}
		public ShapeUnit GetUnitType( )
		{
			return shapeType;
		}
		public	ShapeFileReader( string file, IDbConnection	conn,	DataTable dt )
		{
			fileName	= Path.GetFileNameWithoutExtension( file ).ToString();
			dirName		= Path.GetDirectoryName( file ).ToString();
			this.conn	= conn;
			this.file	= file;
			this.dt		= dt;
			attrMode	= ShapeAttrMode.ReadAllTable;
			this.conn	= conn;
			Open();
		}

		public	ShapeFileReader( string file )
		{

			fileName	= Path.GetFileNameWithoutExtension( file ).ToString();
			dirName		= Path.GetDirectoryName( file ).ToString();
			this.file	= file;
			attrMode	= ShapeAttrMode.ReadNothing;
//          attrMode = ShapeAttrMode.ReadAttrByRecord;
			Open();
		}

		public	ShapeFileReader( string file, IDbConnection conn )
		{
			fileName	= Path.GetFileNameWithoutExtension( file ).ToString();
			dirName		= Path.GetDirectoryName( file ).ToString();
			this.file	= file;
            attrMode	= ShapeAttrMode.ReadAttrByRecord;
			this.conn	= conn;
			Open();
		}

		private	void	ReadHeader()
		{
			try
			{
				br.BaseStream.Seek( 32, SeekOrigin.Begin );
				shapeType = (ShapeUnit)br.ReadUInt32();
				bound = new Boundary( br );
				br.BaseStream.Seek( 100, SeekOrigin.Begin );
			}
			catch( Exception e )
			{
				eMessage = "Ошибка чтения файла: " + file + e.ToString();
				throw	new ShapeReaderException( "Ошибка чтения файла" );
			}
            //      Не можем читать нереализованные shape-объекты

            if (shapeType != ShapeUnit.Point &&
                shapeType != ShapeUnit.Arc &&
                shapeType != ShapeUnit.PolyLine &&
                shapeType != ShapeUnit.Polygon &&
                shapeType != ShapeUnit.MultiPoint)
                throw new ShapeReaderException("Чтение объектов " + shapeType.ToString() + " не реализовано");
    	}

		private	void	FillDataTable( DataTable dt )
		{
			if( attrMode == ShapeAttrMode.ReadAllTable )
			{
				if( conn is OdbcConnection )
				{
					using( OdbcDataAdapter	da = new OdbcDataAdapter( "select * from " + fileName, 
							   (OdbcConnection)conn ) )
					{
						da.Fill( dt );
						dt.TableName = fileName;
					}
				}
				if( conn is OleDbConnection )
				{
					using( OleDbDataAdapter	da = new OleDbDataAdapter( "select * from " + fileName, 
							   (OleDbConnection)conn ) )
					{
						da.Fill( dt );
						dt.TableName = fileName;
					}
				}
			}
		}

		public	bool	OpenDBF()
		{
			dbf = null;
			dbf = new DBFConnection( conn );

			attrNames = new ArrayList();
			attrTypes = new ArrayList();
			dbf.ReadHeader( this.fileName, ref attrNames, ref attrTypes );
			dbf.OpenAttrTable( fileName, out table );
			attrs	= new object[ table.FieldCount ];
			return	true;
		}

		public	void	Dispose( )
		{
			br.Close();
			fs.Close();
			if( attrMode == ShapeAttrMode.ReadAttrByRecord )
				dbf.CloseAttrTable( table );

		}

		public	bool	Read( )
		{

			try
			{
				if( br.PeekChar() == -1 )
					return	false;

				ushort	s1 = br.ReadUInt16();
				ushort	s2 = br.ReadUInt16();
				ushort	s3 = br.ReadUInt16();
				ushort	s4 = br.ReadUInt16();

				int i3 = br.ReadInt32();

				current = null;
				switch( shapeType )
				{
					case	ShapeUnit.Point:	
					{
						current = new ShapePoint( br );
						break;
					}
					case		ShapeUnit.MultiPoint:
					{
						current = new ShapePointGroup( br );
						break;
					}
					case		ShapeUnit.Arc:
					{
						current = new ShapeArc( br );
						break;
					}
					case	ShapeUnit.Polygon:
					{
						current = new ShapePolygon( br );
						break;
					}
					case	ShapeUnit.PolyLine:
					{
						current = new ShapePolyline( br );
						break;
					}
					default:
					{
						current = null;
						break;
					}
				}

				if( attrMode == ShapeAttrMode.ReadAttrByRecord )
					NextAttr();

				return	true;
			}
			catch( Exception e )
			{
				throw	new ShapeReaderException( "Ошибка чтения файла" + e.ToString() );
			}

		}
		private	void	NextAttr()
		{
			if( attrMode == ShapeAttrMode.ReadAttrByRecord )
			{
				table.Read();
				for( int i = 0; i < table.FieldCount; i++ )
					attrs[ i ] = table[ i ];
			}
		}

        public	ShapeObject	 Get(out object[] attrs)
        {
            attrs = this.attrs;
            return current;
        }
        public	ShapeObject	Get()
        {
            return current;
        }

        public object[] GetAttrNames()
		{
			if( attrMode ==ShapeAttrMode.ReadAttrByRecord )
				return	attrNames.ToArray();

			return	null;
		}
		public	object[]	GetAttrTypes()
		{
			if( attrMode == ShapeAttrMode.ReadAttrByRecord )
				return	attrTypes.ToArray();

			return	null;
		}
	}
}
