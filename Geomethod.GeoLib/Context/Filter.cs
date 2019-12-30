using System;
using System.Collections;
using System.Drawing;

namespace Geomethod.GeoLib
{
	public interface IFilter
	{
		int Scale{get;}
		bool Includes(int typeId);
		bool Intersects(Rect rect);
		bool Intersects(Point point);
		bool Includes(BatchLevel batchLevel);
	}

	public class Filter: IFilter
	{
		int scale=0;
		Layer layer=null;
		Region region=null;
		BatchLevel batchLevel;

		public bool IsAll{get{return scale==0 && layer==null && region==null && batchLevel==BatchLevel.Object;}}

		public Filter(int scale,Layer layer,Region region,BatchLevel batchLevel)
		{
			this.scale=scale;
			this.layer=layer;
			this.region=region;
			this.batchLevel=batchLevel;
		}
		public Filter(BatchLevel batchLevel)
		{
			this.batchLevel=batchLevel;
		}
		static public Filter Current{get{return null;}}
		static public Filter All{get{return new Filter(BatchLevel.Object);}}

		public int Scale{get{return scale;}}
		public bool Includes(int typeId){return layer==null ? true : layer.Includes(typeId);}
		public bool Intersects(Rect rect){return region==null ? true : region.IsVisible(rect);}
		public bool Intersects(Point point){return region==null ? true : region.IsVisible(point);}
		public bool Includes(BatchLevel batchLevel){return (int)batchLevel<=(int)this.batchLevel;}
	}
}
