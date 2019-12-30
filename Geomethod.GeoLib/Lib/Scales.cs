using System;
using System.IO;
using System.Collections.Generic;
using System.Data;

namespace Geomethod.GeoLib
{
	public class Scales
	{
		GLib lib;
		int[] scales=new int[0];

		public int Count{get{return scales.Length;}}
		public int SMin{get{if(lib.SMin>0) return lib.SMin; return Constants.minScale;}}
		public int SMax{get{if(lib.SMax>0) return lib.SMax; return Constants.maxScale;}}
		
		public Scales(GLib lib)
		{
			this.lib=lib;
		}
		public void InitScales()
		{
			List<int> ar=new List<int>();
			ar.Add(SMin);
			int scale=1;
			int count=0;
			while(scale<SMax)
			{
				if(scale>SMin) ar.Add(scale);
				if(++count%3==2) scale=scale*5/2;
				else scale*=2;
			}
			ar.Add(SMax);
			scales=(int[])ar.ToArray();
		}
		public void Clear()
		{
			scales=new int[0];
		}
		public int[] Values
		{
			get
			{
				return (int[]) scales.Clone();
			}
			set
			{
				if(value==null) value=new int[0];
				List<int> ar = new List<int>(value);
				for(int i=ar.Count-1;i>=0;i--)
				{
					if(!IsValid((int)ar[i]))
					{
						ar.RemoveAt(i);
					}
				}
				ar.Sort();
				scales=(int[]) ar.ToArray();
			}
		}
		public int Next(int scale)
		{
			foreach(int sc in scales) if(scale<sc) return sc;
			int sc1=1;
			while(sc1<=scale && sc1<SMax) sc1<<=1;
			return sc1<SMax ? sc1 : SMax;
		}
		public int Prev(int scale)
		{
			for(int i=scales.Length-1;i>=0;i--) if(scales[i]<scale) return scales[i];
			int sc1=1;
			while(sc1<scale && sc1<SMax) sc1<<=1;
			sc1>>=1;
			return sc1>lib.SMin ? sc1 : lib.SMin;
		}
		public bool IsValid(int scale)
		{
			return scale>=SMin && scale<=SMax;
		}
	}
}
