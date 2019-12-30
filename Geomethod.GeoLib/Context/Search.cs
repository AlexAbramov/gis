using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Collections;
using Geomethod.Data;

namespace Geomethod.GeoLib
{
	public class SearchUtils
	{
		public static void Search(GLib lib,string text,int typeId,DataTable dataTable)
		{
			dataTable.BeginInit();
			dataTable.Rows.Clear();
			dataTable.Columns.Clear();
			dataTable.Clear();

			dataTable.Columns.Add("Type",typeof(string));
			dataTable.Columns.Add(ObjectField.Name.ToString(),typeof(string));
			dataTable.Columns.Add(ObjectField.Caption.ToString(),typeof(string));
			dataTable.Columns.Add(ObjectField.RangeId.ToString(),typeof(int)).ColumnMapping=MappingType.Hidden;
			dataTable.Columns.Add(ObjectField.Id.ToString(),typeof(int)).ColumnMapping=MappingType.Hidden;

			ArrayList ar=new ArrayList(Constants.maxSearchCount);
			Search(lib,text,typeId,ar);
			foreach(GObject obj in ar)
			{
				dataTable.Rows.Add(new object[5]{obj.Type.Name,obj.Name,obj.Caption,obj.RangeId,obj.Id});
			}
			dataTable.EndInit();
		}
		public static void SqlSearch(GLib lib,string text,int typeId,DataTable dataTable)
		{
			text=text.ToLower();
			string query=string.Format("select top {0} o.Name, t.Name as Type, o.Caption, o.RangeId, o.Id from Objects o left join Types t on TypeId=t.Id where o.Name like '%{1}%' or Caption like '%{1}%' ",Constants.maxSearchCount,text);
			if(typeId!=0) query+=" and t.Id="+typeId.ToString();
			using(Context context=lib.GetContext())
			{
				using(DbDataAdapter dataAdapter=context.Conn.CreateDataAdapter(query))
				{
					dataAdapter.Fill(dataTable);
				}
			}
		}
		public static void Search(GLib lib,string text,int typeId,ArrayList ar)
		{
			text=text.ToLower();			
			if(typeId==0) foreach(GType type in lib.Types) Search(type,text,ar);
			else
			{
				GType type=lib.GetType(typeId);
				if(type!=null) Search(type,text,ar);
			}
		}
		static void Search(GType type,string text,ArrayList ar)
		{
			if(type.Ranges!=null) foreach(GRange range in type.Ranges)
			{
				if(range.Objects!=null) foreach(GObject obj in range.Objects)
				{
					if(text.Length==0 || obj.Name.ToLower().IndexOf(text)>=0 || obj.Caption.ToLower().IndexOf(text)>=0)
					{
						if(ar.Count>=Constants.maxSearchCount) return;
						ar.Add(obj);
					}
				}
			}
			if(type.Types!=null) foreach(GType childType in type.Types) Search(childType,text,ar);
		}
	}

}