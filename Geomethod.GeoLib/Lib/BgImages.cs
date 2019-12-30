using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	/// <summary>
	/// Summary description for BgImages.
	/// </summary>
	public class BgImages : IEnumerable<BgImage>
	{
		GLib lib;
		List<BgImage> items = new List<BgImage>();
		public int Count{get{return items.Count;}}

		#region Construction
		public BgImages(GLib lib)
		{
			this.lib=lib;
			Clear();
		}
		#endregion
		public GLib Lib { get { return lib; } }

		public void Clear()
		{
			items.Clear();
		}
		public void Load(Context context)
		{
//      DZ      16.01.09
//			int count=(int)context.Conn.ExecuteScalarById("selectCountFromGisBg");
            
            int count = Convert.ToInt32( context.Conn.ExecuteScalarById( "selectCountFromGisBg" ) );
			if(count<=0) return;
			items.Capacity=count;
			using(IDataReader dr=context.Conn.ExecuteReaderById("selectAllFromGisBg"))
			{
				while(dr.Read())
				{
					items.Add(new BgImage(context,dr));
				}
			}
			items.Sort();
		}
		public void Save(Context context)
		{
			foreach(BgImage bi in items) bi.Save(context);
		}
		public void Write(Context context,BinaryWriter bw)
		{
			foreach(BgImage bi in items) bi.Write(context,bw);
		}
		public bool Add(BgImage bi)
		{
//			if(GetBgImage(bi.Name)!=null) return false;
			items.Add(bi);
			items.Sort();
			return true;
		}
		public void Sort(){items.Sort();}
		public bool Remove(BgImage bi)
		{
			if(bi==null) return false;
			items.Remove(bi);
			return true;
		}
/*		public BgImage GetBgImage(string name)// case insensitive
		{
			name=name.ToLower();
			foreach(BgImage bi in items)
			{
				if(bi.Name.ToLower()==name) return bi;
			}
			return null;
		}*/

		#region IEnumerable<BgImage> Members

		IEnumerator<BgImage> IEnumerable<BgImage>.GetEnumerator()
		{
			return items.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}

		#endregion
	}
}
