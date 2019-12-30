using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Geomethod;

namespace Geomethod.GeoLib
{
	public enum BatchLevel{Current,Lib,Type,Range,Object}

	// glib constants
//	public enum UnitMeasure { Millimeter, Centimeter, Inch, Meter, mkm100 };
	public enum ClassId{None,Lib,Type,Range,Object,View,Layer,Color,BgImage,IdGenerator,CustomTables}
	public enum GeomType{Point,Polyline,Polygon,Caption,Image};

	// attributes
	public enum CommonAttr { ReadOnly = 31, Virtual=30 };
	public enum LibAttr { Mirror };
	public enum TypeAttr{};
	public enum ObjectAttr{};
	public enum LayerAttr { AllInclusive};
	public enum ViewAttr { Overall };
	public enum LibStateAttr { Changed, AllObjectsLoaded };

	// fields
	public enum ColorField{Id,Name,Val};
	public enum LibField{Id,Attr,Name,Style,DefaultStyle,SMin,SMax,Code,IndexerCode,Scales};//,WhenChanged,Guid
	public enum TypeField{Id,ParentId,Priority,Attr,Name,Style,GeomType,SMin,SMax};
	public enum RangeField{Id,TypeId,Code};
	public enum ObjectField{Id,TypeId,RangeId,Attr,Name,Caption,Style,TextAttr,Code};
	public enum LayerField{Id,Attr,Name,Code};
	public enum ViewField{Id,Attr,Name,Code};
	public enum BgImageField{Id,Attr,Name,Style,TextAttr,FilePath,SMin,SMax,Code};
	public enum CustomTableField{Id,Name};
	public enum Direction{Left,Top,Right,Bottom,LeftTop,RightTop,LeftBottom,RightBottom}
	
	// DB
	public enum ColType{}

	public class MaxLength
	{
		public const int LibCode=16;
		public const int RangeCode=16;
		public const int Name=50;
		public const int Caption=50;
		public const int Style=100;
		public const int TextAttr=100;
		public const int IndexerCode=100;
		public const int FilePath=500;
	}

	public class Constants
	{
		public const int imagesHashtableSize=1024;
		public const int currentLib=0;
		public const int priorityInc=100;
		public const int byteBufferSize=1<<16;
		public static readonly byte[] marker = new byte[]{1, 2, 3, 4};
		public const int maxScale=1000000;
		public const int minScale=1;
		public const int maxSearchCount=100000;
		public const int updateAttrCreated=0;
		public const int minRecordId=0;
		public const int maxRecordId = 2000000000;
		public const int poolSize = 1000000;
		public const int appPoolSize = 10;
		public const double cmPerInch=2.54;		
		public const double cmInMeter = 100;
		public const double inchesInMeter = 100 / cmPerInch;
        public const int maxCodeLength = 1000000;

        static Constants()
        {
        }

		#region Names		
		public const string nullFileName="null";
		static string allInclusiveLayerName=null;
//		static string defaultTypeName=null;
		static string defaultViewName=null;
		public static string AllInclusiveLayerName
		{
			get
			{
				if(allInclusiveLayerName!=null) return allInclusiveLayerName;
				allInclusiveLayerName=Locale.Get("allinclusivelayername");
				return allInclusiveLayerName;
			}
		}
/*		public static string DefaultTypeName
		{
			get
			{
				if(defaultTypeName!=null) return defaultTypeName;
				defaultTypeName=Locale.Get("defaulttypename");
				return defaultTypeName;
			}
		}*/
		public static string DefaultViewName
		{
			get
			{
				if(defaultViewName!=null) return defaultViewName;
				defaultViewName=Locale.Get("defaultviewname");
				return defaultViewName;
			}
		}
		#endregion
	}
}