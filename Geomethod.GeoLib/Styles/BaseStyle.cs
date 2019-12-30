using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using Geomethod;

namespace Geomethod.GeoLib
{
    [FlagsAttribute]
    public enum StyleTypes { None = 0, Brush = 1, Pen = 2, Image = 4, Text = 8,
        PenBrush = Pen | Brush, PenBrushImage = Pen|Brush|Image}

    public interface IBaseStyle
    {
        StyleTypes StyleType { get;}
        Pen Pen { get; }
        Brush Brush { get; }
    }

	public abstract class BaseStyleBuilder
	{
		StyleBuilder sb;
		string prefix="";
        Dictionary<string, string> dict = new Dictionary<string, string>();

        #region Properties
        public int Count { get { return dict.Count; } }
        public bool IsEmpty { get { return dict.Count==0; } }
        public IEnumerable<string> Keys { get { return dict.Keys; } }
        #endregion

        #region Construction
        protected BaseStyleBuilder(StyleBuilder sb, string prefix)
		{
			this.sb=sb;
			this.prefix=prefix;
		}
		public void Clear(){dict.Clear();}
        #endregion

        #region Abstract
        public abstract IBaseStyle CreateStyle();
        #endregion

        #region Parameters
        public void AddParam(string key, string val){dict[key]=val;}
        public bool HasKey(string key) { return dict.ContainsKey(key); }
        public string GetValue(string key){return dict[key];}
        #endregion

        #region Aux
        public Color GetColor(string key)
		{
            string val = dict[key];
			Color c=Color.Empty;
			try
			{
				c=sb.ColorIndexer==null ? Colors.FromName(val) : sb.ColorIndexer.GetColor(val);
			}
			catch
			{
			}
			if(c.IsEmpty)
			{
                AddErrorMsg("sbwrongcolorformat", key);
			}
			return c;
		}

		public Image GetImage(string key)
		{
            string val = dict[key];
            Image image = null;
			try
			{
				image=sb.ImageIndexer==null ? null : sb.ImageIndexer.GetImage(val);
			}
			catch(Exception ex)
			{
                AddErrorMsg(ex, key);
                return null;
			}
			if(image==null)
			{
				AddErrorMsg("sbimagenotfound",key);
			}
			return image;
		}
		public float GetFloat(string key)
		{
            string val = dict[key];
            try
			{
                return ParsingUtils.ParseFloat(val);
			}
			catch
			{
                AddErrorMsg("sbwrongfloatformat", key);
				return float.NaN;
			}
		}
		public object GetEnum(string key, Type enumType)
		{
            string val = dict[key];
            try
			{
				return Enum.Parse(enumType,val,true);
			}
			catch
			{
                AddErrorMsg("sbwrongenumval", key);
                return null;
			}
		}
        #endregion

        #region Error handling
        protected void AddErrorMsg(string s) { sb.AddErrorMsg(s); }
        protected void AddErrorMsg(string _msg, string key)
        {
            string val = dict[key];
            string msg = string.Format("{0}: {1}{2}={3}", Locale.Get(_msg), prefix, key, val);
            AddErrorMsg(msg);
        }
        protected void AddErrorMsg(Exception ex, string key)
        {
            string val = dict[key];
            string msg = string.Format("{0}: {1}{2}={3}", ex.Message, prefix, key, val);
            AddErrorMsg(msg);
        }
        #endregion
	}

}
