using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using Geomethod;

namespace Geomethod.GeoLib
{
    public class ImageStyle : IBaseStyle
	{
		public Image image;
		public ImageAttributes attr;// might be null
        public ImageStyle(Image image):this(image,null) {}
		public ImageStyle(Image image, ImageAttributes attr){this.image=image;attr=this.attr;}

        #region IBaseStyle Members
        public StyleTypes StyleType { get { return StyleTypes.Image; } }
        public Pen Pen { get { return null; } }
        public Brush Brush { get { return null; } }
        #endregion
    }

	public class ImageStyleBuilder: BaseStyleBuilder
	{
		public ImageStyleBuilder(StyleBuilder sb,string prefix):base(sb,prefix){}
		public ImageStyle GetImageStyle()
		{
            Image image = base.GetImage("");
            if (image != null)
            {
                return new ImageStyle(image);
            }
            return null;
		}

        public override IBaseStyle CreateStyle()
        {
            return GetImageStyle();
        }
    }
}
