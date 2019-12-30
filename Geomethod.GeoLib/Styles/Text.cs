using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using Geomethod;

namespace Geomethod.GeoLib
{
    public class TextStyle : IBaseStyle
	{
        public Font font;
		public Brush brush;
		public StringFormat stringFormat;
		public TextStyle(){Clear();}
		public TextStyle(Font font,Brush brush,StringFormat stringFormat){this.font=font;this.brush=brush;this.stringFormat=stringFormat;}
		public void Clear(){font=null;brush=null;stringFormat=null;}
        public bool IsEmpty { get { return font == null && brush == null && stringFormat == null; } }
        #region IBaseStyle Members
        public StyleTypes StyleType { get { return StyleTypes.Text; } }
        public Pen Pen { get { return null; } }
        public Brush Brush { get { return brush; } }
        #endregion
    }

	public class TextStyleBuilder: BaseStyleBuilder
	{
		BrushStyleBuilder sbBrush;
		public TextStyleBuilder(StyleBuilder sb):base(sb,""){sbBrush=new BrushStyleBuilder(sb,"tb");}
		public new void Clear(){sbBrush.Clear();base.Clear();}
        public new bool IsEmpty { get { return base.IsEmpty && sbBrush.IsEmpty; } }
        public new void AddParam(string key, string val)
		{
			if(key.StartsWith("tb")) sbBrush.AddParam(key.Substring(2),val);
			else base.AddParam(key,val);
		}

		public TextStyle GetTextStyle()
		{
            TextStyle ts=new TextStyle();
            ts.font=GetFont();
            ts.brush = sbBrush.GetBrush();
            ts.stringFormat = GetStringFormat();
            if(ts.IsEmpty) return null;
            if(ts.font == null) ts.font = GConfig.Instance.styles.font;
            if(ts.brush == null) ts.brush = GConfig.Instance.styles.textBrush;
            if (ts.stringFormat == null) ts.stringFormat = GetDefaultStringFormat();
			return ts;
		}

        private StringFormat GetStringFormat()
        {
            StringFormat sf = null;
            if (HasKey("ta"))
            {
                sf = GetDefaultStringFormat();
                string val=GetValue("ta");
                foreach( char c in val) 
                {
                    switch (c)
                    {
                        case 'l': sf.Alignment = StringAlignment.Far; break;
                        case 'r': sf.Alignment = StringAlignment.Near; break;
                        case 't': sf.LineAlignment = StringAlignment.Far; break;
                        case 'b': sf.LineAlignment = StringAlignment.Near; break;
                        case 'c': sf.Alignment = StringAlignment.Center; sf.LineAlignment = StringAlignment.Center; break;
                    }
                }
            }
            return sf;
        }

        private StringFormat GetDefaultStringFormat()
        {
            return new StringFormat(GConfig.Instance.styles.stringFormat);
        }

        private Font GetFont()
        {
			string fn="";
			float fz=10;
            FontStyle fs=FontStyle.Regular;
            foreach(string key in Keys)
            {
                switch(key)
                {
                    case "fn":
                        fn=GetValue(key);
                        break;
                    case "fz":
                        fz=GetFloat(key);
                        break;
                    case "fs":
                        fs=GetFontStyle(key);
                        break;
               }
            }
			if(fz!=float.NaN && fz>0 && fz<100)
            {
                return new Font(fn,fz, fs);
            }
            return null;
        }

        private FontStyle GetFontStyle(string key)
        {
            FontStyle fs=FontStyle.Regular;
            string val=GetValue(key);
            foreach (char c in val)
            {
                switch (c)
                {
                    case 'i': fs|=FontStyle.Italic; break;
                    case 'b': fs|=FontStyle.Bold; break;
                    case 's': fs|=FontStyle.Strikeout; break;
                    case 'u': fs|=FontStyle.Underline; break;
                }
            }
            return fs;
        }

        public override IBaseStyle CreateStyle()
        {
            return GetTextStyle();
        }
    }
}
