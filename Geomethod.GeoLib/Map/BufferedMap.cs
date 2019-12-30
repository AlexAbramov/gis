using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Geomethod.GeoLib
{
	public class BufferedMap: Map
	{
		#region Fields
		Image image=null;
		#endregion

		#region Properties
		public Image Image{get{return image;}}
		#endregion 

		#region Construction
        public BufferedMap(GLib lib, Size size): this(lib, new Bitmap(size.Width,size.Height)){}
        public BufferedMap(GLib lib, Image image): base(lib, image.Size, Graphics.FromImage(image))
		{
            this.image = image;
		}
		#endregion

		#region Methods
		public new void Resize(Size size)
		{
			if(image!=null) image.Dispose();
			image=new Bitmap(size.Width,size.Height);
            InitGraphics(Graphics.FromImage(image));
            base.Resize(size);
		}
		#endregion
	}
}
