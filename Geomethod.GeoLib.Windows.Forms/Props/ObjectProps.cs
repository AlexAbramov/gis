using System;
using System.Data;
using System.ComponentModel;
using System.Collections;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for ObjectProps.
	/// </summary>
	public class ObjectProps: LocalizedObject
	{
		GObject obj;
		public ObjectProps(GObject obj):base(obj)
		{
			this.obj=obj;
		}
		[LocalizedCategory("_misc")]
		[LocalizedProperty("_id",Description="_gobjiddescr")]
		public int Id
		{
			get
			{
				return obj.Id;
			}
		}
		[LocalizedCategory("_misc")]
		[LocalizedProperty("_gobjtype",Description="_gobjtypedescr")]
		public string Type
		{
			get
			{
				return obj.Type.Name;
			}
		}
		[LocalizedCategory("_misc")]
		[LocalizedProperty("_gobjname",Description="_gobjnamedescr")]
		public string Name
		{
			get
			{
				return obj.Name;
			}
			set
			{
				obj.Name=value;
			}
		}
		[LocalizedCategory("_misc")]
		[LocalizedProperty("_gobjcaption",Description="_gobjcaptiondescr")]
		public string Caption
		{
			get
			{
				return obj.Caption;
			}
			set
			{
				obj.Caption=value;
			}
		}
		[LocalizedCategory("_misc")]
		[LocalizedProperty("_gobjstyle",Description="_gobjstyledescr")]
		public string StyleStr
		{
			get
			{
				return obj.StyleStr;
			}
			set
			{
				obj.StyleStr=value;
			}
		}

		[LocalizedCategory("_misc")]
		public string[] Records
		{
			get
			{
				ArrayList ar=new ArrayList();
/*!!!				foreach(CustomDataTable ct in obj.Lib.CustomTables.GetBoundTables(obj.TypeId))
				{
					ArrayList records=ct.GetRecords(obj.Id);
					if(records.Count>0)
					{
						foreach(DataRow dr in records)
						{
							string s="";
							foreach(DataColumn dc in ct.DataTable.Columns)
							{
								if(s.Length>0) s+=", ";
								s+=string.Format("{0}: {1}",dc.ColumnName,dr[dc].ToString());
							}
							ar.Add(string.Format("[{0}] {1}",ct.Name,s));
						}
					}
				}*/
				return (string[]) ar.ToArray(typeof(string));
			}
		}

	}
}
