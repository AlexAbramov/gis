using System;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using Geomethod;

namespace Geomethod.GeoLib
{
	public class Colors: IColorIndexer,IEnumerable<NamedColor>
	{
		GLib lib;
		Dictionary<string, NamedColor> htColors = null;

		#region Properties
		public int Count { get { return htColors == null ? 0 : htColors.Count; } }
		#endregion

		#region Construction
		public Colors(GLib lib)
		{
			this.lib=lib;
		}
		#endregion

		#region Serialization
		public void Load(Context context)
		{

//          DZ      16.01.09
            //			int count=(int)context.Conn.ExecuteScalarById("selectCountFromGisColors");

            int count = Convert.ToInt32( context.Conn.ExecuteScalarById( "selectCountFromGisColors" ) );
			if(count<=0) return;
			if(htColors==null) htColors=new Dictionary<string,NamedColor>(count);
			using(IDataReader dr=context.Conn.ExecuteReaderById("selectAllFromGisColors"))
			{
				while(dr.Read()) Add(new NamedColor(dr));
			}
		}
		public void Save(Context context)
		{
			if (htColors != null) foreach (NamedColor nc in htColors.Values) nc.Save(context);
		}
		public void Write(Context context,BinaryWriter bw)
		{
			if (htColors != null) foreach (NamedColor nc in htColors.Values) nc.Write(context, bw);
		}
		#endregion

		#region Utils
		public Color GetColor(string name)// throwable
		{
			int count=0;
			foreach(char c in name) if(c=='/') count++;
			switch(count)
			{
				case 0:
					Color c=Color.FromName(name);
					if(c.ToArgb()==0) c=GetCustomColor(name);
					return c;
				case 1:
					string[] ss=name.Split('/');
					Color baseColor=Color.FromName(ss[1]);
					if(baseColor.ToArgb()==0) baseColor=GetCustomColor(name);
					if(baseColor.ToArgb()!=0)
					{
						int a=int.Parse(ss[0]);
						return Color.FromArgb(a,baseColor);
					}
					break;
				case 2:
				case 3:
					return FromName(name);
			}
			return Color.Empty;
		}
		public Color GetCustomColor(string name)
		{
			if(htColors!=null)
			{
				object obj=htColors[name];
				if(obj!=null) return (Color)obj;
			}
			return Color.Empty;
		}
		public static Color FromName(string name)
		{
			int count=0;
			foreach(char c in name) if(c=='/') count++;
			switch(count)
			{
				case 0:
				{
					Color c=Color.FromName(name);
					if(c.ToArgb()!=0) return c;
					if(name.Length==6||name.Length==8)
					{
						string[] ss=name.Split('/');
						int[] ii=new int[ss.Length/2];
						for(int i=0;i<ii.Length;i++) ii[i]=int.Parse(name.Substring(i*2,2),NumberStyles.HexNumber);
						return ii.Length==3 ? Color.FromArgb(ii[0],ii[1],ii[2]) : Color.FromArgb(ii[0],ii[1],ii[2],ii[3]);
					}
				}
					break;
				case 1:
				{
					string[] ss=name.Split('/');
					Color c=Color.FromName(ss[1]);
					if(c.ToArgb()!=0)
					{
						int a=int.Parse(ss[0]);
						return Color.FromArgb(a,c);
					}
				}
					break;
				case 2:
				case 3:
				{
					string[] ss=name.Split('/');
					int[] ii=new int[ss.Length];
					for(int i=0;i<ss.Length;i++) ii[i]=int.Parse(ss[i]);
					return ss.Length==3 ? Color.FromArgb(ii[0],ii[1],ii[2]) : Color.FromArgb(ii[0],ii[1],ii[2],ii[3]);
				}
			}
			return Color.Empty;
		}
		public void Add(NamedColor nc)
		{
			if (htColors == null) htColors = new Dictionary<string,NamedColor>();
			htColors.Add(nc.Name,nc);
		}
		public void Clear()
		{
			if(htColors!=null)
			{
				htColors.Clear();
				htColors=null;
			}
		}
		#endregion

		#region IEnumerable<NamedColor> Members

		public IEnumerator<NamedColor> GetEnumerator()
		{
			return htColors.Values.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return htColors.Values.GetEnumerator();
		}

		#endregion

		public void Remove(NamedColor namedColor)
		{
            htColors.Remove(namedColor.Name);
		}
	}
}