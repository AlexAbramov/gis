using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Geomethod.Windows.Forms;
using Geomethod.GeoLib;
using Geomethod;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for TypesUserControl.
	/// </summary>
	public partial class TypesUserControl : System.Windows.Forms.UserControl
	{
		IGeoApp app;
		Dictionary<int, TreeNode> treeNodes = new Dictionary<int, TreeNode>();//typeId, node
		TreeNode draggedNode=null;
		bool updating=false;
		GComposite mouseOverComp = null;

		#region Events
		public event EventHandler OnCompositeSelected;
		public event EventHandler OnCompositeSearch;
		public event EventHandler OnStatistics;
		public event EventHandler<TypeEventArgs> OnTypeAdded;
		public event EventHandler<TypeEventArgs> OnTypeRemoved;
		public event EventHandler<TypeEventArgs> OnTypeMoved;
		public event EventHandler<CompositeEventArgs> OnCompositeChecked;
		public event EventHandler<CompositeEventArgs> OnMouseOverComposite;
		#endregion

		#region Properties
		public IGeoApp App { get { return app; } }
		public GLib Lib { get { return app.Lib; } }
		public Layer AppLayer { get { return app.Layer; } }
		bool AutoSave { get { return app.GetControlsAttr(ControlsAttr.AutoSave); } }
		TreeNode SelectedNode { get { return tvTypes.SelectedNode; } }
		public GComposite SelectedComposite{get{return SelectedNode==null ? null : (GComposite)SelectedNode.Tag;}}
		GType SelectedType{get{return SelectedComposite is GType ? (GType)SelectedComposite : null;}}
		public GType DraggedType{get{return draggedNode!=null ? (GType)draggedNode.Tag : null;}}
		#endregion

		#region Construction
		public void InitControl(IGeoApp app) 
		{ 
			this.app = app;
		}
		public TypesUserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}
		#endregion

		public void SelectType(GType type)
		{
            if (treeNodes.ContainsKey(type.Id))
            {
                TreeNode node = treeNodes[type.Id] as TreeNode;
                if (node != null)
                {
                    tvTypes.SelectedNode = node;
                    node.EnsureVisible();
                }
            }
		}

		public void UpdateNodeText(GComposite comp)
		{
            if (treeNodes.ContainsKey(comp.Id))
            {
                TreeNode node = null;
                if (comp is GType) node = treeNodes[comp.Id] as TreeNode;
                else if (comp is GLib && tvTypes.Nodes.Count > 0) node = tvTypes.Nodes[0];
                if (node != null)
                {
                    if (node.Text != comp.Name) node.Text = comp.Name;
                }
            }
		}

		void BeginUpdate()
		{
			updating=true;
			tvTypes.BeginUpdate();
		}

		void EndUpdate()
		{
			tvTypes.EndUpdate();
			updating=false;
		}
		
		public void BuildTypesTree()
		{
			BeginUpdate();
			treeNodes.Clear();
			tvTypes.Nodes.Clear();
			GLib lib = Lib;
			if(lib!=null)
			{
				TreeNode node=tvTypes.Nodes.Add(lib.Name);
				node.Tag=lib;
				node.Checked=AppLayer.Includes(lib.Id);
				BuildTypesTree(node);
				node.Expand();
			}
			EndUpdate();
		}

		void BuildTypesTree(TreeNode node)
		{
			GComposite comp=node.Tag as GComposite;
			if (comp.Types != null)
			{
				foreach (GType type in comp.Types)
				{
					TreeNode node1 = AddNode(node, type);
					BuildTypesTree(node1);
				}
			}
		}

		void RemoveNode(TreeNode node)
		{
			treeNodes.Remove(((GComposite)node.Tag).Id);
			node.Remove();
		}

		TreeNode AddNode(TreeNode parNode,GType type)
		{
			TreeNode node=parNode.Nodes.Add(type.Name);
			treeNodes.Add(type.Id,node);
			node.Tag=type;
			node.Checked = AppLayer.Includes(type.Id);
			return node;
		}

		void ExpandTopNodes()
		{
			BeginUpdate();
			foreach(TreeNode node in tvTypes.Nodes)
			{
				node.Expand();
			}
			EndUpdate();
		}

		public void UpdateTypesTree()
		{
			BeginUpdate();
			foreach(TreeNode node in tvTypes.Nodes)
			{
				UpdateTypesTree(node);
			}
			EndUpdate();
		}

		public void UpdateTypesTree(TreeNode node)
		{
			GComposite comp=node.Tag as GComposite;
			bool inc=AppLayer.Includes(comp.Id);
			if(node.Checked!=inc) node.Checked=inc;
			foreach (TreeNode childNode in node.Nodes) UpdateTypesTree(childNode);
		}

		private void tvTypes_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode node=tvTypes.GetNodeAt(e.X,e.Y);
			if (node != null)
			{
				if (tvTypes.SelectedNode != node) //OnTypeSelected(this, null);
				  tvTypes.SelectedNode = node;
			}
		}

		private void tvTypes_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(draggedNode!=null)
			{
				TreeNode node=tvTypes.GetNodeAt(e.X,e.Y);
				if(CanDrop(node))
				{
					try
					{
						GType draggedType=(GType)draggedNode.Tag;
						GComposite comp=(GComposite)node.Tag;
						draggedType.Move(comp);
//						if(lib.HasDb) using(Context context=lib.GetContext()) draggedType.Save(context);
						RemoveNode(draggedNode);
						AddNode(node, draggedType);
						if (OnTypeMoved!=null) OnTypeMoved(this, new TypeEventArgs(draggedType));
					}
					catch(Exception ex)
					{
						Log.Exception(ex);
					}
					finally
					{
					}
				}
				EndDragging();
			}
		}

		public void EndDragging()
		{
			if(draggedNode!=null)
			{
				draggedNode=null;
				base.Cursor=Cursors.Default;
			}
		}

		bool CanDrop(TreeNode node)
		{
			if(draggedNode==null || node==null || draggedNode==node) return false;
			GType draggedType=(GType)draggedNode.Tag;
			GComposite comp=(GComposite)node.Tag;
			if(draggedType.ParentComposite==comp) return false;
			if(comp is GLib) return true;
			if(comp is GType)
			{
				return ((GType)comp).GeomType==draggedType.GeomType;
			}
			return false;
		}

		private void tvTypes_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode node=tvTypes.GetNodeAt(e.X,e.Y);
			if(draggedNode!=null)
			{
				if(e.Button==MouseButtons.Left)
				{
					base.Cursor=CanDrop(node) ? Cursors.Hand : Cursors.No;
				}
				else EndDragging();
			}
			else
			{
				if(e.Button==MouseButtons.Left)
				{
					draggedNode=node;
				}
			}
			GComposite comp = null;
			if(e.Button==MouseButtons.None && node!=null)
			{
				comp=(GComposite)node.Tag;
				app.Status=comp.Path;
			}
			if (mouseOverComp != comp)
			{
				mouseOverComp = comp;
				if (OnMouseOverComposite!=null)	OnMouseOverComposite(this, new CompositeEventArgs(mouseOverComp));
			}
			
		}

		void ShowProperties()
		{
			try
			{
                if (!updating)
                {
                    TreeNode node = tvTypes.SelectedNode;
                    if (node != null)
                    {
                        app.ShowProperties(node.Tag);
                    }
                }
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void tvTypes_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (!updating)
			{
				GComposite comp = e.Node.Tag as GComposite;
				AppLayer.Update(comp.Id,e.Node.Checked);
				if (OnCompositeChecked!=null) OnCompositeChecked(this, new CompositeEventArgs(comp));
                app.CheckRepaint(comp);
			}
		}

		private void miMask_Click(object sender, System.EventArgs e)
		{
			tvTypes.CheckBoxes=!tvTypes.CheckBoxes;
			miMask.Checked=tvTypes.CheckBoxes;
			if(tvTypes.CheckBoxes) this.UpdateTypesTree();
			else
			{
				this.Size=new Size(10,10);// ms bug fix
			}
			ExpandTopNodes();
		}

		private void TypesUserControl_Load(object sender, System.EventArgs e)
		{
		}

		private void tvTypes_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (!updating)
			{
				if (OnCompositeSelected != null) OnCompositeSelected(this, null);
				if (app.GetControlsAttr(ControlsAttr.ShowPropertiesOnSelect)) app.ShowProperties(SelectedComposite);
			}
		}

		private void miSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (OnCompositeSearch != null) OnCompositeSearch(this, null);
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		bool CanAddObject
		{
		  get
			{
				if(app.EditObject!=null) return false;
				TreeNode node=SelectedNode;
				if(node!=null && node.Tag is GType)
				{
					return app.CurrentMap!=null;
				}
				return false;
			}
		}

		private void miAddObject_Click(object sender, System.EventArgs e)
		{
			TreeNode node=tvTypes.SelectedNode;
			if(node==null) return;
			GType type=node.Tag as GType;
			Map map=app.CurrentMap;
			if(map!=null)
			{
				app.StartEditing(type);
			}
		}

		private void tvTypes_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.Delete:
					RemoveType();
					break;
				case Keys.Add:
					AddType();
					break;
				case Keys.Up:
					if(e.Shift)	MoveUp();
					break;
				case Keys.Down:
					if(e.Shift) MoveDown();
					break;
			}
		}

		private void cmTypes_Popup(object sender, System.EventArgs e)
		{
			bool nodeSelected=this.SelectedNode!=null;
			miAddObject.Enabled=CanAddObject;
			this.miAddType.Enabled=nodeSelected;
			this.miStat.Enabled=nodeSelected;
			this.miRemoveType.Enabled=this.SelectedType!=null;
			this.miMoveUp.Enabled=this.CanMoveUp;
			this.miMoveDown.Enabled=this.CanMoveDown;
		}

		private void miRemoveType_Click(object sender, System.EventArgs e)
		{
			RemoveType();
		}

		void RemoveType()
		{
			try
			{
				GType type=SelectedType;
				if(type==null) return;
				TreeNode node=SelectedNode;
				string s=string.Format("{0} '{1}' ?",Locale.Get("_removetypeconf"),type.Name);
				if(MessageBox.Show(s,Application.ProductName,MessageBoxButtons.YesNo)==DialogResult.Yes)
				{
					StatVisitor sv=new StatVisitor();
					type.Visit(sv);
					Stat stat=sv.Stat;
					bool removeFlag=true;
					if(stat.nObjects>0 || stat.nTypes>1)
					{
						s=string.Format(Locale.Get("_removetypeallconf"),type.Name,stat.nTypes-1,stat.nObjects);
						removeFlag=MessageBox.Show(s,Application.ProductName,MessageBoxButtons.YesNo)==DialogResult.Yes;
					}
					if(removeFlag)
					{
						RemoveNode(node);
						if(App.GetControlsAttr(ControlsAttr.AutoSave)) using(Context context=Lib.GetContext()) type.Remove(context);
						else type.Remove();
						if (OnTypeRemoved!=null) OnTypeRemoved(this, new TypeEventArgs(type));
						app.CheckRepaint(type);
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miAddType_Click(object sender, System.EventArgs e)
		{
			try
			{
				AddType();
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		void AddType()
		{
			TreeNode selNode=this.SelectedNode;
			if(selNode==null) return;
			GComposite comp=this.SelectedComposite;
			GType type=null;
			if(comp is GLib)
			{
				TypeForm form=new TypeForm();
				if(form.ShowDialog()==DialogResult.OK && form.geomType is GeomType)
				{
					GeomType gt=(GeomType)form.geomType;
					type=new GType(comp as GLib,gt);
				}
			}
			else if(comp is GType)
			{
				GType parType=(GType) comp;
				type=new GType(parType);
			}
			if(type!=null)
			{
				GenerateDefaultTypeName(type);
				if(app.GetControlsAttr(ControlsAttr.AutoSave))
				{
					using(Context context=Lib.GetContext()) type.Save(context);
				}
				AppLayer.Add(type.Id);
				TreeNode node=AddNode(selNode,type);
				this.tvTypes.SelectedNode=node;
				if (OnTypeAdded!=null) OnTypeAdded(this, new TypeEventArgs(type));
			}
		}

		void GenerateDefaultTypeName(GType type)
		{
			GComposite comp=type.ParentComposite;
			string prefix=comp is GLib || comp.Name.Trim().Length==0 ? GeoLib.GeoLibUtils.GetLocalizedName(type.GeomType) : comp.Name;
			for(int i=0;i<1000000;i++)
			{
				string name=prefix;
				if(i>0) name+=" "+i;
				if(name==comp.Name) continue;
				foreach(GType t in comp.Types)
				{
					if(t.Name==name)
					{
						name=null;
						break;
					}
				}
				if(name!=null)
				{
					type.Name=name;
					return;
				}
			}
		}

		private void miStat_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (OnStatistics!=null) OnStatistics(this, null);
				GComposite comp=this.SelectedComposite;
				StatVisitor sv=new StatVisitor();
				comp.Visit(sv);
				if(comp is GLib) app.ShowProperties(new LibStatProps(sv.Stat));
				else app.ShowProperties(new StatProps(sv.Stat));
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void miMoveUp_Click(object sender, System.EventArgs e)
		{
			MoveUp();
		}

		bool CanMoveUp
		{
			get
			{
				return SelectedNode!=null && SelectedNode.PrevNode!=null && SelectedType!=null;
			}
		}

		bool CanMoveDown
		{
			get
			{
				return SelectedNode!=null && SelectedNode.NextNode!=null && SelectedType!=null;
			}
		}

		void MoveUp()
		{
			try
			{
				if(!CanMoveUp) return;
				TreeNode node=this.SelectedNode;
				TreeNode prevNode=node.PrevNode;
				GType type=(GType)node.Tag;
				GType prevType=(GType)prevNode.Tag;

				node.Remove();
				prevNode.Parent.Nodes.Insert(prevNode.Index,node);
				this.tvTypes.SelectedNode=node;

				int priority=type.Priority;
				type.Priority=prevType.Priority;
				prevType.Priority=priority;

				if(app.GetControlsAttr(ControlsAttr.AutoSave))
				{
					using(Context context=Lib.GetContext())
					{
						type.Save(context);
						prevType.Save(context);
					}
				}

				if (OnTypeMoved!=null) OnTypeMoved(this, new TypeEventArgs(type));
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
			finally
			{
				app.UpdateControls();
			}
		}

		private void miMoveDown_Click(object sender, System.EventArgs e)
		{
			MoveDown();
		}

		void MoveDown()
		{
			try
			{
				if(!CanMoveDown) return;
				TreeNode node=this.SelectedNode;
				TreeNode nextNode=node.NextNode;
				GType type=(GType)node.Tag;
				GType nextType=(GType)nextNode.Tag;

				node.Remove();
				nextNode.Parent.Nodes.Insert(nextNode.Index,node);
				this.tvTypes.SelectedNode=node;

				int priority=type.Priority;
				type.Priority=nextType.Priority;
				nextType.Priority=priority;

				if(app.GetControlsAttr(ControlsAttr.AutoSave))
				{
					using(Context context=Lib.GetContext())
					{
						type.Save(context);
						nextType.Save(context);
						if (OnTypeMoved!=null) OnTypeMoved(this, new TypeEventArgs(type));
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
			finally
			{
				app.UpdateControls();
			}
		}
	}

	public class TypeEventArgs : EventArgs
	{
		GType type;
		public GType Type { get { return type; } }
		public TypeEventArgs(GType type) { this.type = type; }
	}

	public class CompositeEventArgs : EventArgs
	{
		GComposite comp;
		public GComposite Composite { get { return comp; } }
		public CompositeEventArgs(GComposite comp) { this.comp = comp; }
	}
}
