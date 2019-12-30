using System;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	public class LibStatProps: LocalizedObject
	{
		Stat stat;
		public LibStatProps(Stat stat):base(stat)
		{
			this.stat=stat;
		}
		[LocalizedCategory("_misc")]
		[LocalizedProperty("_ntypes",Description="_ntypesdescr")]
		public int NTypes{get{return stat.nTypes;}}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_nranges",Description="_nrangesdescr")]
		public int NRanges{get{return stat.nRanges;}}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_nobjects",Description="_nobjectsdescr")]
		public int NObjects{get{return stat.nObjects;}}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_nbgimages",Description="_nbgimagesdescr")]
		public int NBgImages{get{return stat.nBgImages;}}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_ncolors",Description="_ncolorsdescr")]
		public int NColors{get{return stat.nColors;}}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_nviews",Description="_nviewsdescr")]
		public int NViews{get{return stat.nViews;}}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_nlayers",Description="_nlayersdescr")]
		public int NLayers{get{return stat.nLayers;}}

//		[LocalizedCategory("_misc")]
//		[LocalizedProperty("_nimages",Description="_nimagesdescr")]
//		public int NObjects{get{return stat.nImages;}}


	}
}
