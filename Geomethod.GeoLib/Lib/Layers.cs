using System;
using System.IO;
using System.Data;
using System.Collections.Generic;

namespace Geomethod.GeoLib
{
	/// <summary>
	/// Summary description for Layers.
	/// </summary>
	public class Layers: IEnumerable<Layer>
	{
		GLib lib;
		List<Layer> layers=new List<Layer>();
		public int Count { get { return layers.Count; } }
		public GLib Lib { get { return lib; } }

		#region Construction
		public Layers(GLib lib)
		{
			this.lib=lib;
			Clear();
		}
		public void Clear()
		{
			layers.Clear();
//			Layer layer=new Layer(lib, Constants.AllInclusiveLayerName);
//			layers.Add(layer);
		}
		public bool Add(Layer layer)
		{
			if(GetLayer(layer.Name)!=null) return false;
			layers.Add(layer);
			layers.Sort();
			return true;
		}
		public bool Remove(Layer layer)
		{
			if(layer==null) return false;
			layers.Remove(layer);
			return true;
		}
		public Layer GetLayer(string name)// case insensitive
		{
			name=name.ToLower();
			foreach(Layer layer in layers)
			{
				if(layer.Name.ToLower()==name) return layer;
			}
			return null;
		}
		#endregion

		#region Serialization
		public void Load(Context context)
		{
//      DZ  

//			int count=(int)context.Conn.ExecuteScalarById("selectCountFromGisLayers");
            int count = Convert.ToInt32( context.Conn.ExecuteScalarById( "selectCountFromGisLayers" ) );
			if(count<=0) return;
			layers.Capacity=1+count;
			using(IDataReader dr=context.Conn.ExecuteReaderById("selectAllFromGisLayers"))
			{
				while(dr.Read())
				{
					layers.Add(new Layer(context,dr));
				}
			}
			layers.Sort();
		}
		public void Save(Context context)
		{
			foreach(Layer layer in layers) layer.Save(context);
		}
		public void Write(Context context,BinaryWriter bw)
		{
			foreach(Layer layer in layers) layer.Write(context,bw);
		}
		#endregion

		#region IEnumerable<Layer> Members

		IEnumerator<Layer> IEnumerable<Layer>.GetEnumerator()
		{
			return layers.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return layers.GetEnumerator();
		}

		#endregion
	}
}
