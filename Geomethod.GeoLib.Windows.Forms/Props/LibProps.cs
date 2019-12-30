using System;
using System.ComponentModel;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	public class BoundsProps :LocalizedObject
	{
		Rect bounds;
		public Rect Bounds{get{return bounds;}}
        public BoundsProps(Rect bounds)
            : base(bounds)
		{
			this.bounds=bounds;
		}
		[LocalizedProperty("_leftbound")]
		public int Left
		{
			get
			{
				return bounds.left;
			}
			set
			{
				bounds.left=value;
			}
		}
		[LocalizedProperty("_rightbound")]
		public int Right
		{
			get
			{
				return bounds.right;
			}
			set
			{
				bounds.right=value;
			}
		}
		[LocalizedProperty("_topbound")]
		public int Top
		{
			get
			{
				return bounds.top;
			}
			set
			{
				bounds.top=value;
			}
		}
		[LocalizedProperty("_bottombound")]
		public int Bottom
		{
			get
			{
				return bounds.bottom;
			}
			set
			{
				bounds.bottom=value;
			}
		}
	}

	public class LibProps : LocalizedObject
	{
		GLib lib;
		public LibProps(GLib lib):base(lib)
		{
			this.lib = lib;
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_glibname",Description="_glibnamedescr")]
		public string Name
		{
			get
			{
				return lib.Name;
			}
			set
			{
				lib.Name=value;
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_glibstyle",Description="_glibstyledescr")]
		public string StyleStr
		{
			get
			{
				return lib.StyleStr;
			}
			set
			{
				lib.StyleStr=value;
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_mirror",Description="_mirrordescr")]
		public bool Mirror
		{
			get
			{
				return lib.Mirror;
			}
			set
			{
				lib.Mirror=value;
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_glibdefaultstyle",Description="_glibdefaultstyledescr")]
		public string DefaultStyleStr
		{
			get
			{
				return lib.DefaultStyleStr;
			}
			set
			{
				lib.DefaultStyleStr=value;
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_scalemin",Description="_scalemindescr")]
		public string ScaleMin
		{
			get
			{
				return ScalesForm.GetScale(lib.SMin);
			}
			set
			{
				lib.SMin = ScalesForm.GetScale(value);
			}
		}
		[LocalizedCategory("_misc")]
		[LocalizedProperty("_scalemax",Description="_scalemaxdescr")]
		public string ScaleMax
		{
			get
			{
				return ScalesForm.GetScale(lib.SMax);
			}
			set
			{
				lib.SMax = ScalesForm.GetScale(value);
			}
		}
		//		[LocalizedCategory("_geometry")]
		[LocalizedProperty("_bounds")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public BoundsProps Left
		{
			get
			{
				return new BoundsProps(lib.Bounds);
			}
			set
			{
				lib.SetBounds(value.Bounds);
			}
		}
	}
}