using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Collections.Specialized;
using Geomethod;

namespace Geomethod.GeoLib
{
	public interface IColorIndexer
	{
		Color GetColor(string name);		
	}

	public interface IImageIndexer
	{
		Image GetImage(string name);		
	}

	public class StyleBuilder
	{ 
        const string paramSep="; ";
		IColorIndexer ci;
		IImageIndexer ii;
		string styleStr="";
        SortedDictionary<int, BaseStyleBuilder>  styleBuilders = new SortedDictionary<int, BaseStyleBuilder>();
		StringBuilder sbErrors=new StringBuilder();

        #region Properties
        public IColorIndexer ColorIndexer{get{return ci;}}
		public IImageIndexer ImageIndexer{get{return ii;}}
		public string StyleStr{get{return styleStr;} }
        public bool HasErrors { get { return sbErrors.Length > 0; } }
        #endregion

        #region Construction
        public StyleBuilder(IColorIndexer ci, IImageIndexer ii)
        {
            this.ci = ci;
            this.ii = ii;
        }
        void Clear()
        {
            styleStr = "";
            foreach (BaseStyleBuilder sb in styleBuilders.Values) sb.Clear();
            sbErrors.Length = 0;
        }
        #endregion

        #region Parsing
		public Style Parse(string s)
		{
            if (s != null && s.Length > 0)
            {
                Clear();
                styleStr = s.ToLower();
                try
                {
                    Style style = new Style();
                    AddParams();
                    foreach(BaseStyleBuilder sb in styleBuilders.Values)
                    {
                        if(!sb.IsEmpty)
                        {
                            IBaseStyle st=sb.CreateStyle();
                            if (st != null)
                            {
                                style.AddStyle(st);
                            }
                        }
                    }
                    return style.Count==0 ? null : style;
                }
                catch//(Exception ex)
                {
                    //	!!!			Log.Exception(ex);
                }
            }
            return null;
		}
        void AddParams()
		{
			foreach(string p in styleStr.Split(paramSep.ToCharArray(),StringSplitOptions.RemoveEmptyEntries))
			{
				int i=p.IndexOf('=');
				if(i>=0)
				{
					AddParam(p.Substring(0,i).Trim(),p.Substring(i+1).Trim());
				}
				else
				{
					AddParam(p.Trim(),null);
				}
			}
		}
		void AddParam(string key,string val)
		{
			if(key.Length==0) return;
			char firstChar=key[0];
            string keyWithoutIndex;
            int index = GetIndex(key, out keyWithoutIndex);
            BaseStyleBuilder sb = null;
            bool removeFirstChar = true;
			switch(firstChar)
			{
				case 'p':
                    sb = GetStyleBuilder(StyleTypes.Pen, index);
					break;
				case 'b':
                    sb = GetStyleBuilder(StyleTypes.Brush, index);
                    break;
				case 'i':
                    sb = GetStyleBuilder(StyleTypes.Image, index);
                    break;
				case 't':
				case 'f':
                    sb = GetStyleBuilder(StyleTypes.Text, index);
                    removeFirstChar = false;
                    break;
				default:
					string s=string.Format("{0}: {1}",Locale.Get("sbwrongparname"),key);
					AddErrorMsg(s);
					break;
			}
            if(sb!=null)
                sb.AddParam(removeFirstChar ? keyWithoutIndex.Substring(1) : keyWithoutIndex, val);
		}
        #endregion

        #region Methods
        public bool CheckErrors()
		{
			if(sbErrors.Length==0) return false;
			string msg=sbErrors.ToString()+string.Format("\r\n({0})",styleStr);
            //!!!
			return true;
		}
		internal void AddErrorMsg(string msg){if(sbErrors.Length>0) sbErrors.Append("\r\n"); sbErrors.Append(msg);}
        BaseStyleBuilder GetStyleBuilder(StyleTypes styleType, int index)
        {
            index += ((int)styleType) * 1000;
            if (styleBuilders.ContainsKey(index)) return styleBuilders[index];
            BaseStyleBuilder sb = CreateStyleBuilder(styleType);
            styleBuilders[index] = sb;
            return sb;
        }
        private BaseStyleBuilder CreateStyleBuilder(StyleTypes styleType)
        {
            switch (styleType)
            {
                case StyleTypes.Brush: return new BrushStyleBuilder(this,"b");
                case StyleTypes.Image: return new ImageStyleBuilder(this,"");
                case StyleTypes.Pen: return new PenStyleBuilder(this,"p");
                case StyleTypes.Text: return new TextStyleBuilder(this);
            }
            throw new GeoLibException("Unsupported styleType in CreateStyleBuilder: " + styleType);
        }
        #endregion

        private int GetIndex(string key, out string keyWithoutIndex)
        {
            string indexStr="";
            for(int i=key.Length-1;i>0;i--)
            {
                char c=key[i];
                if (Char.IsDigit(c)) indexStr = c + indexStr;
                else break;            
            }
            keyWithoutIndex=key.Substring(0,key.Length-indexStr.Length);
            return indexStr.Length > 0 ? int.Parse(indexStr) : 0;
        }
		public void OnError(string s)
		{
//			Log.Warning(s,"sberror");
		}
	}
}
