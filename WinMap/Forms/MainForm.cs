using System;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Common;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Data;
using Geomethod.Windows.Forms;
using Geomethod.Data.Windows.Forms;
using Geomethod.GeoLib.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WinMap.Forms
{
	public partial class MainForm : System.Windows.Forms.Form, IStatus
	{
		#region Fields
		internal static SplashForm splashForm = new SplashForm();
		private DeserializeDockContent m_deserializeDockContent;
		bool dockPanelLoaded = false;

		App app;
		TypesForm typesForm;
		LayersForm layersForm;
		ViewsForm viewsForm;
		SearchForm searchForm;
		BackgroundForm backgroundForm;
		PropertiesForm propertiesForm;
        //        CoordsForm coordDTForm;
        ConnectData connectData;
        DockContent[] dockPanels;

        

		bool loaded=false;
		bool updateScaleComboFlag=false;
		List<MapForm> mapForms = new List<MapForm>();
		#endregion

		public MainForm()
		{
	    //
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			if(!base.DesignMode)
			{
				Log.LogSystem.AddLogHandlers(new FileLog("WinMap.log"),new MessageFormLogInformer());
//				Config config = new Config();
//				config.Save();
				Config config = Config.Load();
				Locale.StringSet.Load();// = new StringSet(config.language.ToString(), "Locale\\"+Application.ProductName);
				Assembly assembly = Assembly.GetAssembly(typeof(Geomethod.GeoLib.GLib));
				SqlQueries.StringSet=new StringSet("common",assembly,"Geomethod.GeoLib.Resources.GeoLibSql.csv");
				app=new App(this, config);

				typesForm = new TypesForm(app);
				layersForm = new LayersForm(app);
				viewsForm = new ViewsForm(app);
				searchForm = new SearchForm(app);
				backgroundForm = new BackgroundForm(app);
				propertiesForm = new PropertiesForm(app);

//                coordDTForm = new CoordsForm( app );

				DockContent[] dockPanels ={ typesForm, layersForm, viewsForm, searchForm, 
                    backgroundForm, propertiesForm/*, coordDTForm*/ };
				this.dockPanels = dockPanels;

				m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

                // update database
                //				UpdateScripts.UpdateDb(null, "CardiologyDepartment.Data.UpdateScripts.txt", providerFactory);
                
                string[] providers = { AccessProvider.name, SqlServerProvider.name, OracleProvider.name };
                connectData = new ConnectData("Data", "Map", providers, app.Connections);

            }
        }

		#region Properties
		public int MapCount { get { return mapForms.Count; } }
		App App { get { return app; } }
		GLib Lib { get { return app.Lib; } }
		public TypesUserControl TypesUserControl { get { return typesForm.ucTypes; } }
		public PropertiesUserControl PropertiesUserControl{get{return propertiesForm.ucProperties;}}
		public SearchUserControl SearchUserControl{get{return searchForm.ucSearch;}}
		public string Title
		{
			get{return Text;}
			set
			{
				if(value==null) value="";
				const string appName="WinMap";
				Text=value.Length>0 ? value+" - "+appName : appName;
			}
		}
		public string Status
		{
			get { return lblStatus.Text; }
			set
			{
				if(value==null) value="";
				lblStatus.Text = value;
			}
		}
		#endregion

		#region Methods
		private IDockContent GetContentFromPersistString(string persistString)
		{
			foreach (DockContent dc in dockPanels)
			{
				if (persistString == dc.GetType().FullName) return dc;
			}
/*			if (persistString == typeof(TypesForm).ToString()) return typesForm;
			else if (persistString == typeof(LayersForm).ToString()) return layersForm;
			else if (persistString == typeof(ViewsForm).ToString())	return viewsForm;
			else if (persistString == typeof(SearchForm).ToString()) return searchForm;
			else if (persistString == typeof(BackgroundForm).ToString()) return backgroundForm;
			else if (persistString == typeof(PropertiesForm).ToString()) return propertiesForm;
						else
									{
										string[] parsedStrings = persistString.Split(new char[] { ',' });
										if (parsedStrings.Length != 3)
											return null;

										if (parsedStrings[0] != typeof(DummyDoc).ToString())
											return null;

										DummyDoc dummyDoc = new DummyDoc();
										if (parsedStrings[1] != string.Empty)
											dummyDoc.FileName = parsedStrings[1];
										if (parsedStrings[2] != string.Empty)
											dummyDoc.Text = parsedStrings[2];

										return dummyDoc;
									}*/
			return null;
		}
		private void FixLayout(ToolStripPanel toolStripPanel)
		{
			toolStripPanel.SuspendLayout();
			foreach (Control c in toolStripPanel.Controls)
			{
				ToolStrip ts = c as ToolStrip;
				if (ts != null)
				{
					ts.Location = new Point(0, 0);
				}
			}
			toolStripPanel.ResumeLayout();
		}

		#endregion

		private void miAbout_Click(object sender, System.EventArgs e)
		{
			About();
		}

		void About()
		{
			try
			{
				AboutForm about=new AboutForm();
				about.ShowDialog(this);
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miOpen_Click(object sender, System.EventArgs e)
		{
			Open();
		}

		void New()
		{
			try
			{
				CreateDbForm form=GetCreateDbForm();
				if (form.ShowDialog() == DialogResult.OK)
				{
                    if (form.IsDbProvider)
                    {
                        using (WaitCursor wr = new WaitCursor(this, Locale.Get("_createDb...")))
                        {
                            ConnectionInfo gisConn = MapCreator.CreateGisDb(form.DbCreationProperties);
                            app.CreateLib(gisConn);
                        }
                    }
                    else
                    {
                        app.CreateLib(new ConnectionInfo(form.DbCreationProperties.dbName,form.FilePath));
                    }
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void InitScaleCombo()
		{
			cbScale.BeginUpdate();
			cbScale.Items.Clear();
			if (Lib != null)
			{
				foreach(int sc in Lib.ScalesArray) cbScale.Items.Add(sc);
			}
			cbScale.EndUpdate();
		}

		void Open()
		{
			try
			{
				if (this.dlgOpenFile.ShowDialog() == DialogResult.OK)
				{
					string filePath=dlgOpenFile.FileName;
					string ext=PathUtils.GetExtension(filePath);
					switch(ext)
					{
						case "wdr":
							ConnectionInfo connection = new ConnectionInfo(Path.GetFileNameWithoutExtension(filePath), "", "", "file=" + filePath);
							app.LoadLib(connection);
							break;
                        case "shp":
                            app.CreateShapesLib( dlgOpenFile.FileNames );
                            break;
                        case "mif":
                            app.CreateMIFLib( dlgOpenFile.FileNames );
                            break;
                        case "dxf":
                            app.CreateDXFLib( dlgOpenFile.FileNames );
                            break;
                    }                     
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		void Connect()
		{
			try
			{
				ConnectionsInfoForm form = new ConnectionsInfoForm(connectData);
				DialogResult dialogResult = form.ShowDialog();
				if(form.ConnectionsUpdated) app.SaveConnections();
				if(dialogResult==DialogResult.OK)
				{
					app.LoadLib(form.GetSelectedConnection());
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (!base.DesignMode)
				{
					toolStripContainer.Height = 26;
					FixLayout(toolStripContainer.TopToolStripPanel);
					GmApplication.Initialize(this);
					dlgOpenFile.InitialDirectory = PathUtils.BaseDirectory + "Data";
					dlgOpenFile.Filter = FileFilter.VectorFilter;
					string[] ss2 ={ FileFilter.wdr };
					dlgSaveFile.Filter = FileFilter.GetString(ss2);
					// this.tcLeftTop.SelectedTab=this.tpBackground;//ms bug fix
					// this.tcLeftTop.SelectedTab=this.tpTypes;//ms bug fix
					InitLibControls();
					splashForm.Close();
					splashForm.Dispose();
					splashForm = null;
                    miDebug.Visible = App.Config.debug > 0;
                    dockPanel.ActiveDocumentChanged += new EventHandler(dockPanel_ActiveDocumentChanged);
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

        void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            try
            {
                if (App.HasLib)
                {
                    UpdateMapControls();
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

		void Print()
		{
			MapForm mapForm=this.MapForm;
			if(mapForm!=null)
            {
                if(dlgPrintPreview.ShowDialog()==DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
		}

		public MapForm MapForm
		{
			get
			{
				return dockPanel.ActiveDocument as MapForm;
			}
		}

		public IEnumerable<MapForm> MapForms
		{
			get
			{
				return mapForms;
			}
		}

		void ShowScales()
		{
			try
			{
				if (app.Lib == null) return;
				ScalesForm scalesForm = new ScalesForm(app.Lib);
				if (scalesForm.ShowDialog(this) == DialogResult.OK)
				{
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		internal void Save()
		{
			try
			{
				using(WaitCursor wc=new WaitCursor(this,Locale.Get("_savingData")+"..."))
				{
					if (app.Connection == null)
					{
						string filePath = SaveAs();
						if (filePath != null)
						{
							app.Lib.SetSaved();
							string connName = Path.GetFileNameWithoutExtension(filePath);
							app.Connection = new ConnectionInfo(connName, "", "", "file=" + filePath);
						}
					}
					else
					{
						GLib lib = app.Lib;
						if (lib != null)
						{
							if (app.Connection.connectionString.Length > 0)
							{
								using (Context context = lib.GetContext())
								{
									context.Filter = Filter.All;
									lib.Save(context);
									lib.SetSaved();
								}
							}
							else if (app.Connection.FilePath.Length > 0)
							{
								string filePath = app.Connection.FilePath;
								using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
								{
									using (BinaryWriter bw = new BinaryWriter(fs))
									{
										using (Context context = lib.GetContext())
										{
											context.Filter = new Filter(BatchLevel.Object);
											lib.Write(context, bw);
											lib.SetSaved();
										}
									}
								}
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
			finally
			{
				UpdateLibControls();
			}
		}

		void ShowColors()
		{
			try
			{
				if (app.Lib != null)
				{
					ColorsForm form = new ColorsForm(app);
					if(form.ShowDialog()==DialogResult.OK)
					{
					}
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		void ScaleUp()
		{
			try
			{
				MapForm mapForm=this.MapForm;
				if(mapForm==null) return;
				mapForm.MapUserControl.ScaleUp();
				UpdateMapControls();
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		public void UpdateScaleCombo(){UpdateScaleCombo(null);}
		void UpdateScaleCombo(MapForm mapForm)
		{
			if(mapForm==null) mapForm=this.MapForm;
            if (mapForm != null)
            {
                updateScaleComboFlag = true;
                cbScale.Text = mapForm.Map.Scale.ToString();
                mapForm.UpdateTitle();
                updateScaleComboFlag = false;
            }
		}

		private void miScaleDown_Click(object sender, System.EventArgs e)
		{
			ScaleDown();
		}
		
		void ScaleDown()
		{
			try
			{
				MapForm mapForm=this.MapForm;
				if(mapForm==null) return;
				mapForm.MapUserControl.ScaleDown();
				UpdateMapControls();
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miLeft_Click(object sender, System.EventArgs e)
		{
			MoveLeft();
		}

		void MoveLeft()
		{
			try
			{
				MapForm mapForm=this.MapForm;
				if(mapForm==null) return;
				mapForm.MapUserControl.Move(Direction.Left);
				UpdateMapControls();
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miTop_Click(object sender, System.EventArgs e)
		{
			MoveTop();
		}

		void MoveTop()
		{
			try
			{
				MapForm mapForm=this.MapForm;
				if(mapForm==null) return;
				mapForm.MapUserControl.Move(Direction.Top);
				UpdateMapControls();
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miRight_Click(object sender, System.EventArgs e)
		{
			MoveRight();
		}

		void MoveRight()
		{
			try
			{
				MapForm mapForm=this.MapForm;
				if(mapForm==null) return;
				mapForm.MapUserControl.Move(Direction.Right);
				UpdateMapControls();
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miBottom_Click(object sender, System.EventArgs e)
		{
			MoveBottom();
		}

		void MoveBottom()
		{
			try
			{
				MapForm mapForm=this.MapForm;
				if(mapForm==null) return;
				mapForm.MapUserControl.Move(Direction.Bottom);
				UpdateMapControls();
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miRotateCW_Click(object sender, System.EventArgs e)
		{
			RotateCW();
		}

		void RotateCW()
		{
			try
			{
				MapForm mapForm=this.MapForm;
				if(mapForm==null) return;
				mapForm.MapUserControl.RotateCW();
				UpdateMapControls();
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miRotateCCW_Click(object sender, System.EventArgs e)
		{
			RotateCCW();
		}

		void RotateCCW()
		{
			try
			{
				MapForm mapForm=this.MapForm;
				if(mapForm==null) return;
				mapForm.MapUserControl.RotateCCW();
				UpdateMapControls();
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		public void OnMapFormOpened(MapForm mapForm)
		{
			mapForms.Add(mapForm);
			UpdateScaleCombo(mapForm);
			if(!timer.Enabled) timer.Enabled=true;
		}

		public void OnMapFormClosed(MapForm mapForm)
		{
			mapForms.Remove(mapForm);
			if (MapCount == 0 && timer.Enabled) timer.Enabled = false;
			UpdateMapControls();
		}

		public void LayerChanged()
		{
			typesForm.ucTypes.UpdateTypesTree();
			this.RepaintMapForms();
		}

		void UpdateUserControls()
		{
		}

		#region State
		bool CanSave{get{return app.HasLib && app.Lib.IsChanged;}}
        bool CanSaveAs { get { return app.HasLib; } }
		bool CanScaleUp{get{return this.MapForm!=null && this.MapForm.Map.CanScaleUp;}}
		bool CanScaleDown { get { return this.MapForm != null && this.MapForm.Map.CanScaleDown; } }
		bool CanMove(Direction dir) {  return this.MapForm != null && this.MapForm.Map.CanMove(dir); } 

		public void InitLibControls()
		{
			typesForm.ucTypes.BuildTypesTree();
			layersForm.ucLayers.UpdateList();
			viewsForm.ucViews.UpdateList();
			searchForm.ucSearch.UpdateTypes();
			backgroundForm.ucBg.UpdateList();

			InitScaleCombo();

			app.ShowProperties(null);

            if (app.HasLib)
			{
                if (!dockPanelLoaded)
                {
                    try
                    {
                        DockPanelUtils.LoadFromXml(App.DockPanelConfigFilePath, dockPanel, m_deserializeDockContent);
                    }
                    catch (Exception ex)
                    {
                        Log.Exception(ex);
                        SetDefaultLayout();
                    }
                    finally
                    {
                        dockPanelLoaded = true;
                    }
                }
            }

			foreach (DockContent dc in dockPanels)
			{
				if (dc.DockState != DockState.Unknown)
				{
					if(app.HasLib) dc.Show();
					else dc.Hide();
				}
			}

            if (app.HasLib)
            {
                NewMapView(true);
            }

			UpdateLibControls();
//			this.btnSave.Visible=Lib!=null && !Lib.HasDb;
		}

        private void SetDefaultLayout()
        {
//            throw new Exception("The method or operation is not implemented.");
        }

		public void UpdateLibControls()
		{
			bool hasLib = Lib != null;
			Title=hasLib ? Lib.Name : "";
			Status="";
			miSave.Enabled=CanSave;
			btnSave.Enabled=CanSave;
			miSaveAs.Enabled=CanSaveAs;
			miExport.Enabled=CanSaveAs;
			miView.Visible = hasLib;
			miMap.Visible = hasLib;
            miClose.Enabled = hasLib;
            UpdateMapControls();
            if (hasLib) typesForm.Show();
		}

		internal void CloseMaps()
		{
			foreach(MapForm form in this.mapForms.ToArray()) form.Close();
			mapForms.Clear();
		}


		public void UpdateMapControls()
		{
			bool vis=app.Lib!=null && MapCount>0;
			tsNavigation.Visible =miNavigation.Visible = vis;
			if(vis)
			{
				btnScaleUp.Enabled=miScaleUp.Enabled=CanScaleUp;
				btnScaleDown.Enabled=miScaleDown.Enabled=CanScaleDown;
				btnMoveTop.Enabled = miMoveTop.Enabled = CanMove(Direction.Top);
				btnMoveBottom.Enabled = miMoveBottom.Enabled = CanMove(Direction.Bottom);
				btnMoveLeft.Enabled = miMoveLeft.Enabled = CanMove(Direction.Left);
				btnMoveRight.Enabled = miMoveRight.Enabled = CanMove(Direction.Right);
                UpdateScaleCombo();
			}
            miPrint.Enabled = vis;
            btnPrint.Enabled = vis;
        }
		#endregion

		private void cbScale_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(updateScaleComboFlag) return;
			SetScale();
		}

		void SetScale()
		{
			try
			{
				MapForm mapForm=this.MapForm;
				if (mapForm != null)
				{
					int scale = int.Parse(cbScale.Text);
					mapForm.Map.Scale = scale;
					if (mapForm.Map.Scale != scale)
					{
						this.UpdateScaleCombo(mapForm);
					}
					mapForm.UpdateTitle();
					mapForm.MapUserControl.Repaint();
					UpdateMapControls();
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void timer_Tick(object sender, System.EventArgs e)
		{
//			MapForm mapForm=this.MapForm;
//			if(mapForm!=null) mapForm.MapUserControl.OnTimer();
            if(App.HasLib && App.Lib.IsChanged && !btnSave.Enabled) UpdateLibControls();
			foreach (MapForm mapForm in mapForms) mapForm.ucMap.OnTimer();
		}

		private void MainForm_Activated(object sender, System.EventArgs e)
		{
			if(!loaded)
			{
				loaded=true;
//				Open();
			}
		}

		internal void RepaintMapForms()//to do: wise repaint!!!
		{
			foreach(MapForm form in mapForms)
			{
				if (form.Visible && form.VisibleState != DockState.Hidden)
				{
					form.ucMap.Repaint();
				}
			}		
		}

		private void ucTypes_Load(object sender, System.EventArgs e)
		{
		
		}

		private void tcLeft_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void tpTypes_Click(object sender, System.EventArgs e)
		{
		
		}

		private void miContents_Click(object sender, System.EventArgs e)
		{
			Contents();
		}

		string SaveAs()
		{
			GLib lib=app.Lib;
			if(lib==null) return null;
			try
			{
				dlgSaveFile.InitialDirectory=PathUtils.BaseDirectory+"Data";
				if(dlgSaveFile.ShowDialog()==DialogResult.OK)
				{
					using(WaitCursor wc=new WaitCursor(this,Locale.Get("_savingData")+"..."))
					{
						string filePath=dlgSaveFile.FileName;
						string ext=Path.GetExtension(filePath);
						Save(filePath);
						return filePath;
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
			return null;
		}

		void Save(string filePath)
		{
			GLib lib=app.Lib;
            if (lib != null)
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        using (Context context = lib.GetContext())
                        {
                            context.Filter = new Filter(BatchLevel.Object);
                            lib.Write(context, bw);
                        }
                    }
                }
            }
		}

		private void miOptions_Click(object sender, System.EventArgs e)
		{
			ShowOptions();
		}

		void ShowOptions()
		{
			try
			{
				ConfigForm form = new ConfigForm(app.Config);
				if (form.ShowDialog() == DialogResult.OK)
				{
					app.Config.Save();
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		void SaveRepl()
		{
		}

		void Contents()
		{
			try
			{
				Help.ShowHelp(this, app.HelpFilePath);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miReplication_Click(object sender, System.EventArgs e)
		{
			SaveRepl();
		}

		private void miPrint_Click(object sender, System.EventArgs e)
		{
            try
            {
                Print();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
		}

		private void miTypes_Click(object sender, System.EventArgs e)
		{
			try
			{
				ShowDockForm(typesForm);
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void ShowTypesForm()
		{
		}

		private void ShowDockForm(DockContent dockForm)
		{
			DockState ds=dockForm.DockState;
			if (ds == DockState.Unknown || ds == DockState.Hidden) ds = DockState.DockLeft;
			dockForm.Show(dockPanel,ds);
		}

		private void miScales_Click(object sender, System.EventArgs e)
		{
			ShowScales();
		}

		private void miColors_Click(object sender, System.EventArgs e)
		{
			ShowColors();
		}

		private void miMap_Click(object sender, System.EventArgs e)
		{
			NewMapView(false);		
		}

		private void miScaleUp_Click(object sender, System.EventArgs e)
		{
			ScaleUp();
		}

		private void miCascade_Click(object sender, System.EventArgs e)
		{
			Cascade();
		}

		private void miTileHorizontal_Click(object sender, System.EventArgs e)
		{
			TileHor();
		}

		private void miTileVertical_Click(object sender, System.EventArgs e)
		{
			TileVer();		
		}

		void NewMapView(bool maximized)
		{
			try
			{
				MapForm mapForm=new MapForm();
				mapForm.InitForm(app);
				mapForm.Show(dockPanel,DockState.Document);
				UpdateLibControls();
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		void Cascade()
		{
//!!!			this.LayoutMdi(MdiLayout.Cascade);		
		}

		void TileHor()
		{
//!!!			this.LayoutMdi(MdiLayout.TileHorizontal);		
		}

		void TileVer()
		{
//!!!			this.LayoutMdi(MdiLayout.TileVertical);		
		}

		private void miExit_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void MainForm_Resize(object sender, System.EventArgs e)
		{
		}

		private void cbScale_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(updateScaleComboFlag) return;
			if(e.KeyCode==Keys.Enter)	SetScale();
		}

		void Export()
		{
			try
			{
				GLib lib=app.Lib;
                if (lib != null)
                {
                    CreateDbForm form = GetCreateDbForm();
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        using (WaitCursor wc = new WaitCursor(this, Locale.Get("_exportingData...")))
                        {
                            if(form.IsDbProvider)
                            {
                                ConnectionInfo gisConn = MapCreator.CreateGisDb(form.DbCreationProperties);
                                using (Context context = lib.GetContext())
                                {
                                    context.TargetConn = gisConn.CreateConnection();
                                    context.Filter = Filter.All;
                                    lib.Save(context);
                                }
                                app.UpdateConnectionList(gisConn);
                            }
                            else
                            {
                               Save(form.FilePath);
                            }
                        }
                    }
                }
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

        private CreateDbForm GetCreateDbForm()
        {
            CreateDbForm form = new CreateDbForm(connectData);
            form.InitFileProvider(".wdr", "WinMap files (*.wdr)|*.wdr");
            return form;
        }

		private void miSave_Click(object sender, System.EventArgs e)
		{
			Save();
		}

		private void miConnect_Click(object sender, System.EventArgs e)
		{
			Connect();
		}

		private void miNew_Click(object sender, System.EventArgs e)
		{
			New();
		}

		private void miOpenFile_Click(object sender, System.EventArgs e)
		{
			Open();	
		}

		private void miOpenConnection_Click(object sender, System.EventArgs e)
		{
			Connect();
		}

		private void miSaveAs_Click(object sender, System.EventArgs e)
		{
			SaveAs();
		}

		private void miExport_Click(object sender, System.EventArgs e)
		{
			Export();
		}

		private void ucTypes_OnCompositeChecked(object sender, CompositeEventArgs e)
		{

		}

		private void ucTypes_OnCompositeSearch(object sender, EventArgs e)
		{

		}

		private void ucTypes_OnCompositeSelected(object sender, EventArgs e)
		{
			app.ShowProperties(typesForm.ucTypes.SelectedComposite);
		}

		private void ucProperties_Load(object sender, EventArgs e)
		{

		}

		private void ucProperties_OnPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
		}

		private void btnNew_Click(object sender, EventArgs e)
		{
			New();
		}

		private void btnOpen_Click(object sender, EventArgs e)
		{
			Open();
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			Connect();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			Save();
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			About();
		}

		private void btnPrint_Click(object sender, EventArgs e)
		{
			Print();
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			ScaleUp();
		}

		private void toolStripButton6_Click(object sender, EventArgs e)
		{
			try
			{
				ScaleDown();
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void btnMoveLeft_Click(object sender, EventArgs e)
		{
			MoveLeft();
		}

		private void btnMoveRight_Click(object sender, EventArgs e)
		{
			MoveRight();
		}

		private void btnMoveTop_Click(object sender, EventArgs e)
		{
			MoveTop();
		}

		private void btnMoveBottom_Click(object sender, EventArgs e)
		{
			MoveBottom();
		}

		private void btnRotateCW_Click(object sender, EventArgs e)
		{
			RotateCW();
		}

		private void btnRotateCCW_Click(object sender, EventArgs e)
		{
			RotateCCW();
		}

		private void cbScale_Click(object sender, EventArgs e)
		{
//			SetScale();
		}

		public void ShowSearch(GComposite comp, string str)
		{
			searchForm.ucSearch.SetSearchCondition(comp as GType,str);
			ShowDockForm(searchForm);
		}

		private void miCreateTestLib_Click(object sender, EventArgs e)
		{
			try
			{
				ConnectionInfo conn = new ConnectionInfo();
				app.CreateLib(conn);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miViews_Click(object sender, EventArgs e)
		{
			try
			{
				ShowDockForm(viewsForm);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void ToggleChecked(MenuItem mi)
		{
			mi.Checked = !mi.Checked;
		}

		private void miLayers_Click(object sender, EventArgs e)
		{
			try
			{
				ShowDockForm(layersForm);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miSearch_Click(object sender, EventArgs e)
		{
			try
			{
				ShowDockForm(searchForm);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}


		private void miProperties_Click(object sender, EventArgs e)
		{
			try
			{
				ShowDockForm(propertiesForm);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}


		private void tsNavigation_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{

		}

    private void dlgOpenFile_FileOk( object sender, CancelEventArgs e )
    {

    }

		private void miBackgroundImages_Click(object sender, EventArgs e)
		{
			try
			{
				ShowDockForm(backgroundForm);
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void cbScale_TextChanged(object sender, EventArgs e)
		{
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
                if (app.SaveAndCloseLib())
                {
                    if (dockPanelLoaded) DockPanelUtils.SaveAsXml(App.DockPanelConfigFilePath, dockPanel);
                }
                else e.Cancel = true;
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}
		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			app.Close();
		}

		private void miClose_Click(object sender, EventArgs e)
		{
			try
			{
                app.SaveAndCloseLib();
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

        private void miCreateTestObjects_Click(object sender, EventArgs e)
        {
            try
            {
                if (app.HasLib)
                {
                    using (WaitCursor wr = new WaitCursor(this, "Create test objects..."))
                    {
                        MapCreator.CreateTestObjects(app.Lib, app.Config.testObjectCount);
                        app.Layer.InitFromLib();
                        app.Repaint();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

 /*       private void ShowCoordForm( DockContent dockForm )
        {
            DockState ds = dockForm.DockState;
            if( ds == DockState.Unknown || ds == DockState.Hidden )
                ds = DockState.DockRight;
            dockForm.Show( dockPanel, ds );
        }*/

        private void miCoords_Click( object sender, EventArgs e )
        {
            try
            {
                if (App.HasLib && App.Lib.Selection.Count > 0)
                {
                    GObject obj = App.Lib.Selection.Object as GObject;
                    if (obj != null)
                    {
                        CoordsForm form = new CoordsForm(obj.Points);
                        if (form.ShowDialog() == DialogResult.OK)
                        {

                        }
                    }
                }
//                if( coordDTForm == null )
//                    coordDTForm = new CoordsForm( app );

 //               ShowCoordForm( coordDTForm );
            }
            catch( Exception ex )
            {
                Log.Exception( ex );
            }
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            MapForm mapForm = this.MapForm;
            if (mapForm != null)
            {
			    Graphics g = e.Graphics;
                Size size = new Size((int)(e.PageBounds.Size.Width * g.DpiX / 100), (int)(e.PageBounds.Size.Height * g.DpiY / 100));
                g.PageUnit = GraphicsUnit.Pixel;
                Map map = new Map(mapForm.Map, size, g);
                map.Draw();
            }
        }

        private void printDocument_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
        }
	}
}
