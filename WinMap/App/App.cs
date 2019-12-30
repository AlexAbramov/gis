using System;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Configuration;
using Geomethod;
using Geomethod.Data;
using Geomethod.Windows.Forms;
using Geomethod.GeoLib;
using WinMap.Forms;
using Geomethod.GeoLib.Windows.Forms;
using Geomethod.GeoLib.Converters;
using Geomethod.GeoLib.Windows.Forms.Edit;

namespace WinMap
{				
	public class App: IGeoApp
	{
		#region Static
		static App instance = null;
		public static App Instance { get { return instance; } }
		public static string ConnectionsFilePath{get{return PathUtils.BaseDirectory+"Config\\Connections.xml";}}
        public static string DockPanelConfigFilePath { get { return PathUtils.BaseDirectory + "Config\\DockPanel.cfg"; } }//.bin
		#endregion

		#region Fields
		MainForm mainForm;
		Config config;

		ConnectionsConfig connConfig;
		ConnectionInfo connection=null;
        GLib lib =null;
		Layer layer=null;
		EditObject editObject=null;


		BitArray32 attr=new BitArray32(0);

		#endregion

		#region Properties
		public bool HasLib { get { return lib != null; } }
		public bool EditMode { get { return editObject != null; } }
		public MainForm MainForm{get{return mainForm;}}
		public GType DraggedType{get{return mainForm.TypesUserControl.DraggedType;}}
		public Config Config{get{return config;}}
		public ConnectionsInfo Connections{get{return connConfig.connectionsInfo;}}
		public ConnectionInfo Connection{get{return connection;}set{connection=value;}}
		public string HelpFilePath{get{return string.Format(@"{0}\Help\{1}\{2}.chm",PathUtils.BaseDirectory,Locale.StringSet.Name,Application.ProductName);}}
		#endregion

		#region Construction
		public App(MainForm mainForm, Config config)
		{
			if (instance != null) throw new WinMapException("App instance already initialized.");
			instance = this;
			SetAttr(ControlsAttr.AutoSave, true);
			SetAttr(ControlsAttr.ShowPropertiesOnSelect, true);
			Log.Info("Application started.");
			this.mainForm=mainForm;
			this.config=config;
			connConfig=ConnectionsConfig.DeserializeFile(ConnectionsFilePath);
            GmProviders.Add(new OracleProvider());
        }

        private void SetAttr(ControlsAttr a, bool val)
		{
			attr[(int)a] = val;
		}

		public void Close()
		{
			try
			{
				Log.Info("Application closed.");
				Log.LogSystem.Close();
			}
			catch
			{
			}
		}
		#endregion

		#region Editing
		public void StartEditing(GType type)
		{
			if(DraggedType!=null) mainForm.TypesUserControl.EndDragging();
			editObject=new EditObject(this, type);
			Lib.Selection.Clear();
			Lib.Selection.Add(editObject);
			foreach(MapForm mapForm in mainForm.MapForms)
			{
				mapForm.MapUserControl.Cursor=Cursors.Cross;
			}
		}

		public void StartEditing(GObject obj)
		{
			editObject=new EditObject(this, obj);


			Lib.Selection.Clear();
			Lib.Selection.Add(editObject);
			editObject.CheckRepaint();
			foreach(MapForm mapForm in mainForm.MapForms)
			{
				mapForm.MapUserControl.Cursor=Cursors.Cross;
			}			
		}

