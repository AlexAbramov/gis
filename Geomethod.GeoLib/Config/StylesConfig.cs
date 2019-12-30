using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Geomethod;

namespace Geomethod.GeoLib
{
    public class StylesConfig
    {
        public Pen pen = Pens.DarkGray;
        public Brush brush = Brushes.WhiteSmoke;
        public Brush textBrush = Brushes.Black;
        public Font font = new Font("Times", 10);
        public StringAlignment stringAlignment = StringAlignment.Center;
        public StringFormat stringFormat=new StringFormat();
        // styles
        public Style penStyle;
        public Style brushStyle;
        public Style textStyle;
        public Style selStyle;
        public Style editLineStyle;
        public Style editPointStyle;

        #region Properties
        #endregion
        
        public StylesConfig()
        {
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
        }

        internal void Init()
        {
            StyleBuilder sb = new StyleBuilder(null, null);
            penStyle = sb.Parse("pc=blue");
            brushStyle = sb.Parse("bc=LightGray");
            textStyle = sb.Parse("fn=Tahoma fz=10");
            selStyle = sb.Parse("pc=yellow pw=2 pc2=red pds2=dash");
            editLineStyle = sb.Parse("pc=yellow pw=3 pc2=red");
            editPointStyle = sb.Parse("pc=red");
        }

        #region Methods
        public Style GetDefaultStyle(StyleTypes styleTypes)
        {
            if ((styleTypes & StyleTypes.PenBrushImage) == StyleTypes.PenBrushImage) return penStyle;
            if ((styleTypes & StyleTypes.Brush) == StyleTypes.Brush) return brushStyle;
            if ((styleTypes & StyleTypes.Pen) == StyleTypes.Pen) return penStyle;
            if ((styleTypes & StyleTypes.Text) == StyleTypes.Text) return textStyle;
            return null;
        }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
