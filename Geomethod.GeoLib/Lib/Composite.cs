using System;
using System.Collections.Generic;
using Geomethod;

namespace Geomethod.GeoLib
{
	public abstract class GComposite: IShapedObject
	{
		public ICollection<GType> Types{get{return types;}}

		protected int id;
		protected string name="";
		protected string styleStr="";
		protected int smin=0;
		protected int smax=0;
		protected BitArray32 attr = 0;

		protected List<GType> types;
		protected Style style;
		protected Rect bounds=Rect.Null;
		protected BitArray32 updateAttr=0;

		#region Access
		public string Name{get{return name;}set{if(value==null)value=""; if(name==value)return; name=value; UpdateAttr(NameFieldId);}}
		public int SMin{get{return smin;}set{if(smin==value)return; smin=value; UpdateAttr(SMinFieldId);}}
		public int SMax{get{return smax;}set{if(smax==value)return; smax=value; UpdateAttr(SMaxFieldId);}}
		public string StyleStr
		{
			get{return styleStr;}
			set
			{
				if(Lib.GetContext().SetStyle(value,ref styleStr,ref style))
				{
					UpdateAttr(StyleFieldId);
				}
			}
		}
		protected abstract int NameFieldId{get;}
		protected abstract int StyleFieldId{get;}
		protected abstract int SMinFieldId{get;}
		protected abstract int SMaxFieldId{get;}
		public abstract GComposite ParentComposite{get;}
		public abstract GLib Lib{get;}
		protected void UpdateAttr(int f){updateAttr[f]=true;Lib.SetChanged();}
		public Style Style{get{return style;}}
		public int LastPriority{get{return (types!=null && types.Count>0 ? ((GType)types[types.Count-1]).Priority : 0) + Constants.priorityInc;}}
		public bool Updated{get{return !updateAttr.IsEmpty;}}
		#endregion

		#region IShapedObject
		public Rect Bounds{get{return bounds;}}
		public abstract ClassId ClassId{get;}
		public int Id{get{return id;}}
		public abstract void Draw(Map map);
		public bool IsVisibleOnMap(Map map)
		{
			return Contains(map.Scale) && map.Intersects(Bounds);
		}
		public IShapedObject Parent{get{return this.ParentComposite;}}
		public bool GetCommonAttr(CommonAttr a) { return attr[(int)a];}
		protected void SetCommonAttr(CommonAttr a, bool val) { attr[(int)a] = val; }
		#endregion

		#region Utils
		public bool Contains(int scale){return (smin<=0 || smin <= scale) && (smax<=0 || scale <= smax);}
		public abstract void Visit(IVisitor vis);
		internal void Remove(GType type){if(types!=null){types.Remove(type); Lib.Unregister(type);}}
		internal void Add(GType type){if(types==null) types=new List<GType>(); types.Add(type); if(this is GType) ((GType)this).UpdateBounds(type.Bounds);}
		internal void Sort(){if(types!=null) types.Sort();}
		internal void SortAll()
		{
			if(types!=null)
			{
				types.Sort();
				foreach(GType type in types) type.SortAll();
			}
		}
		public string Path{get{return ParentComposite is GType ? ParentComposite.Path+'/'+name : (this is GLib ? "" : name);}}
		public void DrawSelected(Map map)
		{
			Rect bounds=Bounds;
			if(!map.Intersects(bounds)) return;
			map.DrawPolygon(Lib.Config.styles.selStyle,bounds.Points);
		}
		#endregion
	}
}