		public void EndEditing()
		{
			if(!this.EditMode) return;
			try
			{
				foreach(MapForm mapForm in mainForm.MapForms)
				{
					mapForm.MapUserControl.Cursor=Cursors.Default;
				}
				GObject obj=null;
				if(editObject.OrigObject==null)
				{
					if(MessageBox.Show(Locale.Get("_createobject"),Application.ProductName,MessageBoxButtons.YesNo)==DialogResult.Yes)
					{
						obj=editObject.Create();
					}
				}
				else
				{
					if(MessageBox.Show(Locale.Get("_savechanges"),Application.ProductName,MessageBoxButtons.YesNo)==DialogResult.Yes)
					{
						obj=editObject.UpdateOrigObject();
					}
				}
				if(obj!=null)
				{
//					if(lib.HasDb) using(Context context=lib.GetContext()){obj.Save(context);}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
			finally
			{
				lib.Selection.Clear();
				this.CheckRepaint(editObject);
				editObject=null;
			}
		}

		#endregion

		#region Lib
		public void CreateLib(ConnectionInfo conn)
		{
            if (!SaveAndCloseLib()) return;
            try
            {
                using (WaitCursor wr = new WaitCursor(mainForm, Locale.Get("_createMap...")))
                {
                    connection = conn;
                    Rect bounds = new Rect(0, 0, 1000000, 1000000);
                    lib = new GLib(connection.CreateConnectionFactory(), bounds, new Indexer());
                    lib.Name = conn.name;
                    lib.SMin = 10;
                    lib.SMax = GeoLibUtils.RoundScale(bounds.MaxSize / 10);
                    lib.StyleStr = "pc=red";
                    lib.Scales.InitScales();
                    GType type;
                    GeomType[] geomTypes ={ GeomType.Polygon, GeomType.Polyline, GeomType.Point, GeomType.Caption };
                    foreach (GeomType gt in geomTypes)
                    {
                        type = new GType(lib, gt);
                        type.Name = Locale.Get(gt.ToString());
                    }
                    if (conn.name.Length == 0)
                    {
                        lib.Name = "Generic library";
                        Geomethod.GeoLib.MapCreator.CreateTestObjects(lib, config.testObjectCount);
                    }
                    if (lib.HasDb)
                    {
                        lib.Save(BatchLevel.Object);
                        try
                        {
                            UpdateConnectionList(connection);
                        }
                        catch (Exception ex)
                        {
                            Log.Exception(ex);
                        }
                    }
                    LibLoaded();
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                CloseLib();
            }
		}

        public void CreateShapesLib( string[] fileNames )
        {
            if (!SaveAndCloseLib()) return;
            try
            {
                using( WaitCursor wr = new WaitCursor( mainForm, "_loadingShapeFiles..." ) )
                {
                    ShapeLoader shapeLoader = new ShapeLoader( fileNames );
                    lib = shapeLoader.Load( );
                    LibLoaded();
                }
            }
            catch( Exception ex )
            {
                Log.Exception( ex );
                CloseLib( );
            }
        }

        public void CreateMIFLib( string[] fileNames )
        {
            if (!SaveAndCloseLib()) return;
            try
            {
                using( WaitCursor wr = new WaitCursor( mainForm, Locale.Get("_loadingMifFiles...")) )
                {
                    MIFLoader mifLoader = new MIFLoader( fileNames );
                    lib = mifLoader.Load( );
                    LibLoaded();
                }
            }
            catch( Exception ex )
            {
                Log.Exception( ex );
                CloseLib( );
            }
        }
        public void CreateDXFLib( string[] fileNames )
        {
            if (!SaveAndCloseLib()) return;
            try
            {
                using (WaitCursor wr = new WaitCursor(mainForm, "_loadingDxfFiles..."))
                {
                    DXFLoader dxfLoader = new DXFLoader(fileNames);
                    lib = dxfLoader.Load();
                    LibLoaded();
                }
            }
            catch( Exception ex )
            {
                Log.Exception( ex );
                CloseLib();
            }
        }

		public void LoadLib(ConnectionInfo connection)
		{
            if (!SaveAndCloseLib()) return;
            try
            {
                using (WaitCursor wr = new WaitCursor(mainForm, "_loading..."))
                {
                    this.connection = connection;
                    lib = new GLib(connection);
                    LibLoaded();
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                CloseLib();
            }
		}

        private void LibLoaded()
        {
            layer = new Layer(lib);
            MainForm.InitLibControls();
        }

        public bool SaveAndCloseLib()
        {
            if (lib != null)
            {
                if (lib.IsChanged)
                {
                    DialogResult res = MessageBox.Show(Locale.Get("_savechanges"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (res)
                    {
                        case DialogResult.Yes: mainForm.Save(); break;
                        case DialogResult.No: break;
                        case DialogResult.Cancel: return false;
                    }
                }
                CloseLib();
            }
            return true;
        }

		public void CloseLib()
		{
            if (lib != null)
            {
                this.mainForm.CloseMaps();
                lib.Clear();
                lib = null;
                mainForm.InitLibControls();
            }
            layer = null;
            this.connection = null;
		}

		#endregion

		#region Connections
		public void SaveConnections()
		{
			connConfig.Serialize(App.ConnectionsFilePath);
		}

/*        private void UpdateDb(GisConnection connection)
        {
            // update database
            GmProviderFactory fact = connection.GetProviderFactory();
            if (fact != null)
            {
                using (WaitCursor wr = new WaitCursor(mainForm, Locale.Get("_updatingDb...")))
                {
                    MapCreator.UpdateDb(fact);
                }
            }
        }*/

        public void UpdateConnectionList(ConnectionInfo connection)
        {
            Connections.Add(connection);
            SaveConnections();
            /*			string si=StringUtils.NotNullString(options.serverInstances[mc.providerName] as string);
                        if(si!=mc.serverInstance)
                        {
                            options.serverInstances[mc.providerName]=mc.serverInstance;
                            options.Save();
                        }*/
        }
        #endregion

		#region Misc
		public string Status
		{
			get{return mainForm.Status;}
			set{mainForm.Status=value;}
		}

		#endregion

		#region Checkers
		private void CheckCompositeNameChanged(GComposite comp)
		{
			mainForm.TypesUserControl.UpdateNodeText(comp);
		}

		private void CheckMirrorChanged()
		{
			foreach (MapForm mapForm in mainForm.MapForms)
			{
				if (mapForm.ucMap.Map.Mirror != lib.Mirror)
				{
					mapForm.ucMap.Map.Mirror = lib.Mirror;
					mapForm.ucMap.Repaint();
				}
			}
		}
		public void CheckRepaint(object obj)
		{
            if (obj is IShapedObject)
            {
                foreach (MapForm mapForm in mainForm.MapForms)
                {
                    if ((obj as IShapedObject).IsVisibleOnMap(mapForm.Map)) mapForm.MapUserControl.Repaint();
                }            
            }
            if(obj is Rect)
            {
                Rect bounds = (Rect)obj;
			    foreach (MapForm mapForm in mainForm.MapForms)
			    {
				    if (mapForm.Map.Intersects(bounds)) mapForm.MapUserControl.Repaint();
			    }
            }
		}

        public void Repaint()
        {
            foreach (MapForm mapForm in mainForm.MapForms)
            {
                mapForm.MapUserControl.Repaint();
            }
        }
        #endregion 

		#region IGeoApp Members

		public EditObject EditObject { get { return editObject; } }
		public GLib Lib { get { return lib; } }
		public Layer Layer{get{return layer;}}
		public Map CurrentMap { get { return mainForm.MapForm!=null?mainForm.MapForm.Map:null; } }
        public MapUserControl CurrentMapControl { get { return mainForm.MapForm != null ? mainForm.MapForm.ucMap : null; } }

		public void DataChanged(object obj)
		{
			ISerializableObject ser = obj as ISerializableObject;
			if (ser != null)
			{
				switch (ser.ClassId)
				{ 
					case ClassId.Lib:
						if(lib.IsUpdated(LibField.Attr)) CheckMirrorChanged();
						if(lib.IsUpdated(LibField.Name)) CheckCompositeNameChanged(lib);
						break;
					case ClassId.Type:
						GType type = (GType)obj;
						if (lib.IsUpdated(LibField.Name)) CheckCompositeNameChanged(type);
						break;
					case ClassId.BgImage:
						mainForm.RepaintMapForms();
						break;
					case ClassId.Layer:
						UpdateControls();
						break;
					default:
						break;
				}
			}
			IShapedObject iobj = obj as IShapedObject;
			if (iobj != null)
			{
				CheckRepaint(iobj);
			}
			if (obj is Layers)
			{
				UpdateControls();
			}
			if (obj is Map)
			{
                UpdateControls();
			}
		}

		public void ShowProperties(object obj)
		{
			object selObj = obj;
			if (MainForm.PropertiesUserControl.SelectedObject == selObj) return;
			string caption = Locale.Get("_properties");
			if (obj is GObject)
			{
				GObject gobj = (GObject)obj;
				selObj = new ObjectProps(gobj);
				caption = Locale.Get("_gobject");
				mainForm.TypesUserControl.SelectType(gobj.Type);
			}
			else if (obj is GType)
			{
				selObj = new TypeProps((GType)obj);
				caption = Locale.Get("_gtype");
			}
			else if (obj is GLib)
			{
				selObj = new LibProps((GLib)obj);
				caption = Locale.Get("_glib");
			}
			else if (obj is BgImage)
			{
				selObj = new BgImageProps((BgImage)obj);
				caption = Locale.Get("_bgimage");
			}
/*			else if (obj is Config)
			{
				selObj = new OptionsProps((Config)obj);
				caption = Locale.Get("_optionsprops");
			}*/
			else if (obj is Stat)
			{
				selObj = new StatProps((Stat)obj);
				caption = Locale.Get("_stat");
			}
			else if (obj is LibStatProps || obj is StatProps)
			{
				caption = Locale.Get("_stat");
			}
			StringUtils.RemoveHotKey(ref caption);
			MainForm.PropertiesUserControl.Text = caption;
			MainForm.PropertiesUserControl.SelectedObject = selObj;
		}
		public void UpdateScaleCombo()
		{
			mainForm.UpdateScaleCombo();
		}

        public void UpdateControls()
        {
            mainForm.UpdateMapControls();
        }

		public bool GetControlsAttr(ControlsAttr a)
		{
            if (a == ControlsAttr.AutoSave) return lib!=null && lib.HasDb;
			return attr[(int)a];
		}

		#endregion

    }
}
