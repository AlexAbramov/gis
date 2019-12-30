using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Collections.Generic;
using Geomethod;

namespace Geomethod.GeoLib
{
	public class Style: IEnumerable<IBaseStyle>
	{
        StyleTypes flags = StyleTypes.None;
        List<IBaseStyle> styles = new List<IBaseStyle>();

        #region Properties
        public int Count { get { return styles.Count; }}
        public IEnumerable<IBaseStyle> Styles { get { return styles; } }
        #endregion

        public Style() { }
        public Style(IBaseStyle baseStyle) { AddStyle(baseStyle); }

        #region Methods
        public bool HasFlags(StyleTypes flags) { return (this.flags & flags) == flags; }
        public bool HasAnyFlag(StyleTypes flags) { return (this.flags & flags) != 0; }
        public void Clear() { styles.Clear(); flags = 0; }
        public void AddStyle(IBaseStyle baseStyle)
        {
            if (baseStyle != null)
            {
                styles.Add(baseStyle);
                if (!HasFlags(baseStyle.StyleType))
                    SetFlag(baseStyle.StyleType);
            }
        }
        public IBaseStyle GetStyle(StyleTypes styleType)
        {
            if (HasFlags(styleType))
            {
                foreach (IBaseStyle st in styles) if (st.StyleType == styleType) return st;
            }
            return null;
        }

        private void SetFlag(StyleTypes styleTypes)
        {
            flags |= styleTypes;
        }
        #endregion

        #region IEnumerable Members
        public IEnumerator<IBaseStyle>  GetEnumerator(){return styles.GetEnumerator();}
        IEnumerator  IEnumerable.GetEnumerator(){return styles.GetEnumerator();}
        #endregion
        }
}