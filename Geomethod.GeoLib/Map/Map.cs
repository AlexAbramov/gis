using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Geomethod.GeoLib
{
	public class Map: CoordTransform, IFilter
	{
		#region Fields
		GLib lib;
		Selection selection=new Selection();
		Layer layer=null;
		BatchLevel batchLevel=BatchLevel.Object;
		int scale;
		Rect bounds;
		// graphics
		Graphics graphics=null;
		double dpi;
		Rect rect;
		// aux
		Context context=null;
		#endregion

		#region Properties
        public Size Size{get{return rect.Size;}}
		public double PixelPerUnitMeasure { get { return dpi * lib.UnitMeasure*Constants.inchesInMeter; } }
		public GLib Lib{get{return lib;}}
		public Layer Layer{get{return layer;}set{layer=value;}}
		public double Dpi{get{return dpi;}/*set{dpi=value;PixelScale=scale/PixelPerUnitMeasure;}*/}
        internal Context Context{get{return context;}}
		public Selection Selection{get{return selection;}}
		public Rect Rect{get{return rect;}}
		public bool Mirror { get { return mirror; } set { mirror = value; UpdateBounds(); } }
		#endregion 

		#region Construction
        public Map(Map map, Size size, Graphics graphics) : this(map.lib, size, graphics, map.Pos, map.Angle, map.Scale) {}
        public Map(GLib lib, Size size, Graphics graphics) : this( lib, size, graphics,lib.Bounds.Center, 0.0f, lib.Scales.SMax) { }
        public Map(GLib lib, Size size, Graphics graphics, Point pos, float angle, int scale)
		{
			this.lib=lib;
			this.scale = scale;
            InitGraphics(graphics);
            rect = new Rect(size);
            pixelPos = rect.Center;
			double pixelScale = scale / PixelPerUnitMeasure;
			base.Init(pos, pixelPos, pixelScale, angle, lib.Mirror);
			UpdateBounds();
		}
        protected void InitGraphics(Graphics graphics)
        {
            lock (this)
            {
                if (this.graphics != null) this.graphics.Dispose();
                this.graphics = graphics;
                dpi = graphics.DpiX;
            }
        }
		#endregion

		#region Drawing
//        Style GetStyle(Style style, StyleTypes styleType) { }
//		Pen GetPen(Style style){return style!=null && style.pen!=null ? style.pen : lib.DefaultStyle.Pen;}
//		Brush GetBrush(Style style){return style!=null && style.brush!=null ? style.brush:lib.DefaultStyle.Brush;}
//		TextStyle GetTextStyle(Style style){return style!=null && style.textStyle!=null ? style.textStyle : lib.DefaultStyle.TextStyle;}
		public void Draw()
		{
			graphics.ResetTransform();
			graphics.Clear(Color.White);
			using(context=lib.GetContext())
			{
				lib.Draw(this);
			}
			selection.Draw(this);
		}

        private bool EnsureStyle(ref Style style, StyleTypes styleTypes)
        {
            if (style == null || !style.HasAnyFlag(styleTypes))
            {
                style = Lib.Config.styles.GetDefaultStyle(styleTypes);
            }
            return style != null;
        }

        public void DrawPolyline(Style style, Point[] points)
		{
            if (points.Length >= 2)
            {
                if (EnsureStyle(ref style, StyleTypes.Pen))
                {
                    Point[] pts = (Point[])points.Clone();
                    WToG(pts);
                    foreach (IBaseStyle baseStyle in style)
                    {
                        switch (baseStyle.StyleType)
                        {
                            case StyleTypes.Pen:
                                graphics.DrawLines(baseStyle.Pen, pts);
                                break;
                        }
                    }
                }
            }
        }

		public void DrawPolygon(Style style,Point[] points)
		{
            if (points.Length >= 3)
            {
                if (EnsureStyle(ref style, StyleTypes.PenBrush))
                {
                    Point[] pts = (Point[])points.Clone();
                    WToG(pts);
                    foreach (IBaseStyle baseStyle in style)
                    {
                        switch (baseStyle.StyleType)
                        {
                            case StyleTypes.Brush:
                                graphics.FillPolygon(baseStyle.Brush, pts);
                                break;
                            case StyleTypes.Pen:
                                if (style != lib.DefaultStyle)
                                    graphics.DrawPolygon(baseStyle.Pen, pts);
                                break;
                        }
                    }
                }
            }
		}
		public void DrawRect(Style style,Rect rect){DrawPolygon(style,rect.Points);}
		public void DrawPoint(Style style,Point p)
		{
            if (EnsureStyle(ref style, StyleTypes.PenBrushImage))
            {
                WToG(ref p);
                foreach (IBaseStyle baseStyle in style)
                {
                    switch (baseStyle.StyleType)
                    {
                        case StyleTypes.Brush:
                            {
                                int r = lib.Config.geometry.pointRadius;
                                graphics.FillEllipse(baseStyle.Brush, p.X - r, p.Y - r, r + r, r + r);
                            }
                            break;
                        case StyleTypes.Pen:
                            {
                                int r = lib.Config.geometry.pointRadius;
                                graphics.DrawEllipse(baseStyle.Pen, p.X - r, p.Y - r, r + r, r + r);
                            }
                            break;
                        case StyleTypes.Image:
                            ImageStyle st = baseStyle as ImageStyle;
                            Image image = st.image;
                            p.X -= image.Size.Width / 2;
                            p.Y -= image.Size.Height / 2;
                            graphics.DrawImageUnscaled(image, p);
                            break;
                    }
                }
            }
		}
		public void DrawImage(Image image,ImageAttributes ia,Point pos,float scale,float angle)
		{
//            if (EnsureStyle(ref style, StyleTypes.Image))
            {
                WToG(ref pos);
                graphics.TranslateTransform(pos.X, pos.Y);
                scale /= this.scale;
                graphics.ScaleTransform(scale, scale);
                graphics.RotateTransform(angle - this.Angle);
                Rectangle r = new Rectangle(-image.Width / 2, -image.Height / 2, image.Width, image.Height);
                graphics.DrawImage(image, r, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);
                graphics.ResetTransform();
            }
		}
		public void DrawCircle(Style style,Point p,int rad)
		{
            if (EnsureStyle(ref style, StyleTypes.Pen))
            {
                WToG(ref p);
                foreach (IBaseStyle baseStyle in style)
                {
                    switch (baseStyle.StyleType)
                    {
                        case StyleTypes.Pen:
                            graphics.DrawEllipse(baseStyle.Pen, p.X - rad, p.Y - rad, rad * 2, rad * 2);
                            break;
                    }
                }
            }
		}
		public void DrawText(Style style, Point p, string text)
		{
            if (EnsureStyle(ref style, StyleTypes.Text))
            {
                WToG(ref p);
                foreach (IBaseStyle baseStyle in style)
                {
                    switch (baseStyle.StyleType)
                    {
                        case StyleTypes.Text:
                            TextStyle ts = baseStyle as TextStyle;
                            graphics.DrawString(text, ts.font, ts.brush, p, ts.stringFormat);
                            break;
                    }
                }
            }
		}
		public void DrawText(Style style,Point p,string text,float angle)
		{
            if (text.Length>0 && EnsureStyle(ref style, StyleTypes.Text))
            {
                WToG(ref p);
                graphics.TranslateTransform(p.X, p.Y);
                angle += Angle;
                if (!Mirror) angle = -angle;
                GeomUtils.NormalizeAngle(ref angle);
                graphics.RotateTransform(angle);
                foreach (IBaseStyle baseStyle in style)
                {
                    switch (baseStyle.StyleType)
                    {
                        case StyleTypes.Text:
                            TextStyle ts = baseStyle as TextStyle;
                            graphics.DrawString(text, ts.font, ts.brush, 0, 0, ts.stringFormat);
                            break;
                    }
                }
                graphics.ResetTransform();
            }
		}
		#endregion

		#region Navigation
		public Point Pos
		{
			get { return pos; }
			set
			{
				if (lib.Bounds.Contains(value))
				{
					pos = value;
					UpdateBounds();
				}
			}
		}
		public new float Angle
		{
			get { return base.Angle; }
			set
			{
				if (value != base.Angle)
				{
					base.Angle = value;
					UpdateBounds();
				}
			}
		}
		public bool CanScaleUp { get { return lib.SMin < scale && 1 < scale; } }
		public bool CanScaleDown{get{return lib.SMax==0 || scale<lib.SMax;}}
		public void ScaleUp()
		{
			Scale=Lib.Scales.Prev(Scale);
		}
		public void ScaleDown()
		{
			Scale=Lib.Scales.Next(Scale);
		}
		public void RotateCW()
		{
			Rotate(10);
		}
		public void RotateCCW()
		{
			Rotate(-10);
		}
		public void SetBounds(Rect bounds)
		{
			if (rect.Width > 0 && rect.Height > 0)
			{
			    bool hor = bounds.Width * rect.Height > bounds.Height * rect.Width;
			    pos = bounds.Center;
				double pixScale = hor ? (double)bounds.Width / rect.Width : (double)bounds.Height / rect.Height;
				int newScale = (int)(pixScale * PixelPerUnitMeasure);
			    int prevScale=lib.Scales.Prev(newScale);
			    int nextScale=lib.Scales.Next(newScale);
			    if(newScale<prevScale) newScale=prevScale;
			    else if(newScale>nextScale) newScale=nextScale;
			    if (newScale!=scale) Scale = newScale;
			}
		}
		public bool EnsureVisible(Rect bounds)
		{
			if(Bounds.Contains(bounds)) return false;
			Point wp=bounds.Center;
			bool samePos=pos==wp;
		    if(!samePos) Pos=wp;
			if (bounds.Width == 0 && bounds.Height == 0)
			{
				return !samePos;
			}
			int scale0=Scale;
			while(!Bounds.Contains(bounds))
			{
			  int prevScale=Scale;
				ScaleUp();
				if(prevScale==Scale) break;
			}
			return scale0!=Scale;
		}
		public void SetView(View view)
		{
			if(view.IsOverall)
			{
				SetDefaultView();
			}
			else
			{
				Pos=view.Pos;
				Scale=view.Scale;
				Angle=view.Angle;
			}
		}
		public void SetDefaultView()
		{
			Pos = lib.Bounds.Center;
			Scale=lib.Scales.SMax;
			Angle=0;
			UpdateBounds();
//			SetBounds(lib.Bounds);
//			ScaleDown();
		}
		#endregion

		#region Methods
		public Rect Bounds { get { return bounds; } }
		public bool Intersects(Rect rect) { return bounds.Intersects(rect); }
		public bool Intersects(Point point) { return bounds.Contains(point); }
		public void Resize(Size size)
		{
			rect=new Rect(size);
			pixelPos = rect.Center;
			UpdateBounds();
		}
		public bool IsVisible(long wsize)
		{
			if (wsize < 0) wsize = -wsize;
            return  wsize > PixelScale * lib.Config.geometry.minSizeVisible;
		}
		void UpdateBounds()
		{
			bounds = GToW(rect);
		}
		public void Rotate(float angle)
		{
			if (!mirror) angle = -angle;
			Angle += angle;
			UpdateBounds();
		}
		public Size GetMoveStep(Direction dir)
		{
			Size size = GetVector(dir);
			size.Width *=(int) (rect.Width / 3);
			size.Height *= (int)(rect.Height / 3);
			GToW(ref size);
			return size;
		}
		public void Move(Direction dir)
		{
			Pos += GetMoveStep(dir);
		}
		public bool CanMove(Direction dir)
		{
			Point targetPos = Pos + GetMoveStep(dir);
			return lib.Bounds.Contains(targetPos);
		}
		public static Size GetVector(Direction dir)
		{
			switch (dir)
			{
				case Direction.Left: return new Size(-1, 0);
				case Direction.Top: return new Size(0, -1);
				case Direction.Right: return new Size(1, 0);
				case Direction.Bottom: return new Size(0, 1);
				case Direction.LeftTop: return new Size(-1, -1);
				case Direction.RightTop: return new Size(1, -1);
				case Direction.LeftBottom: return new Size(-1, 1);
				case Direction.RightBottom: return new Size(1, 1);
			}
			return new Size(0, 0);
		}
		#endregion

		#region IFilter Members

		public int Scale
		{
			get { return scale; }
			set
			{
				scale = value;
				if (lib.SMin > 0 && scale < lib.SMin) scale = lib.SMin;
				if (lib.SMax > 0 && scale > lib.SMax) scale = lib.SMax;
				if (scale < 1) scale = 1;
				PixelScale = scale / PixelPerUnitMeasure;
				UpdateBounds();
			}
		}
		public bool Includes(int typeId) { return layer == null ? true : layer.Includes(typeId); }
		public bool Includes(BatchLevel batchLevel) { return (int)batchLevel <= (int)this.batchLevel; }

		#endregion
	}
}
