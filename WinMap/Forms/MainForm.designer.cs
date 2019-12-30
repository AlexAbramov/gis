using System;
using System.Collections.Generic;
using System.Text;

namespace WinMap.Forms
{
	partial class MainForm
	{
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.dlgPrint = new System.Windows.Forms.PrintDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.miFile = new System.Windows.Forms.MenuItem();
            this.miNew = new System.Windows.Forms.MenuItem();
            this.miOpen = new System.Windows.Forms.MenuItem();
            this.miOpenFile = new System.Windows.Forms.MenuItem();
            this.miOpenConnection = new System.Windows.Forms.MenuItem();
            this.miClose = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.miSave = new System.Windows.Forms.MenuItem();
            this.miSaveAs = new System.Windows.Forms.MenuItem();
            this.miExport = new System.Windows.Forms.MenuItem();
            this.miPrint = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.miExit = new System.Windows.Forms.MenuItem();
            this.miView = new System.Windows.Forms.MenuItem();
            this.miMap = new System.Windows.Forms.MenuItem();
            this.miSep = new System.Windows.Forms.MenuItem();
            this.miTypes = new System.Windows.Forms.MenuItem();
            this.miViews = new System.Windows.Forms.MenuItem();
            this.miLayers = new System.Windows.Forms.MenuItem();
            this.miBackgroundImages = new System.Windows.Forms.MenuItem();
            this.miSearch = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.miProperties = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.miEdit = new System.Windows.Forms.MenuItem();
            this.miColors = new System.Windows.Forms.MenuItem();
            this.miScales = new System.Windows.Forms.MenuItem();
            this.miCoords = new System.Windows.Forms.MenuItem();
            this.miNavigation = new System.Windows.Forms.MenuItem();
            this.miScaleUp = new System.Windows.Forms.MenuItem();
            this.miScaleDown = new System.Windows.Forms.MenuItem();
            this.miMoveLeft = new System.Windows.Forms.MenuItem();
            this.miMoveRight = new System.Windows.Forms.MenuItem();
            this.miMoveTop = new System.Windows.Forms.MenuItem();
            this.miMoveBottom = new System.Windows.Forms.MenuItem();
            this.miRotateCW = new System.Windows.Forms.MenuItem();
            this.miRotateCCW = new System.Windows.Forms.MenuItem();
            this.miTools = new System.Windows.Forms.MenuItem();
            this.miOptions = new System.Windows.Forms.MenuItem();
            this.miWindow = new System.Windows.Forms.MenuItem();
            this.miCascade = new System.Windows.Forms.MenuItem();
            this.miTileHorizontal = new System.Windows.Forms.MenuItem();
            this.miTileVertical = new System.Windows.Forms.MenuItem();
            this.miHelp = new System.Windows.Forms.MenuItem();
            this.miContents = new System.Windows.Forms.MenuItem();
            this.miAbout = new System.Windows.Forms.MenuItem();
            this.miDebug = new System.Windows.Forms.MenuItem();
            this.miCreateTestLib = new System.Windows.Forms.MenuItem();
            this.miCreateTestObjects = new System.Windows.Forms.MenuItem();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.tsNavigation = new System.Windows.Forms.ToolStrip();
            this.btnMoveLeft = new System.Windows.Forms.ToolStripButton();
            this.btnMoveRight = new System.Windows.Forms.ToolStripButton();
            this.btnMoveTop = new System.Windows.Forms.ToolStripButton();
            this.btnMoveBottom = new System.Windows.Forms.ToolStripButton();
            this.btnRotateCW = new System.Windows.Forms.ToolStripButton();
            this.btnRotateCCW = new System.Windows.Forms.ToolStripButton();
            this.cbScale = new System.Windows.Forms.ToolStripComboBox();
            this.btnScaleUp = new System.Windows.Forms.ToolStripButton();
            this.btnScaleDown = new System.Windows.Forms.ToolStripButton();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.dlgPrintPreview = new System.Windows.Forms.PrintPreviewDialog();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.tsNavigation.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            this.errorProvider.DataMember = "";
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            this.printDocument.QueryPageSettings += new System.Drawing.Printing.QueryPageSettingsEventHandler(this.printDocument_QueryPageSettings);
            // 
            // dlgPrint
            // 
            this.dlgPrint.Document = this.printDocument;
            // 
            // timer
            // 
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miFile,
            this.miView,
            this.miEdit,
            this.miNavigation,
            this.miTools,
            this.miWindow,
            this.miHelp,
            this.miDebug});
            // 
            // miFile
            // 
            this.miFile.Index = 0;
            this.miFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miNew,
            this.miOpen,
            this.miClose,
            this.menuItem4,
            this.miSave,
            this.miSaveAs,
            this.miExport,
            this.miPrint,
            this.menuItem5,
            this.miExit});
            this.miFile.Text = "_file";
            // 
            // miNew
            // 
            this.miNew.Index = 0;
            this.miNew.Text = "_new";
            this.miNew.Click += new System.EventHandler(this.miNew_Click);
            // 
            // miOpen
            // 
            this.miOpen.Index = 1;
            this.miOpen.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miOpenFile,
            this.miOpenConnection});
            this.miOpen.Text = "_open";
            this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
            // 
            // miOpenFile
            // 
            this.miOpenFile.Index = 0;
            this.miOpenFile.Text = "_openfile";
            this.miOpenFile.Click += new System.EventHandler(this.miOpenFile_Click);
            // 
            // miOpenConnection
            // 
            this.miOpenConnection.Index = 1;
            this.miOpenConnection.Text = "_openconnection";
            this.miOpenConnection.Click += new System.EventHandler(this.miOpenConnection_Click);
            // 
            // miClose
            // 
            this.miClose.Index = 2;
            this.miClose.Text = "_close";
            this.miClose.Click += new System.EventHandler(this.miClose_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "-";
            // 
            // miSave
            // 
            this.miSave.Index = 4;
            this.miSave.Text = "_save";
            this.miSave.Click += new System.EventHandler(this.miSave_Click);
            // 
            // miSaveAs
            // 
            this.miSaveAs.Index = 5;
            this.miSaveAs.Text = "_saveas";
            this.miSaveAs.Click += new System.EventHandler(this.miSaveAs_Click);
            // 
            // miExport
            // 
            this.miExport.Index = 6;
            this.miExport.Text = "_export";
            this.miExport.Click += new System.EventHandler(this.miExport_Click);
            // 
            // miPrint
            // 
            this.miPrint.Index = 7;
            this.miPrint.Text = "_print";
            this.miPrint.Click += new System.EventHandler(this.miPrint_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 8;
            this.menuItem5.Text = "-";
            // 
            // miExit
            // 
            this.miExit.Index = 9;
            this.miExit.Text = "_exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miView
            // 
            this.miView.Index = 1;
            this.miView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miMap,
            this.miSep,
            this.miTypes,
            this.miViews,
            this.miLayers,
            this.miBackgroundImages,
            this.miSearch,
            this.menuItem1,
            this.miProperties,
            this.menuItem2,
            this.menuItem3,
            this.menuItem6});
            this.miView.Text = "_view";
            // 
            // miMap
            // 
            this.miMap.Index = 0;
            this.miMap.Text = "_newMapWindow";
            this.miMap.Click += new System.EventHandler(this.miMap_Click);
            // 
            // miSep
            // 
            this.miSep.Index = 1;
            this.miSep.Text = "-";
            // 
            // miTypes
            // 
            this.miTypes.Index = 2;
            this.miTypes.Text = "_types";
            this.miTypes.Click += new System.EventHandler(this.miTypes_Click);
            // 
            // miViews
            // 
            this.miViews.Index = 3;
            this.miViews.Text = "_views";
            this.miViews.Click += new System.EventHandler(this.miViews_Click);
            // 
            // miLayers
            // 
            this.miLayers.Index = 4;
            this.miLayers.Text = "_layers";
            this.miLayers.Click += new System.EventHandler(this.miLayers_Click);
            // 
            // miBackgroundImages
            // 
            this.miBackgroundImages.Index = 5;
            this.miBackgroundImages.Text = "_backgroundImages";
            this.miBackgroundImages.Click += new System.EventHandler(this.miBackgroundImages_Click);
            // 
            // miSearch
            // 
            this.miSearch.Index = 6;
            this.miSearch.Text = "_search";
            this.miSearch.Click += new System.EventHandler(this.miSearch_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 7;
            this.menuItem1.Text = "-";
            // 
            // miProperties
            // 
            this.miProperties.Index = 8;
            this.miProperties.Text = "_properties";
            this.miProperties.Click += new System.EventHandler(this.miProperties_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 9;
            this.menuItem2.Text = "_tables";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 10;
            this.menuItem3.Text = "_records";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 11;
            this.menuItem6.Text = "";
            // 
            // miEdit
            // 
            this.miEdit.Index = 2;
            this.miEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miColors,
            this.miScales,
            this.miCoords});
            this.miEdit.Text = "_edit";
            // 
            // miColors
            // 
            this.miColors.Index = 0;
            this.miColors.Text = "_colors";
            this.miColors.Click += new System.EventHandler(this.miColors_Click);
            // 
            // miScales
            // 
            this.miScales.Index = 1;
            this.miScales.Text = "_scales";
            this.miScales.Click += new System.EventHandler(this.miScales_Click);
            // 
            // miCoords
            // 
            this.miCoords.Index = 2;
            this.miCoords.Text = "_coords";
            this.miCoords.Click += new System.EventHandler(this.miCoords_Click);
            // 
            // miNavigation
            // 
            this.miNavigation.Index = 3;
            this.miNavigation.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miScaleUp,
            this.miScaleDown,
            this.miMoveLeft,
            this.miMoveRight,
            this.miMoveTop,
            this.miMoveBottom,
            this.miRotateCW,
            this.miRotateCCW});
            this.miNavigation.Text = "_navigation";
            // 
            // miScaleUp
            // 
            this.miScaleUp.Index = 0;
            this.miScaleUp.Text = "_scaleup";
            this.miScaleUp.Click += new System.EventHandler(this.miScaleUp_Click);
            // 
            // miScaleDown
            // 
            this.miScaleDown.Index = 1;
            this.miScaleDown.Text = "_scaledown";
            this.miScaleDown.Click += new System.EventHandler(this.miScaleDown_Click);
            // 
            // miMoveLeft
            // 
            this.miMoveLeft.Index = 2;
            this.miMoveLeft.Text = "_moveleft";
            this.miMoveLeft.Click += new System.EventHandler(this.miLeft_Click);
            // 
            // miMoveRight
            // 
            this.miMoveRight.Index = 3;
            this.miMoveRight.Text = "_moveright";
            this.miMoveRight.Click += new System.EventHandler(this.miRight_Click);
            // 
            // miMoveTop
            // 
            this.miMoveTop.Index = 4;
            this.miMoveTop.Text = "_movetop";
            this.miMoveTop.Click += new System.EventHandler(this.miTop_Click);
            // 
            // miMoveBottom
            // 
            this.miMoveBottom.Index = 5;
            this.miMoveBottom.Text = "_movebottom";
            this.miMoveBottom.Click += new System.EventHandler(this.miBottom_Click);
            // 
            // miRotateCW
            // 
            this.miRotateCW.Index = 6;
            this.miRotateCW.Text = "_rotatecw";
            this.miRotateCW.Click += new System.EventHandler(this.miRotateCW_Click);
            // 
            // miRotateCCW
            // 
            this.miRotateCCW.Index = 7;
            this.miRotateCCW.Text = "_rotateccw";
            this.miRotateCCW.Click += new System.EventHandler(this.miRotateCCW_Click);
            // 
            // miTools
            // 
            this.miTools.Index = 4;
            this.miTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miOptions});
            this.miTools.Text = "_tools";
            this.miTools.Visible = false;
            // 
            // miOptions
            // 
            this.miOptions.Index = 0;
            this.miOptions.Text = "_options";
            this.miOptions.Click += new System.EventHandler(this.miOptions_Click);
            // 
            // miWindow
            // 
            this.miWindow.Index = 5;
            this.miWindow.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miCascade,
            this.miTileHorizontal,
            this.miTileVertical});
            this.miWindow.Text = "_window";
            this.miWindow.Visible = false;
            // 
            // miCascade
            // 
            this.miCascade.Index = 0;
            this.miCascade.Text = "_cascade";
            this.miCascade.Click += new System.EventHandler(this.miCascade_Click);
            // 
            // miTileHorizontal
            // 
            this.miTileHorizontal.Index = 1;
            this.miTileHorizontal.Text = "_tilehor";
            this.miTileHorizontal.Click += new System.EventHandler(this.miTileHorizontal_Click);
            // 
            // miTileVertical
            // 
            this.miTileVertical.Index = 2;
            this.miTileVertical.Text = "_tilever";
            this.miTileVertical.Click += new System.EventHandler(this.miTileVertical_Click);
            // 
            // miHelp
            // 
            this.miHelp.Index = 6;
            this.miHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miContents,
            this.miAbout});
            this.miHelp.Text = "_help";
            // 
            // miContents
            // 
            this.miContents.Index = 0;
            this.miContents.Text = "_contents";
            this.miContents.Visible = false;
            this.miContents.Click += new System.EventHandler(this.miContents_Click);
            // 
            // miAbout
            // 
            this.miAbout.Index = 1;
            this.miAbout.Text = "_about";
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // miDebug
            // 
            this.miDebug.Index = 7;
            this.miDebug.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miCreateTestLib,
            this.miCreateTestObjects});
            this.miDebug.Text = "_debug";
            // 
            // miCreateTestLib
            // 
            this.miCreateTestLib.Index = 0;
            this.miCreateTestLib.Text = "Create test lib";
            this.miCreateTestLib.Click += new System.EventHandler(this.miCreateTestLib_Click);
            // 
            // miCreateTestObjects
            // 
            this.miCreateTestObjects.Index = 1;
            this.miCreateTestObjects.Text = "Create test objects";
            this.miCreateTestObjects.Click += new System.EventHandler(this.miCreateTestObjects_Click);
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.Multiselect = true;
            this.dlgOpenFile.ShowReadOnly = true;
            this.dlgOpenFile.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgOpenFile_FileOk);
            // 
            // tsNavigation
            // 
            this.tsNavigation.Dock = System.Windows.Forms.DockStyle.None;
            this.tsNavigation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMoveLeft,
            this.btnMoveRight,
            this.btnMoveTop,
            this.btnMoveBottom,
            this.btnRotateCW,
            this.btnRotateCCW,
            this.cbScale,
            this.btnScaleUp,
            this.btnScaleDown});
            this.tsNavigation.Location = new System.Drawing.Point(3, 0);
            this.tsNavigation.Name = "tsNavigation";
            this.tsNavigation.Size = new System.Drawing.Size(281, 27);
            this.tsNavigation.TabIndex = 1;
            this.tsNavigation.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsNavigation_ItemClicked);
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveLeft.Image")));
            this.btnMoveLeft.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnMoveLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(24, 24);
            this.btnMoveLeft.ToolTipText = "_moveLeft";
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveRight.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveRight.Image")));
            this.btnMoveRight.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnMoveRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(24, 24);
            this.btnMoveRight.ToolTipText = "_moveRight";
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // btnMoveTop
            // 
            this.btnMoveTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveTop.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveTop.Image")));
            this.btnMoveTop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnMoveTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveTop.Name = "btnMoveTop";
            this.btnMoveTop.Size = new System.Drawing.Size(24, 24);
            this.btnMoveTop.ToolTipText = "_moveTop";
            this.btnMoveTop.Click += new System.EventHandler(this.btnMoveTop_Click);
            // 
            // btnMoveBottom
            // 
            this.btnMoveBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveBottom.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveBottom.Image")));
            this.btnMoveBottom.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnMoveBottom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveBottom.Name = "btnMoveBottom";
            this.btnMoveBottom.Size = new System.Drawing.Size(24, 24);
            this.btnMoveBottom.ToolTipText = "_moveBottom";
            this.btnMoveBottom.Click += new System.EventHandler(this.btnMoveBottom_Click);
            // 
            // btnRotateCW
            // 
            this.btnRotateCW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateCW.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateCW.Image")));
            this.btnRotateCW.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRotateCW.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateCW.Name = "btnRotateCW";
            this.btnRotateCW.Size = new System.Drawing.Size(24, 24);
            this.btnRotateCW.ToolTipText = "_rotateCW";
            this.btnRotateCW.Click += new System.EventHandler(this.btnRotateCW_Click);
            // 
            // btnRotateCCW
            // 
            this.btnRotateCCW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateCCW.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateCCW.Image")));
            this.btnRotateCCW.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRotateCCW.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateCCW.Name = "btnRotateCCW";
            this.btnRotateCCW.Size = new System.Drawing.Size(24, 24);
            this.btnRotateCCW.ToolTipText = "_rotateCCW";
            this.btnRotateCCW.Click += new System.EventHandler(this.btnRotateCCW_Click);
            // 
            // cbScale
            // 
            this.cbScale.Name = "cbScale";
            this.cbScale.Size = new System.Drawing.Size(75, 27);
            this.cbScale.ToolTipText = "_scale";
            this.cbScale.SelectedIndexChanged += new System.EventHandler(this.cbScale_SelectedIndexChanged);
            this.cbScale.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbScale_KeyDown);
            this.cbScale.TextChanged += new System.EventHandler(this.cbScale_TextChanged);
            this.cbScale.Click += new System.EventHandler(this.cbScale_Click);
            // 
            // btnScaleUp
            // 
            this.btnScaleUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnScaleUp.Image = ((System.Drawing.Image)(resources.GetObject("btnScaleUp.Image")));
            this.btnScaleUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnScaleUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnScaleUp.Name = "btnScaleUp";
            this.btnScaleUp.Size = new System.Drawing.Size(24, 24);
            this.btnScaleUp.ToolTipText = "_scaleUp";
            this.btnScaleUp.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // btnScaleDown
            // 
            this.btnScaleDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnScaleDown.Image = ((System.Drawing.Image)(resources.GetObject("btnScaleDown.Image")));
            this.btnScaleDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnScaleDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnScaleDown.Name = "btnScaleDown";
            this.btnScaleDown.Size = new System.Drawing.Size(24, 24);
            this.btnScaleDown.ToolTipText = "_scaleDown";
            this.btnScaleDown.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // tsMain
            // 
            this.tsMain.Dock = System.Windows.Forms.DockStyle.None;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnConnect,
            this.btnSave,
            this.btnPrint,
            this.btnHelp});
            this.tsMain.Location = new System.Drawing.Point(3, 27);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(156, 27);
            this.tsMain.TabIndex = 0;
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(24, 24);
            this.btnNew.Text = "_new";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(24, 24);
            this.btnOpen.Text = "btnOpen";
            this.btnOpen.ToolTipText = "_open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnConnect.Image")));
            this.btnConnect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(24, 24);
            this.btnConnect.Text = "btnConnect";
            this.btnConnect.ToolTipText = "_connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(24, 24);
            this.btnSave.Text = "btnSave";
            this.btnSave.ToolTipText = "_save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(24, 24);
            this.btnPrint.Text = "toolStripButton5";
            this.btnPrint.ToolTipText = "_print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(24, 24);
            this.btnHelp.Text = "toolStripButton4";
            this.btnHelp.ToolTipText = "_help";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 560);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(892, 22);
            this.statusStrip.TabIndex = 92;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(118, 17);
            this.lblStatus.Text = "toolStripStatusLabel1";
            // 
            // toolStripContainer
            // 
            this.toolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(892, 17);
            this.toolStripContainer.ContentPanel.Visible = false;
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStripContainer.LeftToolStripPanelVisible = false;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.RightToolStripPanelVisible = false;
            this.toolStripContainer.Size = new System.Drawing.Size(892, 71);
            this.toolStripContainer.TabIndex = 97;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.tsNavigation);
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.tsMain);
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.Location = new System.Drawing.Point(0, 71);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(892, 489);
            this.dockPanel.TabIndex = 99;
            // 
            // dlgPrintPreview
            // 
            this.dlgPrintPreview.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.dlgPrintPreview.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.dlgPrintPreview.ClientSize = new System.Drawing.Size(400, 300);
            this.dlgPrintPreview.Document = this.printDocument;
            this.dlgPrintPreview.Enabled = true;
            this.dlgPrintPreview.Icon = ((System.Drawing.Icon)(resources.GetObject("dlgPrintPreview.Icon")));
            this.dlgPrintPreview.Name = "dlgPrintPreview";
            this.dlgPrintPreview.Visible = false;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(892, 582);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WinMap";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.tsNavigation.ResumeLayout(false);
            this.tsNavigation.PerformLayout();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Drawing.Printing.PrintDocument printDocument;
		private System.Windows.Forms.PrintDialog dlgPrint;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.SaveFileDialog dlgSaveFile;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem miFile;
		private System.Windows.Forms.MenuItem miOpen;
		private System.Windows.Forms.MenuItem miPrint;
		private System.Windows.Forms.MenuItem miExit;
		private System.Windows.Forms.MenuItem miView;
		private System.Windows.Forms.MenuItem miTypes;
		private System.Windows.Forms.MenuItem miScales;
		private System.Windows.Forms.MenuItem miColors;
		private System.Windows.Forms.MenuItem miMap;
		private System.Windows.Forms.MenuItem miNavigation;
		private System.Windows.Forms.MenuItem miScaleUp;
		private System.Windows.Forms.MenuItem miScaleDown;
		private System.Windows.Forms.MenuItem miMoveLeft;
		private System.Windows.Forms.MenuItem miMoveTop;
		private System.Windows.Forms.MenuItem miMoveRight;
		private System.Windows.Forms.MenuItem miMoveBottom;
		private System.Windows.Forms.MenuItem miRotateCW;
		private System.Windows.Forms.MenuItem miRotateCCW;
		private System.Windows.Forms.MenuItem miTools;
		private System.Windows.Forms.MenuItem miOptions;
		private System.Windows.Forms.MenuItem miWindow;
		private System.Windows.Forms.MenuItem miCascade;
		private System.Windows.Forms.MenuItem miTileHorizontal;
		private System.Windows.Forms.MenuItem miTileVertical;
		private System.Windows.Forms.MenuItem miHelp;
		private System.Windows.Forms.MenuItem miContents;
		private System.Windows.Forms.MenuItem miAbout;
		private System.Windows.Forms.MenuItem miSave;
		private System.Windows.Forms.MenuItem miSearch;
		private System.Windows.Forms.MenuItem miViews;
		private System.Windows.Forms.MenuItem miLayers;
		private System.Windows.Forms.MenuItem miSep;
		private System.Windows.Forms.OpenFileDialog dlgOpenFile;
		private System.Windows.Forms.MenuItem miNew;
		private System.Windows.Forms.MenuItem miSaveAs;
		private System.Windows.Forms.MenuItem miOpenFile;
		private System.Windows.Forms.MenuItem miOpenConnection;
		private System.Windows.Forms.MenuItem miExport;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripButton btnNew;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripButton btnConnect;
		private System.Windows.Forms.ToolStripButton btnSave;
		private System.Windows.Forms.ToolStripButton btnHelp;
		private System.Windows.Forms.ToolStripButton btnPrint;
		private System.Windows.Forms.ToolStrip tsNavigation;
		private System.Windows.Forms.ToolStripButton btnScaleUp;
		private System.Windows.Forms.ToolStripButton btnScaleDown;
		private System.Windows.Forms.ToolStripButton btnMoveLeft;
		private System.Windows.Forms.ToolStripButton btnMoveRight;
		private System.Windows.Forms.ToolStripButton btnMoveTop;
		private System.Windows.Forms.ToolStripButton btnMoveBottom;
		private System.Windows.Forms.ToolStripButton btnRotateCW;
		private System.Windows.Forms.ToolStripButton btnRotateCCW;
		private System.Windows.Forms.ToolStripComboBox cbScale;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		private System.Windows.Forms.MenuItem miProperties;
		private System.Windows.Forms.MenuItem miBackgroundImages;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem miEdit;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem miClose;
        private System.Windows.Forms.MenuItem miCreateTestObjects;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem miCoords;
        private System.Windows.Forms.PrintPreviewDialog dlgPrintPreview;
        private System.Windows.Forms.MenuItem miDebug;
        private System.Windows.Forms.MenuItem miCreateTestLib;
	}
}
