using System;
using System.Drawing;
using System.Collections.Generic;

namespace Geomethod.GeoLib
{
	/// <summary>
	/// Summary description for Selection.
	/// </summary>
	public class Selection
	{
		List<IShapedObject> objects = new List<IShapedObject>();
//        IShapedObject editObject = null;
		Rect bounds=Rect.Null;
		
		#region Properties
//		public bool HasEditObject { get { return editObject != null; } }
		public IShapedObject Object { get { return objects.Count>0 ? objects[0] : null; } }
//		public bool IsEmpty { get { return objects.Count == 0; } }
		public Rect Bounds{get{return bounds;}}
		public int Count{get{return objects.Count;}}
		public IEnumerable<IShapedObject> Objects{get{return objects;}}
//		public EditObject EditObject { get { return editObject; } set { editObject = value; } }
		#endregion

		#region Construction
		public Selection()
		{
		}
		#endregion

		#region Methods
		public void Set(IShapedObject obj)
		{
			Clear();
			Add(obj);
		}
		public void Add(IShapedObject obj)
		{
			if (obj != null)
			{
				objects.Add(obj);
				UpdateBounds(obj);
			}
		}
		public void UpdateBounds(IShapedObject obj)
		{
			if(obj != null) bounds.Update(obj.Bounds);
		}
/*		public void UpdateBounds()
		{
			bounds = Rect.Null;
			foreach (IShapedObject obj in objects) UpdateBounds(obj);
			UpdateBounds(editObject);
		}*/
		public void Clear()
		{
			objects.Clear();
//			editObject = null;
			bounds=Rect.Null;
		}
		public void Draw(Map map)
		{
			if(objects.Count>0 && map.Intersects(bounds))
			{
				foreach(IShapedObject obj in objects)
				{
					if(map.Intersects(obj.Bounds))
					{
						obj.DrawSelected(map);
					}
				}
			}
//			if (editObject != null) editObject.Draw(map);
		}
		#endregion
	}
}
