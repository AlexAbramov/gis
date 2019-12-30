using System;
using System.Globalization;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Geomethod.GeoLib.Windows.Forms
{
	public partial class CheckedListUserControl : System.Windows.Forms.UserControl
	{
		ICheckedList checkedList;

		public CheckedListUserControl(ICheckedList checkedList)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.checkedList=checkedList;
			for(int i=0;i<checkedList.Count;i++)
			{
				this.clbItems.Items.Add(checkedList.Item(i),checkedList.IsChecked(i));
			}
		}

		public void UpdateCheckedList()
		{
			for(int i=0;i<this.clbItems.Items.Count;i++)
			{
				bool b=this.clbItems.GetItemChecked(i);
				if(checkedList.IsChecked(i)!=b) checkedList.SetChecked(i,b);
			}	
		}

		private void clbItems_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}
	}


	public interface ICheckedList
	{
		int Count{get;}
		object Item(int index);
		bool IsChecked(int index);
		void SetChecked(int index,bool val);
	}


	public class CheckedList: ICheckedList
	{
		class CheckedItem: IComparable
		{
			public object obj;
			public bool isChecked;
			public CheckedItem(object obj, bool isChecked){this.obj=obj;this.isChecked=isChecked;}
			#region IComparable Members

			public int CompareTo(object obj)
			{
				CheckedItem ci=obj as CheckedItem;
				if(ci!=null && this.obj is IComparable)
				{
					return ((IComparable)this.obj).CompareTo(ci.obj);
				}
				return -1;
			}

			#endregion
		}
		ArrayList items;
		public CheckedList()
		{
			items=new ArrayList();
		}
		public void Add(object obj, bool val)
		{
			items.Add(new CheckedItem(obj,val));
		}

		public void Update(ICheckedList l)
		{
			for(int i=0;i<Count;i++)
			{
				bool b=l.IsChecked(i);
				if(IsChecked(i)!=b) SetChecked(i,b);
			}
		}
		
		public void Sort()
		{
			items.Sort();
		}
		#region ICheckedList Members

		public int Count
		{
			get
			{
				// TODO:  Add CheckedList.Count getter implementation
				return items.Count;
			}
		}

		public object Item(int index)
		{
			// TODO:  Add CheckedList.Item implementation
			return ((CheckedItem)items[index]).obj;
		}

		public bool IsChecked(int index)
		{
			// TODO:  Add CheckedList.IsChecked implementation
			return ((CheckedItem)items[index]).isChecked;
		}

		public void SetChecked(int index, bool val)
		{
			((CheckedItem)items[index]).isChecked=val;
		}

		#endregion

	}


	public class CheckedListEditor : System.Drawing.Design.UITypeEditor
	{        
		public CheckedListEditor()
		{
		}

		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")] 
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
		{            
			if(value is ICheckedList)
			{

				// Uses the IWindowsFormsEditorService to display a 
				// drop-down UI in the Properties window.
				IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if( edSvc != null )
				{
					CheckedListUserControl checkedListControl = new CheckedListUserControl((ICheckedList)value);
					edSvc.DropDownControl( checkedListControl );
					checkedListControl.UpdateCheckedList();
				}
			}
			return value;
		}
	}


	public class CheckedListConverter: TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context,
			System.Type destinationType) 
		{
			if (destinationType.IsSubclassOf(typeof(ICheckedList)))
				return true;

			return base.CanConvertTo(context, destinationType);
		}	

		public override object ConvertTo(ITypeDescriptorContext context, 
			CultureInfo culture, object value, Type destinationType) 
		{  
			if (destinationType == typeof(string) && 
				value is ICheckedList)
			{
				ICheckedList checkedList=(ICheckedList)value;
				StringBuilder sb=new StringBuilder();
				for(int i=0;i<checkedList.Count;i++)
				{
					if(checkedList.IsChecked(i))
					{
						if(sb.Length>0) sb.Append(", ");
						sb.Append(checkedList.Item(i).ToString());
					}
				}
				return sb.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
