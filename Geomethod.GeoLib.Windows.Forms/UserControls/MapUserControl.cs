using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Geomethod.Windows.Forms;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.GeoLib.Converters;
using Geomethod.GeoLib.Windows.Forms.Edit;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for MapUserControl.
	/// </summary>
	public partial class MapUserControl : System.Windows.Forms.UserControl
	{
//		enum TrackingMode{HotTracking,Selecting}
		IGeoApp app=null;
        BufferedMap map = null;
		PointSearchVisitor ps=new PointSearchVisitor();
		Graphics graphics=null;

		// tracking
		static readonly Point nullPoint=new Point(-1,-1);
		Point mouseDownPoint=nullPoint;

		// drawing
		bool repaintMap=false;
		Rectangle clipRect=Rectangle.Empty;
		Rectangle selectionRect=Rectangle.Empty;
		Rectangle prevSelectionRect=Rectangle.Empty;
		Pen selectionPen=new Pen(Color.DarkBlue);

		// state
		bool postponeStatusUpdate=false;
		void SetStatus(string s) { if(app.Status!=null) app.Status = s; }


		#region Events
//		public event EventHandler OnBgImageSelected;
//		public event EventHandler<BgImageEventArgs> OnBgImageAdded;
		#endregion

		#region Properties
		public IGeoApp App { get { return app; } }
		public GLib Lib { get { return app.Lib; } }
		public Map Map { get { return map; } }
		bool AutoSave { get { return app.GetControlsAttr(ControlsAttr.AutoSave); } }
		public Rectangle SelectionRect { set { selectionRect = value; } }
		#endregion

		public MapUserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		public void InitControl(IGeoApp app)
		{
			this.app = app;
		}

		private void MapUserControl_Load(object sender, System.EventArgs e)
		{
			if(!base.DesignMode)
			{
				selectionPen.DashStyle=DashStyle.Dash;
				map=new BufferedMap(Lib, ClientRectangle.Size);
				Map.Layer = app.Layer;
			}
		}

		public void Repaint()
		{
			repaintMap=true;
		}

		void UpdateMapRect(Point p)
		{
			Rectangle r=GetSelectionRect(p);
			selectionRect=Rectangle.Empty;
			if(mouseDownPoint==nullPoint || r.Width<10 || r.Height<10) return;
			Rect bounds=map.GToW(r);
			//						MapForm mapForm=new MapForm(app,bounds);
			//						mapForm.ShowDialog(app.MainForm);
			map.SetBounds(bounds);
			Repaint();
			app.DataChanged(map);
		}

		void UpdateSelection(Point p)
		{
			selectionRect=GetSelectionRect(p);
		}

		Rectangle GetSelectionRect(Point p)
		{
			if(mouseDownPoint==nullPoint) return Rectangle.Empty;
			Rectangle r=Rectangle.FromLTRB(mouseDownPoint.X,mouseDownPoint.Y,p.X,p.Y);
			GeomUtils.Normalize(ref r);
			return r;
		}

		void UpdateStatus(Point p)
		{
			Point wp=map.GToW(p);
			string s=string.Format("{0} ",wp);
			ps.Search(Lib,wp,map.Scale,map);
			GObject obj=ps.LastObject;
			if(obj!=null)
			{
				s+=obj.Path;
			}
			SetStatus(s);
		}

		void SearchClick(Point p)
		{
			Point wp=map.GToW(p);
			GLib lib = app.Lib;
			ps.Search(lib,wp,map.Scale,map);
			switch(ps.Count)
			{
				case 0:
					if(lib.Selection.Count>0)
					{
						Rect bounds=lib.Selection.Bounds;
						lib.Selection.Clear();
						app.CheckRepaint(bounds);
					}
					app.Status=Locale.Get("_objectsnotfound");
					app.ShowProperties(null);
					postponeStatusUpdate=true;
					break;
				default:
					lib.Selection.Set(ps.LastObject);
					app.CheckRepaint(ps.LastObject);
					app.ShowProperties(ps.LastObject);
					break;
					/*				default:
										ObjectsForm objectsForm=new ObjectsForm(app,ps.Objects);
										objectsForm.ShowDialog(this);
										break;*/
			}
		}

		private void Scene2dControl_MouseWheel(object sender, MouseEventArgs e)
		{
			int delta=e.Delta;
			if(delta==0) return;
//			Zoom(delta>0);
		}

		private void MapUserControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Point p=GetPoint(e);
			if(app.EditObject!=null)
			{
				if(e.Button==MouseButtons.Left)
				{
					Point wp=map.GToW(p);
					if(app.EditObject.addPointsMode)
					{
						app.EditObject.AddPoint(wp);
					}
					else
					{
//						app.EditObject.CheckPoint(wp);
					}
				}
				return;
			}
			else mouseDownPoint=p;
		}

		private void MapUserControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(app.EditObject!=null) return;
			Point p=GetPoint(e);
			bool samePoint=p==mouseDownPoint;
			switch(e.Button)
			{
				case MouseButtons.None:
					break;
				case MouseButtons.Left:
					if(app.DraggedType!=null)
					{
						app.StartEditing(app.DraggedType);
					    Point wp=map.GToW(p);
						app.EditObject.AddPoint(wp);
					}
					else
					{
						if(samePoint) SearchClick(p);
						else UpdateMapRect(p);
					}
					break;
				case MouseButtons.Right:
					if(samePoint)
					{
					}
					break;
			}
			mouseDownPoint=nullPoint;
		}

		private void MapUserControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Point p=GetPoint(e);
			if(app.EditObject!=null)
			{
				Point wp=map.GToW(p);
				app.EditObject.HighlightPoint(wp,this.map);
			}

			switch(e.Button)
			{
				case MouseButtons.None:
					if(postponeStatusUpdate) postponeStatusUpdate=false;
					else UpdateStatus(p);
					break;
				case MouseButtons.Left:
					GType type = app.DraggedType;// app.MainForm.TypesUserControl.DraggedType;
					if(type!=null)
					{
						Cursor=Cursors.Cross;
					}
					else UpdateSelection(p);
					break;
			}
		}

		private void MapUserControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if(map==null) return;
			clipRect=e.ClipRectangle;
			CheckDrawing();	
		}

		private void MapUserControl_MouseLeave(object sender, System.EventArgs e)
		{
			SetStatus("");
		}

		private void MapUserControl_Resize(object sender, System.EventArgs e)
		{
			mouseDownPoint=nullPoint;
			if(map==null) return;
			if(graphics!=null)
			{
//				graphics.Clear(this.BackColor);
				graphics.Dispose();
				graphics=null;
			}
		}

		void CheckDrawing()
		{
			if(graphics==null)
			{
				graphics=this.CreateGraphics();
				map.Resize(ClientRectangle.Size);
				repaintMap=true;
			}
			
			if(repaintMap)
			{
				map.Draw();
				graphics.DrawImageUnscaled(map.Image,0,0);
				repaintMap=false;
				clipRect=Rectangle.Empty;
				selectionRect=Rectangle.Empty;
				prevSelectionRect=Rectangle.Empty;
			}
			else if(!clipRect.IsEmpty && clipRect.Width>0 && clipRect.Height>0)
			{
				graphics.DrawImage(map.Image,clipRect.X,clipRect.Y,clipRect,GraphicsUnit.Pixel);
				clipRect=Rectangle.Empty;
			}

			if(selectionRect!=prevSelectionRect)
			{
				if(!prevSelectionRect.IsEmpty)
				{
					Rectangle r=prevSelectionRect;
					r.Width+=1;
					r.Height+=1;
					graphics.DrawImage(map.Image,r.X,r.Y,r,GraphicsUnit.Pixel);					
				}
				if(!selectionRect.IsEmpty)
				{
					graphics.DrawRectangle(selectionPen,selectionRect);
				}
				prevSelectionRect=selectionRect;
			}
		}

		Point GetPoint(MouseEventArgs e)
		{
			return new Point(e.X,e.Y);
		}

		public void OnTimer()
		{
			CheckDrawing();
		}

		public void EnsureVisible(Rect rect)
		{
			map.EnsureVisible(rect);
			Repaint();
		}

		public void Close()
		{
			if(graphics!=null)
			{
				graphics.Dispose();
				graphics=null;
			}
		}

		public void AddObject(GType type)
		{
//			editedObject
		}

		public new void Move(Direction dir)
		{
			Map.Move(dir);
			Repaint();
		}

		public void ScaleUp()
		{
			Map.ScaleUp();
			app.UpdateControls();
			Repaint();
		}

		public void ScaleDown()
		{
			Map.ScaleDown();
			app.UpdateControls();
			Repaint();
		}

		public void RotateCW()
		{
			Map.RotateCW();
			Repaint();
		}

		public void RotateCCW()
		{
			Map.RotateCCW();
			Repaint();
		}

		private void MapUserControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(app.EditObject!=null)
			{
				EditObject editObject=app.EditObject;
				if(e.Control)
				{
					switch(e.KeyCode)
					{
						case Keys.Up:
						case Keys.Right:
							editObject.Prev();
							editObject.CheckRepaint();
							return;
						case Keys.Down:
						case Keys.Left:
							editObject.Next();
							editObject.CheckRepaint();
							return;
					}
				}
				else if(e.Shift)
				{
					switch(e.KeyCode)
					{
						case Keys.Left:	editObject.MovePoint(Direction.Left,map);	return;
						case Keys.Right: editObject.MovePoint(Direction.Right,map);	return;
						case Keys.Up: editObject.MovePoint(Direction.Top,map); return;
						case Keys.Down:	editObject.MovePoint(Direction.Bottom,map);	return;
						case Keys.Home:	editObject.MovePoint(Direction.LeftTop,map); return;
						case Keys.PageUp:	editObject.MovePoint(Direction.RightTop,map);	return;
						case Keys.End: editObject.MovePoint(Direction.LeftBottom,map); return;
						case Keys.PageDown:	editObject.MovePoint(Direction.RightBottom,map); return;
					}
				}
				switch(e.KeyCode)
				{
					case Keys.Delete:	editObject.DeletePoint();	return;
				}
			}

			switch(e.KeyCode)
			{
				case Keys.Left:	Move(Direction.Left);	break;
				case Keys.Right: Move(Direction.Right);	break;
				case Keys.Up: Move(Direction.Top); break;
				case Keys.Down:	Move(Direction.Bottom);	break;
				case Keys.Home:	Move(Direction.LeftTop); break;
				case Keys.PageUp:	Move(Direction.RightTop);	break;
				case Keys.End: Move(Direction.LeftBottom); break;
				case Keys.PageDown:	Move(Direction.RightBottom); break;
				case Keys.Add: ScaleUp();	break;
				case Keys.Subtract:	ScaleDown(); break;
				case Keys.Escape:	EndEditing(); break;
			}
		}

		protected override bool IsInputKey(Keys key)		
		{ 
			switch(key)
			{ 
				case Keys.Up: 
				case Keys.Down: 
				case Keys.Right: 
				case Keys.Left: 
					return true; 
			} 
			switch((int)key)
			{
				case 65573:
				case 65574:
				case 65575:
				case 65576:
					return true;
			}
			return base.IsInputKey(key); 
		}

		private void miSaveAs_Click(object sender, System.EventArgs e)
		{
		  SaveAs();
		}

		void SaveAs()
		{
			try
			{
				dlgSaveFile.FileName="";
				dlgSaveFile.InitialDirectory=PathUtils.BaseDirectory+"Export";
				string[] ss={FileFilter.htm,FileFilter.ImagesCollection};
				dlgSaveFile.Filter=FileFilter.GetString(ss);
				if(dlgSaveFile.ShowDialog()==DialogResult.OK)
				{
					string filePath=dlgSaveFile.FileName;
					string ext=PathUtils.GetExtension(filePath);
					switch(ext)
					{
						case "bmp":
							map.Image.Save(filePath,ImageFormat.Bmp);
							break;
						case "gif":
							map.Image.Save(filePath,ImageFormat.Gif);
							break;
						case "jpg":
							map.Image.Save(filePath,ImageFormat.Jpeg);
							break;
						case "tif":
							map.Image.Save(filePath,ImageFormat.Tiff);
							break;
						case "png":
							map.Image.Save(filePath,ImageFormat.Png);
							break;
						case "htm":
							HtmlGenerator gen=new HtmlGenerator();
							gen.Generate(map,filePath);
							break;
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miEndEditing_Click(object sender, System.EventArgs e)
		{
			EndEditing();
		}		

		void EndEditing()
		{
			app.EndEditing();
		}

		private void cmMap_Popup(object sender, System.EventArgs e)
		{
			bool editMode=app.EditObject!=null;
			this.miSaveAs.Visible=!editMode;
			miEditSelectedObject.Visible=!editMode && Lib.Selection.Object!=null;
			this.miAddPointsMode.Visible=editMode;
			this.miAddPointsMode.Checked=editMode && app.EditObject.addPointsMode;
			miEndEditing.Visible=editMode;
		}

		private void miEditSelectedObject_Click(object sender, System.EventArgs e)
		{
			EditSelectedObject();
		}

		void EditSelectedObject()
		{
			try
			{
				if(Lib.Selection.Object is GObject)
				{
					app.StartEditing((GObject)Lib.Selection.Object);
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miAddPointsMode_Click(object sender, System.EventArgs e)
		{
			if(app.EditObject!=null)
			{
				miAddPointsMode.Checked=!miAddPointsMode.Checked;
				app.EditObject.addPointsMode=miAddPointsMode.Checked;
			}
		}
	}
}
