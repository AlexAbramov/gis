using System;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for BgImageProps.
	/// </summary>
	public class BgImageProps: LocalizedObject
	{
		BgImage bgImage;
        public BgImageProps(BgImage bgImage)
            : base(bgImage)
		{
			this.bgImage = bgImage;
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_name")]
		public string Name
		{
			get
			{
				return bgImage.Name;
			}
		}

//		[LocalizedCategory("_misc")]
		[LocalizedProperty("_filepath",Description="_filepathdesc")]
		public string FilePath
		{
			get
			{
				return bgImage.FilePath;
			}
			set
			{
				bgImage.FilePath=value;
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_style",Description="_styledescr")]
		public string StyleStr
		{
			get
			{
				return bgImage.StyleStr;
			}
			set
			{
				bgImage.StyleStr=value;
			}
		}

		public float Scale
		{
			get
			{
				return bgImage.Scale;
			}
			set
			{
				bgImage.Scale=value;
			}
		}


		[LocalizedCategory("_misc")]
		[LocalizedProperty("_scalemin",Description="_scalemindescr")]
		public string ScaleMin
		{
			get
			{
				return ScalesForm.GetScale(bgImage.SMin);
			}
			set
			{
				bgImage.SMin=ScalesForm.GetScale(value);
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_scalemax",Description="_scalemaxdescr")]
		public string ScaleMax
		{
			get
			{
				return ScalesForm.GetScale(bgImage.SMax);
			}
			set
			{
				bgImage.SMax=ScalesForm.GetScale(value);
			}
		}
	}
}
