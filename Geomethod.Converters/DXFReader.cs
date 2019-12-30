using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Geomethod.Converters
{
	public enum DXFLogBehaviour
	{
		LogToTextFile,
		StoreToList,
		Nothing
	}
	public enum DXFEntityLogBehaviour
	{
		LogToTextFile,
		StoreToList,
		Nothing
	}

	public class DXFReaderException : Exception
	{
		string mess;
		public DXFReaderException( string mess )
		{
			this.mess = mess;
		}
		public DXFReaderException( string mess, int line )
		{
			this.mess = mess + "(line: " + line + ")";
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

	public class DXFFileReader : IGisDataReader<DXFObject, DXFUnit>, IDisposable
    {
		public static int unitcount = 0;
		public static int pointcount = 0;
		public static int dxfline;

        private string file;
        private StreamReader dxSourcefFile;
		public	StreamWriter entityLog;
		public	StreamWriter structurefLog;

		private DXFObject current;
		private DXFPair dxfPair;

		public DXFLogBehaviour logStructureBehaviour;
		public DXFEntityLogBehaviour logEntityBehaviour;

        public DXFFileReader( string file )
        {
			dxfline = 0;
            this.file = file;

			logStructureBehaviour = DXFLogBehaviour.LogToTextFile;
			logEntityBehaviour = DXFEntityLogBehaviour.LogToTextFile;

            Open( );
			SkipSections( );
        }

        public void Dispose( )
        {
			if( dxSourcefFile != null )
				dxSourcefFile.Close( );

			if( entityLog != null )
				entityLog.Close( );

			if( structurefLog != null )
				structurefLog.Close( );
		}

		public DXFObject Get( )
		{
			return current;
		}
		public bool Read( )
		{
			return	ReadaEntity( );
		}
		public	DXFUnit GetUnitType( )
		{
			if( current == null )
				return DXFUnit.Null;
			return current.type;
		}
		private bool	Open()
		{
			dxSourcefFile = new StreamReader( file );
			if( logEntityBehaviour == DXFEntityLogBehaviour.LogToTextFile )
				entityLog = new StreamWriter( file + ".entities", false );

			if( logStructureBehaviour == DXFLogBehaviour.LogToTextFile )
				structurefLog = new StreamWriter( file + ".structure", false );

			return true;
		}

		private void EntityLog( string value )
		{
			if( logEntityBehaviour == DXFEntityLogBehaviour.LogToTextFile )
				entityLog.WriteLine( value );
		}
		private void EntityLog( string value, bool anew )
		{
			if( logEntityBehaviour == DXFEntityLogBehaviour.LogToTextFile )
			{
				if( anew )
					entityLog.WriteLine();
				entityLog.WriteLine( value );
			}
		}

		public	bool ScanAll( )
		{

			dxfPair = new DXFPair( dxSourcefFile, this );
			for( ; ; )
			{
				if( dxfPair == null )
					break;

				if( dxfPair == "EOF" )
				{
					dxfPair.Log( 0 );
					break;
				}
				if( dxfPair == "SECTION" )
					ScanSection();
			}
			return true;
		}

		private bool SkipSections( )
		{
			dxfPair = new DXFPair( dxSourcefFile, this );
			for( ; ; )
			{
				if( dxfPair == null )
					break;

				if( dxfPair == "EOF" )
				{
					dxfPair.Log( 0 );
					break;
				}
				if( dxfPair == "SECTION" )
				{
					dxfPair.Log( 0 );
					dxfPair = new DXFPair( dxSourcefFile, this );
					if( dxfPair == null )
						return false;

					if( dxfPair != "ENTITIES" )
					{
						if( !ReadaSection( ) )
							return false;
					}
					else
					{
						dxfPair.Log( 0 );
						dxfPair = new DXFPair( dxSourcefFile, this );
						break;
					}
				}
			}
			return true;
		}

		private bool ScanSection()
		{
			dxfPair.Log( 0 );
			dxfPair = new DXFPair( dxSourcefFile, this );
			if( dxfPair == null )
				return	false;

			if( dxfPair == "HEADER" )
				return	ReadHeader( );
			if( dxfPair == "CLASSES" )
				return	ReadClasses( );
			if( dxfPair == "TABLES" )
				return	ReadTables( );
			if( dxfPair == "BLOCKS" )
				return	ReadBlocks( );
			if( dxfPair == "ENTITIES" )
				return	ReadEntities( );
			if( dxfPair == "THUMBNAILIMAGE" )
				return	ReadTNI( );
			if( dxfPair == "OBJECTS" )
				return	ReadObjects( );

			return false;
		}

		private	bool	ReadaSection( )
		{
			dxfPair.Log( 0 );
			dxfPair = new DXFPair( dxSourcefFile, this );
			for( ; dxfPair != null; )
			{
				if( dxfPair == "ENDSEC" )
				{
					dxfPair.Log( 0 );
					dxfPair = new DXFPair( dxSourcefFile, this );
					return true;
				}
				dxfPair = new DXFPair( dxSourcefFile, this );
			}
			return	true;
		}

		private	bool	ReadHeader( )
		{
			return	ReadaSection( );
		}
		private bool ReadClasses( )
		{
			return ReadaSection( );
		}
		private bool ReadTables( )
		{
			return ReadaSection( );
		}
		private bool ReadBlocks( )
		{
			return ReadaSection( );
		}
		private bool ReadTNI( )
		{
			return ReadaSection( );
		}
		private bool ReadObjects( )
		{
			return ReadaSection( );
		}
		private bool ReadEntities( )
		{
			dxfPair.Log(0);
			dxfPair = new DXFPair( dxSourcefFile, this );

			for( ; dxfPair != null ; )
			{
				if( !ReadaEntity( ) )
					return	false;

				if( dxfPair == "ENDSEC" )
				{
					dxfPair.Log(0);
					dxfPair = new DXFPair( dxSourcefFile, this );
					return	true;
				}
			}
			return	true;
		}

		private bool	ReadaEntity( )
		{
			if( dxfPair == "ENDSEC" )
				return false;

			if( dxfPair == "LINE" )
			{
				current = new DXFObject( DXFUnitNative.LINE );
				EntityLog( dxfPair.Value, true );
				return ReadLine( current  );
			}

			if( dxfPair == "TEXT" )
			{
				current = new DXFObject( DXFUnitNative.TEXT );
				EntityLog( dxfPair.Value, true );
				return GetXYFromDXFEntity( current, true, true );
			}

			if( dxfPair == "POINT" )
			{
				current = new DXFObject( DXFUnitNative.POINT );
				EntityLog( dxfPair.Value, true );
				return GetXYFromDXFEntity( current, true, true );
			}

			if( dxfPair == "POLYLINE" )
			{
				DXFFileReader.unitcount++;

				current = new DXFObject( DXFUnitNative.POLYLINE );
				EntityLog( dxfPair.Value, true );
				if( !GetXYFromDXFEntity( current, false, true ) )
					return	false;

				for( ;; )
				{
					if( dxfPair == "VERTEX" )
					{
						if( !GetXYFromDXFEntity( current, true, false ) )
							break;
						continue;
					}
					if( dxfPair == "SEQEND" )
					{
						if( !GetXYFromDXFEntity( current, false, false ) )
							break;
						return	true;
					}
				}
				return	true;
			}

			EntityLog( "\n" + dxfPair.Value );
			dxfPair.Log( 4 );

			for( ; dxfPair != null ; )
			{
				dxfPair = new DXFPair( dxSourcefFile, this );
				if( dxfPair == null )
					return	true;

//				if( dxfPair == 8 )
//					dxfPair.Log( 5 );

				if( dxfPair == 0 )
				{
					EntityLog( "\n" + dxfPair.Value );
					dxfPair.Log( 4 );
					break;
				}
			}
			return	true;
		}

		public bool	ReadLine( DXFObject entity )
		{

			dxfPair.Log( 4 );

			double	X1 = 0,	Y1 = 0,	X2 = 0,	Y2 = 0;
			for( ; dxfPair != null ; )
			{
				dxfPair = new DXFPair( dxSourcefFile, this );
				if( dxfPair == null )
					return	true;

				if( dxfPair == 0 )
					break;

/*				if( dxfPair == 8 )
					EntityLog( "\t\t\t\t\t" + dxfPair.Value );			
				if( dxfPair == 1 )
					EntityLog( "\t\t\t\t\t" + dxfPair.Value );
*/
				if( dxfPair == 10 )
					X1 = Double.Parse( dxfPair.Value );
				if( dxfPair == 20 )
					Y1 = Double.Parse( dxfPair.Value );
				if( dxfPair == 11 )
					X2 = Double.Parse( dxfPair.Value );
				if( dxfPair == 21 )
					Y2 = Double.Parse( dxfPair.Value );
			}
			EntityLog( Y1 + " " + X1 + " " + Y2 + " " + X2 );
			entity.Add( new _DXFPoint( X1, Y1 ) );
			entity.Add( new _DXFPoint( X2, Y2 ) );
			return	true;
		}

		public bool	GetXYFromDXFEntity( DXFObject entity, bool write, bool first )
		{

			dxfPair.Log(4);

			double	X = 0,	Y = 0;
			for( ; dxfPair != null ; )
			{

				dxfPair = new DXFPair( dxSourcefFile, this );
				if( dxfPair == null )
					return	true;

				if( dxfPair == 0 )
					break;

/*				if( first )
				{
					if( dxfPair == 8 )
						EntityLog( "\t\t\t\t\t" + dxfPair.Value );
					if( dxfPair == 1 )
						EntityLog( "\t\t\t\t\t" + dxfPair.Value );
				}
*/
				if( dxfPair == 10 )
					X = Double.Parse( dxfPair.Value );

				if( dxfPair == 20 )
					Y = Double.Parse( dxfPair.Value );

				if( dxfPair == 1 )
					entity.text = dxfPair.Value;

				if( dxfPair == 50 )
					entity.angle = Single.Parse( dxfPair.Value );

			}
			if( write )
			{
				entity.Add( new _DXFPoint( X, Y ) );
				EntityLog( X + "\t" + Y );
			}
			return	true;
		}

	}

	class DXFPair
	{

		private DXFFileReader dxfReader;

		private	long	line;
		public	long	DxfLine	{		get { return line; }	}

		private	int		code;
		private string value;

		public string Value
		{
			get	{	return this.value;	}
		}

        public override int GetHashCode( )
        {
            return base.GetHashCode( );
        }
        public override bool Equals( object obj )
        {
            return base.Equals( obj );
        }

		public static bool operator ==( DXFPair dxfPair, string value )
		{
			return dxfPair.value == value;
		}
		public static bool operator !=( DXFPair dxfPair, string value )
		{
			return dxfPair.value != value;
		}
		public static bool operator ==( DXFPair dxfPair, int code )
		{
			return dxfPair.code == code;
		}
		public static bool operator !=( DXFPair dxfPair, int code )
		{
			return dxfPair.code != code;
		}

/*		public	DXFPair( int code, string value, long line )
		{
			this.code = code;
			this.value = value;
			this.line = line;
		}
*/
		public DXFPair( StreamReader sr, DXFFileReader dxfReader )
		{

			string str = sr.ReadLine( );
			this.dxfReader = dxfReader;	
			code = Int32.Parse( str );
			this.value = sr.ReadLine( );
			this.line = DXFFileReader.dxfline += 2;

		}
		public void Log( int pad )
		{
			if( dxfReader.logStructureBehaviour == DXFLogBehaviour.LogToTextFile )
			{
				for( int i = 0; i < pad; i++ )
					dxfReader.structurefLog.Write( "\t" );
				dxfReader.structurefLog.WriteLine( value );
			}
		}
		public void Log( )
		{
			if( dxfReader.logStructureBehaviour == DXFLogBehaviour.LogToTextFile )
				dxfReader.structurefLog.WriteLine( value );
		}
 
	}

}
