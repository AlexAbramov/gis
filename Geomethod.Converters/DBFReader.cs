using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;
using System.IO;

namespace Geomethod.Converters
{

	public	class	DBFConnection
	{
		string	strConnection;
		string	path;
		const	string	drv = "Driver={Microsoft dBase Driver (*.dbf)};DBQ=";
        public	OdbcConnection		con;
		public	bool	opened = false;


		public	DBFConnection( string path, string table )
		{
			this.strConnection = drv + path;
			this.path = path;
		}

		public	DBFConnection( IDbConnection con )
		{
			this.con = (OdbcConnection)con;
		}

		public	bool	Connect()
		{
			if( con.State == ConnectionState.Closed )
			{
//				try
				{
					con   =  new OdbcConnection( this.strConnection );
					con.Open();
					this.opened = true;
				}
//				catch( Exception ex )
//				{
//					MessageBox.Show( ex.ToString() );
//					return	false;
//				}
			}
			return	true;
		}
		public	bool	Close()
		{
			if( con != null && this.opened )
				this.con.Close();
			return	true;

		}
		public	int	Count()
		{
			int	cnt = 0;
			string	strQuery = "select count(*) from mn_st";
//			try
			{
				OdbcCommand cmd = new OdbcCommand( strQuery, con );
				Object	o = cmd.ExecuteScalar();
				if( o.ToString().Length != 0 )
					cnt = Int32.Parse( o.ToString() );

			}
//			catch( Exception ex )
//			{
//				MessageBox.Show( ex.ToString() );
//			}
			return	cnt;

		}

		public	void ReadHeader( string table, ref ArrayList fields, ref ArrayList types )
		{
			string	strQuery = "select * from " + table;
//			try
			{
				OdbcCommand cmd = new OdbcCommand( strQuery, con );
				if( cmd.Connection.State != ConnectionState.Open )
					cmd.Connection.Open();

				using( OdbcDataReader	reader = cmd.ExecuteReader( CommandBehavior.CloseConnection ) )
				{
					DataTable schema = reader.GetSchemaTable();
					for( int i = 0; i < schema.Rows.Count; i++ )
					{
						fields.Add( schema.Rows[i][0].ToString() );
						types.Add( schema.Rows[i][5].ToString() );
					}

				}
			}
//			catch( Exception ex )
//			{
//				MessageBox.Show( ex.ToString() );
//			}
		}

		public	long	Count( string table )
		{
			string	strQuery = "select count(*) from " + table;
			long	cnt = 0;
//			try
			{
				OdbcCommand cmd = new OdbcCommand( strQuery, con );
				if( cmd.Connection.State != ConnectionState.Open )
					cmd.Connection.Open();

				cnt = Int32.Parse( cmd.ExecuteScalar( ).ToString() );
			}
//			catch( Exception ex )
//			{
//				MessageBox.Show( ex.ToString() );
//			}
			return	cnt;
		}
	
		public	int	Read( string table )
		{
			int	cnt = 0;
			string	strQuery = "select * from " + table;
//			try
			{
				OdbcCommand cmd = new OdbcCommand( strQuery, con );
				if( cmd.Connection.State != ConnectionState.Open )
					cmd.Connection.Open();

				using( OdbcDataReader	reader = cmd.ExecuteReader( CommandBehavior.CloseConnection ) )
				{

					using( StreamWriter sw = new StreamWriter( path + "\\" + table + ".txt", false ) ) 
					{

						string	Title = "\t";
						DataTable schema = reader.GetSchemaTable();
						for( int i = 0; i < schema.Rows.Count; i++ )
							Title += schema.Rows[i][0].ToString() + "\t";
								



						sw.WriteLine( Title );
						while( reader.Read())
						{
							string	str = cnt++.ToString();
							sw.WriteLine( str );
						}
					}
				}
			}
//			catch( Exception ex )
//			{
//				MessageBox.Show( ex.ToString() );
//			}
			return	cnt;
		}

		public	bool	OpenAttrTable( string tablename, out OdbcDataReader  table )
		{

			string	strQuery = "select * from " + tablename;
//			try
			{
				OdbcCommand cmd = new OdbcCommand( strQuery, con );
				if( cmd.Connection.State != ConnectionState.Open )
					cmd.Connection.Open();

				table = cmd.ExecuteReader( CommandBehavior.CloseConnection );
			}
//			catch( Exception ex )
//			{
//				MessageBox.Show( ex.ToString() );
//				table = null;
//				return	false;
//			}
			return	true;
		}

		public	void	CloseAttrTable( OdbcDataReader table )
		{
			
			table.Close();

		}

		public	string	GetNextField( OdbcDataReader table, string field )
		{
			table.Read();
			return	table[ field ].ToString();
		}
	}
		
}