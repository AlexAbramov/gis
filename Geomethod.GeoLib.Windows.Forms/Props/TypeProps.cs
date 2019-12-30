using System;
using System.Globalization;
using System.ComponentModel;
using System.Collections;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for TypeProps.
	/// </summary>
	public class TypeProps: LocalizedObject
	{
		GType type;
		public TypeProps(GType type):base(type)
		{
			this.type=type;
		}
		
		[LocalizedCategory("_misc")]
		[LocalizedProperty("_id",Description="_typeiddescr")]
		public int Id
		{
			get
			{
				return type.Id;
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_parenttype",Description="_parenttypedescr")]
		public string Parent
		{
			get
			{
				return type.ParentComposite.Name;
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_gtypename",Description="_gtypenamedescr")]
		public string Name
		{
			get
			{
				return type.Name;
			}
			set
			{
				type.Name=value;
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_geomtype",Description="_geomtypedescr")]
		public GeomType GeomType
		{
			get
			{
				return type.GeomType;
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_gtypestyle",Description="_gtypestyledescr")]
		public string StyleStr
		{
			get
			{
				return type.StyleStr;
			}
			set
			{
				type.StyleStr=value;
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_scalemin",Description="_scalemindescr")]
		public string ScaleMin
		{
			get
			{
				return ScalesForm.GetScale(type.SMin);
			}
			set
			{
				type.SMin = ScalesForm.GetScale(value);
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_scalemax",Description="_scalemaxdescr")]
		public string ScaleMax
		{
			get
			{
				return ScalesForm.GetScale(type.SMax);
			}
			set
			{
				type.SMax = ScalesForm.GetScale(value);
			}
		}

		[LocalizedCategory("_misc")]
		[LocalizedProperty("_boundtables",Description="_boundtablesdescr")]
		[EditorAttribute(typeof(CheckedListEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[TypeConverter(typeof(CheckedListConverter))]
		public ICheckedList BoundTables
		{
			get
			{
				CheckedList checkedList=new CheckedList();
				CustomTables customTables=type.Lib.CustomTables;
/*!!!				ArrayList boundTables=customTables.GetBoundTables(type.Id);
				foreach(CustomDataTable ct in customTables.Tables)
				{
					checkedList.Add(ct,boundTables.Contains(ct));
				}*/
				return checkedList;
			}
			set
			{

			}
		}
	}

}
