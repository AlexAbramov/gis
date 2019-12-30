using System;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	public class StatProps: LocalizedObject
	{
		Stat stat;
		public StatProps(Stat stat): base(stat)
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
	}
}
