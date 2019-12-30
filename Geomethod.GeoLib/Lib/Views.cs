using System;
using System.IO;
using System.Data;
using System.Collections;

namespace Geomethod.GeoLib
{
	/// <summary>
	/// Summary description for Views.
	/// </summary>
	public class Views: IEnumerable
	{
		GLib lib;
		ArrayList views=new ArrayList();
		public int Count{get{return views.Count;}}

		#region Construction
		public Views(GLib lib)
		{
			this.lib=lib;
			Clear();
		}
		#endregion

		public void Clear()
		{
			views.Clear();
		}
		public void Load(Context context)
		{
//      DZ      16.01.09
//			int count=(int)context.Conn.ExecuteScalarById("selectCountFromGisViews");
            int count = Convert.ToInt32( context.Conn.ExecuteScalarById( "selectCountFromGisViews" ) );
			if(count<=0) return;
			views.Capacity=1+count;
			using(IDataReader dr=context.Conn.ExecuteReaderById("selectAllFromGisViews"))
			{
				while(dr.Read())
				{
					views.Add(new View(context,dr));
				}
			}
			views.Sort();
		}
		public void Save(Context context)
		{
			foreach(View view in views) view.Save(context);
		}
		public void Write(Context context,BinaryWriter bw)
		{
			foreach(View view in views) view.Write(context,bw);
		}
		public IEnumerator GetEnumerator()
		{
			return views.GetEnumerator();
		}
		public bool Add(View view)
		{
			if(GetView(view.Name)!=null) return false;
			views.Add(view);
			views.Sort();
			return true;
		}
		public void Sort(){views.Sort();}
		public bool Remove(View view)
		{
			if(view==null) return false;
			views.Remove(view);
			return true;
		}
		public View GetView(string name)// case insensitive
		{
			name=name.ToLower();
			foreach(View view in views)
			{
				if(view.Name.ToLower()==name) return view;
			}
			return null;
		}
	}
}
