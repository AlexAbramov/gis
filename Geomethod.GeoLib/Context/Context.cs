using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class Context: IDisposable
	{
		GLib lib;
		ByteBuffer buf;
		StyleBuilder sb;
		GmConnection conn;
		Filter filter;
		ICryptographer cryptographer=null;
		GmConnection targetConn=null;
		
		public GLib Lib{get{return lib;}}
		public ByteBuffer Buf{get{if(buf==null) buf=new ByteBuffer(Constants.byteBufferSize,cryptographer); return buf;}}
		StyleBuilder Sb{get{if(sb==null) sb=new StyleBuilder(lib.Colors,lib.Images); return sb;}}
		public Filter Filter{get{return filter;}set{filter=value;}}
		public GmConnection Conn{get{return conn;}}
		public GmConnection TargetConn{get{return targetConn==null ? conn : targetConn;}set{targetConn=value;}}
		public bool ExportMode{get{return targetConn!=null;}}
		internal Context(GLib lib)
		{
			this.lib=lib;
			conn=lib.HasDb ? lib.ConnectionFactory.CreateConnection() : null;
			ResetState();
		}

		public void ResetState(){filter=null;}
		public void Close(){ResetState(); if(conn!=null) conn.Close();}
		public void Dispose()
		{
			Close();
		}
		public bool SetStyle(string newStyleStr,ref string styleStr,ref Style style)
		{
			if(newStyleStr==null) newStyleStr="";
			if(styleStr==newStyleStr) return false;
			Style newStyle=Sb.Parse(newStyleStr);
			bool hasErrors=Sb.CheckErrors();
			if(!hasErrors)
			{
				style=newStyle;
				styleStr=newStyleStr;
			}
			return hasErrors;
		}
	}
}
