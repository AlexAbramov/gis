using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using Geomethod;
using Geomethod.GeoLib;
using WeifenLuo.WinFormsUI.Docking;
using Geomethod.Windows.Forms;

namespace WinMap
{
	public class WinMapException : Exception
	{
		public WinMapException() : base() { }
		public WinMapException(string message) : base(message) { }
		public WinMapException(string message, Exception innerException) : base(message, innerException) { }
	}

    public class DockPanelUtils
    {
        internal static void LoadFromXml(string filePath, WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel, WeifenLuo.WinFormsUI.Docking.DeserializeDockContent m_deserializeDockContent)
        {
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(App.DockPanelConfigFilePath, FileMode.Open))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    for (int i = 0; i < bytes.Length; i++) bytes[i] -= (byte)i;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        dockPanel.LoadFromXml(ms, m_deserializeDockContent);
                    }
                }
            }
        }

        internal static void SaveAsXml(string filePath, WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (MemoryStream ms = new MemoryStream(1 << 12))
                {
                    dockPanel.SaveAsXml(ms, System.Text.Encoding.ASCII);
                    byte[] bytes=ms.ToArray();
                    for (int i = 0; i < bytes.Length; i++) bytes[i]+=(byte)i;
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        internal static void Localize(DockContent dockContent)
        {
//!!!            dockContent.TabText = LocaleUtils.LocalizeAndRemoveHotKey(dockContent.TabText);
//            dockContent.ToolTipText = LocaleUtils.LocalizeAndRemoveHotKey(dockContent.ToolTipText);
//            ocaleUtils.Localize(dockContent);
        }
    }

}
