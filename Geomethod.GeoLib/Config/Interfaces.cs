using System;
using System.Drawing;

namespace Geomethod.GeoLib
{
	public interface INamed
	{
		string Name{get;set;}
	}

	public interface ISerializableObject
	{
		ClassId ClassId{get;}
		int Id{get;}
		bool GetCommonAttr(CommonAttr a);
	}

	public interface IShapedObject: ISerializableObject
	{
		Rect Bounds{get;}
		IShapedObject Parent{get;}
		bool IsVisibleOnMap(Map map);
		void Draw(Map map);
		void DrawSelected(Map map);
	}

	//	enum GObjectType{Point,Polyline,Polygon,Composite,Type,Lib}
/*	public interface IStyle
	{
		Pen Pen{get;set;}
		Brush Brush{get;set;}
		Font Font{get;set;}
		Image Image{get;set;}
	}
	public interface IShapedObject
	{
		int Id{get;set;}
		IComposite Parent{get;set;}
		IStyle Style{get;set;}
		bool Visible{get;set;}
		bool IsComposite();
		bool Intersects(Rectangle rect);
		void Draw(IMap map);
	}
	public interface IComposite: IShapedObject
	{
		Rectangle Bounds{get;}
		void Add(IShapedObject gobj);
	}
	public interface ILib: IComposite
	{
		int SMin{get;set;}
		int SMax{get;set;}
	}	
	public interface IMap
	{
		Graphics Graphics{get;set;}
		ILib GLib{get;set;}
		bool Intersects(Rectangle rect);
		bool Contains(Point point);
	  bool IsVisible(Rectangle rect);
		int Scale();
		void Repaint();
	}

	enum AppCmd{}

	public interface IGeoApp
	{
		GLib Lib { get;}
		Layer Layer { get;}
		Map CurrentMap { get;}
		EditObject EditObject { get;}
		GType DraggedType { get; }
		string Status{get;set;}
		void DataChanged(object obj);
		void ShowProperties(object obj);
		void StartEditing(GType type);
		void StartEditing(GObject obj);
		void EndEditing();
		void UpdateScaleCombo();
	}*/
}