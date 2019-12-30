using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using Geomethod;

namespace Geomethod.GeoLib
{
    public class BrushStyle : IBaseStyle
    {
        public Brush brush;
        public BrushStyle(Brush brush) { this.brush = brush; }
        #region IBaseStyle Members
        public StyleTypes StyleType { get { return StyleTypes.Brush; } }
        public Pen Pen { get { return null; } }
        public Brush Brush { get { return brush; } }
        #endregion
    }

	public class BrushStyleBuilder: BaseStyleBuilder
	{
		ImageStyleBuilder sbImage;
		public BrushStyleBuilder(StyleBuilder sb,string prefix):base(sb,prefix){sbImage=new ImageStyleBuilder(sb,prefix+"i");}
        public new void Clear() { sbImage.Clear(); base.Clear(); }
        public new bool IsEmpty { get { return base.IsEmpty && sbImage.IsEmpty; } }
		public new void AddParam(string key,string val)
		{
			if(key.Length==0) return;
			if(key.StartsWith("i"))
			{
				sbImage.AddParam(key.Substring(1),val);
			}
			else base.AddParam(key,val);
		}		
		public Brush GetBrush()
		{
			if(!sbImage.IsEmpty)
			{
				ImageStyle imageStyle=sbImage.GetImageStyle();
				if(imageStyle!=null) return GetTextureBrush(imageStyle);
			}
			if(HasKey("hs")) return GetHatchBrush();
            if(HasKey("c")) return GetSolidBrush();
            return null;
		}
		SolidBrush GetSolidBrush()
		{
			Color c=GetColor("c");
            if (!c.IsEmpty)
            {
                return new SolidBrush(c);
            }
			return null;
		}
		HatchBrush GetHatchBrush()
		{
			object hsObj=base.GetEnum("hs",typeof(HatchStyle));
            if (hsObj != null)
            {
                HatchStyle hs = (HatchStyle)hsObj;
                Color c = GetColor("c");
                if (!c.IsEmpty)
                {
                    Color c2 = GetColor("bc");
                    if (c2.IsEmpty) return new HatchBrush(hs, c);
                    else return new HatchBrush(hs, c, c2);
                }
            }
            return null;
		}
		TextureBrush GetTextureBrush(ImageStyle imageStyle)
		{
			TextureBrush tb=null;
			if(imageStyle.attr==null) tb=new TextureBrush(imageStyle.image);
			else
			{
				Rectangle r=new Rectangle(0,0,imageStyle.image.Width,imageStyle.image.Height);
				tb=new TextureBrush(imageStyle.image,r,imageStyle.attr);
			}
			return tb;
		}

        public override IBaseStyle CreateStyle()
        {
            Brush br=GetBrush();
            return br != null ? new BrushStyle(br) : null;
        }
    }
}
