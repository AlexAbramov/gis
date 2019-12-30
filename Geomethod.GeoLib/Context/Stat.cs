using System;

namespace Geomethod.GeoLib
{
	/// <summary>
	/// Summary description for Stat.
	/// </summary>
	public class Stat
	{
		public int nColors=0;
		public int nImages=0;
		public int nLayers=0;
		public int nViews=0;
		public int nBgImages=0;
		public int nTypes=0;
		public int nRanges=0;
		public int nObjects=0;
		public BatchLevel batchLevel;
		public Stat()
		{
			batchLevel=BatchLevel.Object;
		}
	}
}
