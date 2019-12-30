using System;
using System.Collections.Generic;
using System.Text;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;
using Geomethod.GeoLib.Windows.Forms.Edit;

namespace Geomethod.GeoLib.Windows.Forms
{
	public enum ControlsAttr {AutoSave,ShowPropertiesOnSelect}

	public interface IGeoApp: IStatus
	{
		bool GetControlsAttr(ControlsAttr a);
		GLib Lib { get;}
		Layer Layer { get;}
		Map CurrentMap { get;}
		MapUserControl CurrentMapControl { get;}
		EditObject EditObject { get;}
		GType DraggedType { get; }
		void ShowProperties(object obj);
		void StartEditing(GType type);
		void StartEditing(GObject obj);
		void EndEditing();
        void DataChanged(object obj);
        void CheckRepaint(object obj);
        void UpdateControls();
    }
}
