using System;
using System.Runtime;
using System.IO;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Data;
using Geomethod;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public interface ICryptographer
	{
		void Encrypt(int[] array, int count);
		void Decrypt(int[] array, int count);
	}

	public class RandomCryptographer: ICryptographer
	{
		int key;
		public RandomCryptographer(int key){this.key=key;}

		#region ICryptographer Members

		public void Encrypt(int[] array, int count)
		{
			Random random=new Random(key);
			for(int i=0;i<count;i++) array[i]+=random.Next();
		}

		public void Decrypt(int[] array, int count)
		{
			Random random=new Random(key);
			for(int i=0;i<count;i++) array[i]-=random.Next();
		}

		#endregion
	}

	public class ByteBuffer
	{
		byte [] byteBuf;
		int [] intBuf;
		ICryptographer cryptographer;
		
		public ByteBuffer(int byteBufLength, ICryptographer cryptographer)
		{
			byteBuf=new byte[byteBufLength];
			intBuf=new int[(byteBufLength/4)+1];
			this.cryptographer=cryptographer;
		}

		#region Aux
		Point PointFromIntBuf(){return new Point(intBuf[0],intBuf[1]);}
		void PointToIntBuf(Point p){intBuf[0]=p.X;intBuf[1]=p.Y;}
		Rect RectFromIntBuf(){return new Rect(intBuf[0],intBuf[1],intBuf[2],intBuf[3]);}
		void RectToIntBuf(Rect rect){intBuf[0]=rect.left;intBuf[1]=rect.top;intBuf[2]=rect.right;intBuf[3]=rect.bottom;}
		void PointsToIntBuf(Point[] points)
		{
			for(int i=0;i<points.Length;i++){intBuf[i*2]=points[i].X;intBuf[i*2+1]=points[i].Y;}
		}
		Point[] PointsFromIntBuf(int pointCount)
		{
			Point[] points=new Point[pointCount];
			for(int i=0;i<pointCount;i++){points[i].X=intBuf[i*2];points[i].Y=intBuf[i*2+1];}
			return points;
		}
		public int[] GetIntArray(byte[] byteArray)
		{
			int byteCount=byteArray.Length;
			if(byteCount%4!=0) throw new Exception("Wrong size of binary field (2): "+byteCount.ToString());
			int intCount=byteCount/4;
			int[] intArray=new int[intCount];
			Buffer.BlockCopy(byteArray,0,intArray,0,byteCount);
			if(cryptographer!=null) cryptographer.Decrypt(intArray,intCount);
			return intArray;
		}
		#endregion
		
		#region DB
		public int[] GetIntArray(IDataReader dr,int col)
		{
			int byteCount=(int)dr.GetBytes(col,0,byteBuf,0,byteBuf.Length);
			if(byteCount%4!=0) throw new Exception("Wrong size of binary field (3): "+byteCount.ToString());
			int intCount=byteCount/4;
			int[] intArray=new int[intCount];
			Buffer.BlockCopy(byteBuf,0,intArray,0,byteCount);
			if(cryptographer!=null) cryptographer.Decrypt(intArray,intCount);
			return intArray;
		}

		public Rect GetRect(IDataReader dr,int col)
		{
			int byteCount=(int)dr.GetBytes(col,0,byteBuf,0,byteBuf.Length);
			if(byteCount!=16) throw new Exception("Wrong size of binary field (4): "+byteCount.ToString());
			const int intCount=4;
			Buffer.BlockCopy(byteBuf,0,intBuf,0,byteCount);
			if(cryptographer!=null) cryptographer.Decrypt(intBuf,intCount);
			return RectFromIntBuf();
		}

		public Point GetPoint(IDataReader dr,int col)
		{
			int byteCount=(int)dr.GetBytes(col,0,byteBuf,0,byteBuf.Length);
			if(byteCount!=8) throw new Exception("Wrong size of binary field (5): "+byteCount.ToString());
			const int intCount=2;
			Buffer.BlockCopy(byteBuf,0,intBuf,0,byteCount);
			if(cryptographer!=null) cryptographer.Decrypt(intBuf,intCount);
			return PointFromIntBuf();
		}

		public Point[] GetPoints(IDataReader dr,int col)
		{
			int byteCount=(int)dr.GetBytes(col,0,byteBuf,0,byteBuf.Length);
			if(byteCount<8||byteCount%8!=0) throw new Exception("Wrong size of binary field (1): "+byteCount.ToString());
			Buffer.BlockCopy(byteBuf,0,intBuf,0,byteCount);
			int pointCount=byteCount/8;
			if(cryptographer!=null) cryptographer.Decrypt(intBuf,pointCount*2);
			return PointsFromIntBuf(pointCount);
		}

		public void SetPoints(IDbDataParameter par,Point[] points)
		{
			int intCount=points.Length*2;
			int byteCount=intCount*4;
			if(byteBuf.Length<byteCount) throw new Exception("Buffer limit exceeded: "+byteCount.ToString());
			PointsToIntBuf(points);
			if(cryptographer!=null) cryptographer.Encrypt(intBuf,intCount);
			Buffer.BlockCopy(intBuf,0,byteBuf,0,byteCount);
			par.Value=byteBuf;
			par.Size=byteCount;
		}
		public void SetPoint(IDbDataParameter par,Point point)
		{
			const int intCount=2;
			const int byteCount=intCount*4;
			intBuf[0]=point.X;
			intBuf[1]=point.Y;
			if(cryptographer!=null) cryptographer.Encrypt(intBuf,intCount);
			Buffer.BlockCopy(intBuf,0,byteBuf,0,byteCount);
			par.Value=byteBuf;
			par.Size=byteCount;
		}
		public void SetRect(IDbDataParameter par,Rect rect)
		{
			const int intCount=4;
			const int byteCount=intCount*4;
			RectToIntBuf(rect);
			if(cryptographer!=null) cryptographer.Encrypt(intBuf,intCount);
			Buffer.BlockCopy(intBuf,0,byteBuf,0,byteCount);
			par.Value=byteBuf;
			par.Size=byteCount;
		}
		public byte[] SetRect(Rect rect)
		{
			const int intCount=4;
			const int byteCount=intCount*4;
			byte[] res=new byte[byteCount];
			RectToIntBuf(rect);
			if(cryptographer!=null) cryptographer.Encrypt(intBuf,intCount);
			Buffer.BlockCopy(intBuf,0,res,0,byteCount);
			return res;
		}
		public void SetIntArray(IDbDataParameter par,int[] intArray)
		{
			int intCount=intArray.Length;
			int byteCount=intCount*4;
			Buffer.BlockCopy(intArray,0,intBuf,0,byteCount);
			if(cryptographer!=null) cryptographer.Encrypt(intBuf,intCount);
			Buffer.BlockCopy(intBuf,0,byteBuf,0,byteCount);
			par.Value=byteBuf;
			par.Size=byteCount;
		}
		public byte[] SetIntArray(int[] intArray)
		{
			int intCount=intArray.Length;
			int byteCount=intCount*4;
			byte[] res=new byte[byteCount];
			Buffer.BlockCopy(intArray,0,intBuf,0,byteCount);
			if(cryptographer!=null) cryptographer.Encrypt(intBuf,intCount);
			Buffer.BlockCopy(intBuf,0,res,0,byteCount);
			return res;
		}
		#endregion

		#region IO
		public void WritePoint(BinaryWriter bw,Point p)
		{
			const int intCount=2;
			const int byteCount=intCount*4;
			intBuf[0]=p.X;
			intBuf[1]=p.Y;
			if(cryptographer!=null) cryptographer.Encrypt(intBuf,intCount);
			Buffer.BlockCopy(intBuf,0,byteBuf,0,byteCount);
			bw.Write(byteBuf,0,byteCount);
		}
		public void WriteRect(BinaryWriter bw,Rect rect)
		{
			const int intCount=4;
			const int byteCount=intCount*4;
			RectToIntBuf(rect);
			if(cryptographer!=null) cryptographer.Encrypt(intBuf,intCount);
			Buffer.BlockCopy(intBuf,0,byteBuf,0,byteCount);
			bw.Write(byteBuf,0,byteCount);
		}
		public void WriteIntArray(BinaryWriter bw, int[] intArray)
		{
			int intCount=intArray.Length;
			int byteCount=intCount*4;
			bw.Write(byteCount);
			Buffer.BlockCopy(intArray,0,intBuf,0,byteCount);
			if(cryptographer!=null) cryptographer.Encrypt(intBuf,intCount);
			Buffer.BlockCopy(intBuf,0,byteBuf,0,byteCount);
			bw.Write(byteBuf,0,byteCount);
		}
		public void WritePoints(BinaryWriter bw, Point[] points)
		{
			int intCount=points.Length*2;
			int byteCount=intCount*4;
			bw.Write(byteCount);
			PointsToIntBuf(points);
			if(cryptographer!=null) cryptographer.Encrypt(intBuf,intCount);
			Buffer.BlockCopy(intBuf,0,byteBuf,0,byteCount);
			bw.Write(byteBuf,0,byteCount);
		}
		public Point ReadPoint(BinaryReader br)
		{
			const int intCount=2;
			intBuf[0]=br.ReadInt32();
			intBuf[1]=br.ReadInt32();
			if(cryptographer!=null) cryptographer.Decrypt(intBuf,intCount);
			return PointFromIntBuf();
		}
		public Rect ReadRect(BinaryReader br)
		{
			const int intCount=4;
			intBuf[0]=br.ReadInt32();
			intBuf[1]=br.ReadInt32();
			intBuf[2]=br.ReadInt32();
			intBuf[3]=br.ReadInt32();
			if(cryptographer!=null) cryptographer.Decrypt(intBuf,intCount);
			return RectFromIntBuf();
		}
		public int[] ReadIntArray(BinaryReader br)
		{
			int byteCount=br.ReadInt32();
			int intCount=byteCount/4;
			if(byteCount%4!=0 || byteCount<0 || byteCount>Constants.maxCodeLength) throw new GeoLibException("Wrong size of binary field (6): "+byteCount.ToString());
			br.Read(byteBuf,0,byteCount);
			int[] intArray=new int[intCount];
			Buffer.BlockCopy(byteBuf,0,intArray,0,byteCount);
			if(cryptographer!=null) cryptographer.Decrypt(intArray,intCount);
			return intArray;
		}
		public Point[] ReadPoints(BinaryReader br)
		{
			int byteCount=br.ReadInt32();
			if(byteCount%8!=0 || byteCount<0 || byteCount>Constants.maxCodeLength) throw new GeoLibException("Wrong size of binary field (7): "+byteCount.ToString());
			int intCount=byteCount/4;
			br.Read(byteBuf,0,byteCount);
			Buffer.BlockCopy(byteBuf,0,intBuf,0,byteCount);
			if(cryptographer!=null) cryptographer.Decrypt(intBuf,intCount);
			return PointsFromIntBuf(intCount/2);
		}
		#endregion
	}

	public class GeoLibUtils
	{

		public static bool ReadMarker(BinaryReader reader)
		{
			bool res=true;
			int markerindex = 0;
			while(markerindex < Constants.marker.Length)
			{
				if(Constants.marker[markerindex] == reader.ReadByte())
				{
					markerindex++;
				}
				else
				{
					markerindex=0;
					res=false;
				}
			}
			return res;
		}
/*		public static bool IsVisibleOnMap(IShapedObject obj,Map map)
		{
			if(!obj.IsVisibleOnMap(map)) return false;
			if(obj.Parent==null) return true;
			return IsVisibleOnMap(obj.Parent,map);
		}*/
		public static string GetLocalizedName(GeomType gt)
		{
			switch(gt)
			{
				case GeomType.Caption: return Locale.Get("gtcaption");
				case GeomType.Point: return Locale.Get("gtpoint");
				case GeomType.Polygon: return Locale.Get("gtpolygon");
				case GeomType.Polyline: return Locale.Get("gtpolyline");
			}
			return gt.ToString();
		}
		public static int RoundScale(long scale)
		{
			int res=10;
			while(res<scale && res<100000000) res*=10;
			return res;
		}
	}
}

public class GeoLibException : Exception
{
	public GeoLibException() : base() { }
	public GeoLibException(string message) : base(message) { }
	public GeoLibException(string message, Exception innerException) : base(message, innerException) { }
}