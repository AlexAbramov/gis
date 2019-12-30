using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using Geomethod;

namespace Geomethod.GeoLib
{
    public class PenStyle : IBaseStyle
    {
        public Pen pen;
        public PenStyle(Pen pen) { this.pen = pen; }
        #region IBaseStyle Members
        public StyleTypes StyleType { get { return StyleTypes.Pen; } }
        public Pen Pen { get { return pen; } }
        public Brush Brush { get { return null; } }
        #endregion
    }

	public class PenStyleBuilder: BaseStyleBuilder
	{
		BrushStyleBuilder sbBrush;
		public PenStyleBuilder(StyleBuilder sb,string prefix):base(sb,prefix){sbBrush=new BrushStyleBuilder(sb,prefix+"b");}
		public new void Clear(){sbBrush.Clear();base.Clear();}
        public new bool IsEmpty { get { return base.IsEmpty && sbBrush.IsEmpty; } }
        public new void AddParam(string key, string val)
		{
			if(key.StartsWith("b"))
			{
				sbBrush.AddParam(key.Substring(1),val);
			}
			else base.AddParam(key,val);
		}
		public Pen GetPen()
		{
			Pen pen=null;
			if(sbBrush.Count>0)
			{
				Brush br=sbBrush.GetBrush();
				if(br!=null) pen=new Pen(br);
			}
			if(pen==null)
			{
				if(HasKey("c"))
				{
					Color c=GetColor("c");
					if(!c.IsEmpty) pen=new Pen(c);
				}
			}
			if(pen!=null)
			{
				foreach(string key in Keys)
				{
					switch(key)
					{
						case "w":
                            float w = GetFloat(key);
							if(!float.IsNaN(w))	pen.Width=w;
							break;
						case "ds":
							object dsObj=GetEnum(key,typeof(DashStyle));
							if(dsObj!=null) pen.DashStyle=(DashStyle)dsObj;
							break;
					}
				}
			}
			return pen;
		}

        public override IBaseStyle CreateStyle()
        {
            Pen pen = GetPen();
            return pen != null ? new PenStyle(pen) : null;
        }
    }
}
